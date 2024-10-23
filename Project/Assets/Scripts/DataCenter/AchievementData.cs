using System;

namespace DataCenter
{
	public class AchievementData
	{
		public int ID;

		public string Name;

		public string Icon;

		public string Describe;

		public AchievementType Type;

		public int Tag;

		public int[] TargetValue;

		public int[] AwardItem;

		public int[] AwardCount;

		public int CurrentValue;

		public int Level;

		public bool Reached;

		public bool Completed;
	}
}
