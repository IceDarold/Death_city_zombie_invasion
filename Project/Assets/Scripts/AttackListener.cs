using System;
using UnityEngine;

public class AttackListener : BaseListener
{
	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		this.enemy.SetAnimationEnd(E_ANIMATION.ATTACK);
	}

	protected bool playAnimationOver;
}
