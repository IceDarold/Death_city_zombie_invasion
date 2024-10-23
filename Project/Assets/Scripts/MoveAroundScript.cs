using System;
using UnityEngine;

public class MoveAroundScript : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if ((double)(base.transform.position - this.pointA.position).sqrMagnitude < 1.0)
		{
			this.target = this.pointB;
		}
		else if ((double)(base.transform.position - this.pointB.position).sqrMagnitude < 1.0)
		{
			this.target = this.pointA;
		}
		Vector3 normalized = (this.target.position - base.transform.position).normalized;
		base.transform.Translate(normalized * Time.deltaTime * 10f);
	}

	public Transform pointA;

	public Transform pointB;

	public Transform target;
}
