using System;
using UnityEngine;

public class Timer
{
	public string Name { get; set; }

	public void SetTimer(float interval, bool doAtStart)
	{
		if (doAtStart)
		{
			this.info.lastDoTime = -9999f;
		}
		else
		{
			this.info.lastDoTime = Time.time;
		}
		this.info.interval = interval;
		this.start = true;
	}

	public void Do()
	{
		this.info.lastDoTime = Time.time;
	}

	public bool Ready()
	{
		return this.start && Time.time - this.info.lastDoTime > this.info.interval;
	}

	protected TimerInfo info = new TimerInfo();

	protected bool start;
}
