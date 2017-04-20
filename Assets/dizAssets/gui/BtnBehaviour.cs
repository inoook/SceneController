using UnityEngine;
using System.Collections;


public class BtnBehaviour : MonoBehaviour {

	public SceneController sceneController;
	public int id = -1;

	void OnMouseUpAsButton()
	{
		sceneController.SetScene (id);
	}
}
