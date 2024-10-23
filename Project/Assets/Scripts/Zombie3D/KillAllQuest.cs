using System;

namespace Zombie3D
{
	public class KillAllQuest : Quest
	{
		public override void Init()
		{
			base.Init();
			this.questType = QuestType.KillAll;
		}

		public override void DoLogic()
		{
			base.DoLogic();
		}

		public override string GetQuestInfo()
		{
			string str = string.Empty;
			if (this.enemyLeft < 10)
			{
				str = this.enemyLeft.ToString();
			}
			else
			{
				str = "???";
			}
			string result = "Mission: Kill Them All  " + str;
			if (this.questCompleted)
			{
				result = "Mission Complete";
			}
			return result;
		}

		protected int enemyLeft;
	}
}
