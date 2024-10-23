using System;

namespace Zombie3D
{
	public class PlayerChangeGunState : PlayerState
	{
		public override void OnEnter(Player player)
		{
			bool @bool = player.GetPlayerIK().animator.GetBool("isAim");
			player.GetPlayerIK().SetRequiredIKWeight((float)((!@bool) ? 0 : 1), 1f);
		}

		public override void NextState(Player player, float deltaTime)
		{
			InputController inputController = player.InputController;
			InputInfo inputInfo = new InputInfo();
			inputController.ProcessInput(deltaTime, inputInfo);
			player.SetPlayerAnimatorParameter(inputInfo.IsMoving(), inputInfo.fire, false);
			player.Move(inputInfo.moveDirection * (deltaTime * player.WalkSpeed));
		}
	}
}
