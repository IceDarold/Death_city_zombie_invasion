using System;
using UnityEngine;
using Zombie3D;

public class BaseEnemySpawn : SceneBindAble
{
	public void CreateEnemyPrefab(Transform trans, EnemyType type, EnemyState state, int startAniID, Action<Enemy> afterCreate, Action<Enemy, BaseEnemySpawnInfo> beforeInit = null, BaseEnemySpawnInfo _info = null)
	{
		EnemySpawnType spawnType = Enemy.GetSpawnType(type);
		GameApp.GetInstance().GetGameScene().GetEnemyPool(spawnType.eType).CreateObject(trans.position, trans.rotation, delegate(GameObject enemyObject)
		{
			this.OnEnemyCreated(enemyObject, spawnType, trans, type, state, startAniID, afterCreate, beforeInit, _info);
		});
	}

	private void OnEnemyCreated(GameObject curEnemy, EnemySpawnType spawnType, Transform trans, EnemyType type, EnemyState state, int startAniID, Action<Enemy> afterCreate, Action<Enemy, BaseEnemySpawnInfo> beforeInit, BaseEnemySpawnInfo _info)
	{
		curEnemy.name = "E_" + GameApp.GetInstance().GetGameScene().GetNextEnemyID().ToString();
		Enemy enemy = BaseEnemySpawn.GetEnemy(spawnType.eType);
		if (beforeInit != null)
		{
			beforeInit(enemy, _info);
		}
		enemy.EnemyType = spawnType.eType;
		enemy.Init(curEnemy, state, startAniID, spawnType.isArmored, spawnType.isArmed, spawnType.isRedEye);
		enemy.Name = curEnemy.name;
		enemy.SetTarget(GameApp.GetInstance().GetGameScene().CheckTarget(trans.position, enemy.AttackRange));
		if (afterCreate != null)
		{
			afterCreate(enemy);
		}
		GameApp.GetInstance().GetGameScene().GetEnemies().Add(enemy.Name, enemy);
	}

	public static Enemy GetEnemy(EnemyType type)
	{
		Enemy result = null;
		switch (type)
		{
		case EnemyType.E_TSHIRT:
		case EnemyType.E_FAT1:
		case EnemyType.E_FAT2:
		case EnemyType.E_MAN1:
		case EnemyType.E_MAN2:
		case EnemyType.E_WOMAN1:
		case EnemyType.E_WOMAN2:
		case EnemyType.E_WOMAN3:
		case EnemyType.E_WOMAN4:
			result = new NormalEnemy();
			break;
		case EnemyType.E_SPITTER:
			result = new Spitter();
			break;
		case EnemyType.E_BUTCHER:
			result = new Butcher();
			break;
		case EnemyType.E_BOMBER:
			result = new Boomer();
			break;
		case EnemyType.E_DESPOT:
			result = new Despot();
			break;
		case EnemyType.E_BOSS_FAT:
			result = new BossBoomer();
			break;
		case EnemyType.E_BOSS_BUTCHER:
			result = new BossButcher();
			break;
		case EnemyType.E_BOMBER2:
			result = new Boomer2();
			break;
		}
		return result;
	}

	public virtual EnemyState BornAction2EnemyState(EnemyBornAction bornAction)
	{
		EnemyState result;
		switch (bornAction)
		{
		case EnemyBornAction.WALK:
			result = Enemy.CATCHING_STATE;
			break;
		case EnemyBornAction.APPEAR:
			result = Enemy.COMEOUT_STATE;
			break;
		case EnemyBornAction.WAIT:
			result = Enemy.WAIT_STATE;
			break;
		case EnemyBornAction.PATROL:
			result = Enemy.PATROL_STATE;
			break;
		default:
			result = Enemy.CATCHING_STATE;
			break;
		}
		return result;
	}

	public new virtual void DoActive(int index)
	{
		base.gameObject.SetActive(base.CanUse(index));
	}

	public void SetOnAllEnemyDie(Action action)
	{
		this.OnAllEnemyDie = action;
	}

	public const string ResourceConfig = "ResourceConfig";

	public OnAllEnemyDie allEnemyDie;

	public Action OnEnemyDie;

	public Action OnAllEnemyDie;

	public EnemySpawnCollider _colliderScript;

	[HideInInspector]
	public bool enableWhenStart = true;

	protected int enemyNum;
}
