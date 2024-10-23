using System;

namespace Zombie3D
{
	public class PlayerGotHitState : PlayerState
	{
		public override void OnEnter(Player player)
		{
			player.GetPlayerIK().SetRequiredIKWeight(0f, 0f);
		}

		public override void NextState(Player player, float deltaTime)
		{
			player.SetState(Player.IDLE_STATE);
		}
	}
}
