using System;
using System.Collections.Generic;
using DataCenter;
using UnityEngine;
using Zombie3D;

public class DropItemManager : Singleton<DropItemManager>
{
	public List<DropItemType> GetDropList()
	{
		return this.DropList;
	}

	public void AddDropItem(DropItemType item)
	{
		this.DropList.Add(item);
	}

	private List<DropItemType> GetDropItem(global::EnemyProbability type)
	{
		List<DropItemType> list = new List<DropItemType>();
		if (type == global::EnemyProbability.NORMAL)
		{
			if (this.CanDropDebrisByNormal())
			{
				this.DebrisCount++;
				Singleton<GlobalData>.Instance.DebrisDropCount++;
				list.Add(DropItemType.DEBRIS);
				this.DropList.Add(DropItemType.DEBRIS);
			}
			if (UnityEngine.Random.Range(0, 100) < 15)
			{
				list.Add(DropItemType.GOLD);
			}
			if (UnityEngine.Random.Range(0, 100) < 5)
			{
				list.Add(DropItemType.DNA);
			}
			if (GameApp.GetInstance().GetGameScene().PlayingMode != GamePlayingMode.Cannon)
			{
				if (UnityEngine.Random.Range(0, 100) < 5)
				{
					list.Add(DropItemType.BLOOD);
				}
				if (UnityEngine.Random.Range(0, 100) < 10)
				{
					list.Add(DropItemType.BULLET);
				}
			}
		}
		else if (type == global::EnemyProbability.ELITE)
		{
			if (this.CanDropDebrisByElite())
			{
				this.DebrisCount++;
				Singleton<GlobalData>.Instance.DebrisDropCount++;
				list.Add(DropItemType.DEBRIS);
				this.DropList.Add(DropItemType.DEBRIS);
			}
			if (UnityEngine.Random.Range(0, 100) < 100)
			{
				int num = UnityEngine.Random.Range(3, 6);
				for (int i = 0; i < num; i++)
				{
					list.Add(DropItemType.GOLD);
				}
			}
			if (UnityEngine.Random.Range(0, 100) < 5)
			{
				list.Add(DropItemType.DNA);
			}
			if (GameApp.GetInstance().GetGameScene().PlayingMode != GamePlayingMode.Cannon)
			{
				if (UnityEngine.Random.Range(0, 100) < 100)
				{
					list.Add(DropItemType.BULLET);
				}
				if (UnityEngine.Random.Range(0, 100) < 5)
				{
					int num2 = UnityEngine.Random.Range(1, 4);
					for (int j = 0; j < num2; j++)
					{
						list.Add(DropItemType.BLOOD);
					}
				}
			}
		}
		else if (type == global::EnemyProbability.BOSS)
		{
			if (this.CanDropDebrisByElite())
			{
				this.DebrisCount++;
				Singleton<GlobalData>.Instance.DebrisDropCount++;
				list.Add(DropItemType.DEBRIS);
				this.DropList.Add(DropItemType.DEBRIS);
			}
			if (UnityEngine.Random.Range(0, 100) < 100)
			{
				int num3 = UnityEngine.Random.Range(3, 5);
				for (int k = 0; k < num3; k++)
				{
					list.Add(DropItemType.GOLD);
				}
			}
			if (UnityEngine.Random.Range(0, 100) < 5)
			{
				list.Add(DropItemType.DNA);
			}
			if (GameApp.GetInstance().GetGameScene().PlayingMode != GamePlayingMode.Cannon)
			{
				if (UnityEngine.Random.Range(0, 100) < 30)
				{
					list.Add(DropItemType.BULLET);
				}
				if (UnityEngine.Random.Range(0, 100) < 5)
				{
					int num4 = UnityEngine.Random.Range(3, 5);
					for (int l = 0; l < num4; l++)
					{
						list.Add(DropItemType.BLOOD);
					}
				}
			}
		}
		return list;
	}

	public bool isCannon()
	{
		return GameApp.GetInstance().GetGameScene().PlayingMode == GamePlayingMode.Cannon;
	}

