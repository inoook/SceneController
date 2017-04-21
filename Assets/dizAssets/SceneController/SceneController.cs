using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SceneController : MonoBehaviour
{
	public static void Log(string str){
//		Debug.LogWarning (str);
	}

	public enum TransitionType
	{
		CLOSE_START_,
		CLOSE_COMP_,

		OPEN_START,
		OPEN_COMP,
		CLOSE_START,
		CLOSE_COMP,

		SET_SCENE
	}

	public delegate void TransitionHandler (SceneBase scene,TransitionType transitionType);
	public event TransitionHandler eventTransition;// 画面遷移通知用
	
	public SceneBase currentScene; // 現在表示中のScene
	private SceneBase nextScene; // 次に表示しようとしているScene
	private SceneBase openTransitionScene;//開始処理中のScene
	private SceneBase closeTransitionScene;//終了処理中のScene

	[SerializeField]
	private SceneBase[] scenes;

	public bool enableStartInitSceneId = false;
	[SerializeField]
	private int initSceneId;

	// Use this for initialization
	void Start ()
	{
		currentScene = null;
		nextScene = null;
		openTransitionScene = null;
		closeTransitionScene = null;

		// init All Scene
		for (int i = 0; i < scenes.Length; i++) {
			scenes [i].gameObject.SetActive (false);
			scenes [i].enabled = false;

			scenes [i].Setup(this);
			scenes [i].Initialization ();
		}
		//
		SetInitialScene();
	}

	// 最初のシーンをセットする。
	public void SetInitialScene()
	{
		if (enableStartInitSceneId) {
			SetScene (initSceneId);
		}
	}

	public void SetScene (int sceneId)
	{
		SceneBase scene;// 表示するシーン
		if (sceneId > -1) {
			scene = scenes [sceneId];
		} else {
//			Debug.LogError ("error sceneId: "+sceneId);
			// オープンしているシーンを閉じる。
			ClearScene ();
			return;
		}
		
		if (currentScene == scene) {
			return;
		}

		// OpenScene(シーンが開始した時)にnextSceneはnullになる。
		// CloseScene でシーン終了後に nextScene がオープンする。
		nextScene = scene;

		if (eventTransition != null) {
			eventTransition (scene, TransitionType.SET_SCENE);
		}

		if (closeTransitionScene != null) {
			// 終了処理中の時
			Debug.LogWarning( closeTransitionScene + ">>>>>> 終了処理　中断");
			closeTransitionScene.eventCloseComplete -= SceneCloseComplete;

			closeTransitionScene.eventCloseTransitionInterruptComplete += SceneCloseTransitionInterruptComplete;
			closeTransitionScene.CloseTransitionInterrupt ();
			return;
		}
		if (openTransitionScene != null) {
			// 開始処理の時
			Debug.LogWarning( openTransitionScene + ">>>>>> 開始処理　中断");
			openTransitionScene.eventOpenComplete -= SceneOpenComplete;

			openTransitionScene.eventOpenTransitionInterruptComplete += SceneOpenTransitionInterruptComplete;
			openTransitionScene.OpenTransitionInterrupt ();
			return;
		}

		// 次のシーンを開く。現在開いているシーンがあれば閉じる。
		OpenNextScene();
	}

	void OpenNextScene()
	{
		if (currentScene != null) {
			// 現在のシーンを閉じる処理開始
			// currentSceneを閉じる、完了後、nextSceneを表示
			currentScene.eventCloseComplete += SceneCloseComplete;
			CloseScene (currentScene);
		} else {
			OpenScene (nextScene);
		}
	}

	void SceneOpenTransitionInterruptComplete (SceneBase scene)
	{
		openTransitionScene.eventOpenTransitionInterruptComplete -= SceneCloseTransitionInterruptComplete;

		if (openTransitionScene.gameObject.activeSelf) {
			openTransitionScene.gameObject.SetActive (false);
		}
		openTransitionScene = null;

		// 次のシーンを開く
		OpenNextScene();
	}

	void SceneCloseTransitionInterruptComplete (SceneBase scene)
	{
		closeTransitionScene.eventOpenTransitionInterruptComplete -= SceneCloseTransitionInterruptComplete;
		
		if (closeTransitionScene.gameObject.activeSelf) {
			closeTransitionScene.gameObject.SetActive (false);
		}
		closeTransitionScene = null;

		// 次のシーンを開く
		OpenNextScene();
	}




	// 表示しているシーンを閉じる
	public void ClearScene()
	{
//		SetScene (-1);
		if (currentScene != null) {
			// 現在のシーンを閉じる処理開始
			currentScene.eventCloseComplete += SceneClearComplete;
			CloseScene (currentScene);
		}
	}

	void SceneClearComplete (SceneBase scene)
	{
		scene.eventCloseComplete -= SceneClearComplete;
		// 現在のシーンのクローズが完了。
		closeTransitionScene = null;
		CloseSceneComplete (scene);
	}

	void CloseScene (SceneBase closeScene)
	{
		Log (">>> [Log] CloseScene: " + closeScene);
		// EVENT
		if (eventTransition != null) {
			eventTransition (closeScene, TransitionType.CLOSE_START);
		}

		currentScene = null;
		closeTransitionScene = closeScene;

		// CloseScene -> OnCloseScene -> CloseComplete
		closeScene.CloseScene ();
	}

	void CloseSceneComplete (SceneBase closeScene)
	{
		if (closeScene.gameObject.activeSelf) {
			closeScene.gameObject.SetActive (false);
		}
		if (closeScene.enabled) {
			closeScene.enabled = false;
		}
		closeScene.Initialization ();

		// EVENT
		if (eventTransition != null) {
			eventTransition (closeScene, TransitionType.CLOSE_COMP);
		}
	}

	public int GetCurrentSceneId ()
	{
		return GetSceneIdByScene (currentScene);
	}

	public int GetSceneIdByScene (SceneBase scene)
	{
		if (scene == null) {
			return -1;
		}
		for (int i = 0; i < scenes.Length; i++) {
			if (scene == scenes [i]) {
				return i;
			}
		}
		return -1;
	}
	
	void OpenScene (SceneBase scene)
	{
		Log ("Open: "+scene);
		if (scene == null) { 
			Log ("Wrong Scene");
			return;
		}
		
		if (!scene.gameObject.activeSelf) {
			scene.gameObject.SetActive (true);
		}
		if (!scene.enabled) {
			scene.enabled = true;
		}

		scene.Initialization (); // シーンの初期化

		// EVENT / 遷移開始
		if (eventTransition != null) {
			eventTransition (scene, TransitionType.OPEN_START);
		}

		openTransitionScene = scene; // 遷移中のシーン
		currentScene = null;

		scene.eventOpenComplete += SceneOpenComplete;
		scene.OpenScene ();
	}
	
	void SceneOpenComplete (SceneBase scene)
	{
		Log ("NewSceneOpenComplete: "+nextScene + " / "+scene);
		scene.eventOpenComplete -= SceneOpenComplete;

		// シーンのトランジションが完了した時
		openTransitionScene = null;
		currentScene = nextScene; // 現在選択されている（有効な）シーン
		
		// clear
		nextScene = null;
		
		// EVENT
		if (eventTransition != null) {
			eventTransition (scene, TransitionType.OPEN_COMP);
		}
	}

	void SceneCloseComplete (SceneBase scene)
	{
		scene.eventCloseComplete -= SceneCloseComplete;

		// 現在のシーンのクローズが完了。
		closeTransitionScene = null;
		
		CloseSceneComplete (scene);
		
		// 次のシーンをオープンする。
		if (nextScene != null) {
			OpenScene (nextScene);
		}
	}

	#region cross
	public void SetSceneCross (int sceneId)
	{
		SceneBase scene = scenes [sceneId];
		nextScene = scene;
		if (currentScene != null) {
			if (currentScene == scene) { 
				Debug.Log ("Same Scene");
				return;
			}

			currentScene.eventCloseComplete += HandleCurrentSceneEventEndCrossComplete;
			CloseScene (currentScene);

			// set nextScene
			if (nextScene != null) {
				OpenScene (nextScene);
			}

		} else {
			OpenScene (nextScene);
		}
	}

	void HandleCurrentSceneEventEndCrossComplete (SceneBase scene)
	{
		scene.eventCloseComplete -= HandleCurrentSceneEventEndCrossComplete;

		if (scene.gameObject.activeSelf) {
			scene.gameObject.SetActive (false);
		}
		if (scene.enabled) {
			scene.enabled = false;
		}
	}
	#endregion

	public SceneBase[] GetScenes()
	{
		return scenes;
	}

	public void NextScene()
	{
		int current = GetCurrentSceneId ();
		current++;
		int nextId = (int)Mathf.Repeat (current, scenes.Length);
		SetScene (nextId);
	}

}

