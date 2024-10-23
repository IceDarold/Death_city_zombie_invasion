using System;

namespace Zombie3D
{
	public class DizzinessState : EnemyState
	{
		public override void SetKey()
		{
			this.key = StateKey.DIZZINESS;
		}

		public override void NextState(Enemy enemy, float deltaTime)
		{
			if (enemy.HP <= 0f)
			{
				enemy.OnDead();
				enemy.SetState(Enemy.DEAD_STATE, true);
				return;
			}
			if (enemy.GetAnimationEnds(E_ANIMATION.DIZZINESS))
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

		public override void DoExit(Enemy enemy)
		{
			enemy.SetAnimationEnd(E_ANIMATION.DIZZINESS);
		}

		public override void DoEnter(Enemy enemy)
		{
			enemy.DoDizziness(0f);
		}
	}
}
