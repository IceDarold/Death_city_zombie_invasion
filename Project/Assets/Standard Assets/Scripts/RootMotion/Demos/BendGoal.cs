using System;
using RootMotion.FinalIK;
using UnityEngine;

namespace RootMotion.Demos
{
	public class BendGoal : MonoBehaviour
	{
		private void Start()
		{
			UnityEngine.Debug.Log("BendGoal is deprecated, you can now a bend goal from the custom inspector of the LimbIK component.");
		}

		private void LateUpdate()
		{
			if (this.limbIK == null)
			{
				return;
			}
			this.limbIK.solver.SetBendGoalPosition(base.transform.position, this.weight);
		}

		public LimbIK limbIK;

		[Range(0f, 1f)]
		public float weight = 1f;
	}
}
