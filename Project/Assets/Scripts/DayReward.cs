using System;
using DataCenter;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class DayReward : MonoBehaviour
{
	public void OnEnable()
	{
		this.curDaliy = DailyMissionSystem.GetCurrentDailyMission();
		if (this.curDaliy != null)
		{
			this.DayTaskName.text = Singleton<GlobalData>.Instance.GetText(this.curDaliy.Describe);
			this.DayTaskSlider.maxValue = (float)this.curDaliy.TargetValue;
			this.DayTaskSlider.value = (float)this.curDaliy.CurrentValue;
			this.DayTaskProcessTxt.text = this.curDaliy.CurrentValue + "/" + this.curDaliy.TargetValue;
			this.DayTaskRewardIcon.sprite = Singleton<UiManager>.Instance.GetSprite(ItemDataManager.GetItemData(this.curDaliy.AwardID[0]).Icon);
			this.DayTaskRewardNum.text = "X" + this.curDaliy.AwardCount[0].ToString();
			
			Singleton<FontChanger>.Instance.SetFont(DayTaskName);
			Singleton<FontChanger>.Instance.SetFont(DayTaskProcessTxt);
			Singleton<FontChanger>.Instance.SetFont(DayTaskRewardNum);
		}
	}

	public void Onclick()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		Singleton<UiManager>.Instance.ShowDayTaskMessage(Singleton<GlobalData>.Instance.GetText(this.curDaliy.Describe));
	}

	public void ShowReward()
	{
		this.curDaliy = DailyMissionSystem.GetCurrentDailyMission();
		ItemDataManager.CollectItem(this.curDaliy.AwardID[0], this.curDaliy.AwardCount[0]);
		Singleton<UiManager>.Instance.ShowAward(this.curDaliy.AwardID, this.curDaliy.AwardCount, Singleton<GlobalData>.Instance.GetText("Day_Reward"));
	}

	public Text DayTaskName;

	public Slider DayTaskSlider;

	public Text DayTaskProcessTxt;

	public Image DayTaskRewardIcon;

	public Text DayTaskRewardNum;

	private DailyMission curDaliy;
}
