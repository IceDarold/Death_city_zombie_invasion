using System;
using RootMotion.FinalIK;
using UnityEngine;

namespace RootMotion.Demos
{
	public class TwoHandedProp : MonoBehaviour
	{
		private void Start()
		{
			this.ik = base.GetComponent<FullBodyBipedIK>();
			IKSolverFullBodyBiped solver = this.ik.solver;
			solver.OnPostUpdate = (IKSolver.UpdateDelegate)Delegate.Combine(solver.OnPostUpdate, new IKSolver.UpdateDelegate(this.AfterFBBIK));
			this.ik.solver.leftHandEffector.positionWeight = 1f;
			this.ik.solver.rightHandEffector.positionWeight = 1f;
			if (this.ik.solver.rightHandEffector.target == null)
			{
				UnityEngine.Debug.LogError("Right Hand Effector needs a Target in this demo.");
			}
		}

		private void LateUpdate()
		{
			this.targetPosRelativeToRight = this.ik.references.rightHand.InverseTransformPoint(this.leftHandTarget.position);
			this.targetRotRelativeToRight = Quaternion.Inverse(this.ik.references.rightHand.rotation) * this.leftHandTarget.rotation;
			this.ik.solver.leftHandEffector.position = this.ik.solver.rightHandEffector.target.position + this.ik.solver.rightHandEffector.target.rotation * this.targetPosRelativeToRight;
			this.ik.solver.leftHandEffector.rotation = this.ik.solver.rightHandEffector.target.rotation * this.targetRotRelativeToRight;
		}

		private void AfterFBBIK()
		{
			this.ik.solver.leftHandEffector.bone.rotation = this.ik.solver.leftHandEffector.rotation;
			this.ik.solver.rightHandEffector.bone.rotation = this.ik.solver.rightHandEffector.rotation;
		}

		private void OnDestroy()
		{
			if (this.ik != null)
			{
				IKSolverFullBodyBiped solver = this.ik.solver;
				solver.OnPostUpdate = (IKSolver.UpdateDelegate)Delegate.Remove(solver.OnPostUpdate, new IKSolver.UpdateDelegate(this.AfterFBBIK));
			}
		}

		[Tooltip("The left hand target parented to the right hand.")]
		public Transform leftHandTarget;

		private FullBodyBipedIK ik;

		private Vector3 targetPosRelativeToRight;

		private Quaternion targetRotRelativeToRight;
	}
}
