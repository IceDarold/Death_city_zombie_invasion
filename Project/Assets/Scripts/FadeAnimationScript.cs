using System;
using UnityEngine;
using UnityEngine.UI;

public class FadeAnimationScript : Singleton<FadeAnimationScript>
{
	private void Awake()
	{
		this.mat = base.GetComponent<Renderer>().material;
	}

	private void Start()
	{
		this.MatColor = this.initColor;
	}

	public void StartFade(Color startColor, Color endColor)
	{
		this.startColor = startColor;
		this.endColor = endColor;
		this.MatColor = startColor;
		this.enableAlphaAnimation = true;
		this.startTime = Time.time;
	}

	public void SetColor(Color color, string _text = "")
	{
		this.MatColor = color;
		this.sceneTitle.text = _text;
		this.sceneTitle.color = new Color(1f, 1f, 1f, 1f);
	}

	private void StartFade(Color startColor, Color endColor, float time)
	{
		if (time != 0f)
		{
			this.animationSpeed = 1f / time;
		}
		this.StartFade(startColor, endColor);
	}

	public bool FadeOutComplete()
	{
		return this.MatColor.a == 0f;
	}

	public bool FadeInComplete()
	{
		return this.MatColor.a == 1f;
	}

	public static FadeAnimationScript GetInstance()
	{
		return Singleton<FadeAnimationScript>.Instance;
	}

	public void FadeInBlack(float time, Action callback = null)
	{
		if (this.CheckColor(Color.black, callback))
		{
			return;
		}
		this.fadeBlackCallBack = callback;
		this.StartFade(new Color(0f, 0f, 0f, 0f), Color.black, time);
	}

	public void FadeOutBlack(float time, Action callback = null)
	{
		if (this.CheckColor(new Color(0f, 0f, 0f, 0f), callback))
		{
			return;
		}
		this.fadeBlackCallBack = callback;
		this.StartFade(Color.black, new Color(0f, 0f, 0f, 0f), time);
	}

	private bool CheckColor(Color _color, Action callback)
	{
		Color matColor = this.MatColor;
		if (matColor == _color && callback != null)
		{
			callback();
		}
		return matColor == _color;
	}

	public Color MatColor
	{
		get
		{
			return this.mat.GetColor(this.colorPropertyName);
		}
		set
		{
			this.mat.SetColor(this.colorPropertyName, value);
		}
	}

	public void LateUpdate()
	{
		this.deltaTime = Time.time - this.startTime;
		if (this.deltaTime < 0.01f)
		{
			return;
		}
		if (this.enableAlphaAnimation)
		{
			float a = this.startColor.a;
			float a2 = this.endColor.a;
			float num = Mathf.Sign(a2 - a);
			Color matColor = this.MatColor;
			matColor.a += num * this.animationSpeed * Time.deltaTime;
			this.sceneTitle.color = new Color(1f, 1f, 1f, matColor.a);
			if (Mathf.Sign(a2 - matColor.a) != num)
			{
				matColor.a = a2;
				this.enableAlphaAnimation = false;
				if (this.fadeBlackCallBack != null)
				{
					this.fadeBlackCallBack();
				}
				if (a2 == 0f)
				{
					this.sceneTitle.text = string.Empty;
				}
			}
			this.MatColor = matColor;
		}
		this.deltaTime = 0f;
	}

	public Color startColor = Color.black;

	public Color endColor = new Color(0f, 0f, 0f, 0f);

	public float animationSpeed = 0.5f;

	public bool enableAlphaAnimation;

	public Color initColor = Color.black;

	public Text sceneTitle;

	public string colorPropertyName = "_TintColor";

	protected Material mat;

	protected float deltaTime;

	private Action fadeBlackCallBack;

	protected float startTime;
}
