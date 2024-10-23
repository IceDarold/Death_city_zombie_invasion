using System;
using System.Collections.Generic;
using UnityEngine;
using Zombie3D;

[Serializable]
public class BaseEnemySpawnInfo : BaseEnemySpawnData
{
	public Transform trans;

	public List<SnipePatrolPointInfo> snipePatrolInfos = new List<SnipePatrolPointInfo>();

	public float snipePatrolRadius = 3f;

	public bool keyEnemy;
}
