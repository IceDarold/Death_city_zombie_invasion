using System;
using System.Collections.Generic;
using UnityEngine;
using Zombie3D;

[RequireComponent(typeof(EnemySpawnCollider))]
public class WaveEnemySpawnTrigger : BaseEnemySpawn, IEnemySpawnTrigger
{
	private void Awake()
	{
		this._colliderScript = base.GetComponent<EnemySpawnCollider>();
		this._colliderScript.SetEnemySpawnScript(this);
		foreach (WaveEnemySpawnInfo waveEnemySpawnInfo in this.infos)
		{
			if (Enemy.GetEnemyProbabailityByEnemyType(waveEnemySpawnInfo.type) != EnemyProbability.NORMAL)
			{
				this.ignorMaxEnemy = true;
				break;
			}
		}
	}

	public override void DoActive(int index)
	{
		this._colliderScript.gameObject.GetComponent<Collider>().enabled = (base.CanUse(index) && this.enableWhenStart);
		this.enemyManager = GameApp.GetInstance().GetGameScene().GetEnemyManager();
		this.enemyNum = this.infos.Count;
		this.gameScene = GameApp.GetInstance().GetGameScene();
	}

	public void DoTrigger()
	{
		this.enemyManager.EnableWaveTrigger(this);
		this.enemyManager.SetMaxEnemyInScene(this.maxEnemyInScene);
		this.spawnIndex = 0;
		this.spawnDuration = this.infos[this.spawnIndex].delay;
		base.gameObject.GetComponent<Collider>().enabled = false;
	}

	public void DoLogic(float dt)
	{
		if (this.allSpawned || this.gameScene.PlayingState != PlayingState.GamePlaying)
		{
			return;
		}
		if (this.isBreak)
		{
			return;
		}
		this.spawnDuration -= dt;
		if (this.spawnDuration <= 0f && (this.ignorMaxEnemy || !this.enemyManager.IsEnemyFull()))
		{
			this.CreateEnemy();
		}
	}

	public void CreateEnemy()
	{
		EnemyBornAction bAction = this.infos[this.spawnIndex].bAction;
		EnemyState state;
		Transform trans;
		if (bAction != EnemyBornAction.APPEAR)
		{
			if (bAction != EnemyBornAction.SNIPE_PATROL)
			{
				state = Enemy.CATCHING_STATE;
				trans = this.infos[this.spawnIndex].trans;
			}
			else
			{
				state = Enemy.SNIPE_PATROL;
				if (this.infos[this.spawnIndex].snipePatrolInfos.Count == 0 || (this.infos[this.spawnIndex].snipePatrolInfos.Count == 1 && this.infos[this.spawnIndex].snipePatrolInfos[0].isEscape))
				{
					trans = this.infos[this.spawnIndex].trans;
				}
				else
				{
					trans = this.infos[this.spawnIndex].snipePatrolInfos[0].transform;
				}
			}
		}
		else
		{
			state = Enemy.COMEOUT_STATE;
			trans = this.infos[this.spawnIndex].trans;
		}
		base.CreateEnemyPrefab(trans, this.infos[this.spawnIndex].type, state, this.infos[this.spawnIndex].startAniID, delegate(Enemy enemy)
		{
			enemy.SetDieCallBack(new Action<string>(this.SceneEnemyDie));
			this.spawnedNum++;
			this.enemyManager.DoWaveTriggerSpawnEnemy();
		}, delegate(Enemy _enemy, BaseEnemySpawnInfo _info)
		{
			if (state == Enemy.SNIPE_PATROL)
			{
				_enemy.SetSnipePatrolInfo(_info);
				_enemy.SetIsKeyEnemy(_info.keyEnemy);
			}
		}, this.infos[this.spawnIndex]);
		this.isBreak = this.infos[this.spawnIndex].isBreak;
		this.spawnIndex++;
		if (this.spawnIndex == this.infos.Count)
		{
			this.allSpawned = true;
			this.enemyManager.DisableWaveTrigger(this);
			return;
		}
		this.spawnDuration = this.infos[this.spawnIndex].delay;
	}

	private void SceneEnemyDie(string enemyName)
	{
		this.enemyManager.DoWaveTriggerEnemyDie();
		if (--this.spawnedNum == 0 && this.isBreak)
		{
			this.isBreak = false;
		}
		if (--this.enemyNum == 0 && this.OnAllEnemyDie != null)
		{
			this.OnAllEnemyDie();
		}
	}

	public void DoStop()
	{
		this.spawnIndex = this.infos.Count;
		this.allSpawned = true;
	}

	public List<WaveEnemySpawnInfo> infos = new List<WaveEnemySpawnInfo>();

	public int maxEnemyInScene = 15;

	private bool allSpawned;

	private int spawnIndex;

	protected EnemySpawnManager enemyManager;

	protected float spawnDuration;

	protected GameScene gameScene;

	protected bool isBreak;

	protected int spawnedNum;

	protected bool ignorMaxEnemy;
}
