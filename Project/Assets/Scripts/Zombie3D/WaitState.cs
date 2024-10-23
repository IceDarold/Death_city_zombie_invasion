using System;

namespace Zombie3D
{
	public class WaitState : EnemyState
	{
		public override void SetKey()
		{
			this.key = StateKey.WAIT_STATE;
		}

		public override void NextState(Enemy enemy, float dt)
		{
			if (enemy.CheckWakeUpRange())
			{
				enemy.WakeUp();
			}
		}

		public override void OnHit(Enemy enemy)
		{
			enemy.WakeUp();
		}
	}
}
