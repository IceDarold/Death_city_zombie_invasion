using System;
using SplineUtilities;
using UnityEngine;

public class SplineAnimatorClosestPoint : MonoBehaviour
{
	private void Update()
	{
		if (this.target == null || this.spline == null)
		{
			return;
		}
		float param = SplineUtils.WrapValue(this.spline.GetClosestPointParam(this.target.position, this.iterations, 0f, 1f, 0.01f) + this.offset, 0f, 1f, this.wMode);
		base.transform.position = this.spline.GetPositionOnSpline(param);
		base.transform.rotation = this.spline.GetOrientationOnSpline(param);
	}

	public Spline spline;

	public WrapMode wMode = WrapMode.Once;

	public Transform target;

	public int iterations = 5;

	public float diff = 0.5f;

	public float offset;
}
