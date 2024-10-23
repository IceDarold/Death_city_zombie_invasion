using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zombie3D
{
	public class PathEndState : EnemyState
	{
		public PathEndState(List<string> str, List<float> speed)
		{
			this.aniName = str;
			this.moveSpeed = speed;
		}

		public override void SetKey()
		{
		}

		public string AniName
		{
			get
			{
				return this.aniName[this.aniIndex];
			}
		}

		public int AniCount
		{
			get
			{
				return this.aniName.Count;
			}
		}

		public override void NextState(Enemy enemy, float deltaTime)
		{
			if (enemy.HP <= 0f)
			{
				enemy.OnDead();
				enemy.SetState(Enemy.DEAD_STATE, true);
			}
			if (this.moveSpeed[this.aniIndex] > 0f)
			{
			}
			if (enemy.PathEndAnimationEnds())
			{
				this.aniIndex++;
				if (this.aniIndex >= this.aniName.Count)
				{
					enemy.SetState(Enemy.CATCHING_STATE, true);
				}
				else
				{
					enemy.pathEndTime = Time.time;
				}
			}
		}

		protected List<string> aniName;

		protected List<float> moveSpeed;

		protected int aniIndex;
	}
}
