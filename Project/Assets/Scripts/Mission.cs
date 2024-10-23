using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Mission : BindLevel
{
	private Mission()
	{
	}

	public Mission(int _mID)
	{
		this.mID = _mID;
	}

	public void Reset()
	{
		this.curAmount = 0;
	}

	public void Revive()
	{
		if (this.isTimeLimited)
		{
			this.countDown = this.limitTime;
		}
	}

	public float DoCount(float dt)
	{
		this.countDown -= dt;
		this.countDown = Mathf.Clamp(this.countDown, 0f, this.limitTime);
		return this.countDown;
	}

	public int mID;

	public int plotID;

	public EMissionIcon targetIcon;

	public EMission mType = EMission.NONE;

	public int targetAmount;

	public bool isTimeLimited;

	public float limitTime;

	public bool needHeadShot;

	public bool needKeyEnemy;

	[NonSerialized]
	public int curAmount;

	[NonSerialized]
	public float countDown;

	public bool isMoveAction;

	public NPCCreater npcCreater;

	public NPCSTATE loopAnimation;

	public List<NPCPathPoint> pathPoint = new List<NPCPathPoint>();

	public BaseEnemySpawn missionEnemySpawn;

	public List<MissionItemInfo> missionItems = new List<MissionItemInfo>();

	[NonSerialized]
	public string description_ru;

	[NonSerialized]
	public string description_en;

	[NonSerialized]
	public string intro;
}
