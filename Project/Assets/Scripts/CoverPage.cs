using System;
using System.Collections;
using DataCenter;
using DG.Tweening;
using inApps;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class CoverPage : GamePage
{
	public override void Show()
	{
		this.Content.gameObject.SetActive(false);
		base.Show();
		if (!Singleton<GameAudioManager>.Instance.MusicSource.isPlaying && Singleton<GameAudioManager>.Instance.GameMusic && Singleton<GameAudioManager>.Instance.MusicSource.clip != Singleton<GameAudioManager>.Instance.UIBgm)
		{
			Singleton<GameAudioManager>.Instance.PlayMusic(Singleton<GameAudioManager>.Instance.UIBgm);
			Singleton<GameAudioManager>.Instance.PauseSoundInGame();
		}
		if (null != ScenceControl.instance)
		{
			ScenceControl.instance.ShowStart();
		}
		base.StartCoroutine(this.ShowCover());
	}

	public override void Refresh()
	{
		this.TipText.text = Singleton<GlobalData>.Instance.GetText("START_TIP");
		Singleton<FontChanger>.Instance.SetFont(TipText);
		//string uid = GMGSDK.user.getUid();
		//if (!string.IsNullOrEmpty(uid))
		//{
		//	this.UID.gameObject.SetActive(true);
		//	this.UID.text = "UID: " + uid;
		//}
		//else
		{
			this.UID.gameObject.SetActive(false);
		}
		this.Version.text = "V." + Application.version;
		this.FacebookButton.gameObject.SetActive(false);
		//this.CloseButton.gameObject.SetActive(true);
	}

	private IEnumerator ShowCover()
	{
		yield return new WaitForSeconds(0.5f);
		this.Content.gameObject.SetActive(true);
		this.Content.alpha = 0f;
		this.Content.DOFade(1f, 0.5f);
		this.ShowUpdateCompensate();
		yield break;
	}

	public override void OnBack()
	{
		//GMGSDK.ExitGame();
	}

	private void ShowUpdateCompensate()
	{
		if (Singleton<GlobalData>.Instance.UpdateCompensate)
		{
			Singleton<GlobalData>.Instance.UpdateCompensate = false;
			int[] id = new int[2];
			int[] count = new int[2];
			if (CheckpointDataManager.GetCurrentCheckpoint().ID > 10 && CheckpointDataManager.GetCurrentCheckpoint().ID <= 20)
			{
				ItemDataManager.SetCurrency(CommonDataType.GOLD, 30000);
				ItemDataManager.SetCurrency(CommonDataType.DIAMOND, 100);
				id = new int[]
				{
					1,
					2
				};
				count = new int[]
				{
					30000,
					100
				};
				Singleton<UiManager>.Instance.ShowAward(id, count, Singleton<GlobalData>.Instance.GetText("ERROR_COMPENSATE"));
			}
			else if (CheckpointDataManager.GetCurrentCheckpoint().ID > 20 && CheckpointDataManager.GetCurrentCheckpoint().ID <= 30)
			{
				ItemDataManager.SetCurrency(CommonDataType.GOLD, 60000);
				ItemDataManager.SetCurrency(CommonDataType.DIAMOND, 200);
				id = new int[]
				{
					1,
					2
				};
				count = new int[]
				{
					60000,
					200
				};
				Singleton<UiManager>.Instance.ShowAward(id, count, Singleton<GlobalData>.Instance.GetText("ERROR_COMPENSATE"));
			}
			else if (CheckpointDataManager.GetCurrentCheckpoint().ID > 30 && CheckpointDataManager.GetCurrentCheckpoint().ID <= 40)
			{
				ItemDataManager.SetCurrency(CommonDataType.GOLD, 120000);
				ItemDataManager.SetCurrency(CommonDataType.DIAMOND, 300);
				id = new int[]
				{
					1,
					2
				};
				count = new int[]
				{
					120000,
					300
				};
				Singleton<UiManager>.Instance.ShowAward(id, count, Singleton<GlobalData>.Instance.GetText("ERROR_COMPENSATE"));
			}
		}
	}

	public void OnclickClose()
	{
		//GMGSDK.ExitGame();
	}

	public void StartGame()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		this.Hide();
		Singleton<UiManager>.Instance.ShowTopBar(true);
		Singleton<UiManager>.Instance.ShowPage(PageName.MainPage, null);
		GameLogManager.SendPageLog(this.Name.ToString(), PageName.MainPage.ToString());
		Singleton<InApps>.Instance.Fetch();
	}

	public void OnOpitionClick()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		Singleton<UiManager>.Instance.ShowPage(PageName.OptionPage, null);
	}

	public void OnFacebookClick()
	{
		Singleton<GlobalData>.Instance.FacebookShare(delegate
		{
			DailyMissionSystem.SetDailyMission(DailyMissionType.FACEBOOK_SHARE, 1);
			ItemDataManager.SetCurrency(CommonDataType.DIAMOND, 20);
			int[] id = new int[]
			{
				2
			};
			int[] count = new int[]
			{
				20
			};
			Singleton<UiManager>.Instance.ShowAward(id, count, null);
			this.Refresh();
		});
	}

	public CanvasGroup Content;

	public Text TipText;

	public Button CloseButton;

	public Button FacebookButton;

	public Text UID;

	public Text Version;
}
