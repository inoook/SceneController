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
		
		Go.killAllTweensWithTarget(sprite);
		sprite.alpha = 1.0f;
		sprite.ForceUpdate();
		
		if(this.GetComponent<Collider>() != null){
			this.GetComponent<Collider>().enabled = true;
		}
	}
	public void InitHide()
	{
		Go.killAllTweensWithTarget(sprite);
		sprite.Init();
		sprite.alpha = 0.0f;
		sprite.ForceUpdate();
		
		if(this.GetComponent<Collider>() != null){
			this.GetComponent<Collider>().enabled = false;
		}
		
		this.gameObject.SetActive(false);
	}
	
	// alphaAnimation Only
	public GoTween Appear(System.Action completeAct)
	{
		return Appear(0.5f, 0.0f, completeAct);
	}
	public GoTween Appear(float time = 0.5f, float delay = 0.0f, System.Action completeAct = null )
	{
		this.gameObject.SetActive(true);
		
		if(this.GetComponent<Collider>() != null){
			this.GetComponent<Collider>().enabled = true;
		}
		Go.killAllTweensWithTarget(sprite);
		
		GoTween alphaTw = new GoTween(sprite, time, new GoTweenConfig().floatProp("alpha", 1.0f).setDelay(delay).setEaseType(GoEaseType.Linear));
		alphaTw.setOnCompleteHandler((t) => {
			if(eventAnimState != null){
				eventAnimState(AnimState.Appear_COMPLETE);
			}
			if(completeAct != null){
				completeAct();
			}
		});
		
		Go.addTween(alphaTw);

		return alphaTw;
	}

	public GoTween Disappear(System.Action completeAct)
	{
		return Disappear(0.5f, 0.0f, completeAct);
	}
	public GoTween Disappear(float time = 0.5f, float delay = 0.0f, System.Action completeAct = null)
	{
		if(this.GetComponent<Collider>() != null){
			this.GetComponent<Collider>().enabled = false;
		}
		Go.killAllTweensWithTarget(sprite);
		
		GoTween alphaTw = new GoTween(sprite, time, new GoTweenConfig().floatProp("alpha", 0.0f).setDelay(delay).setEaseType(GoEaseType.Linear));
		alphaTw.setOnCompleteHandler((t) => {
			this.gameObject.SetActive(false);
			if(eventAnimState != null){
				eventAnimState(AnimState.Disapper_COMPLETE);
			}
			if(completeAct != null){
				completeAct();
			}
		});
		Go.addTween(alphaTw);

		return alphaTw;
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
		Go.killAllTweensWithTarget(sprite);

		GoTweenChain chain = new GoTweenChain();
		GoTween alphaTwAppear = new GoTween(sprite, time, new GoTweenConfig().floatProp("alpha", 1.0f).setEaseType(GoEaseType.Linear));
		GoTween alphaTwDisAppear = new GoTween(sprite, time, new GoTweenConfig().floatProp("alpha", 0.0f).setEaseType(GoEaseType.Linear));

		chain.append(alphaTwAppear);
		chain.appendDelay(appearTime);
		chain.append(alphaTwDisAppear);
		
		chain.setOnCompleteHandler((t) => {
			if(eventAnimState != null){
				eventAnimState(AnimState.Disapper_COMPLETE);
			}
		});
		chain.play();
	}

}
