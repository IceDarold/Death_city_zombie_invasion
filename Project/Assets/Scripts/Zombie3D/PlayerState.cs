using System;

namespace Zombie3D
{
	public abstract class PlayerState
	{
		public virtual void NextState(Player player, float deltaTime)
		{
		}

		public virtual void OnHit(Player player, float damage, bool isCritical)
		{
			if (player.HP <= 0f)
			{
				player.OnDead();
				player.SetState(Player.DEAD_STATE);
			}
			else if (isCritical && player.CouldGetAnotherHit())
			{
				player.StopFire();
				player.DoHurt();
			}
		}

		public virtual void OnEnter(Player player)
		{
		}

		public virtual void OnExit(Player player)
		{
		}
	}
}
