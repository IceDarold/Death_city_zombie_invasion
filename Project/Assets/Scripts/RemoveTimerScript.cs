using System;
using UnityEngine;

public class RemoveTimerScript : MonoBehaviour
{
	private void Start()
	{
		this.createdTime = Time.time;
	}

	private void Update()
	{
		if (Time.time - this.createdTime > this.life)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public float life;

	protected float createdTime;
}
