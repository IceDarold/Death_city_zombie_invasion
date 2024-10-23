using System;

namespace Zombie3D
{
	public class PlayerSnipeState : PlayerState
	{
		public override void OnEnter(Player player)
		{
		}

		public override void NextState(Player player, float deltaTime)
		{
			player.InputController.ProcessInput(deltaTime, new InputInfo());
		}
	}
}
