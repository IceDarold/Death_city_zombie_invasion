using System;

namespace Zombie3D
{
	public class RushingStartState : EnemyState
	{
		public override void SetKey()
		{
			this.key = StateKey.RUSH_START;
		}

		public override void NextState(Enemy enemy, float deltaTime)
		{
			if (enemy.HP <= 0f)
			{
				enemy.OnDead();
				enemy.SetState(Enemy.DEAD_STATE, true);
				return;
			}
			enemy.RotateEnemyToTarget();
			if (enemy.GetAnimationEnds(E_ANIMATION.RUSH_START))
			{
				if (enemy.ShouldGoToForceIdle())
				{
					return;
				}
				enemy.SetState(Enemy.RUSH_ATTACK, true);
			}
		}

		public override void OnHit(Enemy enemy)
		{
		}

		public override void DoExit(Enemy enemy)
		{
			enemy.StopAudio("RushStart");
		}

		public override void DoEnter(Enemy enemy)
		{
			enemy.PlayAudio("RushStart");
		}
	}
}
