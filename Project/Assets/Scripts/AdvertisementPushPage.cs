using System;
using ads;
using DataCenter;
using ui.imageTranslator;
using UnityEngine.UI;

public class AdvertisementPushPage : GamePage
{
	public override void Refresh()
	{
		this.Tip.text = Singleton<GlobalData>.Instance.GetText("ADVERTISEMENT_PUSH_TIP");
		this.Confirm.text = Singleton<GlobalData>.Instance.GetText("WATCH");
		Singleton<FontChanger>.Instance.SetFont(Tip);
		Singleton<FontChanger>.Instance.SetFont(Confirm);
	}

	public void ClickOnAdvertisement()
	{
        //Advertisements.Instance.ShowRewardedVideo(OnFinished);
        Ads.ShowReward(() =>
        {
			ItemDataManager.SetCurrency(CommonDataType.DIAMOND, this.AwardDiamond);
	        Singleton<UiManager>.Instance.TopBar.Refresh();
	        this.Close();
	        Singleton<UiManager>.Instance.ShowAward(new int[]
	        {
	        	2
	        }, new int[]
	        {
	        	this.AwardDiamond
	        }, null);
        });
        
  //      Singleton<GlobalData>.Instance.ShowAdvertisement(-9, delegate
		//{
		//	ItemDataManager.SetCurrency(CommonDataType.DIAMOND, this.AwardDiamond);
		//	Singleton<UiManager>.Instance.TopBar.Refresh();
		//	this.Close();
		//	Singleton<UiManager>.Instance.ShowAward(new int[]
		//	{
		//		2
		//	}, new int[]
		//	{
		//		this.AwardDiamond
		//	}, null);
		//}, null);
	}
    private void OnFinished(bool completed, string advertiser)
    {
        if (completed == true)
        {
            ItemDataManager.SetCurrency(CommonDataType.DIAMOND, this.AwardDiamond);
            Singleton<UiManager>.Instance.TopBar.Refresh();
            this.Close();
            Singleton<UiManager>.Instance.ShowAward(new int[]
            {
                2
            }, new int[]
            {
                this.AwardDiamond
            }, null);
        }

    }
    public void ClickOnClosePage()
	{
		this.Close();
	}

	public Text Tip;

	public Text Confirm;

	private int AwardDiamond = 20;
}
