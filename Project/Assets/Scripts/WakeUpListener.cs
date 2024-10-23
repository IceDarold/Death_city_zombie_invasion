using System;
using UnityEngine;

public class WakeUpListener : BaseListener
{
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		this.enemy.SetAnimationEnd(E_ANIMATION.WAKEUP);
	}
}
