using System;
using UnityEngine;

public class AngeryListener : BaseListener
{
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		this.enemy.SetAnimationStart(E_ANIMATION.ANGERY);
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (stateInfo.normalizedTime >= 1f)
		{
			this.enemy.SetAnimationEnd(E_ANIMATION.ANGERY);
		}
	}
}
