using System;

namespace Zombie3D
{
	public class PatrolState : EnemyState
	{
		public override void SetKey()
		{
			this.key = StateKey.PATROL_STATE;
		}

		public override void NextState(Enemy enemy, float deltaTime)
		{
			if (enemy.HP <= 0f)
			{
				enemy.OnDead();
				enemy.SetState(Enemy.DEAD_STATE, true);
				return;
			}
			if (enemy.CheckWakeUpRange())
			{
				enemy.WakeUp();
				return;
			}
			enemy.DoPatrol(deltaTime);
		}

		public override void DoEnter(Enemy enemy)
		{
			enemy.SetPatrolCenter();
		}

		public override void DoExit(Enemy enemy)
		{
			base.DoExit(enemy);
		}
	}
}
