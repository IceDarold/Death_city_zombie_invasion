using System;
using UnityEngine;

public class LerpClass
{
	public LerpClass(float _curValue)
	{
		this.curValue = _curValue;
	}

	protected LerpClass()
	{
	}

	public void Start(float _endValue, float _lerpTime, float _delay = 0f, Action _callback = null)
	{
		this.startValue = this.curValue;
		this.endValue = _endValue;
		this.startTime = Time.time;
		this.lerpTime = _lerpTime;
		this.delay = _delay;
		this.canStart = true;
		this.callback = _callback;
	}

	public float DoLerp()
	{
		if (this.delay > 0f)
		{
			this.delay -= Time.deltaTime;
			if (this.delay <= 0f)
			{
				this.startTime = Time.time;
			}
			return this.curValue;
		}
		this.curValue = Mathf.Lerp(this.startValue, this.endValue, (Time.time - this.startTime) / this.lerpTime);
		if (this.startValue >= this.endValue)
		{
			this.curValue = Mathf.Clamp(this.curValue, this.endValue, this.startValue);
		}
		else
		{
			this.curValue = Mathf.Clamp(this.curValue, this.startValue, this.endValue);
		}
		if (this.curValue == this.endValue)
		{
			if (this.callback != null)
			{
				this.callback();
				this.callback = null;
			}
			this.canStart = false;
		}
		return this.curValue;
	}

	public bool canStart;

	public float endValue;

	public float curValue;

	public float startTime;

	public float lerpTime;

	public float delay;

	protected float startValue;

	protected Action callback;
}
