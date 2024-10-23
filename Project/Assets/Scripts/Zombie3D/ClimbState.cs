using System;

namespace Zombie3D
{
	public class ClimbState : EnemyState
	{
		public override void SetKey()
		{
			this.key = StateKey.CLIMB_STATE;
		}

		public override void NextState(Enemy enemy, float deltaTime)
		{
			if (enemy.HP <= 0f)
			{
				enemy.OnDead();
				enemy.SetState(Enemy.DEAD_STATE, true);
				return;
			}
			enemy.DoClimb(deltaTime);
			if (enemy.GetAnimationEnds(E_ANIMATION.CLIMB))
			{
				if (enemy.ShouldGoToForceIdle())
				{
					return;
				}
				enemy.Nav.ActivateCurrentOffMeshLink(true);
				enemy.FixAndCompleteOffMeshLink();
				enemy.SetState(Enemy.CATCHING_STATE, true);
			}
		}

		public override void DoExit(Enemy enemy)
		{
			enemy.StopFixOffset();
			enemy.Nav.ActivateCurrentOffMeshLink(true);
		}

		public override void OnHit(Enemy enemy)
		{
		}
	}
}
