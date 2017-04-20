using UnityEngine;
using System.Collections;

public class SceneControllerDebug : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[SerializeField]
	Rect drawRect = new Rect(10,10,200,200);

	[SerializeField]
	SceneController sceneController;

	void OnGUI()
	{
		GUILayout.BeginArea (drawRect);
		SceneBase[] scenes = sceneController.GetScenes ();
		for (int i = 0; i < scenes.Length; i++) {
			SceneBase s = scenes[i];
			if (GUILayout.Button (s.name)) {
				sceneController.SetScene (i);
			}
		}
		GUILayout.EndArea ();

	}

	public SceneController GetSceneController()
	{
		return sceneController;
	}
}
