using System;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaRay : MonoBehaviour
{
	private void Start()
	{
		for (int i = 0; i < this.density; i++)
		{
			this.points.Add(UnityEngine.Object.Instantiate<GameObject>(this.sphere));
		}
		this.line.SetVertexCount(this.density + 1);
		this.prevPoint = base.transform.position;
	}

	private void Update()
	{
		Vector3 forward = base.transform.forward;
		forward.y = 0f;
		this.k = base.transform.forward.y / forward.magnitude;
		this.b = this.k;
		bool flag = false;
		for (int i = 0; i < this.density; i++)
		{
			Vector3 position = this.GetPosition((float)i * this.space);
			this.points[i].transform.position = position;
			flag = this.Cast(position);
			if (flag)
			{
				break;
			}
		}
		if (!flag)
		{
		}
	}

	private Vector3 GetPosition(float z)
	{
		float num = this.a * z * z + this.b * z;
		Vector3 vector = base.transform.forward;
		vector.y = 0f;
		vector = vector.normalized;
		Vector3 result = base.transform.position + vector * z;
		result.y = base.transform.position.y + num;
		return result;
	}

	private bool Cast(Vector3 currentPoint)
	{
		Vector3 vector = currentPoint - this.prevPoint;
		RaycastHit raycastHit;
		if (Physics.Raycast(this.prevPoint, vector.normalized, out raycastHit, vector.magnitude))
		{
			this.SetLine(raycastHit.point);
			this.prevPoint = base.transform.position;
			return true;
		}
		this.prevPoint = currentPoint;
		return false;
	}

	private void SetLine(Vector3 endPos)
	{
		Vector3 position = base.transform.position;
		endPos.y = 0f;
		position.y = 0f;
		float num = Vector3.Distance(position, endPos) / (float)this.density;
		for (int i = 0; i < this.density; i++)
		{
			this.line.SetPosition(i, this.GetPosition((float)i * num));
		}
		this.line.SetPosition(this.density, endPos);
		this.sphere.transform.position = endPos;
	}

	public float a;

	private float k;

	private float b;

	public LineRenderer line;

	public int density;

	public float space = 5f;

	public GameObject sphere;

	private List<GameObject> points = new List<GameObject>();

	private Vector3 prevPoint;
}
