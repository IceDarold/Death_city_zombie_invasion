using System;
using DG.Tweening;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RemindPopup : GamePage
{
	public override void Show()
	{
		base.Show();
		this.Content.alpha = 0f;
		this.Content.DOFade(1f, 0.5f);
	}

	public void Init(string _describe, string _name, UnityAction _confirm, UnityAction _cancel)
	{
		this.describe = _describe;
		this.confirm = _name;
		this.ConfirmCallback = _confirm;
		this.CancelCallback = _cancel;
	}

	public override void Refresh()
	{
		base.Refresh();
		this.VedioImage.gameObject.SetActive(this.confirm == "WATCH");
		this.Describe.text = Singleton<GlobalData>.Instance.GetText(this.describe);
		this.ConfirmName.text = Singleton<GlobalData>.Instance.GetText(this.confirm);
		this.CancelName.text = Singleton<GlobalData>.Instance.GetText("CANCEL");
		Singleton<FontChanger>.Instance.SetFont(Describe);
		Singleton<FontChanger>.Instance.SetFont(ConfirmName);
		Singleton<FontChanger>.Instance.SetFont(CancelName);
	}

	public void OnConfirmClick()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		this.Close();
		this.ConfirmCallback();
	}

	public void OnCancelClick()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.CancelClip, false);
		this.Close();
		if (this.CancelCallback != null)
		{
			this.CancelCallback();
		}
	}

	public CanvasGroup Content;

	public Text Describe;

	public Text ConfirmName;

	public Text CancelName;

	public Button ConfirmButton;

	public Image VedioImage;

	private string describe;

	private string confirm;

	private UnityAction ConfirmCallback;

	private UnityAction CancelCallback;
}
