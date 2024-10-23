using System;
using DataCenter;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class ItemNew : MonoBehaviour
{
	public void OnEnable()
	{
		this.ReflashPage();
	}

	public void ReflashPage()
	{
		if (this.itemData != null)
		{
			this.countTxt.gameObject.SetActive(false);
			this.levelNumTxt.text = this.itemData.Level.ToString();
			this.nameTxt.text = Singleton<GlobalData>.Instance.GetText(this.itemData.Name);
			this.itemIcon.sprite = Singleton<UiManager>.Instance.GetSprite(this.itemData.Icon);
		}
		else if (this.propData != null)
		{
			this.countTxt.text = this.count + string.Empty;
			this.nameTxt.text = Singleton<GlobalData>.Instance.GetText(this.propData.Name);
			this.itemIcon.sprite = Singleton<UiManager>.Instance.GetSprite(this.propData.Icon);
		}
		else if (this.equipData != null)
		{
			this.countTxt.gameObject.SetActive(false);
			this.levelNumTxt.text = this.equipData.Level.ToString();
			this.nameTxt.text = Singleton<GlobalData>.Instance.GetText(this.equipData.Name);
			this.itemIcon.sprite = Singleton<UiManager>.Instance.GetSprite(this.equipData.Icon);
		}
		Singleton<FontChanger>.Instance.SetFont(levelNumTxt);
		Singleton<FontChanger>.Instance.SetFont(nameTxt);
	}

	public void OnclickItem()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		if (this.itemData != null)
		{
			WeaponDataManager.CollectWeapon(this.itemData.ID);
			MainPage.instance.ShowItemPopup(this.itemData.Icon, base.transform);
		}
		else if (this.propData != null)
		{
			PropDataManager.CollectProp(this.propData.ID, this.propData.FabricateNumber);
			MainPage.instance.ShowItemPopup(this.propData.Icon, base.transform);
		}
		else if (this.equipData != null)
		{
			EquipmentDataManager.Collect(this.equipData.ID);
			MainPage.instance.ShowItemPopup(this.equipData.Icon, base.transform);
		}
		MainPage.instance.RefleshPage();
	}

	public Image itemIcon;

	public Image newIcon;

	public Text nameTxt;

	public Text levelName;

	public Text levelNumTxt;

	public Text countTxt;

	public WeaponData itemData;

	public PropData propData;

	public EquipmentData equipData;

	public int index;

	public int count;
}
