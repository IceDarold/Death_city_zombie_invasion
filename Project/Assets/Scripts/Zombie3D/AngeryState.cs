using System;

namespace Zombie3D
{
	public class AngeryState : EnemyState
	{
		public override void SetKey()
		{
			this.key = StateKey.ANGERY_STATE;
		}

		public override void NextState(Enemy enemy, float deltaTime)
		{
			if (enemy.HP <= 0f)
			{
				enemy.OnDead();
				enemy.SetState(Enemy.DEAD_STATE, true);
				return;
			}
			if (enemy.GetAnimationEnds(E_ANIMATION.ANGERY))
			{
				enemy.SetState(Enemy.IDLE_STATE, false);
			}
		}
	}
}
