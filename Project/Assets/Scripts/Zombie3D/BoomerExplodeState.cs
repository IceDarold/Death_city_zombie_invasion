using System;

namespace Zombie3D
{
	public class BoomerExplodeState : EnemyState
	{
		public override void SetKey()
		{
			this.key = StateKey.EXPLOD_STATE;
		}

		public override void NextState(Enemy enemy, float deltaTime)
		{
			Boomer boomer = enemy as Boomer;
			if (enemy.GetAnimationEnds(E_ANIMATION.EXPLOD))
			{
				enemy.SetState(Enemy.DEAD_STATE, true);
			}
		}
	}
}
