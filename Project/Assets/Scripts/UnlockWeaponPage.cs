using System;
using DataCenter;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class UnlockWeaponPage : GamePage
{
	public override void Refresh()
	{
		base.Refresh();
		this.Title.text = Singleton<GlobalData>.Instance.GetText("SUCCESS_UNLOCK");
		this.Sure.text = Singleton<GlobalData>.Instance.GetText("CONFIRM");
		this.Cancel.text = Singleton<GlobalData>.Instance.GetText("CANCEL");
		this.NameTxt.text = Singleton<GlobalData>.Instance.GetText(this.wd.Name);
		this.Icon.sprite = Singleton<UiManager>.Instance.GetSprite(this.wd.Icon);
		
		Singleton<FontChanger>.Instance.SetFont(Title);
		Singleton<FontChanger>.Instance.SetFont(Sure);
		Singleton<FontChanger>.Instance.SetFont(Cancel);
		Singleton<FontChanger>.Instance.SetFont(NameTxt);
	}

	public void OnclickSure()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		this.Close();
		Singleton<UiManager>.Instance.CurrentPage.Hide();
		if (Singleton<GlobalData>.Instance.FirstWeapon < 2)
		{
			Singleton<UiManager>.Instance.ShowPage(PageName.FunctionPage, delegate()
			{
				Singleton<UiManager>.Instance.GetPage(PageName.FunctionPage).GetComponent<FunctionPage>().CurrentTag = FunctionPageTag.Weapon;
			});
			Singleton<UiManager>.Instance.ShowTopBar(true);
		}
		else
		{
			Singleton<UiManager>.Instance.ShowPage(PageName.MainPage, null);
			Singleton<GlobalData>.Instance.UnlockShowWd = null;
		}
	}

	public void OnclickCancel()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		Singleton<GlobalData>.Instance.UnlockShowWd = null;
		this.Close();
	}

	public override void OnBack()
	{
		this.OnclickCancel();
	}

	public CanvasGroup Content;

	public WeaponData wd;

	public Text NameTxt;

	public Image Icon;

	public Text Sure;

	public Text Cancel;

	public Text Title;
}
