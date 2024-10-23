using System;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
	private void Start()
	{
		this.timeleft = this.updateInterval;
		this.lastSample = Time.realtimeSinceStartup;
	}

	private float GetFPS()
	{
		return this.fps;
	}

	private bool HasFPS()
	{
		return this.gotIntervals > 2f;
	}

	private void Update()
	{
		this.frames += 1f;
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		float num = realtimeSinceStartup - this.lastSample;
		this.lastSample = realtimeSinceStartup;
		this.timeleft -= num;
		this.accum += 1f / num;
		if ((double)this.timeleft <= 0.0)
		{
			this.fps = this.accum / this.frames;
			this.timeleft = this.updateInterval;
			this.accum = 0f;
			this.frames = 0f;
			this.gotIntervals += 1f;
		}
	}

	private void OnGUI()
	{
		GUI.Box(new Rect((float)(Screen.width - 260), 100f, 300f, 40f), this.fps.ToString("f2") + " | QSetting: " + QualitySettings.currentLevel);
	}

	private float updateInterval = 1f;

	private float accum;

	private float frames;

	private float timeleft;

	private float fps = 15f;

	private float lastSample;

	private float gotIntervals;
}
