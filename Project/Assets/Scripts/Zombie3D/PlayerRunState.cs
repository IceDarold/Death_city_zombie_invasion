using System;

namespace Zombie3D
{
	public class PlayerRunState : PlayerState
	{
		public override void OnEnter(Player player)
		{
			player.GetPlayerIK().SetRequiredIKWeight(0f, 0f);
			player.PlayAudio("Walk");
		}

		public override void OnExit(Player player)
		{
			player.StopAudio("Walk");
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
			player.SetPlayerAnimatorParameter(inputInfo.IsMoving(), inputInfo.fire, false);
			player.Move(inputInfo.moveDirection * (deltaTime * player.WalkSpeed));
			Weapon weapon = player.GetWeapon();
			if (!inputInfo.fire && !inputInfo.IsMoving())
			{
				player.SetState(Player.IDLE_STATE);
			}
			else if (inputInfo.fire && inputInfo.IsMoving())
			{
				if (weapon.HaveBullets())
				{
					player.SetState(Player.RUNSHOOT_STATE);
				}
			}
			else if (inputInfo.fire && !inputInfo.IsMoving())
			{
				if (!player.GetWeapon().IsReload)
				{
					player.SetState(Player.SHOOT_STATE);
				}
				else
				{
					player.SetState(Player.IDLE_STATE);
				}
			}
		}
	}
}
