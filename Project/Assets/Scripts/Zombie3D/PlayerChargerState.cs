using System;
using UnityEngine;

namespace Zombie3D
{
	public class PlayerChargerState : PlayerState
	{
		public override void OnEnter(Player player)
		{
			bool @bool = player.GetPlayerIK().animator.GetBool("isAim");
			player.GetPlayerIK().SetRequiredIKWeight((float)((!@bool) ? 0 : 1), 1f);
		}

		public override void OnExit(Player player)
		{
			if (player.pickOutCharger != null)
			{
				player.pickOutCharger.SetActive(false);
				UnityEngine.Object.Destroy(player.pickOutCharger);
				player.pickOutCharger = null;
			}
			if (player.pickOnCharger != null)
			{
				player.pickOnCharger.SetActive(false);
				UnityEngine.Object.Destroy(player.pickOnCharger);
				player.pickOnCharger = null;
			}
			player.GetWeapon().GunAtt.OnPickOnCharger();
			player.GetWeapon().GunAtt.Reset();
			GameApp.GetInstance().GetGameScene().SetReloadProgressEnable(false, false);
		}

		public override void NextState(Player player, float deltaTime)
		{
			InputController inputController = player.InputController;
			InputInfo inputInfo = new InputInfo();
			inputController.ProcessInput(deltaTime, inputInfo);
			player.SetPlayerAnimatorParameter(inputInfo.IsMoving(), inputInfo.fire, false);
			player.Move(inputInfo.moveDirection * (deltaTime * player.WalkSpeed));
			player.ZoomOut(deltaTime);
		}
	}
}