	private bool CanDropDebrisByElite()
	{
		Write.Log("进入精英怪掉落逻辑");
		Write.Log(string.Concat(new object[]
		{
			"本次掉落碎片总数 ： ",
			this.DebrisCount,
			"今日碎片掉落总数 ： ",
			Singleton<GlobalData>.Instance.DebrisDropCount
		}));
		if (this.DebrisCount >= 2 || Singleton<GlobalData>.Instance.DebrisDropCount >= 4)
		{
			return false;
		}
		if (DebrisDataManager.HasDebris())
		{
			int num = UnityEngine.Random.Range(0, 100);
			float num2 = (float)DebrisDataManager.GetCurrentDebrisCount();
			float num3 = (float)DebrisDataManager.GetTotalDebrisCount();
			float num4 = Mathf.Pow(1f - (Mathf.Min(num2, 15f + num2 / 7f) - 1f) / num3, 13f);
			Write.Log(string.Concat(new object[]
			{
				"拥有: ",
				num2,
				"总共: ",
				num3,
				"概率值：",
				num4
			}));
			if ((float)num < num4 * 100f)
			{
				return true;
			}
		}
		return false;
	}

	private bool CanDropDebrisByNormal()
	{
		Write.Log(string.Concat(new object[]
		{
			"本次掉落碎片总数 ： ",
			this.DebrisCount,
			"今日碎片掉落总数 ： ",
			Singleton<GlobalData>.Instance.DebrisDropCount
		}));
		if (this.DebrisCount >= 2 || Singleton<GlobalData>.Instance.DebrisDropCount >= 4)
		{
			return false;
		}
		CheckpointData currentCheckpoint = CheckpointDataManager.GetCurrentCheckpoint();
		Write.Log("小怪是否掉落" + (currentCheckpoint.Chapters == ChapterEnum.CHAPTERNAME_01 && currentCheckpoint.Index < 5));
		if (currentCheckpoint.Chapters == ChapterEnum.CHAPTERNAME_01 && currentCheckpoint.Index < 5)
		{
			return false;
		}
		if (DebrisDataManager.HasDebris())
		{
			int num = UnityEngine.Random.Range(0, 100);
			float num2 = (float)DebrisDataManager.GetCurrentDebrisCount();
			float num3 = (float)DebrisDataManager.GetTotalDebrisCount();
			float num4 = Mathf.Pow(1f - (Mathf.Min(num2, 15f + num2 / 7f) - 1f) / num3, 13f) * 0.05f;
			Write.Log(string.Concat(new object[]
			{
				"拥有: ",
				num2,
				"总共: ",
				num3,
				"概率值：",
				num4
			}));
			if ((float)num < num4 * 100f)
			{
				return true;
			}
		}
		return false;
	}

	private void ShowDrop(global::EnemyProbability type, Vector3 pos)
	{
		List<DropItemType> dropItem = this.GetDropItem(type);
		for (int i = 0; i < dropItem.Count; i++)
		{
			this.CreateDropItem(dropItem[i], pos);
		}
	}

	public void ShowBossHitDrop(Vector3 pos)
	{
		if (this.CanDrop)
		{
			this.rate = 0f;
			this.CanDrop = false;
			List<DropItemType> list = new List<DropItemType>();
			if (UnityEngine.Random.Range(0, 100) < 40)
			{
				list.Add(DropItemType.BULLET);
			}
			if (UnityEngine.Random.Range(0, 100) < 40 && this.GoldCount < 40)
			{
				this.GoldCount++;
				list.Add(DropItemType.GOLD);
			}
			if (UnityEngine.Random.Range(0, 100) < 20 && this.DnaCount < 40)
			{
				this.DnaCount++;
				list.Add(DropItemType.DNA);
			}
			for (int i = 0; i < list.Count; i++)
			{
				this.CreateDropItem(list[i], pos);
			}
		}
	}

	public void ShowBossDieDrop(Vector3 pos)
	{
		List<DropItemType> list = new List<DropItemType>();
		int num = UnityEngine.Random.Range(20, 31);
		int num2 = UnityEngine.Random.Range(10, 21);
		for (int i = 0; i < num; i++)
		{
			list.Add(DropItemType.GOLD);
		}
		for (int j = 0; j < num2; j++)
		{
			list.Add(DropItemType.DNA);
		}
		for (int k = 0; k < list.Count; k++)
		{
			this.CreateDropItem(list[k], pos);
		}
	}

