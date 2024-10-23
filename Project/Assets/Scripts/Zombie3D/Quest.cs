using System;

namespace Zombie3D
{
	public abstract class Quest
	{
		public QuestType GetQuestType()
		{
			return this.questType;
		}

		public bool QuestCompleted
		{
			get
			{
				return this.questCompleted;
			}
		}

		public virtual void Init()
		{
			this.questCompleted = false;
			this.gameScene = GameApp.GetInstance().GetGameScene();
		}

		public virtual void DoLogic()
		{
		}

		public abstract string GetQuestInfo();

		protected bool questCompleted;

		protected QuestType questType;

		protected GameScene gameScene;
	}
}
