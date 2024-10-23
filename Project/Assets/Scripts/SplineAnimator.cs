using System;
using SplineUtilities;
using UnityEngine;

public class SplineAnimator : MonoBehaviour
{
	private void Update()
	{
		this.passedTime += Time.deltaTime * this.speed;
		float param = SplineUtils.WrapValue(this.passedTime + this.offSet, 0f, 1f, this.wrapMode);
		base.transform.position = this.spline.GetPositionOnSpline(param);
		base.transform.rotation = this.spline.GetOrientationOnSpline(param);
	}

	public Spline spline;

	public WrapMode wrapMode = WrapMode.Once;

	public float speed = 1f;

	public float offSet;

	public float passedTime;
}
