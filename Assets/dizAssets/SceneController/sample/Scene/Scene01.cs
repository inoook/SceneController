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
//			subSceneController.SetInitialScene(); or
			subSceneController.SetScene(0);

			OpenComplete();
		});
	}
	
	protected override void OnCloseScene()
	{
		subSceneController.ClearScene ();
		
		paneSpriteAnim.Disappear(() => {
			CloseComplete();
		});
	}

}
