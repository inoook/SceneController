using UnityEngine;
using System.Collections;

public class GUISpriteAnimation : MonoBehaviour {
	public enum AnimState{
		Appear_COMPLETE, Disapper_COMPLETE
	}

	public delegate void HandlerAnim(AnimState state);
	public event HandlerAnim eventAnimState;

	public GUISprite sprite;
	
	// Use this for initialization
	void Start () {
		
	}

	public void InitAppear()
	{
		this.gameObject.SetActive(true);
		
		CancelSpriteAlphaLT ();
		sprite.alpha = 1.0f;
		sprite.ForceUpdate();
		
		if(this.GetComponent<Collider>() != null){
			this.GetComponent<Collider>().enabled = true;
		}
	}
	public void InitHide()
	{
		CancelSpriteAlphaLT ();
		sprite.Init();
		sprite.alpha = 0.0f;
		sprite.ForceUpdate();
		
		if(this.GetComponent<Collider>() != null){
			this.GetComponent<Collider>().enabled = false;
		}
		
		this.gameObject.SetActive(false);
	}
	
	// alphaAnimation Only
	public void Appear(System.Action completeAct)
	{
		Appear(0.5f, 0.0f, completeAct);
	}

	LTDescr spriteAlphaLT;
	void CancelSpriteAlphaLT()
	{
		if (spriteAlphaLT != null) {
			LeanTween.cancel (spriteAlphaLT.uniqueId);
		}
	}

	public void Appear(float time = 0.5f, float delay = 0.0f, System.Action completeAct = null )
	{
		this.gameObject.SetActive(true);

		if(this.GetComponent<Collider>() != null){
			this.GetComponent<Collider>().enabled = true;
		}

		CancelSpriteAlphaLT ();

		spriteAlphaLT = LeanTween.value (sprite.alpha, 1.0f, time).setDelay (delay).setOnUpdate ((v) => {
			sprite.alpha = v;
		}).setOnComplete(() => {
			if(eventAnimState != null){
				eventAnimState(AnimState.Appear_COMPLETE);
			}
			if(completeAct != null){
				completeAct();
			}
		});
	}


	public void Disappear(System.Action completeAct)
	{
		Disappear(0.5f, 0.0f, completeAct);
	}
	public void Disappear(float time = 0.5f, float delay = 0.0f, System.Action completeAct = null)
	{
		if(this.GetComponent<Collider>() != null){
			this.GetComponent<Collider>().enabled = false;
		}

		CancelSpriteAlphaLT ();

		spriteAlphaLT = LeanTween.value (sprite.alpha, 0.0f, time).setDelay (delay).setOnUpdate ((v) => {
			sprite.alpha = v;
		}).setOnComplete(() => {
			this.gameObject.SetActive(false);
			if(eventAnimState != null){
				eventAnimState(AnimState.Disapper_COMPLETE);
			}
			if(completeAct != null){
				completeAct();
			}
		});
	}
	
	public void DisableBtn()
	{
		if(this.GetComponent<Collider>() != null){
			this.GetComponent<Collider>().enabled = false;
		}
	}

	public void AppearAndDisappear(float time = 0.5f, float appearTime = 0.0f)
	{
		this.gameObject.SetActive(true);
		
		if(this.GetComponent<Collider>() != null){
			this.GetComponent<Collider>().enabled = true;
		}

		CancelSpriteAlphaLT ();

		spriteAlphaLT = LeanTween.value (sprite.alpha, 1.0f, time).setOnUpdate ((v) => {
			sprite.alpha = v;
		}).setOnComplete(() => {
			spriteAlphaLT = LeanTween.value (sprite.alpha, 0.0f, time).setOnUpdate ((v) => {
				sprite.alpha = v;
			}).setOnComplete(() => {
				if(eventAnimState != null){
					eventAnimState(AnimState.Disapper_COMPLETE);
				}
			});
		});
	}

}
