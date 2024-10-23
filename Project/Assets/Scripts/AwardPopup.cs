using System;
using System.Collections.Generic;
using DataCenter;
using DG.Tweening;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class AwardPopup : GamePage
{
	public override void OnBack()
	{
		this.OnClick();
	}

	public override void Show()
	{
		Singleton<UiManager>.Instance.SetTopEnable(false, false);
		base.Show();
		this.DoUI.DORestart(false);
	}

	public override void Refresh()
	{
		this.BtnTxt.text = Singleton<GlobalData>.Instance.GetText("CONFIRM");
		Singleton<FontChanger>.Instance.SetFont(BtnTxt);
		for (int i = 0; i < this.AwardID.Length; i++)
		{
			if (ItemDataManager.GetItemData(this.AwardID[i]) != null)
			{
				this.CreateChild(this.AwardID[i], this.AwardCount[i]);
			}
		}
	}

	private void CreateChild(int id, int count)
	{
		for (int i = 0; i < this.AwardChildren.Count; i++)
		{
			if (!this.AwardChildren[i].gameObject.activeSelf)
			{
				this.AwardChildren[i].Refresh(id, count);
				this.AwardChildren[i].gameObject.SetActive(true);
				return;
			}
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("UI/RewardItem")) as GameObject;
		gameObject.transform.SetParent(this.RewardItemPos);
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localPosition = Vector3.zero;
		RewardItem component = gameObject.GetComponent<RewardItem>();
		component.Refresh(id, count);
		this.AwardChildren.Add(component);
	}

	private void ResetChildren()
	{
		for (int i = 0; i < this.AwardChildren.Count; i++)
		{
			this.AwardChildren[i].gameObject.SetActive(false);
		}
	}

	public void OnClick()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		Singleton<UiManager>.Instance.SetTopEnable(true, true);
		this.ResetChildren();
		this.Close();
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < this.AwardID.Length; i++)
		{
			if (this.AwardID[i] == 1)
			{
				num3 += this.AwardCount[i];
			}
			else if (this.AwardID[i] == 2)
			{
				num2 += this.AwardCount[i];
			}
			else if (this.AwardID[i] == 3)
			{
				num += this.AwardCount[i];
			}
		}
		if (num3 + num2 + num > 0)
		{
			Singleton<UiManager>.Instance.ShowEarnMoneyEffect(num3, num2, num);
		}
		if (Singleton<UiManager>.Instance.TopBar != null)
		{
			Singleton<UiManager>.Instance.TopBar.Refresh();
		}
		if (PlayerPrefs.GetString("FirstPayShow", "true") == "true" && PlayerPrefs.GetString("FirstPay", "true") == "false")
		{
			PlayerPrefs.SetString("FirstPayShow", "false");
			Singleton<UiManager>.Instance.ShowAward(new int[]
			{
				403
			}, new int[]
			{
				3
			}, Singleton<GlobalData>.Instance.GetText("FIRSTPAY"));
		}
	}

	public Transform RewardItemPos;

	public DOTweenAnimation DoUI;

	public Text TitleTxt;

	public Text BtnTxt;

	public int[] AwardID;

	public int[] AwardCount;

	private List<RewardItem> AwardChildren = new List<RewardItem>();
}
