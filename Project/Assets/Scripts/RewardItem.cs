using System;
using DataCenter;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class RewardItem : MonoBehaviour
{
	public void Refresh(int id, int count)
	{
		switch (id)
		{
		case 1:
			this.Icon.sprite = Singleton<UiManager>.Instance.SpecialIcons[0];
			break;
		case 2:
			this.Icon.sprite = Singleton<UiManager>.Instance.SpecialIcons[1];
			break;
		case 3:
			this.Icon.sprite = Singleton<UiManager>.Instance.SpecialIcons[2];
			break;
		default:
			this.Icon.sprite = Singleton<UiManager>.Instance.GetSprite(ItemDataManager.GetItemData(id).Icon);
			break;
		}
		this.Num.text = Singleton<GlobalData>.Instance.GetText(ItemDataManager.GetItemData(id).Name).ToString() + "  X " + count.ToString();
		Singleton<FontChanger>.Instance.SetFont(Num);
	}

	public Text Num;

	public Image Icon;
}
