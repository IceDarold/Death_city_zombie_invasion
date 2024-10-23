using System;

namespace Zombie3D
{
	public class BossAttack : EnemyState
	{
		public override void SetKey()
		{
			this.key = StateKey.BOSS_ATTACK;
		}

		public override void NextState(Enemy enemy, float deltaTime)
		{
			if (enemy.HP <= 0f)
			{
				enemy.OnDead();
				enemy.SetState(Enemy.DEAD_STATE, true);
				return;
			}
		}

		public override void DoExit(Enemy enemy)
		{
			(enemy as BossButcher).OnBossButcherAttackStateExit();
		}

		public override void OnHit(Enemy enemy)
		{
		}
	}
}
