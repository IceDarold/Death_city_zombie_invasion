using System;

namespace Zombie3D
{
	public class GotHitState : EnemyState
	{
		public override void SetKey()
		{
			this.key = StateKey.GOTHIT_STATE;
		}

		public override void NextState(Enemy enemy, float deltaTime)
		{
			if (enemy.HP <= 0f)
			{
				enemy.OnDead();
				enemy.SetState(Enemy.DEAD_STATE, true);
			}
			else if (enemy.GetAnimationEnds(E_ANIMATION.HURT))
			{
				if (enemy.EnemyType == EnemyType.E_BUTCHER || enemy.EnemyType == EnemyType.E_DESPOT)
				{
					enemy.SetState(Enemy.DIZZINESS_STATE, false);
					return;
				}
				if (enemy.ShouldGoToForceIdle())
				{
					return;
				}
				if (enemy.GetPreState() == Enemy.SNIPE_PATROL)
				{
					enemy.SetState(Enemy.SNIPE_PATROL, false);
				}
				else
				{
					enemy.SetState(Enemy.IDLE_STATE, false);
				}
			}
		}

		public override void OnHit(Enemy enemy)
		{
			if (enemy.CanChangeToGotHitState())
			{
				enemy.SetState(Enemy.GOTHIT_STATE, true);
			}
		}

		public override void DoEnter(Enemy enemy)
		{
			enemy.Nav.speed = 0f;
		}
	}
}
