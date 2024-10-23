using System;
using UnityEngine;

namespace Zombie3D
{
	public class BombQuest : Quest
	{
		public override void Init()
		{
			base.Init();
			Transform transform = GameObject.Find("Exit").transform;
			this.exitPosition = transform.position;
			this.exitGlowRenderer = transform.Find("glow").GetComponent<Renderer>();
			this.exitGlowRenderer.enabled = false;
			this.questType = QuestType.Bomb;
			this.bombLeft = this.bombTotal;
		}

		public override void DoLogic()
		{
			base.DoLogic();
			Player player = this.gameScene.GetPlayer();
			if (this.bombCompleted && (player.GetTransform().position - this.exitPosition).sqrMagnitude < this.radius * this.radius)
			{
				this.questCompleted = true;
			}
		}

		public override string GetQuestInfo()
		{
			string result = string.Concat(new object[]
			{
				"Mission: ",
				this.questType.ToString(),
				" ",
				this.bombLeft,
				"/",
				this.bombTotal
			});
			if (this.bombCompleted && !this.questCompleted)
			{
				result = "Mission: Bomb Complete, Get to The Exit!";
			}
			if (this.questCompleted)
			{
				result = "Mission Complete";
			}
			return result;
		}

		protected bool bombCompleted;

		protected Vector3 exitPosition;

		protected Renderer exitGlowRenderer;

		protected float radius = 2f;

		protected int bombTotal;

		protected int bombLeft;
	}
}
