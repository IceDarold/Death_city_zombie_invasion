using System;
using DataCenter;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class AchievementChild : MonoBehaviour
{
	public void init(AchievementData data)
	{
		this.AchievementName.text = Singleton<GlobalData>.Instance.GetText(data.Name);
		this.Describe.text = AchievementDataManager.GetDescribe(data);
		Singleton<FontChanger>.Instance.SetFont(AchievementName);
		Singleton<FontChanger>.Instance.SetFont(Describe);
		int num = (!data.Completed) ? data.AwardItem[data.Level] : data.AwardItem[data.Level - 1];
		if (num == 1)
		{
			this.AchievementIcon.sprite = Singleton<UiManager>.Instance.SpecialIcons[0];
		}
		else if (num == 2)
		{
			this.AchievementIcon.sprite = Singleton<UiManager>.Instance.SpecialIcons[1];
		}
		else if (num == 3)
		{
			this.AchievementIcon.sprite = Singleton<UiManager>.Instance.SpecialIcons[2];
		}
		else
		{
			this.AchievementIcon.sprite = Singleton<UiManager>.Instance.GetSprite(ItemDataManager.GetItemData(num).Icon);
		}
		if (data.Completed)
		{
			this.ProgressText.gameObject.SetActive(false);
			this.Compeleted.gameObject.SetActive(true);
			this.Compeleted.text = Singleton<GlobalData>.Instance.GetText("COMPELETED");
			Singleton<FontChanger>.Instance.SetFont(Compeleted);
			this.AwardButton.interactable = false;
			this.AwardName.text = Singleton<GlobalData>.Instance.GetText("GET_REWARD");
			Singleton<FontChanger>.Instance.SetFont(AwardName);
			this.AwardButton.gameObject.SetActive(false);
			this.AchievementName.gameObject.SetActive(true);
			this.Describe.gameObject.SetActive(true);
			this.AwardCount.gameObject.SetActive(false);
		}
		else
		{
			this.AwardCount.gameObject.SetActive(true);
			this.AwardCount.text = "X" + data.AwardCount[data.Level];
			this.Compeleted.gameObject.SetActive(false);
			if (data.Reached)
			{
				this.ProgressText.gameObject.SetActive(false);
				this.AwardButton.interactable = true;
				this.AwardIcon.gameObject.SetActive(true);
				this.AwardIcon.sprite = Singleton<UiManager>.Instance.GetSprite(ItemDataManager.GetItemData(data.AwardItem[data.Level]).Icon);
				this.AwardName.text = Singleton<GlobalData>.Instance.GetText("GET_REWARD");
				Singleton<FontChanger>.Instance.SetFont(AwardName);
				this.AwardButton.gameObject.SetActive(true);
				this.Describe.gameObject.SetActive(false);
			}
			else
			{
				this.ProgressText.gameObject.SetActive(true);
				this.ProgressText.text = data.CurrentValue + "/" + data.TargetValue[data.Level];
				this.AwardButton.gameObject.SetActive(false);
				this.AchievementName.gameObject.SetActive(true);
				this.Describe.gameObject.SetActive(true);
			}
		}
		this.AwardButton.onClick.RemoveAllListeners();
		this.AwardButton.onClick.AddListener(delegate()
		{
			this.GetAward(data);
		});
	}

	public void GetAward(AchievementData data)
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		if (data.CurrentValue < data.TargetValue[data.Level])
		{
			return;
		}
		int[] array = new int[]
		{
			data.AwardItem[data.Level]
		};
		int[] array2 = new int[]
		{
			data.AwardCount[data.Level]
		};
		if (array[0] == 401 || array[0] == 403 || array[0] == 402)
		{
			ItemDataManager.SetCurrency((CommonDataType)array[0], array2[0]);
			this.CurrentPage.Close();
			Singleton<UiManager>.Instance.CurrentPage.Hide();
			Singleton<UiManager>.Instance.ShowPage(PageName.BoxPage, null);
		}
		else
		{
			ItemDataManager.CollectItem(array[0], array2[0]);
			Singleton<UiManager>.Instance.ShowAward(array, array2, null);
			Singleton<UiManager>.Instance.TopBar.Refresh();
		}
		AchievementDataManager.EarnAward(data);
		this.CurrentPage.Refresh();
	}

	public AchievementPage CurrentPage;

	public Text AchievementName;

	public Image AchievementIcon;

	public Text Describe;

	public Text ProgressText;

	public Slider ProgressSlider;

	public Button AwardButton;

	public Image AwardIcon;

	public Text AwardName;

	public Text Compeleted;

	public Text AwardCount;
}
