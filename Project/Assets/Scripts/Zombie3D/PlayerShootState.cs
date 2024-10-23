using System;

namespace Zombie3D
{
	public class PlayerShootState : PlayerState
	{
		public override void OnEnter(Player player)
		{
			player.GetPlayerIK().SetRequiredIKWeight(1f, 1f);
		}

		public override void NextState(Player player, float deltaTime)
		{
			InputController inputController = player.InputController;
			InputInfo inputInfo = new InputInfo();
			inputController.ProcessInput(deltaTime, inputInfo);
			player.ZoomIn(deltaTime);
			player.SetPlayerAnimatorParameter(inputInfo.IsMoving(), inputInfo.fire, false);
			if (inputInfo.IsMoving() && !player.IsShooting)
			{
				player.SetState(Player.AIM_STATE);
			}
			else if (!inputInfo.IsMoving() && !player.IsShooting)
			{
				player.SetState(Player.AIM_STATE);
			}
			else if (inputInfo.IsMoving() && player.IsShooting)
			{
				player.SetState(Player.RUNSHOOT_STATE);
			}
		}
	}
}
