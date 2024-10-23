using System;
using System.Collections.Generic;
using DataCenter;
using DG.Tweening;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class LackOfMeterialPopup : GamePage
{
	public new void OnEnable()
	{
		this.canvasGroup.alpha = 0f;
		this.canvasGroup.DOFade(1f, 0.4f);
		this.DesTxt.text = Singleton<GlobalData>.Instance.GetText("METERIAL_NOENOUGH");
		this.BtnShopTxt.text = Singleton<GlobalData>.Instance.GetText("SHOP");
		this.BtnLevelTxt.text = Singleton<GlobalData>.Instance.GetText("LEVEL_DROPDOWN");
		this.OpenBoxTxt.text = Singleton<GlobalData>.Instance.GetText("OPENBOX");
		Singleton<FontChanger>.Instance.SetFont(DesTxt);
		Singleton<FontChanger>.Instance.SetFont(BtnShopTxt);
		Singleton<FontChanger>.Instance.SetFont(BtnLevelTxt);
		Singleton<FontChanger>.Instance.SetFont(OpenBoxTxt);
		for (int i = 0; i < this.Btns.Length; i++)
		{
			this.Btns[i].SetActive(false);
		}
	}

	public override void Refresh()
	{
		base.Refresh();
		for (int i = 0; i < this.DropWay.Count; i++)
		{
			this.Btns[this.DropWay[i]].SetActive(true);
		}
	}

	public void OnclickClose()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		this.Close();
	}

	public void OnclickShop()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		this.Close();
		Singleton<UiManager>.Instance.GetPage(PageName.FunctionPage).Hide();
		Singleton<UiManager>.Instance.ShowStorePage(0);
	}

	public void OnclickLevel()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		this.Close();
		Singleton<UiManager>.Instance.GetPage(PageName.FunctionPage).Close();
		Singleton<UiManager>.Instance.ShowPage(PageName.MapsPage, null);
	}

	public void OnclickOpenBox()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		if (CheckpointDataManager.GetCurrentCheckpoint().Chapters > ChapterEnum.CHAPTERNAME_01)
		{
			this.Close();
			Singleton<UiManager>.Instance.GetPage(PageName.FunctionPage).Hide();
			Singleton<UiManager>.Instance.ShowPage(PageName.BoxPage, null);
		}
		else
		{
			Singleton<UiManager>.Instance.ShowMessage(Singleton<GlobalData>.Instance.GetText("TIPBOX"), 0.5f);
		}
	}

	public Text DesTxt;

	public Text BtnShopTxt;

	public Text BtnLevelTxt;

	public Text OpenBoxTxt;

	public CanvasGroup canvasGroup;

	public GameObject[] Btns;

	public List<int> DropWay = new List<int>();
}
