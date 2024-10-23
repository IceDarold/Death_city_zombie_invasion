using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zombie3D;

[RequireComponent(typeof(BoxCollider))]
public class GamePlot : MonoBehaviour, IBindAble
{
	public void DoPlot(UnityAction end = null)
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		for (int i = 0; i < this.ActionList.Count; i++)
		{
			if (this.ActionList[i].Type == PlotActionType.关卡结束)
			{
				GameApp.GetInstance().GetGameScene().PlayingState = PlayingState.WaitForEnd;
			}
			this.OnPlotBegin.AddListener(new BaseEventHandler(this.ActionList[i].OnActionBegin));
			this.OnPlotEnded.AddListener(new BaseEventHandler(this.ActionList[i].OnActionEnded));
		}
		this.current = 0f;
		this.isPlot = true;
		Singleton<UiManager>.Instance.CanBack = false;
		this.EndedAction = end;
	}

	public void RemoveListener()
	{
		for (int i = 0; i < this.ActionList.Count; i++)
		{
			this.OnPlotBegin.RemoveListener(new BaseEventHandler(this.ActionList[i].OnActionBegin));
			this.OnPlotEnded.RemoveListener(new BaseEventHandler(this.ActionList[i].OnActionEnded));
		}
	}

	public BindLevel GetBindInfo()
	{
		return this.bindLevelInfo;
	}

	public bool CanUse(int index)
	{
		return this.bindLevelInfo.GetEnable(index);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			GameScene gameScene = GameApp.GetInstance().GetGameScene();
			gameScene.gamePlotManager.ShowPlot(this.ID, null);
			this.PlotCollider.enabled = false;
		}
	}

	private void Update()
	{
		if (this.isPlot)
		{
			this.OnPlotBegin.TriggerEvent(this.current);
			this.OnPlotEnded.TriggerEvent(this.current);
			this.current += Time.deltaTime;
			if (this.current > this.TotalTime)
			{
				this.current = 0f;
				this.isPlot = false;
				Singleton<UiManager>.Instance.CanBack = true;
				if (this.EndedAction != null)
				{
					this.EndedAction();
				}
				GameScene gameScene = GameApp.GetInstance().GetGameScene();
				if (this.isTriggerMission)
				{
					gameScene.sceneMissions.ActiveMission(this.MissionID);
				}
				this.RemoveListener();
				base.gameObject.SetActive(false);
			}
		}
	}

	public int ID;

	public List<PlotAction> ActionList = new List<PlotAction>();

	public float TotalTime = 60f;

	public UnityAction EndedAction;

	public bool isTriggerMission;

	public int MissionID;

	public BoxCollider PlotCollider;

	[HideInInspector]
	public BindLevel bindLevelInfo = new BindLevel();

	private GamePlotEvent OnPlotBegin;

	private GamePlotEvent OnPlotEnded;

	public bool isPlot;

	private GameScene scene;

	private float current;
}
