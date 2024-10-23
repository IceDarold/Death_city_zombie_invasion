using System;
using UnityEngine;

public class HurtListener : BaseListener
{
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		this.enemy.SetAnimationStart(E_ANIMATION.HURT);
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		this.enemy.SetAnimationEnd(E_ANIMATION.HURT);
	}
}
