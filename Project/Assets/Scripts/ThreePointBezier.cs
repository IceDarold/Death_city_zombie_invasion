using System;
using UnityEngine;

public class ThreePointBezier
{
	public ThreePointBezier(Vector3 _p1, Vector3 _p2, Vector3 _p3)
	{
		this.p1 = _p1;
		this.p2 = _p2;
		this.p3 = _p3;
	}

	public Vector3 GetPointAtTime(float percent)
	{
		Vector3 a = this.p2 - this.p1;
		Vector3 a2 = this.p3 - this.p2;
		Vector3 vector = this.p1 + a * percent;
		Vector3 a3 = this.p2 + a2 * percent;
		return vector + (a3 - vector) * percent;
	}

	private Vector3 p1;

	private Vector3 p2;

	private Vector3 p3;
}
