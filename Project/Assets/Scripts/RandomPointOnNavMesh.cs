using System;
using UnityEngine;
using UnityEngine.AI;

public class RandomPointOnNavMesh : MonoBehaviour
{
	private bool RandomPoint(Vector3 center, float range, out Vector3 result)
	{
		for (int i = 0; i < 30; i++)
		{
			Vector3 sourcePosition = center + UnityEngine.Random.insideUnitSphere * range;
			NavMeshHit navMeshHit;
			if (NavMesh.SamplePosition(sourcePosition, out navMeshHit, 1f, -1))
			{
				result = navMeshHit.position;
				return true;
			}
		}
		result = Vector3.zero;
		return false;
	}

	private void Update()
	{
		Vector3 start;
		if (this.RandomPoint(base.transform.position, this.range, out start))
		{
			UnityEngine.Debug.DrawRay(start, Vector3.up, Color.blue, 1f);
		}
	}

	public float range = 10f;
}
