using UnityEngine;
using System.Collections;

public class Scene01 : SceneWithSubBase {
	
	public GUISpriteAnimation paneSpriteAnim;

	void Update()
	{
		
	}

	protected override void OnInitScene()
	{
		subSceneController.ClearScene ();

		paneSpriteAnim.InitHide();
	}
	
	protected override void OnOpenScene()
	{
		paneSpriteAnim.Appear(() => {
			OpenComplete();

			subSceneController.SetInitialScene();
		});
	}
	
	protected override void OnCloseScene()
	{
		subSceneController.ClearScene ();
		
		paneSpriteAnim.Disappear(() => {
			CloseComplete();
		});
	}
	
	// subScene Event
	protected override void OnRevertTransitionStart (SceneBase scene)
	{
		Debug.Log("OpenSubScene: "+scene);
	}
	protected override void OnRevertTransitionComplete (SceneBase scene)
	{
		Debug.Log("OpenCompleteSubScene: "+scene);
	}
	protected override void OnCloseTransitionStart (SceneBase scene)
	{
		Debug.Log("CloseSubScene: "+scene);
	}
	protected override void OnCloseTransitionComplete (SceneBase scene)
	{
		Debug.Log("CloseCompleteSubScene: "+scene);
	}
}
