using System;
using System.Collections.Generic;
using ads;
using DataCenter;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class StorePage : GamePage
{
	private new void Awake()
	{
		for (int i = 0; i < 20; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("UI/StoreItem")) as GameObject;
			gameObject.transform.SetParent(this.ItemRoot);
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localPosition = Vector3.zero;
			StoreItem component = gameObject.GetComponent<StoreItem>();
			this.StoreItems.Add(component);
		}
		for (int j = 0; j < this.PageTags.Count; j++)
		{
			int tag = j;
			this.PageTags[j].onValueChanged.AddListener(delegate(bool isOn)
			{
				this.ClickOnTag(isOn, tag);
			});
		}
	}

	public void OnDisable()
	{
		Singleton<UiManager>.Instance.SetTopEnable(true, true);
	}

	public override void Show()
	{
		base.Show();
		base.transform.SetAsFirstSibling();
		Singleton<UiManager>.Instance.SetTopEnable(true, false);
		for (int i = 0; i < this.PageTags.Count; i++)
		{
			if (i == this.PageIndex)
			{
				this.PageTags[i].isOn = true;
			}
			else
			{
				this.PageTags[i].isOn = false;
			}
		}
	}

	public override void Refresh()
	{
		this.GiftName.text = Singleton<GlobalData>.Instance.GetText("GIFT");
		this.GoldName.text = Singleton<GlobalData>.Instance.GetText("GOLD");
		this.DiamondName.text = Singleton<GlobalData>.Instance.GetText("DIAMOND");
		this.BtnBoxTxt.text = Singleton<GlobalData>.Instance.GetText("OPENBOX");
		this.AdvertisementButton.gameObject.SetActive(Singleton<GlobalData>.Instance.StorePageAdvertisement > 0);
		Singleton<FontChanger>.Instance.SetFont(GiftName);
		Singleton<FontChanger>.Instance.SetFont(GoldName);
		Singleton<FontChanger>.Instance.SetFont(DiamondName);
		Singleton<FontChanger>.Instance.SetFont(BtnBoxTxt);
		this.RefreshItems();
	}

	private void RefreshItems()
	{
		switch (this.PageIndex)
		{
		case 0:
			this.StoreDatas = StoreDataManager.GetStoreList(4);
			break;
		case 1:
			this.StoreDatas = StoreDataManager.GetStoreList(1);
			break;
		case 2:
			this.StoreDatas = StoreDataManager.GetStoreList(2);
			break;
		case 3:
			this.StoreDatas = StoreDataManager.GetStoreList(3);
			break;
		}
		this.ItemRoot.GetComponent<RectTransform>().sizeDelta = new Vector2((float)(this.StoreDatas.Count * 250 + 20), 360f);
		this.ItemRoot.localPosition = new Vector3(-200f, 0f, 0f);
		for (int i = 0; i < this.StoreItems.Count; i++)
		{
			if (i < this.StoreDatas.Count)
			{
				this.StoreItems[i].gameObject.SetActive(true);
				this.StoreItems[i].Refresh(this.StoreDatas[i]);
			}
			else
			{
				this.StoreItems[i].gameObject.SetActive(false);
			}
		}
	}
	
	
	public void Product(StoreData data)
	{
		StoreDataManager.Buy(data.ID);
		if (data.ID == 9010)
		{
			UITick.SetVipTime();
			Singleton<GlobalData>.Instance.TodayVIPReward = 0;
		}
		else if (data.ID == 9013)
		{
			PlayerPrefs.SetInt("MONTHLY_CARD", 30);
			PlayerPrefs.SetInt("MONTHLY_CARD_DAILY_AWARD", 1);
		}
		Refresh();
		//this.Refresh(data);
		Singleton<UiManager>.Instance.ShowAward(data.ItemID, data.ItemCount, null);
	}
	
    private void OnFinished(bool completed, string advertiser)
    {
        Debug.Log("Closed rewarded from: " + advertiser + " -> Completed " + completed);
        if (completed == true)
        {
            Singleton<GlobalData>.Instance.StorePageAdvertisement--;
            this.AdvertisementButton.gameObject.SetActive(Singleton<GlobalData>.Instance.StorePageAdvertisement > 0);
            ItemDataManager.SetCurrency(CommonDataType.DIAMOND, 10);
            Singleton<UiManager>.Instance.ShowEarnMoneyEffect(0, 10, 0);
            Singleton<UiManager>.Instance.TopBar.Refresh();
        }

    }
    public void ClickOnAdvertisement()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
        //Advertisements.Instance.ShowRewardedVideo(OnFinished);
        Ads.ShowReward(() =>
        {
	        Singleton<GlobalData>.Instance.StorePageAdvertisement--;
	        this.AdvertisementButton.gameObject.SetActive(Singleton<GlobalData>.Instance.StorePageAdvertisement > 0);
	        ItemDataManager.SetCurrency(CommonDataType.DIAMOND, 10);
	        Singleton<UiManager>.Instance.ShowEarnMoneyEffect(0, 10, 0);
	        Singleton<UiManager>.Instance.TopBar.Refresh();    
        });
  //      Singleton<GlobalData>.Instance.ShowAdvertisement(-4, delegate
		//{
		//	Singleton<GlobalData>.Instance.StorePageAdvertisement--;
		//	this.AdvertisementButton.gameObject.SetActive(Singleton<GlobalData>.Instance.StorePageAdvertisement > 0);
		//	ItemDataManager.SetCurrency(CommonDataType.DIAMOND, 10);
		//	Singleton<UiManager>.Instance.ShowEarnMoneyEffect(0, 10, 0);
		//	Singleton<UiManager>.Instance.TopBar.Refresh();
		//}, null);
	}

	private void ClickOnTag(bool isOn, int tag)
	{
		this.PageIndex = tag;
		this.Refresh();
	}

	public void OnclickBox()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		if (CheckpointDataManager.GetCurrentCheckpoint().ID > 3)
		{
			this.Close();
			Singleton<UiManager>.Instance.CurrentPage.Hide();
			Singleton<UiManager>.Instance.ShowPage(PageName.BoxPage, null);
		}
		else
		{
			Singleton<UiManager>.Instance.ShowMessage(Singleton<GlobalData>.Instance.GetText("TIPBOX"), 0.5f);
		}
	}

	public Transform ItemRoot;

	public Text BtnBoxTxt;

	public Text GiftName;

	public Text GoldName;

	public Text DiamondName;

	public Button AdvertisementButton;

	public List<Toggle> PageTags = new List<Toggle>();

	[HideInInspector]
	public int PageIndex;
	
	[HideInInspector]
	public List<StoreData> StoreDatas = new List<StoreData>();


	private List<StoreItem> StoreItems = new List<StoreItem>();
}
