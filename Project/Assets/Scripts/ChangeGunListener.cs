using System;
using UnityEngine;

public class ChangeGunListener : StateMachineBehaviour
{
	public void SetDelegate(Action<bool> callback)
	{
		this.SetPlayerIsChangeGun = callback;
	}

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (this.SetPlayerIsChangeGun != null)
		{
			this.SetPlayerIsChangeGun(true);
		}
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (this.SetPlayerIsChangeGun != null)
		{
			this.SetPlayerIsChangeGun(false);
		}
	}

	protected Action<bool> SetPlayerIsChangeGun;
}
