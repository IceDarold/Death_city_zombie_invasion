using System;
using DataCenter;
using DG.Tweening;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class PowerShortagePage : GamePage
{
	public override void Show()
	{
		this.GetReferWeapon();
		base.Show();
		this.Content.DOFade(0f, 0.5f).From<Tweener>();
	}

	private void GetReferWeapon()
	{
		WeaponData weaponData = WeaponDataManager.GetWeaponData(PlayerDataManager.Player.Weapon1);
		WeaponData weaponData2 = WeaponDataManager.GetWeaponData(PlayerDataManager.Player.Weapon2);
		int num = 0;
		int num2 = 0;
		if (weaponData != null)
		{
			num = WeaponDataManager.GetCurrentFightingStrength(weaponData);
		}
		if (weaponData2 != null)
		{
			num2 = WeaponDataManager.GetCurrentFightingStrength(weaponData2);
		}
		this.ReferWeapon = ((num < num2) ? weaponData2 : weaponData);
	}

	public override void Refresh()
	{
		base.Refresh();
		this.TitleName.text = Singleton<GlobalData>.Instance.GetText("WARNING");
		this.RecommendText.text = Singleton<GlobalData>.Instance.GetText("POWER_RECOMMEND") + " : " + CheckpointDataManager.SelectCheckpoint.RequireFighting;
		this.MineText.text = Singleton<GlobalData>.Instance.GetText("POWER_SELF") + " : " + WeaponDataManager.GetCurrentFightingStrength(this.ReferWeapon);
		this.StartName.text = Singleton<GlobalData>.Instance.GetText("START");
		this.Describe.text = Singleton<GlobalData>.Instance.GetText("POWER_SHORTAGE_TIP1");
		this.JumpName.text = Singleton<GlobalData>.Instance.GetText("UPGRADE");
		Singleton<FontChanger>.Instance.SetFont(TitleName);
		Singleton<FontChanger>.Instance.SetFont(RecommendText);
		Singleton<FontChanger>.Instance.SetFont(MineText);
		Singleton<FontChanger>.Instance.SetFont(StartName);
		Singleton<FontChanger>.Instance.SetFont(Describe);
		Singleton<FontChanger>.Instance.SetFont(JumpName);
	}

	public void ClickOnClose()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.CancelClip, false);
		this.Close();
		CheckpointDataManager.SelectCheckpoint = MapsPage.instance.SelectData;
		Singleton<UiManager>.Instance.CurrentPage.Refresh();
	}

	public void ClickOnStart()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		if (CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.BOSS)
		{
			Singleton<GlobalData>.Instance.SetEnergy(-2);
		}
		else
		{
			Singleton<GlobalData>.Instance.SetEnergy(-1);
		}
		this.Close();
		MapsPage.instance.isScence = true;
		MapsPage.instance.Close();
		Singleton<UiManager>.Instance.ShowTopBar(false);
		if (CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.RACING)
		{
			GameLogManager.SendPageLog(this.Name.ToString(), PageName.InRacingPage.ToString());
			Singleton<UiManager>.Instance.ShowLoadingPage("RacingScene", PageName.InRacingPage);
		}
		else
		{
			GameLogManager.SendPageLog(this.Name.ToString(), PageName.InGamePage.ToString());
			Singleton<UiManager>.Instance.ShowLoadingPage(CheckpointDataManager.GetScene(CheckpointDataManager.SelectCheckpoint.SceneID), PageName.InGamePage);
		}
	}

	public void ClickOnJump()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		this.Close();
		MapsPage.instance.Hide();
		Singleton<UiManager>.Instance.ShowPage(PageName.FunctionPage, delegate()
		{
			Singleton<UiManager>.Instance.GetPage(PageName.FunctionPage).GetComponent<FunctionPage>().CurrentTag = FunctionPageTag.Weapon;
		});
	}

	public CanvasGroup Content;

	public Text TitleName;

	public Text Describe;

	public Text RecommendText;

	public Text MineText;

	public Text StartName;

	public Text JumpName;

	private WeaponData ReferWeapon;
}
