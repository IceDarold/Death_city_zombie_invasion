using System;

namespace Zombie3D
{
	public class StopState : EnemyState
	{
		public override void SetKey()
		{
			this.key = StateKey.STOP_STATE;
		}

		public override void NextState(Enemy enemy, float deltaTime)
		{
		}
	}
}
