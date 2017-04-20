using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GUISprite : MonoBehaviour {

	public bool useSharedMaterial = false;

	public float _alpha = 1.0f;
	
	//[HideInInspector]
	public float t_alpha = 1.0f;
	
	[HideInInspector]
	public float _scale = 1.0f;
	
	public Material mat;
	public Color _color = Color.white;
	
	//[HideInInspector]
	public Vector3 orgScale;
	
	public Texture textrue;
	
	public GUISprite[] childSprite;
	
	private Transform myTrans;
	
	public string colorTypeName = "_Color";
	public bool enableUpdateChildrenColor = false;
	
	
	public bool enableUserColor = false;
	
	// Use this for initialization
	void Awake () {
		isInitParam = false;
		GetInitParam();
		
		UpdateChildren();
		ForceUpdate();
	}
	
	public void Init()
	{
		GetInitParam();
	}
	
	public bool isInitParam = false;
	private void GetInitParam()
	{
		if(isInitParam){ return; }
		
		myTrans = this.gameObject.transform;
		
		if(this.gameObject.GetComponent<Renderer>() != null){
			if(mat == null){
				mat = this.gameObject.GetComponent<Renderer>().material;
			}
			
			if(!enableUserColor){ // user MaterialColor
				_color = mat.GetColor(colorTypeName);
				_color.a = t_alpha;
			}
			textrue = mat.mainTexture;
		}else{
			_color = new Color(1.0f, 1.0f, 1.0f, t_alpha);
		}
		//
		isInitParam = true;
	}
	
	void Start()
	{
		//GetInitParam();
		
		// 最初の描画がみだれるので追加。
		ForceUpdate();
	}
	
	public void ForceUpdate()
	{
		if(Application.isPlaying){
			GetInitParam();
		}
		UpdateChildren();
		Render();
	}

	private float pre_alpha;
	public float alphaOffset = 1.0f;
	
	void Render () {
		// render color and alpha

		_color.a = _alpha * t_alpha;
		/*
		if(colorTypeName == "_TintColor"){
			_color.a *= 0.5f;
		}
		*/
		_color.a *= alphaOffset;
		
		if(this.gameObject.GetComponent<Renderer>() != null && mat != null){
			mat.SetColor(colorTypeName, _color);
			if(textrue != null){
				//mat.mainTexture = textrue;
			}
		}
		// update children
		foreach(GUISprite sprite in childSprite)
		{
			if(sprite != this){
				sprite.t_alpha = _alpha * t_alpha;
				if(enableUpdateChildrenColor){
					sprite._color = _color;
				}
				sprite.ForceUpdate();
			}
		}

	}
	
	public void SetTexture(Texture src, bool autoFit = false)
	{
		if(this.gameObject.GetComponent<Renderer>() != null){
			mat = this.gameObject.GetComponent<Renderer>().material;
			
			if(mat.mainTexture != src){
				mat.mainTexture = src;
				textrue = src;
			}
		}
		if(autoFit){
			AutoFit();
		}
	}
	
	public void AutoFit()
	{
		// 最初に指定した矩形内に入るようにスケール自動調整
		float maxSize = Mathf.Max(orgScale.x, orgScale.y);
		//float v = (float)textrue.width / (float)textrue.height;
		
		Vector3 tScale = orgScale;
		float vv;
		if((float)textrue.height > (float)textrue.width){
			// horaizon
			vv = (float)textrue.width / (float)textrue.height;
			tScale.x = maxSize * vv;
			tScale.y = maxSize;
			
			orgScale = tScale;
			this.transform.localScale = tScale;
		}else{
			// landscape
			vv = (float)textrue.height / (float)textrue.width;
			tScale.x = maxSize;
			tScale.y = maxSize * vv;
			
			orgScale = tScale;
			this.transform.localScale = tScale;
		}
	}
	
	public void setAspect(float aspect)
	{
		Vector3 tScale = orgScale;
		tScale.x = tScale.y * aspect;
		
		this.transform.localScale = tScale;
	}
	
	public Vector3 position{
		set { myTrans.localPosition = value; }
		get { return myTrans.localPosition; }
	}
	
	public void addChild(GameObject gObj){
		gObj.transform.parent = this.gameObject.transform;
		UpdateChildren();
	}
	
	public void removeChild(GameObject gObj){
		gObj.transform.parent = null;
		UpdateChildren();
	}
	
	
	private void UpdateChildren()
	{
		List<GUISprite> sprites = new List<GUISprite>();
		foreach(Transform child in this.transform){
			GUISprite sprite = child.GetComponent<GUISprite>();
			if(sprite != null){
				sprites.Add(sprite);
			}
		}
		childSprite = sprites.ToArray();

		//childSprite = this.gameObject.GetComponentsInChildren<GUISprite>();
	}
	
	public float alpha
	{
		set{
			//GetInitParam();
			_alpha = value;
			ForceUpdate();
		}
		get{ 
			//GetInitParam();
			return _alpha;
		}
	}
	
	public Color color
	{
		set{
			//GetInitParam();
			_color = value;
			_alpha = value.a;
			_color.a = _alpha;
			ForceUpdate();
		}
		get{
			//GetInitParam();
			_color.a = _alpha; // add
			return _color;
		}
	}
	
	public float m_scale{
		get{
			return _scale;
		}
		set{
			_scale = value;
			this.transform.localScale = orgScale * _scale;
		}
	}
	
	
	void OnEnable()
	{
		UpdateChildren();
		
		foreach(GUISprite sprite in childSprite)
		{
			if(sprite != this){
				sprite.enabled = true;
				sprite.ForceUpdate();
			}
		}
		ForceUpdate();
	}
	void OnDisable()
	{
		UpdateChildren();
		
		ForceUpdate();
		foreach(GUISprite sprite in childSprite)
		{
			if(sprite != this){
				sprite.enabled = false;
			}
		}
	}
	
	public void enableRenderer(bool isEnable)
	{
		this.gameObject.GetComponent<Renderer>().enabled = isEnable;
	}
}
