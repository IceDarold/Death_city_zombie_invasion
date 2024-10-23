using System;
using DataCenter;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;
using Zombie3D;

public class PickInGamePage : GamePage
{
	public new void OnEnable()
	{
		Time.timeScale = 0f;
		GameApp.GetInstance().GetGameScene().PlayingState = PlayingState.GamePause;
		this.TitleNameTxt.text = Singleton<GlobalData>.Instance.GetText("GET_DEBRIS");
		this.BtnTxt.text = Singleton<GlobalData>.Instance.GetText("CONFIRM");
		Singleton<FontChanger>.Instance.SetFont(TitleNameTxt);
		Singleton<FontChanger>.Instance.SetFont(BtnTxt);
		DebrisData inGameCurItem = Singleton<GlobalData>.Instance.InGameCurItem;
		ItemData itemData = ItemDataManager.GetItemData(inGameCurItem.ItemID);
		if (itemData.ItemTag == DataCenter.ItemType.Prop)
		{
			PropData propData = PropDataManager.GetPropData(itemData.ID);
			this.NumDebris.text = inGameCurItem.Count + "/" + propData.RequiredDebris;
			this.DebrisSlider.maxValue = (float)propData.RequiredDebris;
			this.DebrisSlider.value = (float)inGameCurItem.Count;
			this.ItemIcon.sprite = Singleton<UiManager>.Instance.GetSprite(propData.Icon);
		}
		else if (itemData.ItemTag == DataCenter.ItemType.Weapon)
		{
			WeaponData weaponData = WeaponDataManager.GetWeaponData(itemData.ID);
			this.NumDebris.text = inGameCurItem.Count + "/" + weaponData.RequiredDebris;
			this.DebrisSlider.maxValue = (float)weaponData.RequiredDebris;
			this.DebrisSlider.value = (float)inGameCurItem.Count;
			this.ItemIcon.sprite = Singleton<UiManager>.Instance.GetSprite(weaponData.Icon);
		}
	}

	public void Onclick()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		this.Close();
	}

	public override void OnBack()
	{
		base.Close();
	}

	public override void Close()
	{
		GameApp.GetInstance().GetGameScene().PlayingState = (PlayingState)Singleton<GlobalData>.Instance.InGameState;
		Time.timeScale = 1f;
		base.Close();
	}

	public Text TitleNameTxt;

	public Text BtnTxt;

	public Image ItemIcon;

	public Slider DebrisSlider;

	public Text NumDebris;
}
