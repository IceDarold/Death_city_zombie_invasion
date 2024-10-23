using System;
using UnityEngine;

public class ScaleAnimationScript : MonoBehaviour
{
	private void Start()
	{
		this.destScale = UnityEngine.Random.Range(this.destScaleMin, this.destScaleMax);
	}

	private void Update()
	{
		if (base.transform.localScale.x < this.destScale)
		{
			base.transform.localScale += Vector3.one * Time.deltaTime * this.scaleSpeed;
		}
	}

	public float destScaleMax = 0.4f;

	public float destScaleMin = 0.2f;

	protected float destScale;

	public float scaleSpeed = 1f;
}
