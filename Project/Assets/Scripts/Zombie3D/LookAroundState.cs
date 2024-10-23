using System;
using UnityEngine;

namespace Zombie3D
{
	public class LookAroundState : EnemyState
	{
		public override void SetKey()
		{
		}

		public override void NextState(Enemy enemy, float deltaTime)
		{
			if (enemy.HP <= 0f)
			{
				enemy.OnDead();
				enemy.SetState(Enemy.DEAD_STATE, true);
				return;
			}
			Hunter hunter = enemy as Hunter;
			if (hunter.LookAroundTimOut())
			{
				if (hunter.ReadyForJump())
				{
					int num = UnityEngine.Random.Range(0, 100);
					if (num < 100)
					{
						hunter.StartJump();
						hunter.SetState(Hunter.JUMP_STATE, true);
					}
					else
					{
						hunter.SetState(Enemy.CATCHING_STATE, true);
					}
				}
				else
				{
					hunter.SetState(Enemy.CATCHING_STATE, true);
				}
			}
		}
	}
}
