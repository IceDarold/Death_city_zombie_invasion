using System;
using System.Collections.Generic;
using UnityEngine;
using Zombie3D;

public class AutoEnemySpawnTrigger : BaseEnemySpawn, IEnemySpawnTrigger
{
	private void Awake()
	{
		for (int i = 0; i < this.allColliders.Count; i++)
		{
			this.allColliders[i].SetEnemySpawnScript(this);
			this.allColliders[i].gameObject.GetComponent<Collider>().enabled = false;
		}
	}

	public override void DoActive(int index)
	{
		for (int i = 0; i < this.allColliders.Count; i++)
		{
			this.allColliders[i].gameObject.GetComponent<Collider>().enabled = (base.CanUse(index) && this.enableWhenStart);
		}
		this.enemyManager = GameApp.GetInstance().GetGameScene().GetEnemyManager();
	}

	public void DoTrigger()
	{
		if (this.isActived)
		{
			return;
		}
		if (this.gameCamera == null || this.gameScene == null)
		{
			this.gameCamera = GameApp.GetInstance().GetGameScene().GetCamera();
			this.gameScene = GameApp.GetInstance().GetGameScene();
		}
		this.isActived = true;
		this.enemyManager = GameApp.GetInstance().GetGameScene().GetEnemyManager();
		for (int i = 0; i < this.allColliders.Count; i++)
		{
			this.allColliders[i].gameObject.GetComponent<Collider>().enabled = false;
		}
		int j = 0;
		int count = this.allPoints.Count;
		while (j < count)
		{
			this.enemyManager.EnablePoint(this.allPoints[j]);
			if (!this.activePoints.Contains(this.allPoints[j].pt_name))
			{
				this.activePoints.Add(this.allPoints[j].pt_name);
			}
			this.allPoints[j].collider.enabled = true;
			j++;
		}
		this.spawnIntervalCount = this.spawnInterval;
		this.spawnLastTimeCount = this.spawnLastTime;
		this.spawnColdDownCount = 0f;
		this.enemyManager.ActiveSpwanTrigger(this);
	}

	public void DoLogic(float deltaTime)
	{
		this.spawnColdDownCount -= deltaTime;
		if (this.spawnColdDownCount > 0f)
		{
			return;
		}
		this.spawnLastTimeCount = ((this.spawnLastTimeCount >= 0f) ? this.spawnLastTimeCount : this.spawnLastTime);
		this.spawnLastTimeCount -= deltaTime;
		if (this.spawnLastTimeCount < 0f)
		{
			this.spawnColdDownCount = this.spawnColdDown;
		}
		this.spawnIntervalCount -= deltaTime;
		if (this.spawnIntervalCount <= 0f)
		{
			this.spawnIntervalCount = this.spawnInterval;
			this.enemyManager.CreateEnemy();
		}
	}

	public void DoInactive(bool ignorAlwaysCanTrigger = false)
	{
		int i = 0;
		int count = this.activePoints.Count;
		while (i < count)
		{
			this.enemyManager.DisablePoint(this.activePoints[i]);
			i++;
		}
		this.isActived = false;
		for (int j = 0; j < this.allColliders.Count; j++)
		{
			this.allColliders[j].gameObject.GetComponent<Collider>().enabled = (!ignorAlwaysCanTrigger && this.alwaysCanTrigger);
		}
		this.enemyManager.InactiveAutoSpawnTrigger(this);
	}

	public void DoStop()
	{
		this.DoInactive(true);
	}

	[HideInInspector]
	public List<SpawnPointInfo> allPoints = new List<SpawnPointInfo>();

	[HideInInspector]
	public List<EnemySpawnCollider> allColliders = new List<EnemySpawnCollider>();

	[HideInInspector]
	public List<string> activePoints = new List<string>();

	[HideInInspector]
	public bool closeOtherTriggers = true;

	[HideInInspector]
	public List<BaseEnemySpawnData> allEnemyInfos = new List<BaseEnemySpawnData>();

	public int maxEnemyInScene = 15;

	public int totalCreateNum = 100;

	public int firstWaveNum;

	public float spawnInterval = 3f;

	public float spawnLastTime = 9999f;

	public float spawnColdDown;

	public bool alwaysCanTrigger = true;

	protected Camera mainCam;

	protected EnemySpawnManager enemyManager;

	protected bool isActived;

	protected BaseCameraScript gameCamera;

	protected GameScene gameScene;

	protected float spawnIntervalCount;

	protected float spawnLastTimeCount;

	protected float spawnColdDownCount;
}
