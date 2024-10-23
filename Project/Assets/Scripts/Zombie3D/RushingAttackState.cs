using System;

namespace Zombie3D
{
	public class RushingAttackState : EnemyState
	{
		public override void SetKey()
		{
			this.key = StateKey.RUSH_ATTACK;
		}

		public override void NextState(Enemy enemy, float deltaTime)
		{
			if (enemy.HP <= 0f)
			{
				enemy.OnDead();
				enemy.SetState(Enemy.DEAD_STATE, true);
				return;
			}
			if (enemy.GetAnimationEnds(E_ANIMATION.RUSH_ATTACK))
			{
				if (enemy.ShouldGoToForceIdle())
				{
					return;
				}
				enemy.SetState(Enemy.RUSH_END, true);
			}
		}

		public override void DoExit(Enemy enemy)
		{
			enemy.OnEnemyAttackBoxDisable();
			enemy.StopAudio("Rush");
		}

		public override void DoEnter(Enemy enemy)
		{
			enemy.PlayAudio("Rush");
		}

		public override void OnHit(Enemy enemy)
		{
		}
	}
}
