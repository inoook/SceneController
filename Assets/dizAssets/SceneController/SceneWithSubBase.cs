using UnityEngine;
using System.Collections;

public class SceneWithSubBase : SceneBase {

	public SceneController subSceneController;

	protected override void OnSetup()
	{
		RegisterTransitionEvent();
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

	public void RegisterTransitionEvent()
	{
		sceneController.eventTransition += HandleEventTransition;
	}

	void HandleEventTransition (SceneBase scene, SceneController.TransitionType transitionType)
	{
		if(transitionType == SceneController.TransitionType.CLOSE_START_){
			//Debug.Log(">> OnCloseTransitionStart: "+ this.gameObject.name + " / "+ scene);
			OnCloseTransitionStart(scene);
		}else if(transitionType == SceneController.TransitionType.CLOSE_COMP_){
			//Debug.Log(">> OnCloseTransitionComplete: "+ this.gameObject.name + " / "+ scene);
			OnCloseTransitionComplete(scene);
		}
	}

	protected virtual void OnCloseTransitionStart (SceneBase scene)
	{
		
	}
	protected virtual void OnCloseTransitionComplete (SceneBase scene)
	{
		
	}

	public void SetSubScene(int sceneId){
		subSceneController.SetScene (sceneId);
	}
	public SceneController GetSubSceneController (){
		return subSceneController;
	}
}
