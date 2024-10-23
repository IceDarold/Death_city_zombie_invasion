using System;

namespace Zombie3D
{
	public class AttackState : EnemyState
	{
		public override void SetKey()
		{
			this.key = StateKey.ATTACK_STATE;
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
			if (enemy.GetAnimationEnds(E_ANIMATION.ATTACK))
			{
				if (enemy.ShouldGoToForceIdle())
				{
					return;
				}
				if (enemy.CouldEnterAttackState())
				{
					if (enemy.CheckAttackInterval(deltaTime))
					{
						enemy.OnAttack();
					}
				}
				else
				{
					enemy.SetState(Enemy.CATCHING_STATE, true);
				}
			}
		}

		public override void OnHit(Enemy enemy)
		{
		}
	}
}
