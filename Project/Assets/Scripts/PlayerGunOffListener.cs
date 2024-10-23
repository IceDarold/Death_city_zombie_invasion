using System;
using UnityEngine;
using Zombie3D;

public class PlayerGunOffListener : StateMachineBehaviour
{
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (this.playerIK == null)
		{
			this.playerIK = GameApp.GetInstance().GetGameScene().GetPlayer().GetPlayerIK();
		}
		this.playerIK.RecorderPlayerIk();
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		this.playerIK.SetIkWeightInChangeGun(stateInfo.normalizedTime);
	}

	protected PlayerIK playerIK;
}
