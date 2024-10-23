using System;
using ads;
using DataCenter;
using DG.Tweening;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class EnergyShortagePage : GamePage
{
	public override void Show()
	{
		base.Show();
		this.Content.alpha = 0f;
		this.Content.DOFade(1f, 0.5f);
	}

	public override void Refresh()
	{
		int energy = Singleton<GlobalData>.Instance.GetEnergy();
		this.SetEnergyObjects(energy);
		this.EnergyCountText.text = energy.ToString();
		this.TitleText.text = Singleton<GlobalData>.Instance.GetText("WARNING");
		this.TipText.text = Singleton<GlobalData>.Instance.GetText("ENERGY_TIP");
		Singleton<FontChanger>.Instance.SetFont(EnergyCountText);
		Singleton<FontChanger>.Instance.SetFont(TitleText);
		Singleton<FontChanger>.Instance.SetFont(TipText);
		if (ItemDataManager.GetCurrency(CommonDataType.VIP) == 0)
		{
			this.VipButton.gameObject.SetActive(true);
			this.VipButtonName.text = Singleton<GlobalData>.Instance.GetText("ENERGY_DOUBLE");
			Singleton<FontChanger>.Instance.SetFont(VipButtonName);
		}
		else
		{
			this.VipButton.gameObject.SetActive(false);
		}
		if (energy < Singleton<GlobalData>.Instance.GetMaxEnergy())
		{
			this.DescribeText.text = Singleton<GlobalData>.Instance.GetText("ENERGY_SHORTAGE_TIP1");
			this.VedioButton.gameObject.SetActive(true);
			this.VedioButtonName.text = Singleton<GlobalData>.Instance.GetText("ENERGY_RECOVERY");
			Singleton<FontChanger>.Instance.SetFont(VedioButtonName);
			this.TimerText.gameObject.SetActive(true);
			this.timer = Singleton<GlobalData>.Instance.GetEnergyRecoveryTime();
		}
		else
		{
			this.DescribeText.text = Singleton<GlobalData>.Instance.GetText("ENERGY_SHORTAGE_TIP2");
			this.VedioButton.gameObject.SetActive(false);
			this.TimerText.gameObject.SetActive(false);
			this.timer = 0f;
		}
		Singleton<FontChanger>.Instance.SetFont(DescribeText);
	}

	private void SetEnergyObjects(int count)
	{
		float fillAmount = (float)count / (float)Singleton<GlobalData>.Instance.GetMaxEnergy();
		this.EnergyProgress.fillAmount = fillAmount;
	}

	public void OnVipClick()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		Singleton<UiManager>.Instance.ShowPushGift(9010);
		this.Refresh();
		Singleton<UiManager>.Instance.TopBar.Refresh();
	}
    private void OnFinished(bool completed, string advertiser)
    {
        Debug.Log("Closed rewarded from: " + advertiser + " -> Completed " + completed);
        if (completed == true)
        {
            Singleton<GlobalData>.Instance.SetEnergy(1);
            this.Refresh();
        }

    }
   
	public void OnVedioClick()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
        //Advertisements.Instance.ShowRewardedVideo(OnFinished);
        Ads.ShowReward(() =>
        {
	        Singleton<GlobalData>.Instance.SetEnergy(1);
	        this.Refresh();
        });
	}

	public void OnCloseClick()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.CancelClip, false);
		this.Close();
	}

	public void ClickOnAddEnergy()
	{
		Singleton<GlobalData>.Instance.SetEnergy(1);
	}

	private void Update()
	{
		if (this.TimerText.gameObject.activeSelf)
		{
			if (this.timer > 0f)
			{
				this.TimerText.text = ((int)this.timer / 60).ToString("D2") + ":" + ((int)this.timer % 60).ToString("D2");
				this.timer -= Time.deltaTime;
			}
			else
			{
				this.Refresh();
			}
		}
	}

	public CanvasGroup Content;

	public Button VipButton;

	public Button VedioButton;

	public Text TitleText;

	public Text DescribeText;

	public Text TipText;

	public Text EnergyCountText;

	public Text VipButtonName;

	public Text VedioButtonName;

	public Text TimerText;

	public Image EnergyProgress;

	private float timer;

	private bool isTiming;
}
