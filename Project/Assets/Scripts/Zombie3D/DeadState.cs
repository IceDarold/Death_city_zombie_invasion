using System;

namespace Zombie3D
{
	public class DeadState : EnemyState
	{
		public override void SetKey()
		{
			this.key = StateKey.DEAD_STATE;
		}

		public override void NextState(Enemy enemy, float deltaTime)
		{
			enemy.RemoveDeadBodyTimer(deltaTime);
		}

		public override void OnHit(Enemy enemy)
		{
		}

		public override void DoExit(Enemy enemy)
		{
			enemy.OnEnemyAttackBoxDisable();
		}
	}
}
