using System;
using UnityEngine;

public class DayMissionPointLogic : MonoBehaviour
{
	private void Awake()
	{
		Singleton<DailyTaskMgr>.Instance.RefreshDayMissionBtn = new Action<bool>(this.RefreshRedPoint);
		this.RefreshRedPoint(Singleton<DailyTaskMgr>.Instance.GetHasReadyBox());
	}

	private void RefreshRedPoint(bool show)
	{
		this.redPoint.SetActive(show);
	}

	public void OnDestroy()
	{
		if (Singleton<DailyTaskMgr>.Instance)
		{
			Singleton<DailyTaskMgr>.Instance.RefreshDayMissionBtn = null;
		}
	}

	public GameObject redPoint;
}
