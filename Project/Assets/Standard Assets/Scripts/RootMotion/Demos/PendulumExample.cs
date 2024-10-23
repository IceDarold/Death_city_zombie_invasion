using System;
using RootMotion.FinalIK;
using UnityEngine;

namespace RootMotion.Demos
{
	[RequireComponent(typeof(FullBodyBipedIK))]
	public class PendulumExample : MonoBehaviour
	{
		private void Start()
		{
			this.ik = base.GetComponent<FullBodyBipedIK>();
			Quaternion rotation = this.target.rotation;
			this.target.rotation = this.leftHandTarget.rotation;
			FixedJoint fixedJoint = this.target.gameObject.AddComponent<FixedJoint>();
			fixedJoint.connectedBody = this.leftHandTarget.GetComponent<Rigidbody>();
			this.target.GetComponent<Rigidbody>().MoveRotation(rotation);
			this.rootRelativeToPelvis = Quaternion.Inverse(this.pelvisTarget.rotation) * base.transform.rotation;
			this.pelvisToRoot = Quaternion.Inverse(this.ik.references.pelvis.rotation) * (base.transform.position - this.ik.references.pelvis.position);
			this.rootTargetPosition = base.transform.position;
			this.rootTargetRotation = base.transform.rotation;
			this.lastWeight = this.weight;
		}

		private void LateUpdate()
		{
			if (this.weight > 0f)
			{
				this.ik.solver.leftHandEffector.positionWeight = this.weight;
				this.ik.solver.leftHandEffector.rotationWeight = this.weight;
			}
			else
			{
				this.rootTargetPosition = base.transform.position;
				this.rootTargetRotation = base.transform.rotation;
				if (this.lastWeight > 0f)
				{
					this.ik.solver.leftHandEffector.positionWeight = 0f;
					this.ik.solver.leftHandEffector.rotationWeight = 0f;
				}
			}
			this.lastWeight = this.weight;
			if (this.weight <= 0f)
			{
				return;
			}
			base.transform.position = Vector3.Lerp(this.rootTargetPosition, this.pelvisTarget.position + this.pelvisTarget.rotation * this.pelvisToRoot * this.hangingDistanceMlp, this.weight);
			base.transform.rotation = Quaternion.Lerp(this.rootTargetRotation, this.pelvisTarget.rotation * this.rootRelativeToPelvis, this.weight);
			this.ik.solver.leftHandEffector.position = this.leftHandTarget.position;
			this.ik.solver.leftHandEffector.rotation = this.leftHandTarget.rotation;
			Vector3 fromDirection = this.ik.references.pelvis.rotation * this.pelvisDownAxis;
			Quaternion b = Quaternion.FromToRotation(fromDirection, this.rightHandTarget.position - this.headTarget.position);
			this.ik.references.rightUpperArm.rotation = Quaternion.Lerp(Quaternion.identity, b, this.weight) * this.ik.references.rightUpperArm.rotation;
			Quaternion b2 = Quaternion.FromToRotation(fromDirection, this.leftFootTarget.position - this.bodyTarget.position);
			this.ik.references.leftThigh.rotation = Quaternion.Lerp(Quaternion.identity, b2, this.weight) * this.ik.references.leftThigh.rotation;
			Quaternion b3 = Quaternion.FromToRotation(fromDirection, this.rightFootTarget.position - this.bodyTarget.position);
			this.ik.references.rightThigh.rotation = Quaternion.Lerp(Quaternion.identity, b3, this.weight) * this.ik.references.rightThigh.rotation;
		}

		[Tooltip("The master weight of this script.")]
		[Range(0f, 1f)]
		public float weight = 1f;

		[Tooltip("Multiplier for the distance of the root to the target.")]
		public float hangingDistanceMlp = 1.3f;

		[Tooltip("Where does the root of the character land when weight is blended out?")]
		[HideInInspector]
		public Vector3 rootTargetPosition;

		[Tooltip("How is the root of the character rotated when weight is blended out?")]
		[HideInInspector]
		public Quaternion rootTargetRotation;

		[SerializeField]
		private Transform target;

		[SerializeField]
		private Transform leftHandTarget;

		[SerializeField]
		private Transform rightHandTarget;

		[SerializeField]
		private Transform leftFootTarget;

		[SerializeField]
		private Transform rightFootTarget;

		[SerializeField]
		private Transform pelvisTarget;

		[SerializeField]
		private Transform bodyTarget;

		[SerializeField]
		private Transform headTarget;

		[SerializeField]
		private Vector3 pelvisDownAxis = Vector3.right;

		private FullBodyBipedIK ik;

		private Quaternion rootRelativeToPelvis;

		private Vector3 pelvisToRoot;

		private float lastWeight;
	}
}
