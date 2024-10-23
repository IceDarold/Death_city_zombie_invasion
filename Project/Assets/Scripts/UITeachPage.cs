using System;
using DataCenter;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class UITeachPage : GamePage
{
	public override void Close()
	{
		base.Close();
	}

	public override void OnBack()
	{
	}

	public void OnclickBootom()
	{
		this.BootomPos.gameObject.SetActive(false);
		this.EffectObj.SetActive(true);
		this.EffectObj.transform.position = this.Button.transform.position;
		this.GuideGo.transform.position = this.Button.transform.position;
		this.Button.SetActive(true);
		Singleton<FontChanger>.Instance.SetFont(GuideTxt);
		switch (this.type)
		{
		case TeachUIType.Prop:
			this.GuideTxt.text = Singleton<GlobalData>.Instance.GetText("GUIDETEXT_02");
			break;
		case TeachUIType.Weapon:
			this.GuideTxt.text = Singleton<GlobalData>.Instance.GetText("GUIDETEXT_02");
			break;
		case TeachUIType.Equipment:
			this.GuideTxt.text = Singleton<GlobalData>.Instance.GetText("GUIDETEXT_01");
			break;
		case TeachUIType.Talent:
			this.GuideTxt.text = Singleton<GlobalData>.Instance.GetText("GUIDETEXT_04");
			break;
		}
	}

	public void RefreshPage()
	{
		if (PlayerDataManager.Player.Cap != 0)
		{
			Close();
			return;
		}
		this.GuideTxt.transform.localPosition = new Vector3(-376f, 0f, 0f);
		Singleton<FontChanger>.Instance.SetFont(GuideTxt);
		Singleton<FontChanger>.Instance.SetFont(Guide02Txt);
		switch (this.type)
		{
		case TeachUIType.Prop:
		{
			Singleton<UiManager>.Instance.SetTopEnable(false, false);
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("UI/PropInfoChild")) as GameObject;
			gameObject.transform.SetParent(this.BootomPos);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			gameObject.GetComponent<Button>().onClick.AddListener(delegate()
			{
				this.OnclickBootom();
			});
			this.curPropInfo = gameObject.GetComponent<PropInfoChild>();
			this.GuideGo.transform.position = this.curPropInfo.transform.position;
			this.GuideTxt.text = Singleton<GlobalData>.Instance.GetText(string.Empty);
			this.Guide02Txt.text = Singleton<GlobalData>.Instance.GetText("GUIDETEXT_05");
			break;
		}
		case TeachUIType.TopBar:
			this.Button.SetActive(false);
			UnityEngine.Object.Destroy(this.Button);
			Singleton<UiManager>.Instance.SetTopEnable(false, false);
			Singleton<UiManager>.Instance.TopBar.BackButton.interactable = true;
			this.EffectObj.transform.position = Singleton<UiManager>.Instance.TopBar.BackButton.transform.position;
			this.Button = this.ProduceGo(Singleton<UiManager>.Instance.TopBar.BackButton.gameObject);
			this.GuideTxt.text = Singleton<GlobalData>.Instance.GetText(string.Empty);
			break;
		case TeachUIType.Weapon:
		{
			Singleton<UiManager>.Instance.SetTopEnable(false, false);
			for (int i = 1; i < this.BootomPos.childCount; i++)
			{
				this.BootomPos.GetChild(i).gameObject.SetActive(false);
			}
			for (int j = 0; j < this.Pos01.childCount; j++)
			{
				this.Pos01.GetChild(j).gameObject.SetActive(false);
			}
			GameObject gameObject2 = UnityEngine.Object.Instantiate(Resources.Load("UI/WeaponInfoChild")) as GameObject;
			gameObject2.transform.SetParent(this.BootomPos);
			gameObject2.transform.localPosition = Vector3.zero;
			gameObject2.transform.localScale = Vector3.one;
			gameObject2.GetComponent<Button>().onClick.AddListener(delegate()
			{
				this.OnclickBootom();
			});
			this.curWeaponInfo = gameObject2.GetComponent<WeaponInfoChild>();
			this.GuideGo.transform.position = this.curWeaponInfo.transform.position;
			this.GuideTxt.text = Singleton<GlobalData>.Instance.GetText(string.Empty);
			this.Guide02Txt.text = Singleton<GlobalData>.Instance.GetText("GUIDETEXT_07");
			break;
		}
		case TeachUIType.Equipment:
		{

			Singleton<UiManager>.Instance.SetTopEnable(false, false);
			GameObject gameObject3 = UnityEngine.Object.Instantiate(Resources.Load("UI/EquipmentChild")) as GameObject;
			gameObject3.transform.SetParent(this.BootomPos);
			gameObject3.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(185f, 140f);
			gameObject3.transform.localPosition = Vector3.zero;
			gameObject3.transform.localScale = Vector3.one;
			gameObject3.GetComponent<Button>().onClick.AddListener(delegate()
			{
				this.OnclickBootom();
			});
			this.curEquipInfo = gameObject3.GetComponent<EquipmentChild>();
			this.GuideGo.transform.position = this.curEquipInfo.transform.position;
			this.Guide02Txt.text = Singleton<GlobalData>.Instance.GetText("GUIDETEXT_05");
			this.GuideTxt.text = Singleton<GlobalData>.Instance.GetText(string.Empty);

			break;
		}
		case TeachUIType.Main:
			this.Button.SetActive(false);
			UnityEngine.Object.Destroy(this.Button);
			Singleton<UiManager>.Instance.SetTopEnable(false, false);
			this.EffectObj.transform.position = Singleton<UiManager>.Instance.GetPage(PageName.MainPage).GetComponent<MainPage>().WordBtnTran.position;
			this.Button = this.ProduceGo(Singleton<UiManager>.Instance.GetPage(PageName.MainPage).GetComponent<MainPage>().WordBtnTran.gameObject);
			this.GuideTxt.text = Singleton<GlobalData>.Instance.GetText(string.Empty);
			break;
		case TeachUIType.Map:
			Singleton<UiManager>.Instance.SetTopEnable(false, false);
			this.GuideTxt.text = Singleton<GlobalData>.Instance.GetText("GUIDETEXT_06");
			break;
		case TeachUIType.Talent:
		{
			Singleton<UiManager>.Instance.SetTopEnable(false, false);
			for (int k = 1; k < this.BootomPos.childCount; k++)
			{
				this.BootomPos.GetChild(k).gameObject.SetActive(false);
			}
			for (int l = 0; l < this.Pos01.childCount; l++)
			{
				this.Pos01.GetChild(l).gameObject.SetActive(false);
			}
			GameObject gameObject4 = UnityEngine.Object.Instantiate(Resources.Load("UI/TalentChild")) as GameObject;
			gameObject4.transform.SetParent(this.BootomPos);
			gameObject4.transform.localPosition = Vector3.zero;
			gameObject4.transform.localScale = Vector3.one;
			gameObject4.GetComponent<Button>().onClick.AddListener(delegate()
			{
				this.OnclickBootom();
			});
			this.curTalent = gameObject4.GetComponent<TalentChild>();
			this.GuideGo.transform.position = this.curTalent.transform.position;
			this.GuideTxt.text = Singleton<GlobalData>.Instance.GetText(string.Empty);
			this.Guide02Txt.text = Singleton<GlobalData>.Instance.GetText("GUIDETEXT_03");
			break;
		}
		case TeachUIType.MainEquipMent:
		case TeachUIType.MainProp:
		case TeachUIType.MainTalent:
		case TeachUIType.MainWeapon:
			this.GuideTxt.text = Singleton<GlobalData>.Instance.GetText("GUIDETEXT_08");
			this.BootomPos.gameObject.SetActive(false);
			this.EffectObj.transform.position = this.Button.transform.position;
			Singleton<UiManager>.Instance.SetTopEnable(false, false);
			break;
		case TeachUIType.MapUpgrade:
			Singleton<UiManager>.Instance.SetTopEnable(false, false);
			this.BootomPos.gameObject.SetActive(false);
			this.EffectObj.transform.position = this.Button.transform.position;
			this.GuideTxt.text = Singleton<GlobalData>.Instance.GetText("GUIDETEXT_09");
			this.GuideGo.transform.position = this.Button.transform.position;
			this.GuideTxt.transform.localPosition = new Vector3(-398f, 0f, 0f);
			break;
		case TeachUIType.WeaponUpgrade:
			Singleton<UiManager>.Instance.SetTopEnable(false, false);
			this.BootomPos.gameObject.SetActive(false);
			this.EffectObj.transform.position = this.Button.transform.position;
			this.GuideTxt.text = Singleton<GlobalData>.Instance.GetText("GUIDETEXT_04");
			this.GuideTxt.transform.localPosition = new Vector3(-179f, -64f, 0f);
			break;
		case TeachUIType.MainBox:
			this.Button.SetActive(false);
			UnityEngine.Object.Destroy(this.Button);
			for (int m = 0; m < this.Pos01.childCount; m++)
			{
				this.Pos01.GetChild(m).gameObject.SetActive(false);
			}
			Singleton<UiManager>.Instance.SetTopEnable(false, false);
			this.BootomPos.gameObject.SetActive(false);
			this.EffectObj.transform.position = Singleton<UiManager>.Instance.GetPage(PageName.MainPage).GetComponent<MainPage>().OpenBoxGo.transform.position;
			this.Button = this.ProduceGo(Singleton<UiManager>.Instance.GetPage(PageName.MainPage).GetComponent<MainPage>().OpenBoxGo);
			this.GuideTxt.text = Singleton<GlobalData>.Instance.GetText("GUIDETEXT_10");
			break;
		case TeachUIType.Box:
			this.Button.SetActive(false);
			UnityEngine.Object.Destroy(this.Button);
			Singleton<UiManager>.Instance.SetTopEnable(false, false);
			this.Button = this.ProduceGo(Singleton<UiManager>.Instance.GetPage(PageName.BoxPage).GetComponent<BoxPage>().TDBoxTrans.gameObject);
			this.EffectObj.transform.position = this.Button.transform.position;
			this.GuideTxt.text = Singleton<GlobalData>.Instance.GetText("GUIDETEXT_11");
			break;
		case TeachUIType.WeaponUpgradeBack:
			this.Button.SetActive(false);
			UnityEngine.Object.Destroy(this.Button);
			Singleton<UiManager>.Instance.SetTopEnable(false, false);
			Singleton<UiManager>.Instance.TopBar.BackButton.interactable = true;
			this.EffectObj.transform.position = Singleton<UiManager>.Instance.TopBar.BackButton.transform.position;
			this.Button = this.ProduceGo(Singleton<UiManager>.Instance.TopBar.BackButton.gameObject);
			this.GuideTxt.text = Singleton<GlobalData>.Instance.GetText(string.Empty);
			break;
		case TeachUIType.MapsSpecialLevel:
			Singleton<UiManager>.Instance.SetTopEnable(false, false);
			for (int n = 0; n < this.Pos01.childCount; n++)
			{
				this.Pos01.GetChild(n).gameObject.SetActive(false);
			}
			this.BootomPos.gameObject.SetActive(false);
			this.EffectObj.transform.position = this.Button.transform.position;
			this.GuideTxt.text = Singleton<GlobalData>.Instance.GetText("GUIDETEXT_12");
			this.GuideTxt.transform.localPosition = new Vector3(-25f, -97f, 0f);
			break;
		case TeachUIType.None:
			this.GuideTxt.text = Singleton<GlobalData>.Instance.GetText(string.Empty);
			Singleton<UiManager>.Instance.SetTopEnable(true, true);
			break;
		}
		this.GuideGo.transform.position = this.Button.transform.position;
	}

	public void OnclickMainType()
	{
		this.Button.SetActive(false);
		this.type = TeachUIType.None;
	}

	public GameObject ProduceGo(GameObject go)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(go);
		gameObject.transform.SetParent(this.Pos01);
		gameObject.transform.localScale = go.transform.localScale;
		gameObject.transform.position = go.transform.position;
		return gameObject;
	}

	public Transform Top;

	public Transform Pos01;

	public Transform Pos02;

	public Transform BootomPos;

	public PropInfoChild curPropInfo;

	public WeaponInfoChild curWeaponInfo;

	public TalentChild curTalent;

	public EquipmentChild curEquipInfo;

	public GameObject Button;

	public TeachUIType type;

	public GameObject EffectObj;

	public GameObject GuideGo;

	public Text GuideTxt;

	public Text Guide02Txt;
}
