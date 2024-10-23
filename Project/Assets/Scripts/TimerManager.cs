using System;
using UnityEngine;

public class TimerManager
{
	public static TimerManager GetInstance()
	{
		if (TimerManager.instance == null)
		{
			TimerManager.instance = new TimerManager();
		}
		return TimerManager.instance;
	}

	public void SetTimer(int index, float interval, bool doAtStart)
	{
		TimerInfo timerInfo = new TimerInfo();
		if (doAtStart)
		{
			timerInfo.lastDoTime = -9999f;
		}
		else
		{
			timerInfo.lastDoTime = Time.time;
		}
		timerInfo.interval = interval;
		this.timerInfos[index] = timerInfo;
	}

	public void Do(int index)
	{
		this.timerInfos[index].lastDoTime = Time.time;
	}

	public bool Ready(int index)
	{
		return Time.time - this.timerInfos[index].lastDoTime > this.timerInfos[index].interval;
	}

	protected static TimerManager instance;

	public TimerInfo[] timerInfos = new TimerInfo[100];
}
