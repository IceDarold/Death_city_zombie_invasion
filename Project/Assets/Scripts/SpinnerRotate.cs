using System;
using UnityEngine;

public class SpinnerRotate : MonoBehaviour
{
	private void Start()
	{
		this.interval = 360 / this.Segments;
	}

	private void Update()
	{
		if (DateTime.Now >= this.nextFrame)
		{
			this.nextFrame = DateTime.Now.AddSeconds(0.1);
			this.rotation = (this.rotation + 1) % this.Segments;
			base.transform.localRotation = Quaternion.AngleAxis((float)(this.rotation * this.interval), Vector3.back);
		}
	}

	public int Segments = 12;

	private int interval;

	private int rotation;

	private DateTime nextFrame;
}
