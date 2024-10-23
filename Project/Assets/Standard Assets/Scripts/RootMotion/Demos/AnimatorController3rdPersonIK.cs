using System;
using RootMotion.FinalIK;
using UnityEngine;

namespace RootMotion.Demos
{
	[RequireComponent(typeof(AimIK))]
	[RequireComponent(typeof(FullBodyBipedIK))]
	public class AnimatorController3rdPersonIK : AnimatorController3rdPerson
	{
		protected override void Start()
		{
			base.Start();
			this.aim = base.GetComponent<AimIK>();
			this.ik = base.GetComponent<FullBodyBipedIK>();
			IKSolverFullBodyBiped solver = this.ik.solver;
			solver.OnPreRead = (IKSolver.UpdateDelegate)Delegate.Combine(solver.OnPreRead, new IKSolver.UpdateDelegate(this.OnPreRead));
			this.aim.enabled = false;
			this.ik.enabled = false;
			this.headLookAxis = this.ik.references.head.InverseTransformVector(this.ik.references.root.forward);
			this.animator.SetLayerWeight(1, 1f);
		}

		public override void Move(Vector3 moveInput, bool isMoving, Vector3 faceDirection, Vector3 aimTarget)
		{
			base.Move(moveInput, isMoving, faceDirection, aimTarget);
			this.aimTarget = aimTarget;
			this.Read();
			this.AimIK();
			this.FBBIK();
			this.HeadLookAt(aimTarget);
		}

		private void Read()
		{
			this.leftHandPosRelToRightHand = this.ik.references.rightHand.InverseTransformPoint(this.ik.references.leftHand.position);
			this.leftHandRotRelToRightHand = Quaternion.Inverse(this.ik.references.rightHand.rotation) * this.ik.references.leftHand.rotation;
		}

		private void AimIK()
		{
			this.aim.solver.IKPosition = this.aimTarget;
			this.aim.solver.Update();
		}

		private void FBBIK()
		{
			this.rightHandRotation = this.ik.references.rightHand.rotation;
			Vector3 b = this.ik.references.rightHand.rotation * this.gunHoldOffset;
			this.ik.solver.rightHandEffector.positionOffset += b;
			if (this.recoil != null)
			{
				this.recoil.SetHandRotations(this.rightHandRotation * this.leftHandRotRelToRightHand, this.rightHandRotation);
			}
			this.ik.solver.Update();
			if (this.recoil != null)
			{
				this.ik.references.rightHand.rotation = this.recoil.rotationOffset * this.rightHandRotation;
				this.ik.references.leftHand.rotation = this.recoil.rotationOffset * this.rightHandRotation * this.leftHandRotRelToRightHand;
			}
			else
			{
				this.ik.references.rightHand.rotation = this.rightHandRotation;
				this.ik.references.leftHand.rotation = this.rightHandRotation * this.leftHandRotRelToRightHand;
			}
		}

		private void OnPreRead()
		{
			Quaternion rotation = (!(this.recoil != null)) ? this.rightHandRotation : (this.recoil.rotationOffset * this.rightHandRotation);
			Vector3 a = this.ik.references.rightHand.position + this.ik.solver.rightHandEffector.positionOffset + rotation * this.leftHandPosRelToRightHand;
			this.ik.solver.leftHandEffector.positionOffset += a - this.ik.references.leftHand.position - this.ik.solver.leftHandEffector.positionOffset + rotation * this.leftHandOffset;
		}

		private void HeadLookAt(Vector3 lookAtTarget)
		{
			Quaternion b = Quaternion.FromToRotation(this.ik.references.head.rotation * this.headLookAxis, lookAtTarget - this.ik.references.head.position);
			this.ik.references.head.rotation = Quaternion.Lerp(Quaternion.identity, b, this.headLookWeight) * this.ik.references.head.rotation;
		}

		private void OnDestroy()
		{
			if (this.ik != null)
			{
				IKSolverFullBodyBiped solver = this.ik.solver;
				solver.OnPreRead = (IKSolver.UpdateDelegate)Delegate.Remove(solver.OnPreRead, new IKSolver.UpdateDelegate(this.OnPreRead));
			}
		}

		[Range(0f, 1f)]
		public float headLookWeight = 1f;

		public Vector3 gunHoldOffset;

		public Vector3 leftHandOffset;

		public Recoil recoil;

		private AimIK aim;

		private FullBodyBipedIK ik;

		private Vector3 headLookAxis;

		private Vector3 leftHandPosRelToRightHand;

		private Quaternion leftHandRotRelToRightHand;

		private Vector3 aimTarget;

		private Quaternion rightHandRotation;
	}
}
