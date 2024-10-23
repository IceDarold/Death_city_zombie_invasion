using System;

namespace DataCenter
{
	public class CheckpointData
	{
		public int ID;

		public string Name;

		public string Mission;

		public string Describe;

		public CheckpointType Type;

		public ChapterEnum Chapters;

		public int Index;

		public int SceneID;

		public int DataID;

		public int RequireFighting;

		public int TimeLimit;

		public int[] AwardItem;

		public int[] AwardCount;

		public bool Unlock;

		public bool Passed;

		public int Stars;
	}
}
