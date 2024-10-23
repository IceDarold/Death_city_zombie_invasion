using UnityEngine;

public class AchievementGroup
{
	public int currentIndex
	{
		get
		{
			return PlayerPrefs.GetInt(this.achievementInfo.AchiveType.ToString(), 0);
		}
		set
		{
			PlayerPrefs.SetInt(this.achievementInfo.AchiveType.ToString(), value);
		}
	}

	public bool isComplete
	{
		get
		{
			return PlayerPrefs.GetInt(this.achievementInfo.AchiveType.ToString() + "_isComplete", 0) == 1;
		}
		set
		{
			PlayerPrefs.SetInt(this.achievementInfo.AchiveType.ToString() + "_isComplete", (!value) ? 0 : 1);
		}
	}

	public bool isReceive
	{
		get
		{
			return PlayerPrefs.GetInt(this.achievementInfo.AchiveType.ToString() + this.achievementInfo.AchiveType.ToString(), 0) == 1;
		}
		set
		{
			PlayerPrefs.SetInt(this.achievementInfo.AchiveType.ToString() + this.achievementInfo.AchiveType.ToString(), (!value) ? 0 : 1);
		}
	}

	public int showIndex
	{
		get
		{
			return PlayerPrefs.GetInt(this.achievementInfo.AchiveType.ToString() + "_isShow", 0);
		}
		set
		{
			PlayerPrefs.SetInt(this.achievementInfo.AchiveType.ToString() + "_isShow", value);
		}
	}

	public int tempShowIndex
	{
		get
		{
			return PlayerPrefs.GetInt(this.achievementInfo.AchiveType.ToString() + "_tempShowIndex", 0);
		}
		set
		{
			PlayerPrefs.SetInt(this.achievementInfo.AchiveType.ToString() + "_tempShowIndex", value);
		}
	}

	public int totalProcess;

	public AchievementItem nextItem;

	public AchievementInfo achievementInfo;
}
