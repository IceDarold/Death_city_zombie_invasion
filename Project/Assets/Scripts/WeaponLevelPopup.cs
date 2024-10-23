using System;
using DataCenter;
using DG.Tweening;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class WeaponLevelPopup : GamePage
{
	public override void Show()
	{
		this.weapon = WeaponDataManager.GetTryWeapon();
		this.data = CheckpointDataManager.GetWeaponCheckpoint();
		base.Show();
		this.Content.alpha = 0f;
		this.Content.DOFade(1f, 0.5f);
	}

	public override void Refresh()
	{
		base.Refresh();
		this.TitleName.text = Singleton<GlobalData>.Instance.GetText("WEAPON_TRY_TITLE");
		this.Describe.text = Singleton<GlobalData>.Instance.GetText("WEAPON_TRY_DESCRIBE");
		this.WeaponIcon.sprite = Singleton<UiManager>.Instance.GetSprite(this.weapon.Icon);
		this.ConfirmName.text = Singleton<GlobalData>.Instance.GetText("START");
		this.WeaponName.text = Singleton<GlobalData>.Instance.GetText(this.weapon.Name);
		Singleton<FontChanger>.Instance.SetFont(TitleName);
		Singleton<FontChanger>.Instance.SetFont(Describe);
		Singleton<FontChanger>.Instance.SetFont(ConfirmName);
		Singleton<FontChanger>.Instance.SetFont(WeaponName);
	}

	public void ClickOnClose()
	{
		this.Close();
	}

	public void ClickOnConfirm()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		this.Close();
		if (Singleton<GlobalData>.Instance.GetEnergy() <= 0)
		{
			Singleton<UiManager>.Instance.ShowPage(PageName.EnergyShortagePage, null);
			return;
		}
		Singleton<GlobalData>.Instance.SetEnergy(-1);
		CheckpointDataManager.SelectCheckpoint = this.data;
		Singleton<GlobalData>.Instance.AdvertisementReviveTimes = 0;
		MapsPage.instance.isScence = true;
		MapsPage.instance.Close();
		Singleton<UiManager>.Instance.ShowTopBar(false);
		GameLogManager.SendPageLog(this.Name.ToString(), PageName.InGamePage.ToString());
		Singleton<UiManager>.Instance.ShowLoadingPage(CheckpointDataManager.GetScene(this.data.SceneID), PageName.InGamePage);
	}

	public CanvasGroup Content;

	public Text TitleName;

	public Image WeaponIcon;

	public Text ConfirmName;

	public Text Describe;

	public Text WeaponName;

	private WeaponData weapon;

	private CheckpointData data;
}
