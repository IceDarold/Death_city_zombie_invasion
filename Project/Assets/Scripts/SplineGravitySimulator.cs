using System;
using UnityEngine;

public class SplineGravitySimulator : MonoBehaviour
{
	private void Start()
	{
		base.GetComponent<Rigidbody>().useGravity = false;
	}

	private void FixedUpdate()
	{
		if (base.GetComponent<Rigidbody>() == null || this.spline == null)
		{
			return;
		}
		Vector3 positionOnSpline = this.spline.GetPositionOnSpline(this.spline.GetClosestPointParam(base.GetComponent<Rigidbody>().position, this.iterations, 0f, 1f, 0.01f));
		Vector3 a = positionOnSpline - base.GetComponent<Rigidbody>().position;
		Vector3 force = a * Mathf.Pow(a.magnitude, -3f) * this.gravityConstant * base.GetComponent<Rigidbody>().mass;
		base.GetComponent<Rigidbody>().AddForce(force);
	}

	public Spline spline;

	public float gravityConstant = 9.81f;

	public int iterations = 5;
}
