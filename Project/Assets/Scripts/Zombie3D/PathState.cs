using System;

namespace Zombie3D
{
	public class PathState : EnemyState
	{
		public override void SetKey()
		{
			this.key = StateKey.PATHWALK_STATE;
		}

		public override void NextState(Enemy enemy, float deltaTime)
		{
			if (enemy.HP <= 0f)
			{
				enemy.OnDead();
				enemy.SetState(Enemy.DEAD_STATE, true);
			}
		}

		public override void OnHit(Enemy enemy)
		{
			enemy.SetState(Enemy.GOTHIT_STATE, true);
		}
	}
}
