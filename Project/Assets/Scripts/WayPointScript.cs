using System;
using UnityEngine;

public class WayPointScript : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnDrawGizmos()
	{
		if (base.transform.position.y < 10005f)
		{
			Gizmos.color = Color.white;
		}
		else
		{
			Gizmos.color = Color.magenta;
		}
		Gizmos.DrawSphere(base.transform.position, 1f);
		foreach (WayPointScript wayPointScript in this.nodes)
		{
			Gizmos.DrawLine(base.transform.position, wayPointScript.transform.position);
		}
	}

	public WayPointScript[] nodes;

	public WayPointScript parent;
}
