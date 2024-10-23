using System;

public class AchievementSystem
{
	public static AchievementSystem Instance
	{
		get
		{
			if (AchievementSystem.instance == null)
			{
				AchievementSystem.instance = new AchievementSystem();
			}
			return AchievementSystem.instance;
		}
	}

	public void CalculateAchievement(AchievementType _type, double _number)
	{
		AchievementGroup achieveGroup = Singleton<AchievementInfoManager>.Instance.GetAchieveGroup(_type);
		if (!achieveGroup.isComplete && achieveGroup.achievementInfo.AchiItems[achieveGroup.currentIndex].Count <= _number)
		{
			achieveGroup.isComplete = true;
			if (!this.isShow)
			{
				this.isShow = true;
			}
		}
	}

	public void AchievementPopUI(AchievementType _type, double _number)
	{
		AchievementGroup achieveGroup = Singleton<AchievementInfoManager>.Instance.GetAchieveGroup(_type);
		if (achieveGroup.showIndex <= achieveGroup.totalProcess && achieveGroup.achievementInfo.AchiItems[achieveGroup.showIndex].Count <= _number)
		{
			AchievementUIPop.instance.AddItemToStack(achieveGroup);
			achieveGroup.showIndex++;
		}
	}

	public void OnReceive(AchievementGroup _achieveGather)
	{
		if (_achieveGather.currentIndex <= _achieveGather.totalProcess)
		{
			_achieveGather.currentIndex++;
			if (_achieveGather.currentIndex > _achieveGather.totalProcess)
			{
				_achieveGather.currentIndex = _achieveGather.totalProcess;
				_achieveGather.nextItem = _achieveGather.achievementInfo.AchiItems[_achieveGather.totalProcess];
				_achieveGather.isReceive = true;
				_achieveGather.isComplete = true;
				return;
			}
			_achieveGather.nextItem = _achieveGather.achievementInfo.AchiItems[_achieveGather.currentIndex];
			_achieveGather.isReceive = false;
			_achieveGather.isComplete = false;
			this.isShow = false;
		}
	}

	private static AchievementSystem instance;

	private bool isShow;
}
