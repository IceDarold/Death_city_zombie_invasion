using System;

namespace Zombie3D
{
	public class ThrowState : EnemyState
	{
		public override void SetKey()
		{
			this.key = StateKey.THROW_STATE;
		}

		public override void NextState(Enemy enemy, float dt)
		{
			if (enemy.HP <= 0f)
			{
				enemy.OnDead();
				enemy.SetState(Enemy.DEAD_STATE, true);
				return;
			}
			if (enemy.GetAnimationEnds(E_ANIMATION.THROW))
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
