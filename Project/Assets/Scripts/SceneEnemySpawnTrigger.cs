using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombie3D;

[RequireComponent(typeof(EnemySpawnCollider))]
public class SceneEnemySpawnTrigger : BaseEnemySpawn, IEnemySpawnTrigger
{
	private void Awake()
	{
		this._colliderScript = base.GetComponent<EnemySpawnCollider>();
		this._colliderScript.SetEnemySpawnScript(this);
	}

	public override void DoActive(int index)
	{
		bool flag = base.CanUse(index);
		this._colliderScript.gameObject.GetComponent<Collider>().enabled = flag;
		if (!flag)
		{
			return;
		}
		this.callToSpawn = true;
		if (!this.spawnWhenActive)
		{
			base.gameObject.SetActive(false);
			return;
		}
		this.CreateAllEnemy();
	}

	public void OnEnable()
	{
		if (this.callToSpawn && !this.spawnWhenActive)
		{
			this.CreateAllEnemy();
		}
	}

	private void CreateAllEnemy()
	{
		this.enemyNum = this.infos.Count;
		for (int i = 0; i < this.infos.Count; i++)
		{
			EnemyState action = this.BornAction2EnemyState(this.infos[i].bornAction);
			Transform trans = (action != Enemy.PATROL_STATE || !this.infos[i].patrolInfo.patrolInPath) ? this.infos[i].trans : this.infos[i].patrolInfo.patrolPath[0].trans;
			if (action == Enemy.SNIPE_PATROL)
			{
				if (this.infos[i].snipePatrolInfos.Count == 0 || (this.infos[i].snipePatrolInfos.Count == 1 && this.infos[i].snipePatrolInfos[0].isEscape))
				{
					trans = this.infos[i].trans;
				}
				else
				{
					trans = this.infos[i].snipePatrolInfos[0].transform;
				}
			}
			float wakeupRadius = this.infos[i].wakeupRadius;
			base.CreateEnemyPrefab(trans, this.infos[i].type, action, this.infos[i].startAniID, delegate(Enemy _enemy)
			{
				GameApp.GetInstance().GetGameScene().CreateSceneSpawnedEnemy(_enemy);
				_enemy.SetDieCallBack(new Action<string>(this.SceneEnemyDie));
				_enemy.SetWakeupCallback(new Action(this.DoTrigger));
				_enemy.SetSnipePatrolOnHitCallback(new Action<Enemy>(this.DoAmazeSpawnGroup));
				this.allEnemys.Add(_enemy);
				_enemy.WakeUpRange = wakeupRadius;
				this.StartCoroutine(this.CheckNeedWakeUp(_enemy));
			}, delegate(Enemy _enemy, BaseEnemySpawnInfo _info)
			{
				SceneEnemySpawnInfo sceneEnemySpawnInfo = _info as SceneEnemySpawnInfo;
				if (action == Enemy.PATROL_STATE)
				{
					_enemy.SetPatrolInfo(sceneEnemySpawnInfo.patrolInfo);
				}
				else if (action == Enemy.SNIPE_PATROL)
				{
					_enemy.SetSnipePatrolInfo(_info);
				}
				_enemy.SetIsKeyEnemy(_info.keyEnemy);
			}, this.infos[i]);
		}
	}

	private IEnumerator CheckNeedWakeUp(Enemy enemy)
	{
		yield return new WaitForSeconds(0.1f);
		if (this.isActived)
		{
			enemy.WakeUp();
		}
		yield break;
	}

	private void SceneEnemyDie(string enemyName)
	{
		Enemy enemyByID = GameApp.GetInstance().GetGameScene().GetEnemyByID(enemyName);
		GameApp.GetInstance().GetGameScene().DeleteSceneSpawnedEnemy(enemyByID);
		if (--this.enemyNum == 0 && this.OnAllEnemyDie != null)
		{
			this.OnAllEnemyDie();
		}
	}

	public void DoTrigger()
	{
		if (this.isActived)
		{
			return;
		}
		this.isActived = true;
		base.GetComponent<Collider>().enabled = false;
		for (int i = 0; i < this.allEnemys.Count; i++)
		{
			if (this.allEnemys[i].GetState() == Enemy.WAIT_STATE || this.allEnemys[i].GetState() == Enemy.PATROL_STATE)
			{
				this.allEnemys[i].WakeUp();
			}
		}
	}

	public void DoAmazeSpawnGroup(Enemy enemy)
	{
		for (int i = 0; i < this.allEnemys.Count; i++)
		{
			if (this.allEnemys[i] != enemy)
			{
				if (this.allEnemys[i].GetState() == Enemy.SNIPE_PATROL)
				{
					this.allEnemys[i].DoChangeToEscapeAction();
					this.allEnemys[i].DoChangeSnipePatrolAction();
				}
			}
		}
	}

	public void DoLogic(float dt)
	{
	}

	public void DoStop()
	{
	}

	public override EnemyState BornAction2EnemyState(EnemyBornAction bornAction)
	{
		EnemyState result = Enemy.WAIT_STATE;
		if (bornAction != EnemyBornAction.PATROL)
		{
			if (bornAction == EnemyBornAction.SNIPE_PATROL)
			{
				result = Enemy.SNIPE_PATROL;
			}
		}
		else
		{
			result = Enemy.PATROL_STATE;
		}
		return result;
	}

	public List<SceneEnemySpawnInfo> infos = new List<SceneEnemySpawnInfo>();

	[HideInInspector]
	public bool spawnWhenActive = true;

	protected List<Enemy> allEnemys = new List<Enemy>();

	protected bool isActived;

	protected bool callToSpawn;
}
