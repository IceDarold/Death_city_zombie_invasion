using System;

namespace Zombie3D
{
	public class WakeUpState : EnemyState
	{
		public override void SetKey()
		{
			this.key = StateKey.WAKEUP_STATE;
		}

		public override void NextState(Enemy enemy, float deltaTime)
		{
			if (enemy.HP <= 0f)
			{
				enemy.OnDead();
				enemy.SetState(Enemy.DEAD_STATE, true);
				return;
			}
			if (enemy.GetAnimationEnds(E_ANIMATION.WAKEUP))
			{
				if (enemy.ShouldGoToForceIdle())
				{
					return;
				}
				enemy.SetState(Enemy.CATCHING_STATE, true);
			}
		}

		public override void OnHit(Enemy enemy)
		{
		}
	}
}
