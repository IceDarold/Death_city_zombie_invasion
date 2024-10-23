using System;
using System.Collections.Generic;
using ads;
using DataCenter;
using DG.Tweening;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class LogonAwardPage : GamePage
{
	private new void Awake()
	{
		this.Awards = LogonAwardSystem.LogonAwards;
		this.LogonParentIndex = Mathf.Clamp((LogonAwardSystem.GetLogonDays() - 1) / 5, 0, 4);
		this.AwardsRoot.GetComponent<RectTransform>().sizeDelta = new Vector2(760f, 1210f);
		for (int i = 0; i < this.Awards.Count; i++)
		{
			int index = this.Awards[i].ID;
			LogonAwardChild logonAwardChild = UnityEngine.Object.Instantiate<LogonAwardChild>(this.ChildPrefab);
			logonAwardChild.transform.SetParent(this.AwardsRoot);
			logonAwardChild.transform.localScale = Vector3.one;
			logonAwardChild.transform.localPosition = Vector3.zero;
			logonAwardChild.ReceiveButton.onClick.AddListener(delegate()
			{
				this.GetAward(index);
			});
			this.LogonAwardChildren.Add(logonAwardChild);
		}
	}

	public override void Show()
	{
		base.Show();
		this.Content.alpha = 0f;
		this.Content.DOFade(1f, 0.5f);
	}

	public override void Refresh()
	{
		this.TitleName.text = Singleton<GlobalData>.Instance.GetText("DAILY_LOGON_AWARD");
		Singleton<FontChanger>.Instance.SetFont(TitleName);
		this.AwardsRoot.localPosition = new Vector3(0f, (float)(this.LogonParentIndex * 200 + 230), 0f);
		this.TopTag.SetActive(this.AwardsRoot.localPosition.y > 440f);
		this.DownTag.SetActive(this.AwardsRoot.localPosition.y < 780f);
		this.RefreshFinalAward();
		for (int i = 0; i < this.LogonAwardChildren.Count; i++)
		{
			this.LogonAwardChildren[i].init(this.Awards[i]);
		}
	}

	public override void Close()
	{
		base.Close();
		Singleton<UiManager>.Instance.CurrentPage.Refresh();
	}

	private void RefreshFinalAward()
	{
		WeaponData weaponData = WeaponDataManager.GetWeaponData(this.Awards[19].AwardItem);
		if (weaponData != null)
		{
			this.FinalAwardName.text = Singleton<GlobalData>.Instance.GetText(weaponData.Name) + "<size=24>(" + Singleton<GlobalData>.Instance.GetText(weaponData.Type.ToString()) + ")</size>";
			this.FinalAwardDescribe.text = Singleton<GlobalData>.Instance.GetText("LOGON_THIRTY_DAYS_AWARD");
			this.FinalAwardPower.text = Singleton<GlobalData>.Instance.GetText("POWER_MAX") + " : " + WeaponDataManager.GetMaxFightingStrength(weaponData);
			Singleton<FontChanger>.Instance.SetFont(FinalAwardName);
			Singleton<FontChanger>.Instance.SetFont(FinalAwardDescribe);
			Singleton<FontChanger>.Instance.SetFont(FinalAwardPower);
			this.FinalAwardIcon.sprite = Singleton<UiManager>.Instance.GetSprite(weaponData.Icon);
		}
	}
    
    public void GetAward(int index)
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		LogonAward award = LogonAwardSystem.GetLogonAward(index);
		if (award.State == 1)
		{
			ItemDataManager.CollectItem(award.AwardItem, award.AwardCount);
			LogonAwardSystem.SetAwardReceived(award);
			Singleton<UiManager>.Instance.TopBar.Refresh();
			int[] id = new int[]
			{
				award.AwardItem
			};
			int[] count = new int[]
			{
				award.AwardCount
			};
			Singleton<UiManager>.Instance.ShowAward(id, count, null);
			this.Refresh();
		}
		else if (award.State == 2 && award.ReceiveTimes == 1)
		{
            //Advertisements.Instance.ShowRewardedVideo(OnFinished3, );
            Ads.ShowReward(() =>
            {
	            award.ReceiveTimes++;
	            int[] array = new int[]
	            {
		            award.AwardItem
	            };
	            int[] array2 = new int[]
	            {
		            award.AwardCount
	            };
	            if (award.AwardItem >= 2000 && award.AwardItem < 3000)
	            {
		            array[0] = 1;
		            array2[0] = 5000;
	            }
	            ItemDataManager.CollectItem(array[0], array2[0]);
	            Singleton<UiManager>.Instance.TopBar.Refresh();
	            Singleton<UiManager>.Instance.ShowAward(array, array2, null);
	            this.Refresh();
            });
   //          Singleton<GlobalData>.Instance.ShowAdvertisement(-1, delegate
			// {
			// 	award.ReceiveTimes++;
			// 	int[] array = new int[]
			// 	{
			// 		award.AwardItem
			// 	};
			// 	int[] array2 = new int[]
			// 	{
			// 		award.AwardCount
			// 	};
			// 	if (award.AwardItem >= 2000 && award.AwardItem < 3000)
			// 	{
			// 		array[0] = 1;
			// 		array2[0] = 5000;
			// 	}
			// 	ItemDataManager.CollectItem(array[0], array2[0]);
			// 	Singleton<UiManager>.Instance.TopBar.Refresh();
			// 	Singleton<UiManager>.Instance.ShowAward(array, array2, null);
			// 	this.Refresh();
			// }, null);
		}
	}

	public void OnValueChange()
	{
		this.TopTag.SetActive(this.AwardsRoot.localPosition.y > 440f);
		this.DownTag.SetActive(this.AwardsRoot.localPosition.y < 780f);
	}
    private void OnFinished(bool completed, string advertiser)
    {
        Debug.Log("Closed rewarded from: " + advertiser + " -> Completed " + completed);
        if (completed == true)
        {
            LogonAward currentLogonAward = LogonAwardSystem.GetCurrentLogonAward();
            currentLogonAward.ReceiveTimes++;
            int[] array = new int[]
            {
                        currentLogonAward.AwardItem
            };
            int[] array2 = new int[]
            {
                        currentLogonAward.AwardCount
            };
            if (currentLogonAward.AwardItem >= 2000 && currentLogonAward.AwardItem < 3000)
            {
                array[0] = 1;
                array2[0] = 5000;
            }
            ItemDataManager.CollectItem(array[0], array2[0]);
            Singleton<UiManager>.Instance.TopBar.Refresh();
            Singleton<UiManager>.Instance.CurrentPage.Close();
            Singleton<UiManager>.Instance.ShowAward(array, array2, null);
        }

    }
    public void ClickOnClose()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.CancelClip, false);
		if (LogonAwardSystem.CanReceiveAgain())
		{
			Singleton<UiManager>.Instance.ShowRemind("RECEIVE_AGAIN", "WATCH", delegate
			{
				Ads.ShowReward(() =>
				{
					LogonAward currentLogonAward = LogonAwardSystem.GetCurrentLogonAward();
					currentLogonAward.ReceiveTimes++;
					int[] array = new int[]
					{
						currentLogonAward.AwardItem
					};
					int[] array2 = new int[]
					{
						currentLogonAward.AwardCount
					};
					if (currentLogonAward.AwardItem >= 2000 && currentLogonAward.AwardItem < 3000)
					{
						array[0] = 1;
						array2[0] = 5000;
					}
					ItemDataManager.CollectItem(array[0], array2[0]);
					Singleton<UiManager>.Instance.TopBar.Refresh();
					Singleton<UiManager>.Instance.CurrentPage.Close();
					Singleton<UiManager>.Instance.ShowAward(array, array2, null);
				});
                //Advertisements.Instance.ShowRewardedVideo(OnFinished);
    //            Singleton<GlobalData>.Instance.ShowAdvertisement(-5, delegate
				//{
				//	LogonAward currentLogonAward = LogonAwardSystem.GetCurrentLogonAward();
				//	currentLogonAward.ReceiveTimes++;
				//	int[] array = new int[]
				//	{
				//		currentLogonAward.AwardItem
				//	};
				//	int[] array2 = new int[]
				//	{
				//		currentLogonAward.AwardCount
				//	};
				//	if (currentLogonAward.AwardItem >= 2000 && currentLogonAward.AwardItem < 3000)
				//	{
				//		array[0] = 1;
				//		array2[0] = 5000;
				//	}
				//	ItemDataManager.CollectItem(array[0], array2[0]);
				//	Singleton<UiManager>.Instance.TopBar.Refresh();
				//	Singleton<UiManager>.Instance.CurrentPage.Close();
				//	Singleton<UiManager>.Instance.ShowAward(array, array2, null);
				//}, null);
			}, delegate
			{
				Singleton<UiManager>.Instance.CurrentPage.Close();
			});
		}
		else
		{
			this.Close();
		}
	}

	public CanvasGroup Content;

	public LogonAwardChild ChildPrefab;

	public Transform AwardsRoot;

	public Text TitleName;

	public GameObject DownTag;

	public GameObject TopTag;

	public Text FinalAwardName;

	public Text FinalAwardDescribe;

	public Text FinalAwardPower;

	public Image FinalAwardIcon;

	private int LogonParentIndex;

	private List<LogonAward> Awards = new List<LogonAward>();

	private List<LogonAwardChild> LogonAwardChildren = new List<LogonAwardChild>();
}
