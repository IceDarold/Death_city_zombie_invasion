using System;
using System.Collections.Generic;
using ads;
using DataCenter;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class DailyMissionChild : MonoBehaviour
{
	public void Refresh(DailyMission mission)
	{
		this._mission = mission;
		this.ids = (int[])this._mission.AwardID.Clone();
		this.counts = (int[])this._mission.AwardCount.Clone();
		this.Describe.text = this.GetDescribe();
		Singleton<FontChanger>.Instance.SetFont(Describe);
		this.Progress.text = mission.CurrentValue + "/" + mission.TargetValue;
		for (int i = 0; i < this.ids.Length; i++)
		{
			if (this.ids[i] == 1 || this.ids[i] == 2 || this.ids[i] == 3)
			{
				this.counts[i] = (int)((float)this.counts[i] * (1f + (float)(CheckpointDataManager.GetCurrentCheckpoint().Chapters - ChapterEnum.CHAPTERNAME_01) * 0.1f));
			}
		}
		this.AwardImage01.sprite = Singleton<UiManager>.Instance.GetSprite(ItemDataManager.GetItemData(this.ids[0]).Icon);
		this.AwardText01.text = this.counts[0].ToString();
		this.Award01.gameObject.SetActive(true);
		if (this.ids.Length > 1)
		{
			this.AwardImage02.sprite = Singleton<UiManager>.Instance.GetSprite(ItemDataManager.GetItemData(this.ids[1]).Icon);
			this.AwardText02.text = this.counts[1].ToString();
			this.Award02.gameObject.SetActive(true);
		}
		else
		{
			this.Award02.gameObject.SetActive(false);
		}
		int state = mission.State;
		if (state != 0)
		{
			if (state != 1)
			{
				if (state == 2)
				{
					this.FunctionButton.interactable = false;
					this.ButtonImage.gameObject.SetActive(false);
					this.ButtonName.text = Singleton<GlobalData>.Instance.GetText("COMPLETE");
					this.ButtonName.color = Color.white;
					this.VedioIcon.gameObject.SetActive(false);
				}
			}
			else
			{
				this.FunctionButton.interactable = true;
				this.ButtonImage.sprite = this.ButtonState[1];
				this.ButtonImage.gameObject.SetActive(true);
				this.ButtonName.text = Singleton<GlobalData>.Instance.GetText("RECEIVE");
				this.VedioIcon.gameObject.SetActive(false);
			}
		}
		else if (mission.Type == DailyMissionType.COMPLETE_ALL)
		{
			this.FunctionButton.interactable = false;
			this.ButtonImage.gameObject.SetActive(false);
			this.ButtonName.text = Singleton<GlobalData>.Instance.GetText("IN_PROGRESS");
			this.ButtonName.color = Color.white;
			this.VedioIcon.gameObject.SetActive(false);
		}
		else
		{
			this.FunctionButton.interactable = true;
			this.ButtonImage.sprite = this.ButtonState[0];
			this.ButtonImage.gameObject.SetActive(true);
			this.ButtonName.text = Singleton<GlobalData>.Instance.GetText("GO_TO");
			this.VedioIcon.gameObject.SetActive(mission.Type == DailyMissionType.WATCH_VIDEO);
		}
		Singleton<FontChanger>.Instance.SetFont(ButtonName);
	}

	private string GetDescribe()
	{
		if (string.IsNullOrEmpty(this._mission.Describe))
		{
			return string.Empty;
		}
		string text = Singleton<GlobalData>.Instance.GetText(this._mission.Describe);
		if (text.Contains("#value#"))
		{
			text = text.Replace("#value#", this._mission.TargetValue.ToString());
		}
		return text;
	}
    private void OnFinished(bool completed, string advertiser)
    {
        Debug.Log("Closed rewarded from: " + advertiser + " -> Completed " + completed);
        if (completed == true)
        {
            ItemDataManager.SetCurrency(CommonDataType.DIAMOND, 10);
            Singleton<UiManager>.Instance.ShowEarnMoneyEffect(0, 10, 0);
            Singleton<UiManager>.Instance.TopBar.Refresh();
            Singleton<UiManager>.Instance.GetPage(PageName.DailyMissionPage).Refresh();
        }

    }
    public void OnClick()
	{
		if (this._mission.State == 0)
		{
			switch (this._mission.Type)
			{
			case DailyMissionType.FACEBOOK_SHARE:
				Singleton<GlobalData>.Instance.FacebookShare(delegate
				{
					DailyMissionSystem.SetDailyMission(DailyMissionType.FACEBOOK_SHARE, 1);
					ItemDataManager.SetCurrency(CommonDataType.DIAMOND, 20);
					Singleton<UiManager>.Instance.ShowAward(new int[]
					{
						2
					}, new int[]
					{
						20
					}, null);
					Singleton<UiManager>.Instance.GetPage(PageName.DailyMissionPage).Refresh();
				});
				break;
			case DailyMissionType.WATCH_VIDEO:
                    //Advertisements.Instance.ShowRewardedVideo(OnFinished);
				Ads.ShowReward(() =>
				{
					DailyMissionSystem.SetDailyMission(DailyMissionType.WATCH_VIDEO, 1);
					//DailyMissionSystem.GetAward(this._mission);
				    // ItemDataManager.SetCurrency(CommonDataType.DIAMOND, 10);
				    // Singleton<UiManager>.Instance.ShowEarnMoneyEffect(0, 10, 0);
				    // Singleton<UiManager>.Instance.TopBar.Refresh();
				    Singleton<UiManager>.Instance.GetPage(PageName.DailyMissionPage).Refresh();
				});
    //                Singleton<GlobalData>.Instance.ShowAdvertisement(-1, delegate
				//{
				//	ItemDataManager.SetCurrency(CommonDataType.DIAMOND, 10);
				//	Singleton<UiManager>.Instance.ShowEarnMoneyEffect(0, 10, 0);
				//	Singleton<UiManager>.Instance.TopBar.Refresh();
				//	Singleton<UiManager>.Instance.GetPage(PageName.DailyMissionPage).Refresh();
				//}, null);
				break;
			case DailyMissionType.KILL_ZOMBIE:
			case DailyMissionType.HEAD_SHOOT:
			case DailyMissionType.FINISH_RANDOM_CHECKPOINT:
			case DailyMissionType.KILL_ELITE:
				if (Singleton<UiManager>.Instance.GetPage(PageName.DailyMissionPage) != null)
				{
					Singleton<UiManager>.Instance.GetPage(PageName.DailyMissionPage).Close();
				}
				if (Singleton<UiManager>.Instance.GetPage(PageName.MainPage) != null)
				{
					Singleton<UiManager>.Instance.GetPage(PageName.MainPage).Hide();
				}
				Singleton<UiManager>.Instance.ShowPage(PageName.MapsPage, null);
				if (MapsPage.instance != null)
				{
					MapsPage.instance.OnclickRandomLevel();
				}
				break;
			case DailyMissionType.OPEN_BOXES:
				if (Singleton<UiManager>.Instance.GetPage(PageName.DailyMissionPage) != null)
				{
					Singleton<UiManager>.Instance.GetPage(PageName.DailyMissionPage).Close();
				}
				if (Singleton<UiManager>.Instance.GetPage(PageName.MainPage) != null)
				{
					Singleton<UiManager>.Instance.GetPage(PageName.MainPage).Hide();
				}
				Singleton<UiManager>.Instance.ShowPage(PageName.BoxPage, null);
				break;
			case DailyMissionType.FINISH_GOLD_CHECKPOINT:
				if (Singleton<UiManager>.Instance.GetPage(PageName.DailyMissionPage) != null)
				{
					Singleton<UiManager>.Instance.GetPage(PageName.DailyMissionPage).Close();
				}
				if (Singleton<UiManager>.Instance.GetPage(PageName.MainPage) != null)
				{
					Singleton<UiManager>.Instance.GetPage(PageName.MainPage).Hide();
				}
				Singleton<UiManager>.Instance.ShowPage(PageName.MapsPage, null);
				if (MapsPage.instance != null)
				{
					MapsPage.instance.OnclickGoldLevel();
				}
				break;
			case DailyMissionType.FINISH_BOSS_CHECKPOINT:
				if (Singleton<UiManager>.Instance.GetPage(PageName.DailyMissionPage) != null)
				{
					Singleton<UiManager>.Instance.GetPage(PageName.DailyMissionPage).Close();
				}
				if (Singleton<UiManager>.Instance.GetPage(PageName.MainPage) != null)
				{
					Singleton<UiManager>.Instance.GetPage(PageName.MainPage).Hide();
				}
				Singleton<UiManager>.Instance.ShowPage(PageName.MapsPage, null);
				if (MapsPage.instance != null)
				{
					MapsPage.instance.OnclickBossLevel();
				}
				break;
			}
		}
		else if (this._mission.State == 1)
		{
			DailyMissionSystem.GetAward(this._mission);
			for (int i = 0; i < this.ids.Length; i++)
			{
				if (this.ids[i] >= 520 && this.ids[i] <= 525)
				{
					List<DebrisData> weaponDebrisList = DebrisDataManager.GetWeaponDebrisList(this.ids[i] - 520);
					if (weaponDebrisList.Count > 0)
					{
						this.ids[i] = weaponDebrisList[UnityEngine.Random.Range(0, weaponDebrisList.Count)].ID;
						this.counts[i] = 1;
					}
					else
					{
						this.ids[i] = 1;
						this.counts[i] = 5000;
					}
				}
				ItemDataManager.CollectItem(this.ids[i], this.counts[i]);
			}
			Singleton<UiManager>.Instance.ShowAward(this.ids, this.counts, null);
			Singleton<UiManager>.Instance.GetPage(PageName.DailyMissionPage).Refresh();
		}
	}

	public Text Describe;

	public Text Progress;

	public GameObject Award01;

	public Image AwardImage01;

	public Text AwardText01;

	public GameObject Award02;

	public Image AwardImage02;

	public Text AwardText02;

	public Button FunctionButton;

	public Image ButtonImage;

	public Text ButtonName;

	public Image VedioIcon;

	public Sprite[] ButtonState;

	private DailyMission _mission;

	private int[] ids;

	private int[] counts;
}
