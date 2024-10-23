using System;
using SplineUtilities;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class SplineAnimatorCustomValue : MonoBehaviour
{
	private void Update()
	{
		this.passedTime += Time.deltaTime * this.speed;
		float param = SplineUtils.WrapValue(this.passedTime + this.offSet, 0f, 1f, this.wrapMode);
		float customValueOnSpline = this.spline.GetCustomValueOnSpline(param);
		base.transform.position = this.spline.GetPositionOnSpline(param);
		base.transform.rotation = this.spline.GetOrientationOnSpline(param);
		base.GetComponent<Renderer>().material.color = Color.red * (1f - customValueOnSpline) + Color.blue * customValueOnSpline;
	}

	public Spline spline;

	public WrapMode wrapMode = WrapMode.Once;

	public float speed = 1f;

	public float offSet;

	public float passedTime;
}
