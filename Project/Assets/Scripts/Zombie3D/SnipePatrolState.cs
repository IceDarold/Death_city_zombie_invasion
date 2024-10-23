using System;

namespace Zombie3D
{
	public class SnipePatrolState : EnemyState
	{
		public override void SetKey()
		{
			this.key = StateKey.SNIPE_PATROL;
		}

		public override void NextState(Enemy enemy, float deltaTime)
		{
			if (enemy.HP <= 0f)
			{
				enemy.OnDead();
				enemy.SetState(Enemy.DEAD_STATE, true);
				return;
			}
			enemy.DoSnipePatrol(deltaTime);
		}

		public override void DoExit(Enemy enemy)
		{
			base.DoExit(enemy);
		}

		public override void DoEnter(Enemy enemy)
		{
			enemy.DoChangeSnipePatrolAction();
		}

		public override void OnHit(Enemy enemy)
		{
			enemy.SetState(Enemy.GOTHIT_STATE, true);
			enemy.SetPreState(Enemy.SNIPE_PATROL);
			enemy.DoChangeToEscapeAction();
		}
	}
}
