using System;
using UnityEngine;

public class SplineNode : MonoBehaviour
{
	public Transform Transform
	{
		get
		{
			return base.transform;
		}
	}

	public Vector3 Position
	{
		get
		{
			return this.Transform.position;
		}
		set
		{
			this.Transform.position = value;
		}
	}

	public Quaternion Rotation
	{
		get
		{
			return this.Transform.rotation;
		}
		set
		{
			this.Transform.rotation = value;
		}
	}

	public float PosInSpline
	{
		get
		{
			return (float)this.posInSpline;
		}
	}

	public float Length
	{
		get
		{
			return (float)this.length;
		}
	}

	public void Reset()
	{
		this.posInSpline = 0.0;
		this.length = 0.0;
	}

	public double posInSpline;

	public double length;

	public float customValue;

	public Spline spline;
}
