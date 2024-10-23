using System;
using UnityEngine;

namespace Zombie3D
{
	public class PlayerFullbodyActionState : PlayerState
	{
		public override void OnEnter(Player player)
		{
			player.GetPlayerIK().SetRequiredIKWeight(0f, 0f);
		}

		public override void NextState(Player player, float deltaTime)
		{
			player.ZoomOut(deltaTime);
			player.SetPlayerAnimatorParameter(false, false, false);
			InputInfo inputInfo = new InputInfo();
			player.InputController.ProcessInput(deltaTime, inputInfo);
			if (!player.IsFullBodyActionOver)
			{
				player.InputController.CameraRotation = Vector2.zero;
				return;
			}
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
