using System;
using Zombie3D;

public class EnemySkillState : EnemyState
{
	public override void SetKey()
	{
		this.key = StateKey.SKILL_STATE;
	}

	public override void NextState(Enemy enemy, float deltaTime)
	{
		if (enemy.HP <= 0f)
		{
			enemy.OnDead();
			enemy.SetState(Enemy.DEAD_STATE, true);
			return;
		}
		if (enemy.GetAnimationEnds(E_ANIMATION.SKILL))
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
}