	public void DoDropItem(global::EnemyProbability probability, Vector3 pos, CheckpointType type)
	{
		if (type == CheckpointType.BOSS && probability == global::EnemyProbability.BOSS)
		{
			this.ShowBossDieDrop(pos);
		}
		else if (type == CheckpointType.GOLD)
		{
			this.ShowGoldBarrierDrop(probability, pos);
		}
		else
		{
			this.ShowDrop(probability, pos);
		}
	}

	private void ShowGoldBarrierDrop(global::EnemyProbability type, Vector3 pos)
	{
		List<DropItemType> list = new List<DropItemType>();
		if (type == global::EnemyProbability.NORMAL)
		{
			int num = UnityEngine.Random.Range(1, 6);
			for (int i = 0; i < num; i++)
			{
				list.Add(DropItemType.GOLD);
			}
			if (UnityEngine.Random.Range(0, 100) < 15)
			{
				list.Add(DropItemType.BULLET);
			}
			if (UnityEngine.Random.Range(0, 100) < 5)
			{
				list.Add(DropItemType.DNA);
			}
			if (UnityEngine.Random.Range(0, 100) < 10)
			{
				int num2 = UnityEngine.Random.Range(3, 5);
				for (int j = 0; j < num2; j++)
				{
					list.Add(DropItemType.BLOOD);
				}
			}
		}
		else if (type == global::EnemyProbability.ELITE)
		{
			if (this.CanDropDebrisByElite())
			{
				this.DebrisCount++;
				Singleton<GlobalData>.Instance.DebrisDropCount++;
				list.Add(DropItemType.DEBRIS);
				this.DropList.Add(DropItemType.DEBRIS);
			}
			int num3 = UnityEngine.Random.Range(15, 21);
			for (int k = 0; k < num3; k++)
			{
				list.Add(DropItemType.GOLD);
			}
			if (UnityEngine.Random.Range(0, 100) < 10)
			{
				list.Add(DropItemType.BULLET);
			}
			if (UnityEngine.Random.Range(0, 100) < 5)
			{
				list.Add(DropItemType.DNA);
			}
			if (UnityEngine.Random.Range(0, 100) < 5)
			{
				int num4 = UnityEngine.Random.Range(3, 5);
				for (int l = 0; l < num4; l++)
				{
					list.Add(DropItemType.BLOOD);
				}
			}
		}
		for (int m = 0; m < list.Count; m++)
		{
			this.CreateDropItem(list[m], pos);
		}
	}

	private void CreateDropItem(DropItemType type, Vector3 pos)
	{
		for (int i = 0; i < this.Pool.Count; i++)
		{
			if (this.Pool[i].Type == type && !this.Pool[i].gameObject.activeSelf)
			{
				this.Pool[i].transform.position = pos;
				this.Pool[i].gameObject.SetActive(true);
				return;
			}
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("DropItem/" + type.ToString())) as GameObject;
		gameObject.transform.SetParent(base.transform);
		gameObject.transform.position = pos;
		DropItem component = gameObject.GetComponent<DropItem>();
		component.Type = type;
		this.Pool.Add(component);
		gameObject.SetActive(true);
	}

	public void Recycle()
	{
		this.DropList.Clear();
		this.DebrisCount = 0;
		this.GoldCount = 0;
		this.DnaCount = 0;
		this.rate = 0f;
		this.CanDrop = true;
		for (int i = 0; i < this.Pool.Count; i++)
		{
			this.Pool[i].gameObject.SetActive(false);
		}
	}

	private void Update()
	{
		if (this.rate < 0.5f)
		{
			this.CanDrop = false;
			this.rate += Time.deltaTime;
		}
		else
		{
			this.CanDrop = true;
		}
	}

	private List<DropItemType> DropList = new List<DropItemType>();

	private List<DropItem> Pool = new List<DropItem>();

	private int DebrisCount;

	private float rate;

	private int GoldCount;

	private int DnaCount;

	private bool CanDrop = true;
}
