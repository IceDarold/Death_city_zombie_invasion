using System;
using UnityEngine;

public class DelayCameraDisplayScript : MonoBehaviour
{
	private void Start()
	{
		base.GetComponent<Camera>().enabled = false;
		this.startTime = Time.time;
	}

	private void Update()
	{
		if (Time.time - this.startTime > this.delayTime)
		{
			base.GetComponent<Camera>().enabled = true;
		}
	}

	public float delayTime = 0.5f;

	protected float startTime;
}
