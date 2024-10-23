using System;
using UnityEngine;

namespace Zombie3D
{
	public class SurvivalQuest : Quest
	{
		public override void Init()
		{
			base.Init();
			this.questType = QuestType.KillAll;
			this.startedTime = Time.time;
		}

		public override void DoLogic()
		{
			base.DoLogic();
			Player player = this.gameScene.GetPlayer();
		}

		public override string GetQuestInfo()
		{
			return string.Format("{0:00}", this.timeSurvive / 60) + ":" + string.Format("{0:00}", this.timeSurvive % 60);
		}

		protected int enemyKilled;

		protected float survivalTime;

		protected float startedTime;

		protected int timeSurvive;
	}
}
