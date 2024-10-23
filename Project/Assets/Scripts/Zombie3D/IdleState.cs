using System;

namespace Zombie3D
{
	public class IdleState : EnemyState
	{
		public override void SetKey()
		{
			this.key = StateKey.IDLE_STATE;
		}

		public override void NextState(Enemy enemy, float deltaTime)
		{
			if (enemy.HP <= 0f)
			{
				enemy.OnDead();
				enemy.SetState(Enemy.DEAD_STATE, true);
				return;
			}
			if (enemy.SqrDistanceFromPlayer < enemy.DetectionRange * enemy.DetectionRange)
			{
				enemy.SetState(Enemy.CATCHING_STATE, true);
			}
		}
	}
}
