using System;
using System.Collections.Generic;
using DataCenter;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentChild : MonoBehaviour
{
	public void init(EquipmentData data, bool selected)
	{
		this.SelectImage.gameObject.SetActive(selected);
		this.Name.text = Singleton<GlobalData>.Instance.GetText(data.Name);
		this.Icon.sprite = Singleton<UiManager>.Instance.GetSprite(data.Icon);
		Singleton<FontChanger>.Instance.SetFont(Name);
		if (data.State == EquipmentState.未解锁)
		{
			this.Icon.color = new Color(1f, 1f, 1f, 0.32f);
		}
		else
		{
			this.Icon.color = new Color(1f, 1f, 1f, 1f);
		}
		this.SetQuality(data.Quality);
		if (data.State == EquipmentState.未解锁)
		{
			this.LockImage.gameObject.SetActive(true);
		}
		else
		{
			this.LockImage.gameObject.SetActive(false);
		}
		switch (data.State)
		{
		case EquipmentState.待制作:
			this.StateInfo.text = "可制作";
			this.StateInfo.gameObject.SetActive(true);
			goto IL_1B3;
		case EquipmentState.制作中:
			this.StateInfo.text = "制作中";
			this.StateInfo.gameObject.SetActive(true);
			goto IL_1B3;
		case EquipmentState.待领取:
			this.StateInfo.text = "可领取";
			this.StateInfo.gameObject.SetActive(true);
			goto IL_1B3;
		case EquipmentState.升级中:
			this.StateInfo.text = "升级中";
			this.StateInfo.gameObject.SetActive(true);
			goto IL_1B3;
		}
		this.StateInfo.gameObject.SetActive(false);
		IL_1B3:
		if (EquipmentDataManager.isEquip(data.ID))
		{
			EquipmentDataManager.RemoveNewTag(data.ID);
			this.EquipTagTxt.text = Singleton<GlobalData>.Instance.GetText("HAVEEQUIP");
			this.EquipTag.gameObject.SetActive(true);
		}
		else
		{
			this.EquipTagTxt.text = Singleton<GlobalData>.Instance.GetText("HAVEEQUIP");
			this.EquipTag.gameObject.SetActive(false);
		}
		Singleton<FontChanger>.Instance.SetFont(EquipTagTxt);
		if (data.isNew)
		{
			this.NewTag.gameObject.SetActive(true);
		}
		else
		{
			this.NewTag.gameObject.SetActive(false);
		}
	}

	private void SetQuality(int quality)
	{
		for (int i = 0; i < this.Stars.Count; i++)
		{
			this.Stars[i].gameObject.SetActive(i < quality);
		}
	}

	public Text Name;

	public Text EquipTagTxt;

	public Text StateInfo;

	public Image SelectImage;

	public Image Icon;

	public Image NewTag;

	public Image EquipTag;

	public Image LockImage;

	public List<Transform> Stars;
}
