using System;
using System.Collections;
using UnityEngine;
using Zombie3D;

public class ArenaTriggerScript : MonoBehaviour
{
	private IEnumerator Start()
	{
		yield return 0;
		this.waveNum = 1;
		this.spawnSpeed = 2f - (float)this.waveNum * 0.1f;
		this.timeBetweenWaves = 1f;
		this.gameScene = GameApp.GetInstance().GetGameScene();
		Algorithem<EnemySpawnScript>.RandomSort(this.spawns);
		this.waveStartTime = Time.time;
		yield break;
	}

	public int WaveNum
	{
		get
		{
			return this.waveNum;
		}
	}

	private void Update()
	{
		if (Time.time - this.lastUpdateTime < this.spawnSpeed)
		{
			return;
		}
		this.lastUpdateTime = Time.time;
		if (this.currentSpawnIndex == this.spawns.Length)
		{
			int count = GameApp.GetInstance().GetGameScene().GetEnemies().Count;
			if (count == 0 || (Time.time - this.waveStartTime > 120f && count < 5))
			{
				this.waveNum++;
				this.currentSpawnIndex = 0;
				float num = (float)(this.waveNum - 1) / 10f;
				this.waveEndTime = Time.time;
				Algorithem<EnemySpawnScript>.RandomSort(this.spawns);
				this.spawnSpeed = 2f - (float)this.waveNum * 0.1f;
				if (this.spawnSpeed < 0.5f)
				{
					this.spawnSpeed = 0.5f;
				}
				UnityEngine.Debug.Log("Wave " + this.waveNum);
			}
		}
		else if (Time.time - this.waveEndTime > this.timeBetweenWaves)
		{
			if (this.currentSpawnIndex == 0)
			{
				this.waveStartTime = Time.time;
			}
			EnemySpawnScript enemySpawnScript = this.spawns[this.currentSpawnIndex];
			if (this.waveNum % enemySpawnScript.onlySpawnEvery == 0 && this.waveNum >= enemySpawnScript.onlySpawnFromRound)
			{
				enemySpawnScript.Spawn(1);
			}
			this.currentSpawnIndex++;
		}
	}

	private void OnDrawGizmos()
	{
	}

	protected Player player;

	protected int waveNum;

	public EnemySpawnScript[] spawns;

	protected GameScene gameScene;

	protected float lastUpdateTime = -1000f;

	protected int currentSpawnIndex;

	protected float spawnSpeed;

	protected float timeBetweenWaves;

	protected float waveStartTime;

	protected float waveEndTime;
}
