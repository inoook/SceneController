using UnityEngine;
using System.Collections;

public class SceneWithSubBase : SceneBase {

	public SceneController subSceneController;

	protected override void OnSetup()
	{
		
	}

	protected override void OnInitScene()
	{
		base.OnInitScene();
	}

	protected override void OnOpenScene()
	{
		base.OnOpenScene ();

		sceneController.SetInitialScene();
	}

	protected override void OnCloseScene()
	{
		base.OnCloseScene ();
	}
	
	public void SetSubScene(int sceneId){
		subSceneController.SetScene (sceneId);
	}
	public SceneController GetSubSceneController (){
		return subSceneController;
	}
}
