using System;
using System.Collections.Generic;
using DataCenter;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInfoChild : MonoBehaviour
{
	public void init(WeaponData data, bool selected)
	{
		this.SetQuality(data.Quality);
		this.SelectImage.gameObject.SetActive(selected);
		this.Icon.sprite = Singleton<UiManager>.Instance.GetSprite(data.Icon);
		if (data.State == WeaponState.未解锁)
		{
			this.Icon.color = new Color(1f, 1f, 1f, 0.32f);
		}
		else
		{
			this.Icon.color = new Color(1f, 1f, 1f, 1f);
		}
		if (data.ID == PlayerDataManager.Player.Weapon1)
		{
			data.isNew = false;
			this.EquipIndex.text = Singleton<GlobalData>.Instance.GetText("HAVEEQUIP");
			Singleton<FontChanger>.Instance.SetFont(EquipIndex);
			this.EquipTag.gameObject.SetActive(true);
		}
		else if (data.ID == PlayerDataManager.Player.Weapon2)
		{
			data.isNew = false;
			this.EquipIndex.text = Singleton<GlobalData>.Instance.GetText("HAVEEQUIP");
			Singleton<FontChanger>.Instance.SetFont(EquipIndex);
			this.EquipTag.gameObject.SetActive(true);
		}
		else
		{
			this.EquipTag.gameObject.SetActive(false);
		}
		if (data.isNew)
		{
			this.NewTag.gameObject.SetActive(true);
		}
		else
		{
			this.NewTag.gameObject.SetActive(false);
		}
		if (data.State >= WeaponState.待制作)
		{
			this.Name.text = Singleton<GlobalData>.Instance.GetText(data.Name);
			Singleton<FontChanger>.Instance.SetFont(Name);
			this.Name.gameObject.SetActive(true);
			this.LockImage.gameObject.SetActive(false);
		}
		else
		{
			this.Name.gameObject.SetActive(false);
			this.LockImage.gameObject.SetActive(true);
			this.Debris.text = DebrisDataManager.GetDebrisData(data.DebrisID).Count + "/" + data.RequiredDebris;
		}
	}

	private void SetQuality(int quality)
	{
		for (int i = 0; i < this.Stars.Count; i++)
		{
			this.Stars[i].gameObject.SetActive(i < quality);
		}
	}

	public Image SelectImage;

	public Image Background;

	public Image Icon;

	public Image NewTag;

	public Image EquipTag;

	public Text EquipIndex;

	public Image LockImage;

	public Text Name;

	public Text Debris;

	public List<Transform> Stars;
}
