using UnityEngine;
using System.Collections;

public class SceneTemplate : SceneBase {

	protected override void OnInitScene()
	{

	}
	
	protected override void OnOpenScene()
	{
		OpenComplete();
	}
	
	protected override void OnCloseScene()
	{
		CloseComplete();
	}
}
