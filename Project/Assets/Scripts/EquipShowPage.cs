using System;
using DataCenter;
using DG.Tweening;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class EquipShowPage : GamePage
{
	public override void Show()
	{
		base.Show();
	}

	public override void Close()
	{
		base.Close();
	}

	public override void Hide()
	{
		base.Hide();
	}

	public override void Refresh()
	{
		base.Refresh();
		this.canvasGroup.alpha = 0f;
		this.canvasGroup.DOFade(1f, 1f).OnComplete(delegate
		{
		});
		this.InitData();
		this.RefreshPage();
	}

	public void InitData()
	{
		this.player = PlayerDataManager.Player;
		this.role = RoleDataManager.GetRoleData(this.player.Role);
		this.mainWeapon = WeaponDataManager.GetWeaponData(this.player.Weapon1);
		this.secondWeapon = WeaponDataManager.GetWeaponData(this.player.Weapon2);
		this.prop1 = PropDataManager.GetPropData(this.player.Prop1);
		this.prop2 = PropDataManager.GetPropData(this.player.Prop2);
		this.equip01 = EquipmentDataManager.GetEquipmentData(this.player.Cap);
		this.equip02 = EquipmentDataManager.GetEquipmentData(this.player.Coat);
		this.equip03 = EquipmentDataManager.GetEquipmentData(this.player.Shoes);
		this.talentPoints = TalentDataManager.GetCurrentPoints(1, 0) + "/" + TalentDataManager.GetTotalPoints(1, 0);
	}

	public void RefreshProp(Image PropImg, Text PropNumTxt, PropData prop)
	{
		if (prop != null)
		{
			PropImg.gameObject.SetActive(true);
			PropNumTxt.gameObject.SetActive(true);
			PropNumTxt.text = this.prop1.Count.ToString();
		}
		else
		{
			PropNumTxt.gameObject.SetActive(false);
			PropImg.gameObject.SetActive(false);
		}
	}

	public void RefreshEquip(Image DefineImg, Text EquipDefineTxt, Text EquipDefineNumTxt, EquipmentData equip)
	{
		if (equip != null)
		{
			EquipDefineTxt.gameObject.SetActive(true);
			EquipDefineNumTxt.gameObject.SetActive(true);
			DefineImg.gameObject.SetActive(true);
		}
		else
		{
			EquipDefineTxt.gameObject.SetActive(false);
			DefineImg.gameObject.SetActive(false);
			EquipDefineNumTxt.gameObject.SetActive(false);
		}
	}

	public void RefreshPage()
	{
		this.BtnTxt.text = Singleton<GlobalData>.Instance.GetText("CHANGEROLE");
		this.WeaponTxt.text = Singleton<GlobalData>.Instance.GetText("WEAPONTITLE");
		this.WeaponFireTxt01.text = Singleton<GlobalData>.Instance.GetText("FIREPOWER2");
		this.WeaponFireTxt02.text = Singleton<GlobalData>.Instance.GetText("FIREPOWER2");
		this.PropTxt.text = Singleton<GlobalData>.Instance.GetText("PROPTITLE");
		this.EquipTxt.text = Singleton<GlobalData>.Instance.GetText("EQUIPTITLE");
		this.EquipDefineTxt01.text = Singleton<GlobalData>.Instance.GetText("DEFINE");
		this.EquipDefineTxt02.text = Singleton<GlobalData>.Instance.GetText("DEFINE");
		this.EquipDefineTxt03.text = Singleton<GlobalData>.Instance.GetText("DEFINE");
		this.TalentTxt.text = Singleton<GlobalData>.Instance.GetText("TOLENTITLE");
		this.TolenNumTxt.text = this.talentPoints;
		Singleton<FontChanger>.Instance.SetFont(BtnTxt);
		Singleton<FontChanger>.Instance.SetFont(WeaponTxt);
		Singleton<FontChanger>.Instance.SetFont(WeaponFireTxt01);
		Singleton<FontChanger>.Instance.SetFont(WeaponFireTxt02);
		Singleton<FontChanger>.Instance.SetFont(PropTxt);
		Singleton<FontChanger>.Instance.SetFont(EquipTxt);
		Singleton<FontChanger>.Instance.SetFont(EquipDefineTxt01);
		Singleton<FontChanger>.Instance.SetFont(EquipDefineTxt02);
		Singleton<FontChanger>.Instance.SetFont(EquipDefineTxt03);
		Singleton<FontChanger>.Instance.SetFont(TalentTxt);
		Singleton<FontChanger>.Instance.SetFont(TolenNumTxt);
		this.RefreshProp(this.PropImg01, this.PropNumTxt01, this.prop1);
		this.RefreshProp(this.PropImg02, this.PropNumTxt02, this.prop2);
		this.RefreshProp(this.PropImg03, this.PropNumTxt03, this.prop3);
		this.RefreshEquip(this.DefineImg01, this.EquipDefineTxt01, this.EquipDefineNumTxt01, this.equip01);
		this.RefreshEquip(this.DefineImg02, this.EquipDefineTxt02, this.EquipDefineNumTxt02, this.equip02);
		this.RefreshEquip(this.DefineImg03, this.EquipDefineTxt03, this.EquipDefineNumTxt03, this.equip03);
	}

	public void OnclickChangeRole()
	{
	}

	public void OnclickWeapon()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		this.Hide();
		Singleton<UiManager>.Instance.ShowPage(PageName.FunctionPage, delegate()
		{
			Singleton<UiManager>.Instance.GetPage(PageName.FunctionPage).GetComponent<FunctionPage>().CurrentTag = FunctionPageTag.Weapon;
		});
		Singleton<UiManager>.Instance.ShowTopBar(true);
	}

	public void OnclickProp()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		Singleton<UiManager>.Instance.ShowPage(PageName.FunctionPage, delegate()
		{
			Singleton<UiManager>.Instance.GetPage(PageName.FunctionPage).GetComponent<FunctionPage>().CurrentTag = FunctionPageTag.Prop;
		});
		this.Hide();
		Singleton<UiManager>.Instance.ShowTopBar(true);
	}

	public void OnclickEquip()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		this.Hide();
		Singleton<UiManager>.Instance.ShowPage(PageName.FunctionPage, delegate()
		{
			Singleton<UiManager>.Instance.GetPage(PageName.FunctionPage).GetComponent<FunctionPage>().CurrentTag = FunctionPageTag.Equipment;
		});
		Singleton<UiManager>.Instance.ShowTopBar(true);
	}

	public void OnclickTalent()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		this.Hide();
		Singleton<UiManager>.Instance.ShowPage(PageName.FunctionPage, delegate()
		{
			Singleton<UiManager>.Instance.GetPage(PageName.FunctionPage).GetComponent<FunctionPage>().CurrentTag = FunctionPageTag.Talent;
		});
		Singleton<UiManager>.Instance.ShowTopBar(true);
	}

	public Text BtnTxt;

	public Text DamgeTxt;

	public Text DamgeNumTxt;

	public Text DefineTxt;

	public Text DefineNumTxt;

	public Text FireTxt;

	public Text FireNumTxt;

	public Text WeaponTxt;

	public Text WeaponFireTxt01;

	public Text WeaponFireTxt02;

	public Text WeaponFireNumTxt01;

	public Text WeaponFireNumTxt02;

	public Text PropTxt;

	public Text PropNumTxt01;

	public Text PropNumTxt02;

	public Text PropNumTxt03;

	public Text EquipTxt;

	public Text EquipDefineTxt01;

	public Text EquipDefineTxt02;

	public Text EquipDefineTxt03;

	public Text EquipDefineNumTxt01;

	public Text EquipDefineNumTxt02;

	public Text EquipDefineNumTxt03;

	public Text TalentTxt;

	public Text TolenNumTxt;

	public Image WeaponImg01;

	public Image WeaponImg02;

	public Image PropImg01;

	public Image PropImg02;

	public Image PropImg03;

	public Image DefineImg01;

	public Image DefineImg02;

	public Image DefineImg03;

	public Image TalentImg;

	public CanvasGroup canvasGroup;

	private RoleData role;

	private WeaponData mainWeapon;

	private WeaponData secondWeapon;

	private PropData prop1;

	private PropData prop2;

	private PropData prop3;

	private EquipmentData equip01;

	private EquipmentData equip02;

	private EquipmentData equip03;

	private string talentPoints;

	private PlayerData player;
}
