using System;

namespace Zombie3D
{
	public class CatchingState : EnemyState
	{
		public override void SetKey()
		{
			this.key = StateKey.CATCHING_STATE;
		}

		public override void NextState(Enemy enemy, float deltaTime)
		{
			if (enemy.HP <= 0f)
			{
				enemy.OnDead();
				enemy.SetState(Enemy.DEAD_STATE, true);
				return;
			}
			if (enemy.CouldEnterAttackState())
			{
				enemy.SetState(Enemy.ATTACK_STATE, true);
			}
			else
			{
				enemy.DoMove(deltaTime);
			}
		}

		public override void DoEnter(Enemy enemy)
		{
			enemy.DoFindPath();
			enemy.PlayAudio("Walk");
		}

		public override void DoExit(Enemy enemy)
		{
			enemy.StopAudio("Walk");
		}
	}
}
