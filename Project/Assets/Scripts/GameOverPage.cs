using System;
using System.Collections;
using DataCenter;
using DG.Tweening;
using RacingMode;
using UnityEngine;
using UnityEngine.UI;
using Zombie3D;

public class GameOverPage : GamePage
{
	public new void OnEnable()
	{
		if (CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.RACING)
		{
			GameLogManager.SendGameOverLog(Singleton<UiManager>.Instance.GameSuccess == 1, RacingSceneManager.Instance.ReviveTimes);
			if (Singleton<UiManager>.Instance.GameSuccess == 1)
			{
				Singleton<GlobalData>.Instance.RacingTimes++;
			}
		}
		else
		{
			GameLogManager.SendGameOverLog(Singleton<UiManager>.Instance.GameSuccess == 1, GameApp.GetInstance().GetGameScene().reviveCount);
		}
		if (Singleton<UiManager>.Instance.GameSuccess == 1)
		{
			Singleton<GameAudioManager>.Instance.PlaySound(this.PageAudio[0], false);
			this.GameLose.SetActive(false);
			this.GameWin.SetActive(true);
			this.iconAni.DORestart(false);
			this.BgAni.DORestart(false);
			this.beijingAni.DORestart(false);
			this.donghuaBeijing.DORestart(false);
		}
		else if (Singleton<UiManager>.Instance.GameSuccess == -1)
		{
			Singleton<GameAudioManager>.Instance.PlaySound(this.PageAudio[1], false);
			this.GameLose.SetActive(true);
			this.GameWin.SetActive(false);
			this.iconLoseAni.DORestart(false);
			this.BgLoseAni.DORestart(false);
			this.BgAni.DORestart(false);
			this.beijingLoseAni.DORestart(false);
			this.donghuaLoseBeijing.DORestart(false);
		}
	}

	public void CloseOver()
	{
		base.StartCoroutine(this.Onclick());
	}

	private IEnumerator Onclick()
	{
		yield return new WaitForSeconds(0.2f);
		this.Close();
		if (CheckpointDataManager.GetCurrentCheckpoint().Chapters == ChapterEnum.CHAPTERNAME_01 && CheckpointDataManager.GetCurrentCheckpoint().Index == 1)
		{
			PlayerDataManager.SetStatisticsDatas(PlayerStatistics.CheckpointPassedTimes, 1);
			CheckpointDataManager.SetCheckpointPassed(CheckpointDataManager.SelectCheckpoint);
			CheckpointDataManager.SelectCheckpoint = CheckpointDataManager.GetCheckpointData(ChapterEnum.CHAPTERNAME_01, 2);
			Singleton<UiManager>.Instance.RemovePage(PageName.InGamePage);
			Singleton<DropItemManager>.Instance.Recycle();
			Singleton<GamePropManager>.Instance.Recycle();
			Singleton<UiManager>.Instance.ShowLoadingPage(CheckpointDataManager.GetScene(CheckpointDataManager.SelectCheckpoint.SceneID), PageName.InGamePage);
			GameLogManager.SendPageLog(this.Name.ToString(), PageName.InGamePage.ToString());
		}
		else
		{
			GameLogManager.SendPageLog(this.Name.ToString(), PageName.CommonFinishPage.ToString() + Singleton<UiManager>.Instance.GameSuccess);
			Singleton<UiManager>.Instance.ShowPage(PageName.CommonFinishPage, null);
		}
		yield break;
	}

	public override void OnBack()
	{
	}

	public Text TitleTxt;

	public DOTweenAnimation BgAni;

	public DOTweenAnimation iconAni;

	public DOTweenAnimation beijingAni;

	public DOTweenAnimation donghuaBeijing;

	public Text TitleLoseTxt;

	public DOTweenAnimation BgLoseAni;

	public DOTweenAnimation iconLoseAni;

	public DOTweenAnimation beijingLoseAni;

	public DOTweenAnimation donghuaLoseBeijing;

	public GameObject GameWin;

	public GameObject GameLose;

	public AudioClip[] PageAudio;
}
