using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public SceneController rootSceneController;
	public SceneBase toScene;

	void OnGUI()
	{
		if (GUILayout.Button ("TransrateScene")) {
			SceneTransitionUtils.TransrateScene (rootSceneController, toScene);
		}
		//		if (GUILayout.Button ("TransrateSceneFromTo")) {
		//			TransrateSceneFromTo (rootSceneController, fromScene, toScene);
		//		}
		//		if (GUILayout.Button ("GetCurrentScene")) {
		//			SceneBase current;
		//			if (GetCurrentScene (rootSceneController, out current)) {
		//				Debug.Log ("current: "+current);
		//			}
		//		}
	}
}
