using UnityEngine;
using System.Collections;

public class Scene01_2 : SceneBase {

	public GUISpriteAnimation paneSpriteAnim;
	
	protected override void OnInitScene()
	{
		paneSpriteAnim.InitHide();
	}
	
	protected override void OnOpenScene()
	{
		OpenComplete();
		
		paneSpriteAnim.Appear();
	}
	
	protected override void OnCloseScene()
	{
		CloseComplete();
	}
}
