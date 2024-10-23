using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombie3D;

public class EnemySpawnManager : BaseEnemySpawn
{
	private IEnumerator Start()
	{
		yield return null;
		this.maxEnemyInScene = 0;
		this.gameCamera = GameApp.GetInstance().GetGameScene().GetCamera();
		this.gameScene = GameApp.GetInstance().GetGameScene();
		yield break;
	}

	public void EnablePoint(SpawnPointInfo info)
	{
		this.allPoints.Add(info.pt_name, info);
	}

	public void DisablePoint(string pointName)
	{
		this.allPoints[pointName].collider.enabled = false;
		this.allPoints.Remove(pointName);
		if (this.activePoints.ContainsKey(pointName))
		{
			this.activePoints.Remove(pointName);
		}
	}

	public void EnableWaveTrigger(IEnemySpawnTrigger trigger)
	{
		this.waveTriggers.Add(trigger);
	}

	public void DisableWaveTrigger(IEnemySpawnTrigger trigger)
	{
		this.waveTriggers.Remove(trigger);
	}

	public void ActiveSpwanTrigger(AutoEnemySpawnTrigger trigger)
	{
		if (trigger.closeOtherTriggers)
		{
			for (int i = 0; i < this.activeTriggers.Count; i++)
			{
				this.activeTriggers[i].DoInactive(false);
			}
			this.activeTriggers.Clear();
		}
		this.activeTriggers.Add(trigger);
		this.SetMaxEnemyInScene(trigger.maxEnemyInScene);
		this.maxCreateEnemy = trigger.totalCreateNum;
		this.enemySpawnData = trigger.allEnemyInfos;
		this.enemySpawnWi.Clear();
		for (int j = 0; j < this.enemySpawnData.Count; j++)
		{
			if (j == 0)
			{
				this.enemySpawnWi.Add(this.enemySpawnData[j].wi);
			}
			else
			{
				this.enemySpawnWi.Add(this.enemySpawnWi[j - 1] + this.enemySpawnData[j].wi);
			}
		}
		if (trigger.firstWaveNum > 0)
		{
			base.StartCoroutine(this.CreateFirstWaveDelay(0.1f, trigger.firstWaveNum));
		}
	}

	public void InactiveAutoSpawnTrigger(AutoEnemySpawnTrigger trigger)
	{
		if (this.activeTriggers.Contains(trigger))
		{
			this.activeTriggers.Remove(trigger);
		}
	}

	private IEnumerator CreateFirstWaveDelay(float delay, int num)
	{
		yield return new WaitForSeconds(delay);
		this.CreateEnemy(num);
		yield break;
	}

	public void DoLogic(float dt)
	{
		if (this.gameScene != null && this.gameScene.PlayingState != PlayingState.GamePlaying)
		{
			return;
		}
		for (int i = 0; i < this.waveTriggers.Count; i++)
		{
			this.waveTriggers[i].DoLogic(dt);
		}
		if (this.activeTriggers.Count > 0)
		{
			this.activeTriggers[0].DoLogic(dt);
		}
	}

	public void SetMaxEnemyInScene(int num)
	{
		this.maxEnemyInScene = ((num <= this.maxEnemyInScene) ? this.maxEnemyInScene : num);
	}

	public void SetMaxCreateEnemy(int num)
	{
	}

	public bool IsEnemyFull()
	{
		return this.spawnedEnemy >= this.maxEnemyInScene;
	}

	public void DoWaveTriggerSpawnEnemy()
	{
		this.spawnedEnemy++;
	}

	public void DoWaveTriggerEnemyDie()
	{
		this.spawnedEnemy--;
	}

	public void RemoveAllEnemySpawnInScene()
	{
		foreach (IEnemySpawnTrigger enemySpawnTrigger in this.waveTriggers)
		{
			enemySpawnTrigger.DoStop();
		}
		for (int i = this.activeTriggers.Count - 1; i >= 0; i--)
		{
			this.activeTriggers[i].DoStop();
		}
	}

	public void CreateEnemy()
	{
		if (this.spawnedEnemy >= this.maxEnemyInScene)
		{
			return;
		}
		string[] array = new string[this.activePoints.Count];
		this.activePoints.Keys.CopyTo(array, 0);
		List<string> list = new List<string>();
		int i = 0;
		int num = array.Length;
		while (i < num)
		{
			if (this.activePoints[array[i]].alwaysSpawn || !this.gameCamera.IsInViewport(this.activePoints[array[i]].trans.position))
			{
				list.Add(array[i]);
			}
			i++;
		}
		if (list.Count == 0)
		{
			return;
		}
		int index = UnityEngine.Random.Range(0, list.Count);
		BaseEnemySpawnData spawnEnemyData = this.GetSpawnEnemyData();
		if (!this.allPoints[list[index]].enableElite && this.CheckEnemyTypeElite(spawnEnemyData.type))
		{
			spawnEnemyData.type = ((UnityEngine.Random.Range(0f, 1f) < 0.5f) ? EnemyType.E_MAN1 : EnemyType.E_WOMAN1);
		}
		base.CreateEnemyPrefab(this.allPoints[list[index]].trans, spawnEnemyData.type, Enemy.CATCHING_STATE, 0, delegate(Enemy enemy)
		{
			enemy.SetDieCallBack(new Action<string>(this.OnEnemyDie));
		}, null, null);
		this.spawnedEnemy++;
		this.maxCreateEnemy--;
		if (this.maxCreateEnemy == 0)
		{
			this.activeTriggers[0].DoInactive(false);
		}
	}

