using UnityEngine;
using System.Collections;

public class SceneBase : MonoBehaviour {
	
	public delegate void SceneEventHandler(SceneBase scene);
	public event SceneEventHandler eventOpenComplete;
	public event SceneEventHandler eventCloseComplete;
	public event SceneEventHandler eventOpenTransitionInterruptComplete;
	public event SceneEventHandler eventCloseTransitionInterruptComplete;

	public event SceneEventHandler eventRevertFromSubSceneComplete;

	protected SceneController sceneController;

	public void SetParentScene(int id)
	{
		sceneController.SetScene (id);
	}

	// 起動時にSceneControllerから一度だけ呼ばれる。
	private bool isSetup = false;
	public void Setup(SceneController sceneController_)
	{
		if (!isSetup) {
			sceneController = sceneController_;
			OnSetup ();
			isSetup = true;
		}else{
			Debug.LogWarning ("Setupは起動時のみよばれます。");
		}
	}

	// セットアップ時
	protected virtual void OnSetup()
	{
		
	}

	// 初期状態へ
	// シーンの初期化時にSceneControllerから呼ばれる。
	// シーンの開始時／シーン終了時
	public void Initialization()
	{
		OnInitScene();
	}

	// SceneController から呼ばれる
	public void OpenScene()
	{
		OnOpenScene ();
	}
	// SceneController から呼ばれる
	public void CloseScene()
	{
		OnCloseScene();
	}
	public void OpenTransitionInterrupt()
	{
		OnOpenTransitionInterrupt ();
	}
	public void CloseTransitionInterrupt()
	{
		OnCloseTransitionInterrupt ();
	}

	#region complete
	// シーン表示完了時に継承先でこれを呼ぶ　eventOpenComplete を発行する
	protected void OpenComplete()
	{
		OnOpenSceneComplete();

		if(eventOpenComplete != null){
			eventOpenComplete(this);
		}
	}

	protected void CloseComplete()
	{
		StopAllCoroutines();
		CancelInvoke ();

		OnCloseSceneComplete();

		if(eventCloseComplete != null){
			eventCloseComplete(this);
		}
	}

	protected void OpenTransitionInterruptComplete()
	{
		OnOpenTransitionInterruptComplete();

		if(eventOpenTransitionInterruptComplete != null){
			eventOpenTransitionInterruptComplete(this);
		}
	}

	protected void CloseTransitionInterruptComplete()
	{
		OnCloseTransitionInterruptComplete();

		if(eventCloseTransitionInterruptComplete != null){
			eventCloseTransitionInterruptComplete(this);
		}
	}
	#endregion


	#region override
	// 初期化(シーンの開始時／シーン終了時)の処理はこの関数をoverrideして書く
	protected virtual void OnInitScene()
	{
		SceneController.Log ("[" + this.gameObject.name + "] no Implementation 'OnInitScene'");
	}
	// シーントランジションの開始
	protected virtual void OnOpenScene()
	{
		SceneController.Log ("[" + this.gameObject.name + "] no Implementation 'OnOpenScene'");
		OpenComplete();
	}
	// シーントランジションクローズの開始
	protected virtual void OnCloseScene()
	{
		SceneController.Log ("[" + this.gameObject.name + "] no Implementation 'OnCloseScene'");
		CloseComplete();
	}
	// 表示シーンの割り込み時
	public virtual void OnOpenTransitionInterrupt()
	{
		SceneController.Log ("[" + this.gameObject.name + "] no Implementation 'OnOpenTransitionInterrupt'");
		Initialization ();
		OpenTransitionInterruptComplete ();
	}
	public virtual void OnCloseTransitionInterrupt()
	{
		SceneController.Log ("[" + this.gameObject.name + "] no Implementation 'OnCloseTransitionInterrupt'");
		Initialization ();
		CloseTransitionInterruptComplete ();
	}
	
	// シーン表示完了時何か実行する時
	protected virtual void OnOpenSceneComplete()
	{
		SceneController.Log ("[" + this.gameObject.name + "] no Implementation 'OnOpenSceneComplete'");
	}
	
	// シーン終了完了時何か行う時
	protected virtual void OnCloseSceneComplete()
	{
		SceneController.Log ("[" + this.gameObject.name + "] no Implementation 'OnCloseSceneComplete'");
	}

	protected virtual void OnOpenTransitionInterruptComplete()
	{
		
	}
	protected virtual void OnCloseTransitionInterruptComplete()
	{

	}
	#endregion

	// TODO: check
	// no use?
	public virtual void RevertFromSubScene()
	{
		RevertFromSubSceneComplete();
	}
	public void RevertFromSubSceneComplete()
	{
		if(eventRevertFromSubSceneComplete != null){
			eventRevertFromSubSceneComplete(this);
		}
	}

	public SceneController GetSceneController()
	{
		return sceneController;
	}

}
