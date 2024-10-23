using System;
using UnityEngine;

public class EnemyAnimationListener : BaseListener
{
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (this.ListenIntoStateEnter)
		{
			this.enemy.SetAnimationStart(this.curAnimationState);
		}
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (this.ListenIntoStateUpdate && stateInfo.normalizedTime >= this.loopTimes)
		{
			this.enemy.SetAnimationEnd(this.curAnimationState);
		}
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (this.ListenIntoStateExit)
		{
			this.enemy.SetAnimationEnd(this.curAnimationState);
		}
	}

	[Space(5f)]
	public E_ANIMATION curAnimationState = E_ANIMATION.NONE;

	[Space(5f)]
	public bool ListenIntoStateEnter;

	public bool ListenIntoStateUpdate;

	public bool ListenIntoStateExit;

	public float loopTimes = 1f;
}