	private bool CheckEnemyTypeElite(EnemyType eType)
	{
		return eType == EnemyType.E_BOMBER || eType == EnemyType.E_BUTCHER || eType == EnemyType.E_DESPOT || eType == EnemyType.E_BOSS_BUTCHER || eType == EnemyType.E_BOSS_FAT || eType == EnemyType.E_SPITTER;
	}

	public void CreateEnemy(int num)
	{
		string[] array = new string[this.activePoints.Count];
		this.activePoints.Keys.CopyTo(array, 0);
		List<string> list = new List<string>();
		int i = 0;
		int num2 = array.Length;
		while (i < num2)
		{
			if (this.activePoints[array[i]].alwaysSpawn || !this.gameCamera.IsInViewport(this.activePoints[array[i]].trans.position))
			{
				list.Add(array[i]);
			}
			i++;
		}
		if (list.Count == 0)
		{
			return;
		}
		List<string> createPoint = new List<string>();
		for (int j = num; j > 0; j--)
		{
			if (createPoint.Count == 0)
			{
				list.ForEach(delegate(string _key)
				{
					createPoint.Add(_key);
				});
			}
			int index = UnityEngine.Random.Range(0, createPoint.Count);
			BaseEnemySpawnData spawnEnemyData = this.GetSpawnEnemyData();
			if (!this.allPoints[list[index]].enableElite && this.CheckEnemyTypeElite(spawnEnemyData.type))
			{
				spawnEnemyData.type = ((UnityEngine.Random.Range(0f, 1f) < 0.5f) ? EnemyType.E_MAN1 : EnemyType.E_WOMAN1);
			}
			base.CreateEnemyPrefab(this.allPoints[createPoint[index]].trans, spawnEnemyData.type, Enemy.CATCHING_STATE, 0, delegate(Enemy enemy)
			{
				enemy.SetDieCallBack(new Action<string>(this.OnEnemyDie));
			}, null, null);
			createPoint.RemoveAt(index);
			this.spawnedEnemy++;
			this.maxCreateEnemy--;
			if (this.maxCreateEnemy == 0 || this.spawnedEnemy >= this.maxEnemyInScene)
			{
				break;
			}
		}
		if (this.maxCreateEnemy == 0)
		{
			this.activeTriggers[0].DoInactive(false);
		}
	}

	private BaseEnemySpawnData GetSpawnEnemyData()
	{
		float num = UnityEngine.Random.Range(0f, this.enemySpawnWi[this.enemySpawnWi.Count - 1]);
		for (int i = 0; i < this.enemySpawnWi.Count; i++)
		{
			if (num < this.enemySpawnWi[i])
			{
				return this.enemySpawnData[i];
			}
		}
		UnityEngine.Debug.LogError("Find AutoSpawn By Wi Error!");
		return this.enemySpawnData[0];
	}

	public new void OnEnemyDie(string enemyName)
	{
		this.spawnedEnemy--;
	}

	public void OnTriggerEnter(Collider other)
	{
		string name = other.gameObject.name;
		if (!this.activePoints.ContainsKey(name))
		{
			this.activePoints.Add(name, this.allPoints[name]);
		}
	}

	public void OnTriggerExit(Collider other)
	{
		string name = other.gameObject.name;
		if (this.activePoints.ContainsKey(name))
		{
			this.activePoints.Remove(name);
		}
	}

	public Dictionary<string, SpawnPointInfo> activePoints = new Dictionary<string, SpawnPointInfo>();

	public Dictionary<string, SpawnPointInfo> allPoints = new Dictionary<string, SpawnPointInfo>();

	public List<AutoEnemySpawnTrigger> activeTriggers = new List<AutoEnemySpawnTrigger>();

	[CNName("同时存在的最大数")]
	public int maxEnemyInScene;

	[CNName("刷怪最大数")]
	public int maxCreateEnemy = int.MaxValue;

	protected float curDuration;

	protected int spawnedEnemy;

	protected List<IEnemySpawnTrigger> waveTriggers = new List<IEnemySpawnTrigger>();

	protected BaseCameraScript gameCamera;

	protected GameScene gameScene;

	protected List<BaseEnemySpawnData> enemySpawnData;

	protected List<float> enemySpawnWi = new List<float>();
}
