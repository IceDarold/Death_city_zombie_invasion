using System;
using UnityEngine;
using Zombie3D;

public class PlayerReload : StateMachineBehaviour
{
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		GameApp.GetInstance().GetGameScene().SetReloadProgressPercent(0f);
		GameApp.GetInstance().GetGameScene().SetReloadProgressEnable(true, true);
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		GameApp.GetInstance().GetGameScene().SetReloadProgressPercent(stateInfo.normalizedTime);
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		GameApp.GetInstance().GetGameScene().SetReloadProgressPercent(1f);
		GameApp.GetInstance().GetGameScene().SetReloadProgressEnable(false, true);
	}
}
