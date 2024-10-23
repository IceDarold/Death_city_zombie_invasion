using System;
using RootMotion.FinalIK;
using UnityEngine;

namespace RootMotion.Demos
{
	public class EffectorOffset : OffsetModifier
	{
		protected override void OnModifyOffset()
		{
			this.ik.solver.leftHandEffector.maintainRelativePositionWeight = this.handsMaintainRelativePositionWeight;
			this.ik.solver.rightHandEffector.maintainRelativePositionWeight = this.handsMaintainRelativePositionWeight;
			this.ik.solver.bodyEffector.positionOffset += base.transform.rotation * this.bodyOffset * this.weight;
			this.ik.solver.leftShoulderEffector.positionOffset += base.transform.rotation * this.leftShoulderOffset * this.weight;
			this.ik.solver.rightShoulderEffector.positionOffset += base.transform.rotation * this.rightShoulderOffset * this.weight;
			this.ik.solver.leftThighEffector.positionOffset += base.transform.rotation * this.leftThighOffset * this.weight;
			this.ik.solver.rightThighEffector.positionOffset += base.transform.rotation * this.rightThighOffset * this.weight;
			this.ik.solver.leftHandEffector.positionOffset += base.transform.rotation * this.leftHandOffset * this.weight;
			this.ik.solver.rightHandEffector.positionOffset += base.transform.rotation * this.rightHandOffset * this.weight;
			this.ik.solver.leftFootEffector.positionOffset += base.transform.rotation * this.leftFootOffset * this.weight;
			this.ik.solver.rightFootEffector.positionOffset += base.transform.rotation * this.rightFootOffset * this.weight;
		}

		[Range(0f, 1f)]
		public float handsMaintainRelativePositionWeight;

		public Vector3 bodyOffset;

		public Vector3 leftShoulderOffset;

		public Vector3 rightShoulderOffset;

		public Vector3 leftThighOffset;

		public Vector3 rightThighOffset;

		public Vector3 leftHandOffset;

		public Vector3 rightHandOffset;

		public Vector3 leftFootOffset;

		public Vector3 rightFootOffset;
	}
}
