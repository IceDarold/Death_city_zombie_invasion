using System;

namespace Zombie3D
{
	public class ShoutState : EnemyState
	{
		public override void SetKey()
		{
			this.key = StateKey.SHOUT_STATE;
		}

		public override void NextState(Enemy enemy, float deltaTime)
		{
			if (enemy.HP <= 0f)
			{
				enemy.OnDead();
				enemy.SetState(Enemy.DEAD_STATE, true);
				return;
			}
			BossBoomer bossBoomer = enemy as BossBoomer;
			if (bossBoomer.DoShout(deltaTime))
			{
				if (bossBoomer.ShouldGoToForceIdle())
				{
					enemy.SetState(Enemy.FORCE_IDLE, true);
				}
				else
				{
					enemy.SetState(Enemy.DIZZINESS_STATE, true);
				}
			}
		}

		public override void DoExit(Enemy enemy)
		{
			GameApp.GetInstance().GetGameScene().SetUIDisplayEvnt(UIDisplayEvnt.SHOW_BOSS_SKILL_EFFECT, new float[1]);
		}

		public override void OnHit(Enemy enemy)
		{
		}
	}
}
