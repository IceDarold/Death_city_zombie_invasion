using System;
using UnityEngine;
using UnityEngine.UI;
using Zombie3D;

public class AchievementUIItem : MonoBehaviour
{
	private void Awake()
	{
	}

	public void Init(AchievementGroup _achieveGath, double _num)
	{
		this.numValue = _num;
		this.achieveGather = _achieveGath;
		AchievementSystem.Instance.CalculateAchievement(this.achieveGather.achievementInfo.AchiveType, _num);
		this.nameText.text = _achieveGath.achievementInfo.Name + " " + ArabitToRoman.Exchange(_achieveGath.currentIndex + 1);
		this.processBar.maxValue = (float)_achieveGath.achievementInfo.AchiItems[_achieveGath.currentIndex].Count;
		float value = (float)_num;
		this.processText.text = _num + "/" + _achieveGath.achievementInfo.AchiItems[_achieveGath.currentIndex].Count;
		if (_num > _achieveGath.achievementInfo.AchiItems[_achieveGath.currentIndex].Count)
		{
			this.processText.text = _achieveGath.achievementInfo.AchiItems[_achieveGath.currentIndex].Count + "/" + _achieveGath.achievementInfo.AchiItems[_achieveGath.currentIndex].Count;
			value = (float)_achieveGath.achievementInfo.AchiItems[_achieveGath.currentIndex].Count;
		}
		this.processBar.value = value;
		this.descText.text = string.Format(_achieveGath.achievementInfo.Desc, _achieveGath.achievementInfo.AchiItems[_achieveGath.currentIndex].Count);
		this.ShowReward();
		this.RefreshStar();
		if (!_achieveGath.isComplete)
		{
			this.buttonText.text = "UnDone";
			this.DisableButton();
		}
		else if (_achieveGath.isComplete && !_achieveGath.isReceive)
		{
			UnityEngine.Debug.LogError("成就已达成");
			this.buttonText.text = "UnReceive";
			this.RefreshButton();
		}
		else if (_achieveGath.isReceive && _achieveGath.isComplete)
		{
			this.processText.text = string.Empty;
			this.buttonText.text = "Completed";
			this.HideReward();
		}
	}

	private void RefreshStar()
	{
		for (int i = 0; i < this.starArray.Length; i++)
		{
			if (this.achieveGather.currentIndex == 0)
			{
				break;
			}
			if (this.achieveGather.isReceive && this.achieveGather.isComplete)
			{
				this.starArray[i].SetActive(true);
			}
			else if (i < this.achieveGather.currentIndex)
			{
				this.starArray[i].SetActive(true);
			}
			else
			{
				this.starArray[i].SetActive(false);
			}
		}
	}

	private void HideReward()
	{
		this.button.gameObject.SetActive(false);
		this.rewardParent.gameObject.SetActive(false);
		this.rewardMax.SetActive(true);
	}

	private void DisableButton()
	{
		this.button.interactable = false;
		this.buttonImage.color = new Color(255f, 255f, 255f, 0.5f);
	}

	private void RefreshButton()
	{
		this.button.interactable = true;
		this.buttonImage.color = new Color(255f, 255f, 255f, 1f);
	}

	public void OnReceiveClick()
	{
		if (!this.achieveGather.isComplete)
		{
			return;
		}
		if (this.achieveGather.isComplete && !this.achieveGather.isReceive)
		{
			this.GetReward(this.achieveGather.achievementInfo.AchiItems[this.achieveGather.currentIndex].Rewards);
			AchievementSystem.Instance.OnReceive(this.achieveGather);
			this.Init(this.achieveGather, this.numValue);
		}
		else if (!this.achieveGather.isComplete || this.achieveGather.isReceive)
		{
		}
	}

	private void GetReward(string _reward)
	{
		this.GetAchieveReward(_reward, new Action<double>(this.AddCoin), new Action<double>(this.AddDiamond));
	}

	private void AddCoin(double _num)
	{
		int delta = (int)_num;
		this.gameState.Alert_cash(delta);
		GameApp.GetInstance().Save();
	}

	private void AddDiamond(double _num)
	{
		int delta = (int)_num;
		this.gameState.Alert_diamond(delta);
		GameApp.GetInstance().Save();
	}

	public void ShowReward()
	{
		this.RefreshRewardParent();
	}

	public void GetAchieveReward(string _reward, Action<double> _coinCallBack, Action<double> _diamondCallBack)
	{
		string[] array = _reward.Split(new char[]
		{
			'_',
			','
		});
		string text = string.Empty;
		if (array != null && array.Length > 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (i % 2 == 0)
				{
					if (i + 1 < array.Length - 1)
					{
						string text2 = text;
						text = string.Concat(new string[]
						{
							text2,
							array[i],
							" +",
							array[i + 1],
							"\n"
						});
					}
					else
					{
						text = text + array[i] + " +" + array[i + 1];
					}
					if (array[i] == "Coin")
					{
					}
					this.CalculateReward(array[i], double.Parse(array[i + 1]), _coinCallBack, _diamondCallBack);
				}
			}
		}
	}

	private void CalculateReward(string _text, double _num, Action<double> _coinCallBack, Action<double> _diamondCallBack)
	{
		if (_text != null)
		{
			if (!(_text == "Coin"))
			{
				if (_text == "Diamond")
				{
					if (_diamondCallBack != null)
					{
						_diamondCallBack(_num);
					}
				}
			}
			else if (_coinCallBack != null)
			{
				_coinCallBack(_num);
			}
		}
	}

	private void RefreshRewardParent()
	{
		string[] array = this.achieveGather.achievementInfo.AchiItems[this.achieveGather.currentIndex].Rewards.Split(new char[]
		{
			'_',
			','
		});
		if (array != null && array.Length > 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (i % 2 == 0)
				{
					if (array[i] == "Coin")
					{
					}
					this.rewardText.text = array[i + 1];
				}
			}
		}
	}

	public void RefreshAchievementItemShow(AchievementType _type)
	{
		AchievementSystem.Instance.CalculateAchievement(_type, 0.0);
	}

	public Text nameText;

	public Text descText;

	public Text buttonText;

	public Text processText;

	public Slider processBar;

	public Button button;

	public Image buttonImage;

	public GameObject[] starArray;

	public Transform rewardParent;

	public GameObject rewardMax;

	private AchievementGroup achieveGather;

	private string prefabPath = "Prefabs/UIPrefabs/AchieveReward";

	protected GameState gameState;

	public Image rewardImg;

	public Text rewardText;

	protected double numValue;
}
