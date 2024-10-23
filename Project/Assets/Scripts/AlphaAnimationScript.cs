using System;
using UnityEngine;

public class AlphaAnimationScript : MonoBehaviour
{
	private void Start()
	{
		this.startTime = Time.time;
	}

	private void Update()
	{
		this.deltaTime += Time.deltaTime;
		if (this.deltaTime < 0.02f)
		{
			return;
		}
		Color value = Color.white;
		if (this.enableAlphaAnimation || this.enableBrightAnimation)
		{
			value = base.GetComponent<Renderer>().material.GetColor(this.colorPropertyName);
		}
		if (this.enableAlphaAnimation)
		{
			if (this.increasing)
			{
				value.a += this.animationSpeed * this.deltaTime;
				value.a = Mathf.Clamp(value.a, this.minAlpha, this.maxAlpha);
				if (value.a == this.maxAlpha)
				{
					this.increasing = false;
				}
			}
			else
			{
				value.a -= this.animationSpeed * this.deltaTime;
				value.a = Mathf.Clamp(value.a, this.minAlpha, this.maxAlpha);
				if (value.a == this.minAlpha)
				{
					this.increasing = true;
				}
			}
		}
		if (this.enableBrightAnimation)
		{
			if (this.increasing)
			{
				value.r += this.animationSpeed * this.deltaTime;
				value.g += this.animationSpeed * this.deltaTime;
				value.b += this.animationSpeed * this.deltaTime;
				if (value.r >= this.maxBright || value.g >= this.maxBright || value.b >= this.maxBright)
				{
					this.increasing = false;
				}
			}
			else
			{
				value.r -= this.animationSpeed * this.deltaTime;
				value.g -= this.animationSpeed * this.deltaTime;
				value.b -= this.animationSpeed * this.deltaTime;
				if (value.r <= this.minBright || value.g <= this.minBright || value.b <= this.minBright)
				{
					this.increasing = true;
				}
			}
		}
		base.GetComponent<Renderer>().material.SetColor(this.colorPropertyName, value);
		this.deltaTime = 0f;
	}

	public float maxAlpha = 1f;

	public float minAlpha;

	public float animationSpeed = 5.5f;

	public float maxBright = 1f;

	public float minBright;

	public bool enableAlphaAnimation;

	public bool enableBrightAnimation;

	public string colorPropertyName = "_TintColor";

	protected float alpha;

	protected float startTime;

	protected bool increasing = true;

	public Color startColor = Color.yellow;

	protected float lastUpdateTime;

	protected float deltaTime;
}
