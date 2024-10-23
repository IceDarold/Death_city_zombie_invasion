using System;
using DataCenter;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class FacebookNoticePage : GamePage
{
	public override void Show()
	{
		base.Show();
		//GMGSDK.sendJoinGroupPageShowLog();
	}

	public override void Refresh()
	{
		base.Refresh();
		this.NoticeDescribe.text = ((!this.isGroup) ? "Come and join our Facebook Page to collence it!" : "Come and join our Facebook Group to collence it!");
		this.JoinButtonName.text = ((!this.isGroup) ? Singleton<GlobalData>.Instance.GetText("JOIN_FACEBOOK_PAGE") : Singleton<GlobalData>.Instance.GetText("JOIN_FACEBOOK_GROUP"));
		Singleton<FontChanger>.Instance.SetFont(JoinButtonName);
	}

	public void Init(string _url)
	{
		this.url = _url;
		this.isGroup = _url.Contains("groups");
	}

	public void OnJoinPressed()
	{
		//GMGSDK.jumpToFBPage(this.url);
		PlayerPrefs.SetString("FACEBOOK_NOTICE_RESULT", "SUCCESS");
		ItemDataManager.CollectItem(2, 20);
		Singleton<UiManager>.Instance.TopBar.Refresh();
		this.Close();
	}

	public void OnCancelPressed()
	{
		this.Close();
	}

	protected string url = string.Empty;

	protected bool isGroup;

	protected const string GROUP_KEY = "groups";

	protected const string PAGE_KEY = "fb://page";

	public Text NoticeDescribe;

	public Text JoinButtonName;
}
