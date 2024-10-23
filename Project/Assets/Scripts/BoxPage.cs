using System;
using System.Collections;
using System.Collections.Generic;
using ads;
using DataCenter;
using DG.Tweening;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class BoxPage : GamePage
{
	public new void OnEnable()
	{
		this.canvasGroup.alpha = 0f;
		Singleton<UiManager>.Instance.SetTopEnable(false, false);
        //Singleton<GlobalData>.Instance.ShowAdvertisement(13, null, null);
        Ads.ShowInter();
        //Advertisements.Instance.ShowInterstitial();
		this.canvasGroup.DOFade(1f, 0.5f).OnComplete(delegate
		{
			Singleton<UiManager>.Instance.SetTopEnable(true, true);
			this.InitOpen();
		});
		if (ItemDataManager.GetCurrency(CommonDataType.BOX_GOLDEN) > 0)
		{
			this.NoGo.SetActive(false);
			this.HaveGo.SetActive(true);
			this.DesTxt.text = Singleton<GlobalData>.Instance.GetText("HAVEGOT") + ":";
			Singleton<FontChanger>.Instance.SetFont(DesTxt);
			this.HaveOpenTxt.text = Singleton<GlobalData>.Instance.GetText("OPEN");
			this.NumTxt.text = ItemDataManager.GetCurrency(CommonDataType.BOX_GOLDEN).ToString();
			Singleton<FontChanger>.Instance.SetFont(NumTxt);
		}
		else
		{
			this.NoGo.SetActive(true);
			this.HaveGo.SetActive(false);
			this.HaveOpenTxt.gameObject.SetActive(false);
		}
		this.PushTitleTxt.text = Singleton<GlobalData>.Instance.GetText("Arbitrary_consumption");
		this.PushTxt.text = Singleton<GlobalData>.Instance.GetText("GIVE_3_GOLDBOX");
		this.ChooseTxt.text = Singleton<GlobalData>.Instance.GetText("CHOOSE");
		this.CardTxt.text = Singleton<GlobalData>.Instance.GetText("CARD");
		this.PlayVideoTxt.text = Singleton<GlobalData>.Instance.GetText("FREE");
		this.RemindTxt.text = Singleton<GlobalData>.Instance.GetText("REMIND");
		this.ContinueTxt.text = Singleton<GlobalData>.Instance.GetText("CONTINUE");
		Singleton<FontChanger>.Instance.SetFont(PushTitleTxt);
		Singleton<FontChanger>.Instance.SetFont(PushTxt);
		Singleton<FontChanger>.Instance.SetFont(ChooseTxt);
		Singleton<FontChanger>.Instance.SetFont(CardTxt);
		Singleton<FontChanger>.Instance.SetFont(PlayVideoTxt);
		Singleton<FontChanger>.Instance.SetFont(RemindTxt);
		Singleton<FontChanger>.Instance.SetFont(ContinueTxt);
		this.VipGO.SetActive(PlayerPrefs.GetString("FirstPay", "true") == "true");
		this.BoxParticle[0].SetActive(true);
		for (int i = 1; i < this.BoxParticle.Length; i++)
		{
			this.BoxParticle[i].SetActive(false);
		}
		this.num = 4;
		this.BoxGo.SetActive(true);
		this.TitleGo02.SetActive(false);
		this.NumCountTxt.text = this.num.ToString();
		if (Singleton<GlobalData>.Instance.SliverBoxAdvertisement > 0)
		{
			this.SilverBoxButton.interactable = true;
			this.SliverButtonIcon.gameObject.SetActive(true);
			this.SilverBoxTimer.gameObject.SetActive(false);
			this.TDBtnTxt.gameObject.SetActive(true);
			this.TDBtnTxt.text = Singleton<GlobalData>.Instance.GetText("FREE");
			Singleton<FontChanger>.Instance.SetFont(TDBtnTxt);
		}
		else
		{
			this.SilverBoxButton.interactable = false;
			this.SliverButtonIcon.gameObject.SetActive(false);
			this.TDBtnTxt.gameObject.SetActive(false);
			this.SilverBoxTimer.gameObject.SetActive(true);
		}
		this.GoldBtnTxt.text = this.GoldCost.ToString();
		this.CardGo.SetActive(false);
		if (UITick.getFreeBoxNeedSec().Length == 0 && UITick.getVideoBoxNum() == 3)
		{
			UITick.setVideoBoxNum(0);
			this.FreeBtn.enabled = true;
			this.FreeBtn.GetComponent<Image>().sprite = Singleton<UiManager>.Instance.CommonSprites[7];
		}
		else if (UITick.getFreeBoxNeedSec().Length == 0 && UITick.getVideoBoxNum() < 3)
		{
			this.FreeBtn.enabled = true;
			this.FreeBtn.GetComponent<Image>().sprite = Singleton<UiManager>.Instance.CommonSprites[7];
		}
		else
		{
			this.FreeBtn.enabled = false;
			this.FreeBtn.GetComponent<Image>().sprite = Singleton<UiManager>.Instance.CommonSprites[2];
		}
		this.jiantou.SetActive(Singleton<GlobalData>.Instance.FirstBox > 0);
		this.CheckTeach();
		if (null == this.curTeachPage && AchievementDataManager.GetAchievementData(DataCenter.AchievementType.OPEN_BOXES).CurrentValue == 1 && Singleton<GlobalData>.Instance.FirAdvice == 1)
		{
			//TODO: Five Star popup
			//Singleton<UiManager>.Instance.ShowPage(PageName.FiveStartPopup, null);
			Singleton<GlobalData>.Instance.FirAdvice = 0;
		}
	}

	public void CheckTeach()
	{
		if (Singleton<GlobalData>.Instance.FirstBoxGuide > 0 && null != Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage) && Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>().type == TeachUIType.MainBox)
		{
			this.jiantou.SetActive(false);
			this.curTeachPage = Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>();
			this.curTeachPage.type = TeachUIType.Box;
			this.curTeachPage.RefreshPage();
		}
		else
		{
			this.curTeachPage = null;
		}
	}

	public void InitOpen()
	{
		if (ItemDataManager.GetCurrency(CommonDataType.BOX_SILVER) > 0)
		{
			this.OpenBoxAni(1);
			ItemDataManager.SetCurrency(CommonDataType.BOX_SILVER, -1);
		}
		else if (ItemDataManager.GetCurrency(CommonDataType.BOX_COPPER) > 0)
		{
			this.OpenBoxAni(0);
			ItemDataManager.SetCurrency(CommonDataType.BOX_COPPER, -1);
		}
	}

	public override void Close()
	{
		if (this.num > 0 && this.CardGo.activeSelf)
		{
			return;
		}
		UnityEngine.Debug.Log(Singleton<UiManager>.Instance.PageStack.Peek().Name);
		base.Close();
		Singleton<UiManager>.Instance.RemovePage(this.Name);
	}

	public override void OnBack()
	{
		base.OnBack();
	}

	public void OnDisable()
	{
		Singleton<UiManager>.Instance.SetTopEnable(true, true);
	}

	public void Update()
	{
		if (UITick.getFreeBoxNeedSec().Length == 0 && UITick.getVideoBoxNum() == 3)
		{
			UITick.setVideoBoxNum(0);
			this.FreeBtn.enabled = true;
			this.FreeBtn.GetComponent<Image>().sprite = Singleton<UiManager>.Instance.CommonSprites[7];
		}
	}

	public void OpenBoxAni(int num)
	{
		Singleton<UiManager>.Instance.SetTopEnable(false, false);
		AchievementDataManager.SetAchievementValue(DataCenter.AchievementType.OPEN_BOXES, 1);
		DailyMissionSystem.SetDailyMission(DailyMissionType.OPEN_BOXES, 1);
		this.UnlockWeapon.Clear();
		this.isAlreadyUnlock.Clear();
		this.numBox = num;
		if (num != 0)
		{
			if (num != 1)
			{
				if (num == 2)
				{
					GameLogManager.SendPageLog("GOLD", "OPEN");
				}
			}
			else
			{
				GameLogManager.SendPageLog("Sliver", "OPEN");
			}
		}
		else
		{
			GameLogManager.SendPageLog("Wooden", "OPEN");
		}
		this.CurrentBg.DOColor(this.CurrentColor, 1.6f);
		BoxDataManager.CensusAllOpenCount(num, 1);
		if (BoxDataManager.getAllOpenCount(num) > 2)
		{
			this.BoxItemList = BoxDataManager.GetBoxDatas(num, false);
		}
		else
		{
			this.BoxItemList = BoxDataManager.GetBoxDatas(num, true);
		}
		for (int i = 0; i < this.BoxItemList.Count; i++)
		{
			if (this.BoxItemList[i].ItemID >= 8200 && this.BoxItemList[i].ItemID < 8300)
			{
				DebrisData debrisData = DebrisDataManager.GetDebrisData(this.BoxItemList[i].ItemID);
				WeaponData weaponData = WeaponDataManager.GetWeaponData(debrisData.ItemID);
			}
			ItemDataManager.CollectItem(this.BoxItemList[i].ItemID, this.BoxItemList[i].ItemCount);
		}
		this.BoxTreasure[num].transform.SetParent(this.CenterPoint);
		this.BoxTreasure[num].transform.GetComponent<DOTweenAnimation>().DOPause();
		this.BoxGo.SetActive(false);
		Singleton<UiManager>.Instance.ShowTopBar(false);
		base.StartCoroutine(this.ShowOpenBox(num));
	}

	private IEnumerator ShowOpenBox(int index)
	{
		this.BoxTreasure[index].transform.DOScale(new Vector3(1200f, 1200f, 1200f), 0.3f);
		this.BoxTreasure[index].transform.DOLocalMove(Vector3.zero, 0.3f, false);
		yield return new WaitForSeconds(0.3f);
		this.BoxAni[index].Play("Open");
		this.BoxParticle[0].SetActive(false);
		yield return new WaitForSeconds(1.1f);
		this.BoxParticle[index + 1].SetActive(true);
		yield break;
	}

	public void FinishAnimator(int num)
	{
		base.Invoke("InitCard", 0.2f);
		this.BoxTreasure[num].transform.DOScale(Vector3.zero, 0.5f).OnComplete(delegate
		{
			this.CardGo.SetActive(true);
		});
	}

	private void InitCard()
	{
		for (int i = 0; i < this.num; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("UI/CardItem")) as GameObject;
			gameObject.transform.SetParent(this.Layer);
			gameObject.transform.localScale = Vector3.zero;
			gameObject.transform.localPosition = new Vector3(0f, 400f, 0f);
			CardItem component = gameObject.GetComponent<CardItem>();
			component.data = ItemDataManager.GetItemData(this.BoxItemList[i].ItemID);
			component.CurBox = this;
			component.BoxDataId = this.BoxItemList[i].ID;
			this.listCard.Add(component);
			component.RefreshPage();
		}
		this.TitleGo02.SetActive(false);
		this.ContinueGo.SetActive(false);
		base.StartCoroutine(this.MoveCard());
	}

	private IEnumerator MoveCard()
	{
		for (int i = 0; i < this.listCard.Count; i++)
		{
			this.listCard[i].transform.DOLocalMove(new Vector3(this.cardPosX[i], this.cardPosy, 0f), 0.5f, false);
			this.listCard[i].transform.DOScale(Vector3.one, 0.5f).OnComplete(delegate
			{
			});
			yield return new WaitForSeconds(0.1f);
		}
		yield break;
	}

	public void OnclickContinue()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		this.BoxTreasure[this.numBox].transform.GetComponent<DOTweenAnimation>().DOPlay();
		UnityEngine.Debug.Log("1" + Singleton<UiManager>.Instance.PageStack.Peek().name);
		Singleton<UiManager>.Instance.PageStack.Pop();
		UnityEngine.Debug.Log("2" + Singleton<UiManager>.Instance.PageStack.Peek().name);
		Singleton<UiManager>.Instance.RemovePage(this.Name);
		Singleton<UiManager>.Instance.ShowTopBar(true);
		Singleton<UiManager>.Instance.SetTopEnable(true, true);
		Singleton<UiManager>.Instance.ShowPage(PageName.BoxPage, null);
	}

	public void OpenFreeBox()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(this.QualityAudio[0], false);
		if (UITick.getVideoBoxNum() < 3)
		{
			UITick.setVideoBoxNum(1);
			if (UITick.getVideoBoxNum() == 3)
			{
				UITick.setVideoBoxSec();
				this.FreeBtn.enabled = false;
				this.FreeBtn.GetComponent<Image>().sprite = Singleton<UiManager>.Instance.CommonSprites[2];
			}
			this.OpenBoxAni(0);
		}
	}

	public void OnclickFreeDia()
	{
        //Advertisements.Instance.ShowRewardedVideo(OnFinished);
        Ads.ShowReward(() =>
        {
	        ItemDataManager.SetCurrency(CommonDataType.DIAMOND, 10);
	        Singleton<UiManager>.Instance.ShowEarnMoneyEffect(0, 10, 0);
	        Singleton<UiManager>.Instance.TopBar.Refresh();    
        });
  //      Singleton<GlobalData>.Instance.ShowAdvertisement(-3, delegate
		//{
		//	ItemDataManager.SetCurrency(CommonDataType.DIAMOND, 10);
		//	Singleton<UiManager>.Instance.ShowEarnMoneyEffect(0, 10, 0);
		//	Singleton<UiManager>.Instance.TopBar.Refresh();
		//}, null);
	}
    private void OnFinished(bool completed, string advertiser)
    {
        Debug.Log("Closed rewarded from: " + advertiser + " -> Completed " + completed);
        if (completed == true)
        {
            ItemDataManager.SetCurrency(CommonDataType.DIAMOND, 10);
            Singleton<UiManager>.Instance.ShowEarnMoneyEffect(0, 10, 0);
            Singleton<UiManager>.Instance.TopBar.Refresh();
        }

    }
    public void OnclickVip()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		Singleton<UiManager>.Instance.PageStack.Pop();
		Singleton<UiManager>.Instance.RemovePage(this.Name);
		Singleton<UiManager>.Instance.ShowStorePage(0);
		Singleton<UiManager>.Instance.SetTopEnable(true, true);
	}

	public void OnclickFreeBox()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
        //Advertisements.Instance.ShowRewardedVideo(OnFinished2);
        Ads.ShowReward(OpenFreeBox);
        //Singleton<GlobalData>.Instance.ShowAdvertisement(-3, delegate
        //{
        //	this.OpenFreeBox();
        //}, null);
    }
    private void OnFinished2(bool completed, string advertiser)
    {
        Debug.Log("Closed rewarded from: " + advertiser + " -> Completed " + completed);
        if (completed == true)
        {
            this.OpenFreeBox();
        }

    }
    private void OnFinished3(bool completed, string advertiser)
    {
        Debug.Log("Closed rewarded from: " + advertiser + " -> Completed " + completed);
        if (completed == true)
        {
            if (Singleton<GlobalData>.Instance.FirstBox > 0)
            {
                Singleton<GlobalData>.Instance.FirstBox = 0;
                Singleton<GlobalData>.Instance.FirstBoxGuide = 0;
            }
            Singleton<GlobalData>.Instance.SliverBoxAdvertisement--;
            this.OpenBoxAni(1);
        }

    }
    public void OnclickTDBox()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(this.QualityAudio[1], false);
		if (Singleton<GlobalData>.Instance.SliverBoxAdvertisement > 0)
        {
            //Advertisements.Instance.ShowRewardedVideo(OnFinished3);
            Ads.ShowReward(() =>
            {
	            if (Singleton<GlobalData>.Instance.FirstBox > 0)
	            {
	            	Singleton<GlobalData>.Instance.FirstBox = 0;
	            	Singleton<GlobalData>.Instance.FirstBoxGuide = 0;
	            }
	            Singleton<GlobalData>.Instance.SliverBoxAdvertisement--;
	            this.OpenBoxAni(1);
            });
   //         Singleton<GlobalData>.Instance.ShowAdvertisement(-1, delegate
			//{
			//	if (Singleton<GlobalData>.Instance.FirstBox > 0)
			//	{
			//		Singleton<GlobalData>.Instance.FirstBox = 0;
			//		Singleton<GlobalData>.Instance.FirstBoxGuide = 0;
			//	}
			//	Singleton<GlobalData>.Instance.SliverBoxAdvertisement--;
			//	this.OpenBoxAni(1);
			//}, null);
		}
		if (null != this.curTeachPage)
		{
			Singleton<GlobalData>.Instance.FirstBoxGuide = 0;
			this.curTeachPage.Button.SetActive(false);
			UnityEngine.Object.Destroy(this.curTeachPage.Button);
			this.curTeachPage.Close();
		}
	}

	public void OnclickGoldBox()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(this.QualityAudio[2], false);
		if (ItemDataManager.GetCurrency(CommonDataType.BOX_GOLDEN) > 0)
		{
			ItemDataManager.SetCurrency(CommonDataType.BOX_GOLDEN, -1);
			this.OpenBoxAni(2);
		}
		else if (ItemDataManager.GetCurrency(CommonDataType.DIAMOND) >= this.GoldCost)
		{
			ItemDataManager.SetCurrency(CommonDataType.DIAMOND, -this.GoldCost);
			GameLogManager.SendCostLog(2, this.GoldCost, 2, 0);
			GameLogManager.SendPageLog("GOLD", "DIMONDOPEN");
			Singleton<UiManager>.Instance.TopBar.Refresh();
			this.OpenBoxAni(2);
		}
		else
		{
			Singleton<UiManager>.Instance.ShowLackOfMoney(CommonDataType.DIAMOND, this.GoldCost - ItemDataManager.GetCurrency(CommonDataType.DIAMOND));
			GameLogManager.SendPageLog(this.Name.ToString(), PageName.LackOfMoneyPopup.ToString());
		}
	}

	public Animator[] BoxAni;

	public GameObject BoxGo;

	public GameObject CardGo;

	public GameObject TitleGo01;

	public GameObject TitleGo02;

	public GameObject ContinueGo;

	public Transform TDBoxTrans;

	public GameObject jiantou;

	public Transform Layer;

	public Text NumRemindTxt;

	public Text NumCountTxt;

	public int num = 4;

	public Button FreeBtn;

	public GameObject VipGO;

	public Transform CenterPoint;

	public GameObject[] BoxTreasure;

	public GameObject[] BoxParticle;

	public Button SilverBoxButton;

	public Image SilverBoxIcon;

	public Text SilverBoxTimer;

	public UILabelTick TimeSliver;

	public UILabelTick FreeTime;

	public GameObject BoxCamera;

	public Image CurrentBg;

	public Color CurrentColor;

	public GameObject NoGo;

	public GameObject HaveGo;

	private int TDCost = 200;

	private int GoldCost = 100;

	private float[] cardPosX = new float[]
	{
		-390f,
		-130f,
		130f,
		390f
	};

	private float cardPosy = 0.5f;

	private List<CardItem> listCard = new List<CardItem>();

	public CanvasGroup canvasGroup;

	[CNName("品质音效，由低到高")]
	public AudioClip[] QualityAudio;

	private int numBox;

	private UITeachPage curTeachPage;

	public Text PlayVideoTxt;

	public Text GetTxt;

	public Text PushTitleTxt;

	public Text PushTxt;

	public Text ContinueTxt;

	public Text ChooseTxt;

	public Text CardTxt;

	public Text RemindTxt;

	public Text TDBtnTxt;

	public Text GoldBtnTxt;

	public Text HaveOpenTxt;

	public Text DesTxt;

	public Text NumTxt;

	public Image SliverButtonIcon;

	private List<BoxData> BoxItemList = new List<BoxData>();

	private List<WeaponData> UnlockWeapon = new List<WeaponData>();

	private List<bool> isAlreadyUnlock = new List<bool>();
}
