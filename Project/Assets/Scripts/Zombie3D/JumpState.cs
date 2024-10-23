using System;

namespace Zombie3D
{
	public class JumpState : EnemyState
	{
		public override void SetKey()
		{
			this.key = StateKey.JUMP_STATE;
		}

		public override void NextState(Enemy enemy, float deltaTime)
		{
			if (enemy.HP <= 0f)
			{
				enemy.OnDead();
				enemy.SetState(Enemy.DEAD_STATE, true);
				return;
			}
			enemy.DoJump(deltaTime);
		}

		public override void DoEnter(Enemy enemy)
		{
			base.DoEnter(enemy);
		}

		public override void DoExit(Enemy enemy)
		{
			base.DoExit(enemy);
			enemy.JumpStateReset();
		}

		public override void OnHit(Enemy enemy)
		{
		}
	}
}
