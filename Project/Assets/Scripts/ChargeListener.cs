using System;
using UnityEngine;

public class ChargeListener : StateMachineBehaviour
{
	public void SetDelegate(Action<bool> isInCharge, Action<float> chargePercent)
	{
		this.SetPlayerIsInCharge = isInCharge;
		this.SetChargePercent = chargePercent;
	}

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (this.SetPlayerIsInCharge != null)
		{
			this.SetPlayerIsInCharge(true);
		}
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (this.SetChargePercent != null)
		{
			this.SetChargePercent(Mathf.Clamp01(stateInfo.normalizedTime));
		}
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (this.SetPlayerIsInCharge != null)
		{
			this.SetPlayerIsInCharge(false);
		}
	}

	protected Action<bool> SetPlayerIsInCharge;

	protected Action<float> SetChargePercent;
}
