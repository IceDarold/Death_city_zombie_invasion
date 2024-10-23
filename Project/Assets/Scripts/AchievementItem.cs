using System;

[Serializable]
public class AchievementItem
{
	public AchievementItem()
	{
	}

	public AchievementItem(double _count, string _reward)
	{
		this.Count = _count;
		this.Rewards = _reward;
	}

	public double Count;

	public string Rewards;
}
