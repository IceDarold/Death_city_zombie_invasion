using System;
using UnityEngine;

public class CNNameAttribute : PropertyAttribute
{
	public CNNameAttribute(string label = "")
	{
		this.label = label;
	}

	public CNNameAttribute(float min, float max, string label = "")
	{
		this.min = min;
		this.max = max;
		this.label = label;
	}

	public float min = -1f;

	public float max = -1f;

	public string label;
}
