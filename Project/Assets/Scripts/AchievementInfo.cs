using System;

[Serializable]
public class AchievementInfo
{
	public AchievementInfo()
	{
	}

	public AchievementInfo(AchievementType _type, string _name, string _desc, AchievementItem[] _items)
	{
		this.AchiveType = _type;
		this.Name = _name;
		this.Desc = _desc;
		this.AchiItems = _items;
	}

	public AchievementType AchiveType;

	public string Name;

	public string Desc;

	public AchievementItem[] AchiItems;
}
