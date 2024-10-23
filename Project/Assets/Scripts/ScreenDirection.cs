using System;
using UnityEngine;

public class ScreenDirection : MonoBehaviour
{
	private void Start()
	{
		this.startTime = Time.time;
	}

	private void Update()
	{
	}

	private void ResetDirection()
	{
	}

	private void LateUpdate()
	{
		this.ResetDirection();
	}

	protected float startTime;
}
