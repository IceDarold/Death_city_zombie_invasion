using System;

namespace Zombie3D
{
	public class PlayerAimState : PlayerState
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
			InputInfo inputInfo = new InputInfo();
			player.InputController.ProcessInput(deltaTime, inputInfo);
			player.ZoomIn(deltaTime);
			player.SetPlayerAnimatorParameter(inputInfo.IsMoving(), inputInfo.fire, false);
			player.Move(inputInfo.moveDirection * (deltaTime * player.WalkSpeed));
			if (inputInfo.fire)
			{
				player.SetState(Player.SHOOT_STATE);
			}
			else if (player.CanChangeToIdle(deltaTime))
			{
				if (inputInfo.IsMoving())
				{
					player.SetState(Player.RUN_STATE);
					player.SetIsAim(false);
				}
				else
				{
					player.SetState(Player.IDLE_STATE);
					player.SetIsAim(false);
				}
			}
		}
	}
}
