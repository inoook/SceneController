using UnityEngine;
using System.Collections;

public class Scene02 : SceneBase {

	public GUISpriteAnimation paneSpriteAnim;

	protected override void OnInitScene()
	{
		paneSpriteAnim.InitHide();
	}
	
	protected override void OnOpenScene()
	{
//		paneSpriteAnim.InitAppear();
//		OpenComplete();
		paneSpriteAnim.Appear(() => {
			OpenComplete();
		});
	}
	
	protected override void OnCloseScene()
	{
//		paneSpriteAnim.InitHide();
//		CloseComplete();
		paneSpriteAnim.Disappear(() => {
			CloseComplete();
		});
	}
}
