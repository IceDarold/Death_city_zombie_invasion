using System;

namespace DataCenter
{
	public class DailyMission
	{
		public int ID;

		public string Describe;

		public DailyMissionType Type;

		public int TargetValue;

		public int[] LevelRange;

		public int[] AwardID;

		public int[] AwardCount;

		public int CurrentValue;

		public int State;
	}
}
