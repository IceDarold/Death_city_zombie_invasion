using System;
using UnityEngine;

public class ComeOutListener : BaseListener
{
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		this.enemy.SetAnimationEnd(E_ANIMATION.COMEOUT);
	}
}
