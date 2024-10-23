using System;
using DataCenter;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class ObtainItemPopup : GamePage
{
	public override void Show()
	{
		base.Show();
	}

	public override void Refresh()
	{
		base.Refresh();
		this.Title.text = Singleton<GlobalData>.Instance.GetText("OBTAIN_ITEM") + " " + Singleton<GlobalData>.Instance.GetText(this.item.Name);
		Singleton<FontChanger>.Instance.SetFont(Title);
		this.ItemIcon.sprite = Singleton<UiManager>.Instance.GetSprite(this.item.Icon);
		if (this.item.ItemTag == DataCenter.ItemType.Weapon)
		{
			this.ItemIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(512f, 256f);
		}
		else if (this.item.ItemTag == DataCenter.ItemType.Equipment || this.item.ItemTag == DataCenter.ItemType.Prop)
		{
			this.ItemIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(256f, 256f);
		}
		this.ConfirmText.text = Singleton<GlobalData>.Instance.GetText("CONFIRM");
		Singleton<FontChanger>.Instance.SetFont(ConfirmText);
	}

	public override void Close()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		base.Close();
	}

	public Text Title;

	public Image ItemIcon;

	public Text ConfirmText;

	public ItemData item;
}
