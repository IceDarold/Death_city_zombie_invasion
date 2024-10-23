using System;
using UnityEngine;
using Zombie3D;

[Serializable]
public class WaveEnemySpawnInfo : BaseEnemySpawnInfo
{
	public WaveEnemySpawnInfo()
	{
	}

	public WaveEnemySpawnInfo(Transform _trans, string name, EnemyBornAction action, float _delay)
	{
		this.Reset(_trans, name, action, _delay);
	}

	public void Reset(Transform _trans, string name, EnemyBornAction action, float _delay)
	{
		this.trans = _trans;
		this.bAction = action;
		this.delay = _delay;
		this.type = Enemy.GetEnemyTypeByName(name);
	}

	public float delay;

	public bool isBreak;
}
