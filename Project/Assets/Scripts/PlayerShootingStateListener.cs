using System;
using UnityEngine;
using Zombie3D;

public class PlayerShootingStateListener : StateMachineBehaviour
{
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		GameApp.GetInstance().GetGameScene().GetPlayer().GetWeapon().SetContinuousShoot(true);
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		GameApp.GetInstance().GetGameScene().GetPlayer().GetWeapon().SetContinuousShoot(false);
	}
}
