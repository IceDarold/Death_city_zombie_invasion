using System;
using UnityEngine;

public class LookAtCameraScript : MonoBehaviour
{
	private void Start()
	{
		if (Camera.main != null)
		{
			this.cameraTransform = Camera.main.transform;
		}
	}

	private void Update()
	{
		if (Time.time - this.lastUpdateTime < 0.02f)
		{
			return;
		}
		this.lastUpdateTime = Time.time;
		base.transform.LookAt(this.cameraTransform);
	}

	protected Transform cameraTransform;

	protected float lastUpdateTime;
}
