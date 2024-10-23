using System;

namespace Zombie3D
{
	public class PlayerCannonState : PlayerState
	{
		public override void OnEnter(Player player)
		{
			player.GetPlayerIK().SetRequiredIKWeight(0f, 0f);
		}

		public override void NextState(Player player, float deltaTime)
		{
			InputController inputController = player.InputController;
			InputInfo inputInfo = new InputInfo();
			inputController.ProcessInput(deltaTime, inputInfo);
			player.ZoomOut(deltaTime);
		}
	}
}
