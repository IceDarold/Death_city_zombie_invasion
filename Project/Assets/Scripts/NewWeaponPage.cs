using System;
using DataCenter;
using ui.imageTranslator;
using UnityEngine.UI;

public class NewWeaponPage : GamePage
{
	public void SetDisplayWeapon(WeaponData _weapon)
	{
		this.weapon = _weapon;
	}

	public override void Show()
	{
		base.Show();
	}

	public override void Refresh()
	{
		base.Refresh();
		this.Title.text = Singleton<GlobalData>.Instance.GetText("NEW_WEAPON_TITLE");
		this.WeaponIcon.sprite = Singleton<UiManager>.Instance.GetSprite(this.weapon.Icon);
		this.WeaponName.text = Singleton<GlobalData>.Instance.GetText(this.weapon.Name);
		this.WeaponType.text = Singleton<GlobalData>.Instance.GetText(this.weapon.Type.ToString());
		this.BasicFighting.text = Singleton<GlobalData>.Instance.GetText("BASIC_FIGHTING") + WeaponDataManager.GetInitFightingStrength(this.weapon);
		this.MaxFighting.text = Singleton<GlobalData>.Instance.GetText("MAX_FIGHTING") + WeaponDataManager.GetMaxFightingStrength(this.weapon);
		ConfirmText.text = Singleton<GlobalData>.Instance.GetText("CONFIRM");
		Singleton<FontChanger>.Instance.SetFont(Title);
		Singleton<FontChanger>.Instance.SetFont(WeaponName);
		Singleton<FontChanger>.Instance.SetFont(WeaponType);
		Singleton<FontChanger>.Instance.SetFont(BasicFighting);
		Singleton<FontChanger>.Instance.SetFont(MaxFighting);
		Singleton<FontChanger>.Instance.SetFont(ConfirmText);
	}

	public void ClickOnConfirm()
	{
		this.Close();
	}

	public Text Title;

	public Text WeaponName;

	public Text WeaponType;

	public Text BasicFighting;

	public Text MaxFighting;
	
	public Text ConfirmText;

	public Image WeaponIcon;

	private WeaponData weapon;
}
