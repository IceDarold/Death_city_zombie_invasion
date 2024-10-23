using System;

namespace Zombie3D
{
	public class PlayerIdleState : PlayerState
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
			if (!inputInfo.fire)
			{
				player.ZoomOut(deltaTime);
			}
			player.Move(inputInfo.moveDirection * (deltaTime * player.WalkSpeed));
			player.SetPlayerAnimatorParameter(inputInfo.IsMoving(), inputInfo.fire, false);
			if (inputInfo.fire && !player.GetWeapon().IsReload && !inputInfo.IsMoving())
			{
				player.SetState(Player.SHOOT_STATE);
			}
			else if (inputInfo.fire && inputInfo.IsMoving())
			{
				player.SetState(Player.RUNSHOOT_STATE);
			}
			else if (!inputInfo.fire && inputInfo.IsMoving())
			{
				player.SetState(Player.RUN_STATE);
			}
		}
	}
}
