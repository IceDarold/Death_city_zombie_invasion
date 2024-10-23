using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class EnemyPatrolInfo
{
	public void Init(float _patrolRadius, Vector3 _patrolCenter, List<PatrolPathPoint> _patrolPath)
	{
		this.patrolRadius = _patrolRadius;
		this.patrolPath = _patrolPath;
	}

	public void SetPatrolCenter(Vector3 _patrolCenter)
	{
		if (this.patrolCenter != Vector3.zero)
		{
			return;
		}
		this.patrolCenter = _patrolCenter;
		this.ResetPatrolRestTime();
	}

	public void ResetPatrolRestTime()
	{
		if (this.patrolInPath)
		{
			if (this.patrolPath[this.pathIndex].resetTime > 0f)
			{
				this.patrolRestTime = this.patrolPath[this.pathIndex].resetTime;
			}
			this.pathIndex = ((++this.pathIndex < this.patrolPath.Count) ? this.pathIndex : 0);
			this.patrolTarget = this.patrolPath[this.pathIndex].trans.position;
			this.findPatrolTargetInRest = true;
		}
		else
		{
			this.patrolRestTime = UnityEngine.Random.Range(1f, 6f);
			this.findPatrolTargetInRest = false;
		}
	}

	public bool DoFindPatrolPoint(Vector3 currentPos)
	{
		Vector3 sourcePosition = this.patrolCenter + UnityEngine.Random.insideUnitSphere * this.patrolRadius;
		NavMeshHit navMeshHit;
		if (!NavMesh.SamplePosition(sourcePosition, out navMeshHit, this.patrolRadius, 1))
		{
			return false;
		}
		Vector3 position = navMeshHit.position;
		float sqrMagnitude = (position - currentPos).sqrMagnitude;
		float num = this.patrolRadius * 0.3333f * this.patrolRadius * 0.33333f;
		if (sqrMagnitude < num)
		{
			return false;
		}
		this.patrolTarget = position;
		return true;
	}

	public bool InRest(float deltaTime, Vector3 pos)
	{
		if (this.patrolRestTime > 0f)
		{
			this.CaculatePatrolRestTime(deltaTime, pos);
			return true;
		}
		return false;
	}

	private void CaculatePatrolRestTime(float deltaTime, Vector3 pos)
	{
		if (this.findPatrolTargetInRest)
		{
			this.patrolRestTime -= deltaTime;
		}
		else
		{
			this.findPatrolTargetInRest = this.DoFindPatrolPoint(pos);
		}
	}

	public Vector3 patrolCenter = Vector3.zero;

	public Vector3 patrolTarget;

	public float patrolRadius = 3f;

	public float patrolSpeed;

	public float patrolRestTime;

	public bool findPatrolTargetInRest;

	public List<PatrolPathPoint> patrolPath = new List<PatrolPathPoint>();

	public bool patrolInPath;

	protected int pathIndex;
}
