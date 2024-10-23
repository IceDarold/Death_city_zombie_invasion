using System;
using UnityEngine;
using Zombie3D;

[RequireComponent(typeof(Collider))]
public class SceneMissionItem : MonoBehaviour
{
	public virtual SceneMissionItem Init(MissionItemInfo info)
	{
		this.gameScene = GameApp.GetInstance().GetGameScene();
		Transform transform = GameApp.GetInstance().GetGameScene().sceneMissions.transform;
		base.transform.parent = transform;
		base.transform.localPosition = info.localPos;
		base.transform.localScale = info.localScale;
		base.transform.localRotation = Quaternion.Euler(info.localRotation);
		this.itemInfo = info;
		return this;
	}

	public virtual void Update()
	{
		if (!this.startCount || this.countTime >= this.itemInfo.stayDuration)
		{
			return;
		}
		if (this.gameScene.PlayingState != PlayingState.GamePlaying)
		{
			return;
		}
		this.countTime += Time.deltaTime;
		this.gameScene.SetUIDisplayEvnt(UIDisplayEvnt.MISSION_ITEM_PERCENT, new float[]
		{
			this.countTime,
			this.itemInfo.stayDuration
		});
		if (this.countTime >= this.itemInfo.stayDuration)
		{
			this.DoTriggerItem();
		}
	}

	public virtual void DoTriggerItem()
	{
		base.gameObject.SetActive(false);
		if (this.itemInfo.needSubmit)
		{
			this.gameScene.sceneMissions.ChangeMissionTargetToSubmit();
		}
		else
		{
			this.gameScene.sceneMissions.ChangeMissionTargetToNext(false);
		}
		this.gameScene.SetUIDisplayEvnt(UIDisplayEvnt.MISSION_ITEM_PERCENT, new float[2]);
		this.gameScene.SetUIDisplayEvnt(UIDisplayEvnt.TRIGGER_MISSIONITEM, new float[0]);
	}

	public virtual void OnTriggerEnter(Collider other)
	{
		this.gameScene.SetUIDisplayEvnt(UIDisplayEvnt.ON_MISSIONITEM, new float[0]);
		if (this.itemInfo.forceStay)
		{
			this.startCount = true;
		}
		else
		{
			this.DoTriggerItem();
		}
	}

	public virtual void OnTriggerExit(Collider other)
	{
		if (this.itemInfo.forceStay && this.startCount)
		{
			this.startCount = false;
		}
		this.gameScene.SetUIDisplayEvnt(UIDisplayEvnt.EXIT_MISSIONITEM, new float[0]);
	}

	public virtual void Delete()
	{
		base.gameObject.SetActive(false);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	[HideInInspector]
	public bool forceStay;

	[HideInInspector]
	public float stayDuration;

	[HideInInspector]
	public bool alwaysShow;

	[HideInInspector]
	public MissionItemType type;

	private GameScene gameScene;

	protected float countTime;

	protected bool startCount;

	protected MissionItemInfo itemInfo;
}
