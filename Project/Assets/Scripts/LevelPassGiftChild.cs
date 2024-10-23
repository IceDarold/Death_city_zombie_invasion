using System;
using DataCenter;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class LevelPassGiftChild : MonoBehaviour
{
	public void Init(int id, int count)
	{
		this.Count.text = count.ToString();
		if (id == 1)
		{
			this.Icon.sprite = Singleton<UiManager>.Instance.SpecialIcons[0];
			this.Count.gameObject.SetActive(true);
			this.NamePart.SetActive(false);
		}
		else if (id == 2)
		{
			this.Icon.sprite = Singleton<UiManager>.Instance.SpecialIcons[1];
			this.Count.gameObject.SetActive(true);
			this.NamePart.SetActive(false);
		}
		else if (id == 3)
		{
			this.Icon.sprite = Singleton<UiManager>.Instance.SpecialIcons[2];
			this.Count.gameObject.SetActive(true);
			this.NamePart.SetActive(false);
		}
		else
		{
			this.NamePart.SetActive(true);
			this.Name.text = Singleton<GlobalData>.Instance.GetText(ItemDataManager.GetItemData(id).Name);
			Singleton<FontChanger>.Instance.SetFont(Name);
			this.Count.gameObject.SetActive(false);
			if (id >= 8200 && id < 8300)
			{
				this.Icon.sprite = Singleton<UiManager>.Instance.GetSprite("debris_weapon");
			}
			else if (id >= 8400 && id < 8500)
			{
				this.Icon.sprite = Singleton<UiManager>.Instance.GetSprite("debris_prop");
			}
			else
			{
				this.Icon.sprite = Singleton<UiManager>.Instance.GetSprite(ItemDataManager.GetItemData(id).Icon);
			}
		}
		this.Icon.preserveAspect = true;
	}

	public Text Name;

	public Text Count;

	public Image Icon;

	public GameObject NamePart;
}
