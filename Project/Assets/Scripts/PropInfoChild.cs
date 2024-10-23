using System;
using DataCenter;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class PropInfoChild : MonoBehaviour
{
	public void init(PropData data, bool selected)
	{
		Singleton<FontChanger>.Instance.SetFont(Name);
		Singleton<FontChanger>.Instance.SetFont(Count);
		Singleton<FontChanger>.Instance.SetFont(EquipIndex);
		Singleton<FontChanger>.Instance.SetFont(Debris);
		if (data.State == PropState.未解锁)
		{
			this.Name.gameObject.SetActive(false);
			this.LockImage.gameObject.SetActive(true);
			this.Debris.text = DebrisDataManager.GetDebrisData(data.DebrisID).Count + "/" + data.RequiredDebris;
		}
		else
		{
			this.Name.text = Singleton<GlobalData>.Instance.GetText(data.Name);
			this.Name.gameObject.SetActive(true);
			this.LockImage.gameObject.SetActive(false);
		}
		this.Count.text = Singleton<GlobalData>.Instance.GetText("AMOUNT") + " " + data.Count;
		this.Icon.sprite = Singleton<UiManager>.Instance.GetSprite(data.Icon);
		if (data.Count == 0)
		{
			this.Icon.color = new Color(1f, 1f, 1f, 0.32f);
		}
		else
		{
			this.Icon.color = new Color(1f, 1f, 1f, 1f);
		}
		this.SelectImage.gameObject.SetActive(selected);
		if (data.ID == PlayerDataManager.Player.Prop1)
		{
			data.isNew = false;
			this.EquipIndex.text = Singleton<GlobalData>.Instance.GetText("HAVEEQUIP");
			this.EquipTag.gameObject.SetActive(true);
		}
		else if (data.ID == PlayerDataManager.Player.Prop2)
		{
			data.isNew = false;
			this.EquipIndex.text = Singleton<GlobalData>.Instance.GetText("HAVEEQUIP");
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
	}

	public Image SelectImage;

	public Image Background;

	public Image Icon;

	public Image NewTag;

	public Image EquipTag;

	public Text EquipIndex;

	public Transform LockImage;

	public Text Name;

	public Text Count;

	public Text Debris;
}
