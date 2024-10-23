using System;
using System.Collections;
using UnityEngine;
using Zombie3D;

public class EnemySpawnScript : MonoBehaviour
{
	private IEnumerator Start()
	{
		yield return 0;
		yield break;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(base.transform.position, 0.3f);
	}

	public void Spawn(int spawnNum)
	{
		if (GameApp.GetInstance().GetGameScene() == null || this.disable)
		{
			return;
		}
		GameObject original = null;
		for (int i = 0; i < spawnNum; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original, base.transform.position, Quaternion.Euler(0f, 0f, 0f));
			gameObject.name = "E_" + GameApp.GetInstance().GetGameScene().GetNextEnemyID().ToString();
			int num = (int)this.enemyType;
			if (num != 0)
			{
				if (num != 1)
				{
					if (num == 2)
					{
						Enemy enemy = new Tank();
					}
				}
				else
				{
					Enemy enemy = new Nurse();
				}
			}
			else
			{
				Enemy enemy = new Zombie();
			}
		}
		this.lastSpawnTime = Time.time;
	}

	private void OnResetSpawnTrigger()
	{
		if (this.triggerBelongsto != null)
		{
			this.triggerBelongsto.EnemyKilled();
		}
		this.isKilled = true;
	}

	public TriggerScript TriggerBelongsto
	{
		set
		{
			this.triggerBelongsto = value;
		}
	}

	public EnemyType enemyType;

	public int onlySpawnFromRound = 1;

	public int onlySpawnEvery = 1;

	public bool isKilled;

	protected float lastSpawnTime;

	protected TriggerScript triggerBelongsto;

	protected static GameObject enemyFolder;

	protected bool disable;
}
