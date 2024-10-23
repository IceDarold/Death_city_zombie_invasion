using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using Zombie3D;

public class SceneMissions : MonoBehaviour
{
	public Mission CurMission
	{
		get
		{
			return this.curMission;
		}
	}

	private IEnumerator Start()
	{
		yield return null;
		this.gameScene = GameApp.GetInstance().GetGameScene();
		string XMLPATH = "DataConfig/" + this.xmlPathPrefix + "_MissionDescription";
		string XMLPATH_RU = XMLPATH + "_RU";
		string XMLPATH_EN = XMLPATH + "_EN";
		XmlDocument xml_ch = new XmlDocument();
		XmlDocument xml_en = new XmlDocument();
		string str_ru = Resources.Load<TextAsset>(XMLPATH_RU).ToString();
		xml_ch.LoadXml(str_ru);
		this.root_RU = xml_ch.DocumentElement;
		string str_en = Resources.Load<TextAsset>(XMLPATH_EN).ToString();
		xml_en.LoadXml(str_en);
		this.root_EN = xml_en.DocumentElement;
		yield break;
	}

	public EMission GetMissionType()
	{
		if (this.curMission == null)
		{
			return EMission.NONE;
		}
		return this.curMission.mType;
	}

	public void ActiveMission(int mid)
	{
		if (this.curMission != null)
		{
			this.curMission.Reset();
		}
		this.curMissionItems.Clear();
		this.curMission = this.missions.Find((Mission mission) => mission.mID == mid);
		XmlNode xmlNode = this.root_EN.SelectSingleNode("/MissionData/Data[@MissionID='" + this.curMission.mID.ToString() + "']");
		XmlNode xmlNode2 = this.root_RU.SelectSingleNode("/MissionData/Data[@MissionID='" + this.curMission.mID.ToString() + "']");
		this.curMission.description_en = xmlNode.InnerText;
		this.curMission.description_ru = xmlNode2.InnerText;
		if (this.curMission.isTimeLimited)
		{
			this.curMission.countDown = this.curMission.limitTime;
		}
		this.gameScene.SetUIDisplayEvnt(UIDisplayEvnt.MISSION_TIME_LIMITED, new float[]
		{
			(float)((!this.curMission.isTimeLimited) ? 0 : 1)
		});
		for (int i = 0; i < this.curMission.missionItems.Count; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("MissionItems/" + this.curMission.missionItems[i].prefabName)) as GameObject;
			SceneMissionItem sceneMissionItem = gameObject.GetComponent<SceneMissionItem>().Init(this.curMission.missionItems[i]);
			this.curMission.missionItems[i].instantiatedObject = gameObject;
			gameObject.SetActive(sceneMissionItem.alwaysShow);
			if (sceneMissionItem.type == MissionItemType.TRANSPORT_SUBMIT)
			{
				this.submitTaraget = gameObject;
			}
			else
			{
				this.curMissionItems.Add(sceneMissionItem);
			}
		}
		if (this.curMission.mType == EMission.PROTECT_NPC)
		{
			this.gameScene.SetEnemyTarget(this.curMission.npcCreater.GetNpc(), false);
			this.curMission.npcCreater.GetNpc().SetMission(this.curMission);
			this.curMission.npcCreater.GetNpc().SetHP2UI(true);
		}
		else if (this.curMission.mType == EMission.KILL_WAVE)
		{
			this.curMission.missionEnemySpawn.SetOnAllEnemyDie(new Action(this.SubmitMission));
		}
		else if (this.curMissionItems.Count > 0)
		{
			this.ChangeMissionTargetToNext(true);
		}
		this.gameScene.GameUI.MissionStart(this.curMission);
		Singleton<GameAudioManager>.Instance.PlayGlobalEffect(GlobalEffect.MissionComplete);
	}

	public void ChangeMissionTargetToSubmit()
	{
		this.missionTarget.SetActive(false);
		this.missionTarget = this.submitTaraget;
		this.missionTarget.SetActive(true);
	}

	public void ChangeMissionTargetToNext(bool isFirst = false)
	{
		if (this.missionTarget != null)
		{
			this.missionTarget.SetActive(false);
		}
		if (!isFirst)
		{
			this.curMission.curAmount++;
		}
		if (this.curMission.curAmount == this.curMission.targetAmount)
		{
			this.SubmitMission();
			return;
		}
		this.missionTarget = this.curMissionItems[this.curMission.curAmount].gameObject;
		this.missionTarget.SetActive(true);
	}

	public void SubmitMission()
	{
		if (this.curMission.mType == EMission.PROTECT_NPC)
		{
			this.curMission.npcCreater.GetNpc().SetHP2UI(false);
			this.gameScene.SetEnemyTarget(null, true);
		}
		this.gameScene.gamePlotManager.ShowPlot(this.curMission.plotID, null);
		if (this.curMission.npcCreater != null)
		{
			this.curMission.npcCreater.GetNpc().DoWakeUp();
		}
		this.gameScene.GameUI.MissionComplete();
	}

	public void DoKillEnemy(bool headShot, bool keyEnemy)
	{
		if (this.curMission == null || this.curMission.mType != EMission.KILL_ENEMY)
		{
			return;
		}
		if (this.curMission.needHeadShot && !headShot)
		{
			return;
		}
		if (this.curMission.needKeyEnemy && !keyEnemy)
		{
			return;
		}
		this.curMission.curAmount++;
		this.gameScene.SetUIDisplayEvnt(UIDisplayEvnt.MISSION_ITEM_PERCENT, new float[]
		{
			(float)this.curMission.curAmount,
			(float)this.curMission.targetAmount
		});
		if (this.curMission.curAmount == this.curMission.targetAmount)
		{
			this.SubmitMission();
			return;
		}
	}

	public bool SimulateKillEnemy(bool headShot, bool keyEnemy, ref int curMissionAmount)
	{
		if (this.curMission == null || this.curMission.mType != EMission.KILL_ENEMY)
		{
			return false;
		}
		if (this.curMission.needHeadShot && !headShot)
		{
			return false;
		}
		if (this.curMission.needKeyEnemy && !keyEnemy)
		{
			return false;
		}
		curMissionAmount++;
		Debug.Log($"!!! {curMissionAmount}   {curMission.targetAmount}");
		return curMissionAmount == this.curMission.targetAmount;
	}

	public bool CheckEnemyInMission(bool headShot, bool keyEnemy)
	{
		Debug.Log($"!!! FLAG 2   {headShot}    {keyEnemy}");
		return this.curMission != null && this.curMission.mType == EMission.KILL_ENEMY && (!this.curMission.needHeadShot || headShot) && (!this.curMission.needKeyEnemy || keyEnemy);
	}

	public Vector3 GetMissionTarget()
	{
		return (this.curMission.mType != EMission.PROTECT_NPC) ? this.missionTarget.transform.position : this.curMission.npcCreater.GetNpc().transform.position;
	}

	public int GetDis2MissionTarget()
	{
		return (int)(base.transform.position - this.missionTarget.transform.position).magnitude;
	}

	public bool CheckMission()
	{
		return this.curMission != null && this.curMission.mType != EMission.KILL_ENEMY && this.curMission.mType != EMission.KILL_WAVE && this.curMission.mType != EMission.NONE;
	}

	public void DoLogic(float dt)
	{
		if (this.gameScene == null)
		{
			return;
		}
		if (this.gameScene.PlayingState != PlayingState.GamePlaying)
		{
			return;
		}
		if (this.curMission == null || !this.curMission.isTimeLimited)
		{
			return;
		}
		if (this.curMission.DoCount(dt) == 0f)
		{
			this.gameScene.LoseGame();
		}
		this.gameScene.SetUIDisplayEvnt(UIDisplayEvnt.COUNTDOWN_PERCENT, new float[]
		{
			this.curMission.countDown,
			this.curMission.limitTime
		});
	}

	public void Revive()
	{
		this.curMission.Revive();
	}

	[ContextMenu("检测重复任务")]
	private void DetectMissionID()
	{
		List<int> list = new List<int>();
		foreach (Mission mission in this.missions)
		{
			if (list.Contains(mission.mID))
			{
				UnityEngine.Debug.LogError(mission.mID + "重复");
			}
			list.Add(mission.mID);
		}
		list.Clear();
	}

	[HideInInspector]
	public List<Mission> missions = new List<Mission>();

	[CNName("任务描述文件前缀")]
	public string xmlPathPrefix;

	private Mission curMission;

	private const string path = "MissionItems/";

	private GameScene gameScene;

	private GameObject missionTarget;

	private GameObject submitTaraget;

	private List<SceneMissionItem> curMissionItems = new List<SceneMissionItem>();

	private XmlElement root_RU;

	private XmlElement root_EN;
}
