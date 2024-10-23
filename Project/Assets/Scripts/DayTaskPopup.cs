using System;
using ui.imageTranslator;
using UnityEngine.UI;

public class DayTaskPopup : GamePage
{
	public new void OnEnable()
	{
		this.ComfirmTxt.text = Singleton<GlobalData>.Instance.GetText("CONFIRM");
		Singleton<FontChanger>.Instance.SetFont(ComfirmTxt);
	}

	public void Onclick()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		this.Close();
	}

	public Text TitleTxt;

	public Text ComfirmTxt;
}
