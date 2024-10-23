using System;
using UnityEngine;
using Zombie3D;

[RequireComponent(typeof(BoxCollider))]
public class EnemySpawnCollider : MonoBehaviour
{
	public void SetEnemySpawnScript(IEnemySpawnTrigger script)
	{
		this._scrpt = script;
	}

	public void OnTriggerEnter(Collider other)
	{
		this._scrpt.DoTrigger();
	}

	protected IEnemySpawnTrigger _scrpt;
}
