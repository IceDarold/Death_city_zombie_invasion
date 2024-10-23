using System;
using System.Collections;
using DataCenter;
using UnityEngine;

public class StartMainSecen : MonoBehaviour
{
    public bool resetGame;
	private void Awake()
	{
		if (resetGame)
		{
			PlayerPrefs.DeleteAll();
		}
        //Advertisements.Instance.Initialize();
		Screen.sleepTimeout = -1;
		Singleton<GlobalData>.Instance.Init();
	}

	private IEnumerator Start()
	{
		yield return null;
		if (this.isDebug)
		{
			Singleton<UiManager>.Instance.ShowPage(this.Page, null);
		}
		else if (!Singleton<GlobalData>.Instance.isDebug)
		{
			if (CheckpointDataManager.GetCurrentCheckpoint().Chapters == ChapterEnum.CHAPTERNAME_01 && CheckpointDataManager.GetCurrentCheckpoint().Index == 1)
			{
				CheckpointDataManager.SelectCheckpoint = CheckpointDataManager.GetCheckpointData(ChapterEnum.CHAPTERNAME_01, 1);
				Singleton<UiManager>.Instance.ShowLoadingPage(CheckpointDataManager.GetScene(CheckpointDataManager.SelectCheckpoint.SceneID), PageName.InGamePage);
				GameLogManager.SendPageLog(PageName.InGamePage.ToString(), PageName.InGamePage.ToString());
			}
			else if (CheckpointDataManager.GetCurrentCheckpoint().Chapters == ChapterEnum.CHAPTERNAME_01 && CheckpointDataManager.GetCurrentCheckpoint().Index == 2)
			{
				CheckpointDataManager.SelectCheckpoint = CheckpointDataManager.GetCheckpointData(ChapterEnum.CHAPTERNAME_01, 2);
				Singleton<UiManager>.Instance.ShowLoadingPage(CheckpointDataManager.GetScene(CheckpointDataManager.SelectCheckpoint.SceneID), PageName.InGamePage);
				GameLogManager.SendPageLog(PageName.InGamePage.ToString(), PageName.InGamePage.ToString());
			}
			else
			{
				Singleton<UiManager>.Instance.ShowLoadingPage("UI", PageName.CoverPage);
			}
		}
		else
		{
			Singleton<UiManager>.Instance.ShowLoadingPage("UI", PageName.CoverPage);
		}
		yield break;
	}

	public bool isDebug;

	public PageName Page;
}
