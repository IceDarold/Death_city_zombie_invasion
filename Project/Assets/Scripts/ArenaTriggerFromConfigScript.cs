using System;
using System.Collections;
using UnityEngine;
using Zombie3D;

public class ArenaTriggerFromConfigScript : MonoBehaviour
{
	public void CreateEnemy(EnemyType enemyType, int spawnNum, int i, GameObject enemyPrefab, SpawnFromType from, Transform grave)
	{
		bool flag = this.EliteSpawn(enemyType, spawnNum, i);
		if (flag)
		{
		}
		Vector3 position = Vector3.zero;
		if (from == SpawnFromType.Door)
		{
			position = this.doors[this.currentDoorIndex].transform.position;
			this.currentDoorIndex++;
			if (this.currentDoorIndex == this.doorCount)
			{
				this.currentDoorIndex = 0;
			}
		}
		else if (from == SpawnFromType.Grave)
		{
			float x = UnityEngine.Random.Range(-grave.localScale.x / 2f, grave.localScale.x / 2f);
			float z = UnityEngine.Random.Range(-grave.localScale.z / 2f, grave.localScale.z / 2f);
			position = grave.position + new Vector3(x, 0f, z);
		}
		GameObject gameObject = null;
		if (flag)
		{
			gameObject = UnityEngine.Object.Instantiate<GameObject>(enemyPrefab, position, Quaternion.Euler(0f, 0f, 0f));
		}
		gameObject.name = "E_" + GameApp.GetInstance().GetGameScene().GetNextEnemyID().ToString();
	}

	public bool EliteSpawn(EnemyType eType, int spawnNum, int index)
	{
		return false;
	}

	public Transform CalculateGravePosition(Transform playerTrans)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Grave");
		GameObject gameObject = null;
		float num = 99999f;
		foreach (GameObject gameObject2 in array)
		{
			float sqrMagnitude = (playerTrans.position - gameObject2.transform.position).sqrMagnitude;
			if (sqrMagnitude < num)
			{
				gameObject = gameObject2;
				num = sqrMagnitude;
			}
		}
		return gameObject.transform;
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
	}

	private void OnDrawGizmos()
	{
		if (this.gameScene != null)
		{
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

	protected Player player;

	protected int waveNum;

	protected GameParametersXML paramXML;

	protected GameScene gameScene;

	protected GameObject[] doors;

	protected float lastUpdateTime = -1000f;

	protected int currentSpawnIndex;

	protected float spawnSpeed;

	protected float timeBetweenWaves;

	protected float waveStartTime;

	protected float waveEndTime;

	protected int currentDoorIndex;

	protected int doorCount;

	protected bool levelClear;

	private SpawnConfig spawnConfigInfo;
}
