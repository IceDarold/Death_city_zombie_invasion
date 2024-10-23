using System;
using UnityEngine;

public class LaserTrembleScript : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (this.increasing)
		{
			if (base.transform.localScale.x < this.maxScaleX)
			{
				base.transform.localScale += Vector3.right * Time.deltaTime * this.scaleSpeed;
			}
			else
			{
				this.increasing = false;
			}
		}
		else if (base.transform.localScale.x > this.minScaleX)
		{
			base.transform.localScale -= Vector3.right * Time.deltaTime * this.scaleSpeed;
		}
		else
		{
			this.increasing = true;
		}
	}

	public float minScaleX = 0.01f;

	public float maxScaleX = 0.02f;

	public float scaleSpeed = 0.1f;

	protected bool increasing;
}
