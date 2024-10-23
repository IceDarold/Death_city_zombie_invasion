using System;

namespace Zombie3D
{
	public class ForceIdleState : EnemyState
	{
		public override void SetKey()
		{
			this.key = StateKey.FORCE_IDLE;
		}

		public override void NextState(Enemy enemy, float deltaTime)
		{
		}
	}
}
