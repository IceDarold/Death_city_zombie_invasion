using System;
using UnityEngine;

public class PlayAnimationScript : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (Time.time - this.lastUpdateTime < 0.02f)
		{
			return;
		}
		this.lastUpdateTime = Time.time;
	}

	public string animationName;

	protected float lastUpdateTime;
}
