using System;

namespace Zombie3D
{
	public abstract class EnemyState
	{
		public EnemyState()
		{
			this.SetKey();
		}

		public abstract void SetKey();

		public virtual void NextState(Enemy enemy, float deltaTime)
		{
		}

		public virtual void OnHit(Enemy enemy)
		{
			enemy.SetState(Enemy.GOTHIT_STATE, true);
		}

		public virtual void DoExit(Enemy enemy)
		{
			enemy.OnEnemyAttackBoxDisable();
		}

		public virtual void DoEnter(Enemy enemy)
		{
		}

		public StateKey key;
	}
}
