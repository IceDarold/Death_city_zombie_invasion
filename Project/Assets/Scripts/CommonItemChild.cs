using System;
using DataCenter;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class CommonItemChild : MonoBehaviour
{
	public void ReflashPage()
	{
		this.name.text = Singleton<GlobalData>.Instance.GetText(this.it.Name);
		this.stateTxt.text = Singleton<GlobalData>.Instance.GetText("UNLOCK");
		Singleton<FontChanger>.Instance.SetFont(name);
		Singleton<FontChanger>.Instance.SetFont(stateTxt);
		this.icon.sprite = Singleton<UiManager>.Instance.GetSprite(this.it.Icon);
		this.icon.SetNativeSize();
	}

	public Image icon;

	public new Text name;

	public Text stateTxt;

	public ItemData it;
}
