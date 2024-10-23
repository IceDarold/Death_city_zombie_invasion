using System;
using System.Collections;
using UnityEngine;
using Zombie3D;

public class TriggerScript : MonoBehaviour
{
	private IEnumerator Start()
	{
		yield return 0;
		this.triggerTransform = base.gameObject.transform;
		this.triggered = false;
		foreach (EnemySpawnScript enemySpawnScript in this.spawns)
		{
			if (enemySpawnScript != null)
			{
				enemySpawnScript.TriggerBelongsto = this;
			}
		}
		this.hasSecondarySpawns = false;
		foreach (EnemySpawnScript enemySpawnScript2 in this.secondarySpawns)
		{
			if (enemySpawnScript2 != null)
			{
				enemySpawnScript2.TriggerBelongsto = this;
				this.hasSecondarySpawns = true;
			}
		}
		this.alreadyMaxSpawned = false;
		this.gameScene = GameApp.GetInstance().GetGameScene();
		yield break;
	}

	public bool AlreadyMaxSpawned
	{
		get
		{
			return this.alreadyMaxSpawned;
		}
	}

	private void Update()
	{
		if (Time.time - this.lastUpdateTime < 1f || this.triggerTransform == null)
		{
			return;
		}
		this.lastUpdateTime = Time.time;
		if (!this.triggered)
		{
			this.player = this.gameScene.GetPlayer();
			bool flag = false;
			if (this.SecondPosition != null && (this.player.GetTransform().position - this.SecondPosition.position).sqrMagnitude <= this.radius * this.radius)
			{
				flag = true;
			}
			if ((this.player.GetTransform().position - this.triggerTransform.position).sqrMagnitude <= this.radius * this.radius || flag)
			{
				foreach (EnemySpawnScript enemySpawnScript in this.spawns)
				{
					if (enemySpawnScript != null)
					{
						enemySpawnScript.Spawn(1);
						this.currentEnemyNum++;
						this.spawnedNum++;
					}
				}
				this.triggered = true;
			}
		}
		else if (this.spawnedNum < this.maxSpawn && this.hasSecondarySpawns)
		{
			if (this.currentEnemyNum <= this.minEnemy)
			{
				foreach (EnemySpawnScript enemySpawnScript2 in this.secondarySpawns)
				{
					if (enemySpawnScript2 != null)
					{
						enemySpawnScript2.Spawn(1);
						this.currentEnemyNum++;
						this.spawnedNum++;
					}
				}
			}
		}
		else
		{
			this.alreadyMaxSpawned = true;
		}
	}

	public void EnemyKilled()
	{
		this.currentEnemyNum--;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(base.transform.position, 0.3f);
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(base.transform.position, this.radius);
		if (this.SecondPosition != null)
		{
			Gizmos.DrawWireSphere(this.SecondPosition.position, this.radius);
		}
		if (this.gameScene != null && this.gameScene.GetEnemies() != null)
		{
			IEnumerator enumerator = this.gameScene.GetEnemies().Values.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Enemy enemy = (Enemy)obj;
					if (enemy.LastTarget != Vector3.zero)
					{
						Gizmos.color = Color.blue;
						Gizmos.DrawSphere(enemy.LastTarget, 0.3f);
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}
	}

	protected Transform triggerTransform;

	protected Player player;

	protected bool triggered;

	protected bool alreadyMaxSpawned;

	protected bool hasSecondarySpawns;

	public Transform SecondPosition;

	public int minEnemy;

	public int maxSpawn;

	protected int currentEnemyNum;

	protected int spawnedNum;

	public float radius;

	public EnemySpawnScript[] spawns = new EnemySpawnScript[5];

	public EnemySpawnScript[] secondarySpawns = new EnemySpawnScript[5];

	protected GameScene gameScene;

	protected float lastUpdateTime = -1000f;
}
