using System;
using System.Collections.Generic;
using DG.Tweening;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class EndlessPage : GamePage
{
	public new void OnEnable()
	{
		this.canvasGroup.alpha = 0f;
		this.canvasGroup.DOFade(1f, 1f).OnComplete(delegate
		{
		});
		this.titleNameTxt.text = Singleton<GlobalData>.Instance.GetText("ENDLESS");
		this.scoreRewardTxt.text = Singleton<GlobalData>.Instance.GetText("SCORE_REWARD");
		this.introduceTxt.text = Singleton<GlobalData>.Instance.GetText("INTRODUCE");
		this.bestScoreTxt.text = Singleton<GlobalData>.Instance.GetText("BEST_SCORE");
		this.endTimeTxt.text = Singleton<GlobalData>.Instance.GetText("ENDTIME");
		this.startTxt.text = Singleton<GlobalData>.Instance.GetText("START");
		Singleton<FontChanger>.Instance.SetFont(titleNameTxt);
		Singleton<FontChanger>.Instance.SetFont(scoreRewardTxt);
		Singleton<FontChanger>.Instance.SetFont(introduceTxt);
		Singleton<FontChanger>.Instance.SetFont(bestScoreTxt);
		Singleton<FontChanger>.Instance.SetFont(endTimeTxt);
		Singleton<FontChanger>.Instance.SetFont(startTxt);
	}

	private new void Awake()
	{
		for (int i = 0; i < this.Childs.Count; i++)
		{
		}
	}

	public void RefreshPage()
	{
	}

	public void OnclickClose()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		Singleton<UiManager>.Instance.ClosePage(PageType.Popup, null);
	}

	private void Update()
	{
	}

	public Text titleNameTxt;

	public Text scoreRewardTxt;

	public Text introduceTxt;

	public Text bestScoreTxt;

	public Text endTimeTxt;

	public Text startTxt;

	public List<EndlessScoreRewardChild> Childs = new List<EndlessScoreRewardChild>();

	public CanvasGroup canvasGroup;
}
