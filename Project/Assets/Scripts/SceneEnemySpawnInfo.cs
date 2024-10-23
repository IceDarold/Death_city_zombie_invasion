using System;
using UnityEngine;
using Zombie3D;

[Serializable]
public class SceneEnemySpawnInfo : BaseEnemySpawnInfo
{
	public SceneEnemySpawnInfo()
	{
	}

	public SceneEnemySpawnInfo(Transform _trans, string name, int aniID)
	{
		this.Reset(_trans, name, aniID);
	}

	public void Reset(Transform _trans, string name, int id)
	{
		this.trans = _trans;
		this.aniID = id;
		this.type = Enemy.GetEnemyTypeByName(name);
	}

	public int aniID;

	public EnemyBornAction bornAction;

	public EnemyPatrolInfo patrolInfo = new EnemyPatrolInfo();

	public float wakeupRadius;
}
