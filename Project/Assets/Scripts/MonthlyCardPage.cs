using System;
using ui.imageTranslator;
using UnityEngine.UI;

public class MonthlyCardPage : GamePage
{
	public override void Refresh()
	{
		base.Refresh();
		this.Title.text = Singleton<GlobalData>.Instance.GetText("MONTHLY_CARD_AWARD");
		this.ButtonName.text = Singleton<GlobalData>.Instance.GetText("RECEIVE");
		Singleton<FontChanger>.Instance.SetFont(Title);
		Singleton<FontChanger>.Instance.SetFont(ButtonName);
	}

	public void ClickOnConfirm()
	{
		this.Close();
		Singleton<UiManager>.Instance.TopBar.Refresh();
	}

	public Text Title;

	public Text ButtonName;
}
