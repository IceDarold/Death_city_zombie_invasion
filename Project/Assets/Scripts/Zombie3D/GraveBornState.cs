using System;

namespace Zombie3D
{
	public class GraveBornState : EnemyState
	{
		public override void SetKey()
		{
			this.key = StateKey.GRAVEBORN_STATE;
		}

		public override void NextState(Enemy enemy, float deltaTime)
		{
			if (enemy.HP <= 0f)
			{
				enemy.OnDead();
				enemy.SetState(Enemy.DEAD_STATE, true);
			}
			if (enemy.MoveFromGrave(deltaTime))
			{
				enemy.SetInGrave(false);
				enemy.SetState(Enemy.IDLE_STATE, true);
			}
		}
	}
}
