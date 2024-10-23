using System;
using System.Collections.Generic;
using DataCenter;
using DG.Tweening;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class DailyMissionPage : GamePage
{
	private new void Awake()
	{
		this.MissionList = DailyMissionSystem.GetCurrentMissions();
		this.MissionRoot.GetComponent<RectTransform>().sizeDelta = new Vector2((float)(270 * this.MissionList.Count + 20), 380f);
		for (int i = 0; i < this.MissionList.Count; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("UI/DailyMissionChild")) as GameObject;
			gameObject.transform.SetParent(this.MissionRoot);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			DailyMissionChild component = gameObject.GetComponent<DailyMissionChild>();
			this.MissionChildren.Add(component);
		}
	}

	public override void Show()
	{
		base.Show();
		this.Content.alpha = 0f;
		this.Content.DOFade(1f, 0.5f);
	}

	public override void Refresh()
	{
		base.Refresh();
		this.TitleName.text = Singleton<GlobalData>.Instance.GetText("DAILY_MISSION");
		Singleton<FontChanger>.Instance.SetFont(TitleName);
		for (int i = 0; i < this.MissionChildren.Count; i++)
		{
			this.MissionChildren[i].Refresh(this.MissionList[i]);
		}
	}

	public override void Close()
	{
		base.Close();
		Singleton<UiManager>.Instance.CurrentPage.Refresh();
	}

	public void OnCloseClick()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.CancelClip, false);
		this.Close();
	}

	public CanvasGroup Content;

	public Transform MissionRoot;

	public Text TitleName;

	private List<DailyMission> MissionList = new List<DailyMission>();

	private List<DailyMissionChild> MissionChildren = new List<DailyMissionChild>();
}
