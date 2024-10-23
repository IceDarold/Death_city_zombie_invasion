using System;

namespace Zombie3D
{
	public class PlayerRunAndShootState : PlayerState
	{
		public override void OnEnter(Player player)
		{
			player.GetPlayerIK().SetRequiredIKWeight(1f, 1f);
			player.PlayAudio("AimWalk");
		}

		public override void OnExit(Player player)
		{
			player.StopAudio("AimWalk");
		}

		public override void NextState(Player player, float deltaTime)
		{
			InputController inputController = player.InputController;
			InputInfo inputInfo = new InputInfo();
			inputController.ProcessInput(deltaTime, inputInfo);
			player.ZoomIn(deltaTime);
			player.SetPlayerAnimatorParameter(inputInfo.IsMoving(), inputInfo.fire, false);
			player.Move(inputInfo.moveDirection * (deltaTime * player.WalkSpeed));
			if (inputInfo.IsMoving() && !player.IsShooting)
			{
				player.SetState(Player.AIM_STATE);
			}
			else if (!inputInfo.IsMoving() && !player.IsShooting)
			{
				player.SetState(Player.AIM_STATE);
			}
			else if (!inputInfo.IsMoving() && player.IsShooting)
			{
				player.SetState(Player.SHOOT_STATE);
			}
		}
	}
}
