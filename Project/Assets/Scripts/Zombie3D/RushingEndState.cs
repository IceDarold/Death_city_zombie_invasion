using System;

namespace Zombie3D
{
	public class RushingEndState : EnemyState
	{
		public override void SetKey()
		{
			this.key = StateKey.RUSH_END;
		}

		public override void NextState(Enemy enemy, float deltaTime)
		{
			if (enemy.HP <= 0f)
			{
				enemy.OnDead();
				enemy.SetState(Enemy.DEAD_STATE, true);
				return;
			}
			if (enemy.GetAnimationEnds(E_ANIMATION.RUSH_END))
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
