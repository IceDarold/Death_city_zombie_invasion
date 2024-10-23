using System;

namespace Zombie3D
{
	public class WalkHitState : EnemyState
	{
		public override void SetKey()
		{
			this.key = StateKey.WALKHIT;
		}

		public override void NextState(Enemy enemy, float deltaTime)
		{
			if (enemy.HP <= 0f)
			{
				enemy.OnDead();
				enemy.SetState(Enemy.DEAD_STATE, true);
				return;
			}
			if (enemy.CouldEnterAttackState())
			{
				enemy.SetState(Enemy.ATTACK_STATE, true);
			}
			else
			{
				enemy.DoMove(deltaTime);
			}
		}
	}
}
