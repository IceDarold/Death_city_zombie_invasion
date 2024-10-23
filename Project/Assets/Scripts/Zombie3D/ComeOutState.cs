using System;

namespace Zombie3D
{
	public class ComeOutState : EnemyState
	{
		public override void SetKey()
		{
			this.key = StateKey.COMEOUT_STATE;
		}

		public override void NextState(Enemy enemy, float deltaTime)
		{
			if (enemy.HP <= 0f)
			{
				enemy.OnDead();
				enemy.SetState(Enemy.DEAD_STATE, true);
			}
			if (enemy.GetAnimationEnds(E_ANIMATION.COMEOUT))
			{
				if (enemy.ShouldGoToForceIdle())
				{
					return;
				}
				enemy.SetState(Enemy.CATCHING_STATE, enemy.EnemyType == EnemyType.E_BOMBER2);
			}
		}

		public override void OnHit(Enemy enemy)
		{
		}
	}
}
