using System;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UiManager : Singleton<UiManager>
{
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.GamePageDic.Add(PageName.MapsPage, this.MapsPage);
		this.GamePageDic.Add(PageName.FunctionPage, this.FunctionPage);
		this.CollectSprites();
	}

	private void CollectSprites()
	{
		for (int i = 0; i < this.CommonSprites.Length; i++)
		{
			this.GameIcons.Add(this.CommonSprites[i].name, this.CommonSprites[i]);
		}
		for (int j = 0; j < this.ItemSprites.Length; j++)
		{
			this.GameIcons.Add(this.ItemSprites[j].name, this.ItemSprites[j]);
		}
		for (int k = 0; k < this.StoreSprites.Length; k++)
		{
			this.GameIcons.Add(this.StoreSprites[k].name, this.StoreSprites[k]);
		}
		for (int l = 0; l < this.GiftSprites.Length; l++)
		{
			this.GameIcons.Add(this.GiftSprites[l].name, this.GiftSprites[l]);
		}
		for (int m = 0; m < this.TalentIcons.Length; m++)
		{
			this.GameIcons.Add(this.TalentIcons[m].name, this.TalentIcons[m]);
		}
	}

	public Sprite GetSprite(string name)
	{
		if (Singleton<GlobalData>.Instance.GetCurrentLanguage() == LanguageEnum.Russian)
		{
			if (this.GameIcons.ContainsKey(name + "_RU"))
			{
				return this.GameIcons[name + "_RU"];
			}
		}
		
		if (this.GameIcons.ContainsKey(name))
		{
			return this.GameIcons[name];
		}
		UnityEngine.Debug.LogError("Key : " + name + " ----不包含该键值");
		return null;
	}

	[Obsolete("使用CurrentPage替代")]
	public GamePage GetCurrentPage()
	{
		if (this.PopupStack.Count > 0)
		{
			return this.PopupStack.Peek();
		}
		if (this.PageStack.Count > 0)
		{
			return this.PageStack.Peek();
		}
		return null;
	}

	private void CreatePage<T>() where T : GamePage
	{
		string text = typeof(T).ToString();
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("UI/" + text.ToString())) as GameObject;
		gameObject.name = text.ToString();
		GamePage component = gameObject.GetComponent<GamePage>();
		if (component.Type == PageType.Normal)
		{
			gameObject.transform.SetParent(this.PageRoot);
			gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
		}
		else if (component.Type == PageType.Popup)
		{
			gameObject.transform.SetParent(this.PopupRoot);
			gameObject.transform.localPosition = new Vector3(0f, 0f, -1000f);
		}
		else if (component.Type == PageType.Fixed)
		{
			gameObject.transform.SetParent(this.FixedRoot);
			gameObject.transform.localPosition = new Vector3(0f, 0f, -2000f);
		}
		gameObject.GetComponent<RectTransform>().offsetMin = Vector2.zero;
		gameObject.GetComponent<RectTransform>().offsetMax = Vector2.zero;
		gameObject.transform.localScale = Vector3.one;
	}

	private void CreatePage(PageName name)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("UI/" + name.ToString())) as GameObject;
		gameObject.name = name.ToString();
		GamePage component = gameObject.GetComponent<GamePage>();
		if (component.Type == PageType.Normal)
		{
			gameObject.transform.SetParent(this.PageRoot);
			gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
		}
		else if (component.Type == PageType.Popup)
		{
			gameObject.transform.SetParent(this.PopupRoot);
			gameObject.transform.localPosition = new Vector3(0f, 0f, -1000f);
		}
		else if (component.Type == PageType.Fixed)
		{
			gameObject.transform.SetParent(this.FixedRoot);
			gameObject.transform.localPosition = new Vector3(0f, 0f, -2000f);
		}
		gameObject.GetComponent<RectTransform>().offsetMin = Vector2.zero;
		gameObject.GetComponent<RectTransform>().offsetMax = Vector2.zero;
		gameObject.transform.localScale = Vector3.one;
		this.GamePageDic.Add(name, component);
	}

	public void ShowPage(PageName name, UnityAction option = null)
	{
		if (!this.GamePageDic.ContainsKey(name))
		{
			this.CreatePage(name);
		}
		if (this.GamePageDic[name].Type != PageType.Fixed)
		{
			this.CurrentPage = this.GamePageDic[name];
		}
		if (option != null)
		{
			option();
		}
		this.GamePageDic[name].Show();
	}

	public T ShowPage<T>(PageName name, UnityAction option = null) where T : GamePage
	{
		if (!this.GamePageDic.ContainsKey(name))
		{
			this.CreatePage(name);
		}
		if (this.GamePageDic[name].Type != PageType.Fixed)
		{
			this.CurrentPage = this.GamePageDic[name];
		}
		if (option != null)
		{
			option();
		}
		this.GamePageDic[name].Show();
		return this.GamePageDic[name].GetComponent<T>();
	}

	public GamePage GetPage(PageName name)
	{
		if (!this.GamePageDic.ContainsKey(name))
		{
			return null;
		}
		return this.GamePageDic[name];
	}

	public void ClosePage(PageType type, UnityAction call = null)
	{
		if (type != PageType.Fixed)
		{
			this.CurrentPage.Close();
		}
	}

	public void RemovePage(PageName name)
	{
		if (this.GamePageDic.ContainsKey(name))
		{
			GamePage gamePage = this.GamePageDic[name];
			this.GamePageDic.Remove(name);
			UnityEngine.Object.DestroyImmediate(gamePage.gameObject);
		}
		else
		{
			UnityEngine.Debug.LogError("不包含该键值");
		}
	}

	public void SetCurrentPage()
	{
		if (this.PopupStack.Count > 0)
		{
			this.CurrentPage = this.PopupStack.Peek();
		}
		else if (this.PageStack.Count > 0)
		{
			this.CurrentPage = this.PageStack.Peek();
		}
		else
		{
			this.CurrentPage = null;
		}
	}

	public void SetUIEnable(bool isEnable)
	{
		this.eventSystem.enabled = isEnable;
	}

	public void ShowLoadingPage(string scene, PageName page)
	{
		base.StartCoroutine(this.LoadScene(scene, page));
	}

	private IEnumerator LoadScene(string scene, PageName page)
	{
		LoadingPage loading = null;
		this.ShowPage(PageName.LoadingPage, delegate()
		{
			loading = this.GetPage(PageName.LoadingPage).GetComponent<LoadingPage>();
			loading.NextScene = scene;
			loading.NextPage = page;
			loading.startLoad = false;
		});
		yield return new WaitForSeconds(0.2f);
		AsyncOperation op = SceneManager.LoadSceneAsync("loading");
		while (!op.isDone)
		{
			yield return new WaitForEndOfFrame();
		}
		op.allowSceneActivation = true;
		loading.startLoad = true;
		loading.Show();
		yield break;
	}

	public void ShowTopBar(bool open)
	{
		if (!this.GamePageDic.ContainsKey(PageName.TopBar))
		{
			this.CreatePage(PageName.TopBar);
		}
		this.TopBar = this.GamePageDic[PageName.TopBar].gameObject.GetComponent<TopBarPage>();
		if (open)
		{
			this.TopBar.transform.SetAsFirstSibling();
			this.TopBar.Show();
		}
		else
		{
			this.TopBar.Hide();
		}
	}

	public void SetTopEnable(bool back, bool info = true)
	{
		if (this.TopBar != null)
		{
			this.TopBar.BackButton.interactable = back;
			this.CanBack = back;
			this.TopBar.EnergyButton.interactable = info;
			this.TopBar.PlayerInfoButton.interactable = info;
			this.TopBar.GoldButton.interactable = info;
			this.TopBar.DiamondButton.interactable = info;
			this.TopBar.DNAButton.interactable = info;
		}
	}

	public void ShowStorePage(int index = 0)
	{
		this.ShowPage(PageName.StorePage, delegate()
		{
			StorePage component = this.GetPage(PageName.StorePage).GetComponent<StorePage>();
			component.PageIndex = index;
		});
	}

	public void ShowDialogue(int type, int tag, int index = 0)
	{
		this.ShowPage(PageName.DialoguePage, delegate()
		{
			DialoguePage component = this.GetPage(PageName.DialoguePage).GetComponent<DialoguePage>();
			component.Dialogues = DialogueDataManager.GetDialogue(type, tag);
			component.index = index;
		});
	}

	public void ShowDayTaskMessage(string mes)
	{
		this.ShowPage(PageName.DayTaskPopup, delegate()
		{
			DayTaskPopup component = this.GetPage(PageName.DayTaskPopup).gameObject.GetComponent<DayTaskPopup>();
			component.TitleTxt.text = mes;
		});
	}

	public void ShowMessage(string mes, float t = 0.5f)
	{
		this.ShowPage(PageName.MessagePopup, delegate()
		{
			MessagePopup component = this.GetPage(PageName.MessagePopup).gameObject.GetComponent<MessagePopup>();
			component.Message.text = mes;
			Singleton<FontChanger>.Instance.SetFont(component.Message);
			component.LastTime = t;
		});
	}

	public void ShowPayPromot(CommonDataType type, int num, string content, UnityAction action)
	{
		this.ShowPage(PageName.PayPromptPopup, delegate()
		{
			PayPromptPopup component = this.GetPage(PageName.PayPromptPopup).gameObject.GetComponent<PayPromptPopup>();
			component.currencyType = type;
			component.Count = num;
			component.SomeThing = content;
			component.action = action;
		});
	}

	public void ShowLackOfMoney(CommonDataType type, int num)
	{
		this.ShowPage(PageName.LackOfMoneyPopup, delegate()
		{
			LackOfMoneyPopup component = this.GetPage(PageName.LackOfMoneyPopup).gameObject.GetComponent<LackOfMoneyPopup>();
			component.currencyType = type;
			if (type != CommonDataType.REVIVE_COIN)
			{
				component.ChargePointID = this.GetLackCurrency(type, num);
			}
			else
			{
				component.ChargePointID = StoreDataManager.GetStoreData(9022).ChargePoint;
			}
		});
	}

	public int GetLackCurrency(CommonDataType type, int num)
	{
		int num2 = 0;
		int type2 = 0;
		if (type == CommonDataType.GOLD)
		{
			type2 = 1;
		}
		else if (type == CommonDataType.DIAMOND)
		{
			type2 = 2;
		}
		else if (type == CommonDataType.DNA)
		{
			type2 = 3;
		}
		List<StoreData> storeList = StoreDataManager.GetStoreList(type2);
		for (int i = 0; i < storeList.Count; i++)
		{
			if (num < storeList[i].ItemCount[0])
			{
				num2 = storeList[i].ChargePoint;
				break;
			}
		}
		if (num2 == 0)
		{
			num2 = storeList[storeList.Count - 1].ChargePoint;
		}
		return num2;
	}

	public void ShowRemind(string remind, string name, UnityAction _confirm, UnityAction _cancel = null)
	{
		this.ShowPage(PageName.RemindPopup, delegate()
		{
			RemindPopup component = this.GamePageDic[PageName.RemindPopup].gameObject.GetComponent<RemindPopup>();
			component.Init(remind, name, _confirm, _cancel);
		});
	}

	public void ShowPlayerUpgradePopup()
	{
		this.ShowPage(PageName.PlayerUpgradePopup, delegate()
		{
			int[] upgradeAward = PlayerDataManager.GetUpgradeAward();
			PlayerUpgradePopup component = this.GamePageDic[PageName.PlayerUpgradePopup].gameObject.GetComponent<PlayerUpgradePopup>();
			component.Golds = upgradeAward[0];
			component.Diamonds = upgradeAward[1];
		});
	}

	public void ShowAward(int[] id, int[] count, string title = null)
	{
		this.ShowPage(PageName.AwardPopup, delegate()
		{
			AwardPopup component = this.GamePageDic[PageName.AwardPopup].gameObject.GetComponent<AwardPopup>();
			component.AwardID = id;
			component.AwardCount = count;
			if (string.IsNullOrEmpty(title))
			{
				component.TitleTxt.text = Singleton<GlobalData>.Instance.GetText("GET_REWARD");
			}
			else
			{
				component.TitleTxt.text = title;
			}
			Singleton<FontChanger>.Instance.SetFont(component.TitleTxt);
		});
	}

	public void ShowUnLockWeapon(WeaponData wd)
	{
		this.ShowPage(PageName.UnlockWeaponPage, delegate()
		{
			UnlockWeaponPage component = this.GamePageDic[PageName.UnlockWeaponPage].GetComponent<UnlockWeaponPage>();
			component.wd = wd;
			Singleton<GlobalData>.Instance.UnlockShowWd = wd;
		});
	}

	public void ShowObtainItem(ItemData item)
	{
		this.ShowPage(PageName.ObtainItemPopup, delegate()
		{
			ObtainItemPopup component = this.GetPage(PageName.ObtainItemPopup).GetComponent<ObtainItemPopup>();
			component.item = item;
		});
	}

	public void SetLevelFinish(bool isWin)
	{
		this.GameSuccess = ((!isWin) ? -1 : 1);
		this.CurrentPage.Close();
		if (isWin)
		{
			GameLogManager.SendPageLog(PageName.InGamePage.ToString(), PageName.GameOverPage.ToString());
			Singleton<UiManager>.Instance.ShowPage(PageName.GameOverPage, null);
		}
		else
		{
			GameLogManager.SendPageLog(PageName.InGamePage.ToString(), PageName.FailedNoticePage.ToString());
			Singleton<UiManager>.Instance.ShowPage(PageName.FailedNoticePage, null);
		}
		CheckpointData selectCheckpoint = CheckpointDataManager.SelectCheckpoint;
		GameLogManager.SendCheckpointLog(isWin, selectCheckpoint.ID);
		if (isWin)
		{
			if (selectCheckpoint.Type == CheckpointType.SNIPE)
			{
				GameLogManager.SendSnipeLog(2);
			}
		}
		else if (selectCheckpoint.Type == CheckpointType.SNIPE)
		{
			GameLogManager.SendSnipeLog(3);
		}
	}

	public void ShowEarnMoneyEffect(int golds, int diamods, int dnas = 0)
	{
		if (null != this.TopBar && this.TopBar.gameObject.activeInHierarchy)
		{
			int GoldCount = (golds != 0) ? ((golds / 500 + 1) * 3) : 0;
			int DiamondCount = diamods;
			int DNACount = dnas;
			if (GoldCount != 0 && GoldCount < 6)
			{
				GoldCount = 6;
			}
			else if (GoldCount > 25)
			{
				GoldCount = 25;
			}
			else if (DiamondCount > 25)
			{
				DiamondCount = 25;
			}
			else if (DNACount > 25)
			{
				DNACount = 25;
			}
			this.ShowPage(PageName.EarnMoneyEffect, delegate()
			{
				EarnMoneyEffectPage component = this.GamePageDic[PageName.EarnMoneyEffect].GetComponent<EarnMoneyEffectPage>();
				component.GoldCount = GoldCount;
				component.DiamondCount = DiamondCount;
				component.DNACount = DNACount;
				if (null != this.TopBar && this.TopBar.gameObject.activeInHierarchy)
				{
					component.DiamondRoot = this.TopBar.DiamondRoot;
					component.GoldRoot = this.TopBar.GoldRoot;
					component.DNARoot = this.TopBar.DNARoot;
				}
			});
		}
	}

	public void ShowPushGift(int Id)
	{
		this.ShowPage(PageName.RecommendGiftPage, delegate()
		{
			GameLogManager.SendPageLog(PageName.RecommendGiftPage.ToString(), "null");
			this.GetPage(PageName.RecommendGiftPage).GetComponent<RecommendGiftPage>().GiftData = StoreDataManager.GetStoreData(Id);
		});
	}

	public void ShowWeaponPush(int id)
	{
		this.ShowPage(PageName.WeaponPushGift, delegate()
		{
			GameLogManager.SendPageLog(PageName.WeaponPushGift.ToString(), "null");
			this.GetPage(PageName.WeaponPushGift).GetComponent<WeaponPushGift>().Weapon = WeaponDataManager.GetWeaponData(id);
		});
	}

	public void ShowNewWeapon(WeaponData _weapon)
	{
		this.ShowPage(PageName.NewWeaponPage, delegate()
		{
			GameLogManager.SendPageLog(PageName.NewWeaponPage.ToString(), "null");
			this.GetPage(PageName.NewWeaponPage).GetComponent<NewWeaponPage>().SetDisplayWeapon(_weapon);
		});
	}

	public void ShowFacebookGroupPage(int _id)
	{
	}

	public void OnBack()
	{
		if (this.CurrentPage != null && this.CanBack)
		{
			this.CurrentPage.OnBack();
		}
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
		{
			this.OnBack();
		}
	}

	public Camera UiCamera;

	public Transform PageRoot;

	public Transform PopupRoot;

	public Transform FixedRoot;

	public TopBarPage TopBar;

	public Sprite[] CommonSprites;

	public Sprite[] ItemSprites;

	public Sprite[] StoreSprites;

	public Sprite[] GiftSprites;

	public Sprite[] TalentIcons;

	public Sprite[] SpecialIcons;

	public EventSystem eventSystem;

	[HideInInspector]
	public Stack<GamePage> PopupStack = new Stack<GamePage>();

	[HideInInspector]
	public Stack<GamePage> PageStack = new Stack<GamePage>();

	[HideInInspector]
	public GamePage CurrentPage = new GamePage();

	private Dictionary<PageName, GamePage> GamePageDic = new Dictionary<PageName, GamePage>();

	private Dictionary<string, Sprite> GameIcons = new Dictionary<string, Sprite>();

	[HideInInspector]
	public int GameSuccess;

	[HideInInspector]
	public bool CanBack = true;

	public bool ShowPages;

	public MapsPage MapsPage;

	public FunctionPage FunctionPage;

	public const string URL_KEY = "group_address";

	public const string PAGE_KEY = "group_display_page";
}
