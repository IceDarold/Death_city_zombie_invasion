using System;
using RootMotion.FinalIK;
using UnityEngine;

namespace RootMotion.Demos
{
	[RequireComponent(typeof(FullBodyBipedIK))]
	public class CharacterAnimationThirdPersonIK : CharacterAnimationThirdPerson
	{
		protected override void Start()
		{
			base.Start();
			this.ik = base.GetComponent<FullBodyBipedIK>();
		}

		protected override void LateUpdate()
		{
			base.LateUpdate();
			if (Vector3.Angle(base.transform.up, Vector3.up) <= 0.01f)
			{
				return;
			}
			Quaternion rotation = Quaternion.FromToRotation(base.transform.up, Vector3.up);
			this.RotateEffector(this.ik.solver.bodyEffector, rotation, 0.1f);
			this.RotateEffector(this.ik.solver.leftShoulderEffector, rotation, 0.2f);
			this.RotateEffector(this.ik.solver.rightShoulderEffector, rotation, 0.2f);
			this.RotateEffector(this.ik.solver.leftHandEffector, rotation, 0.1f);
			this.RotateEffector(this.ik.solver.rightHandEffector, rotation, 0.1f);
		}

		private void RotateEffector(IKEffector effector, Quaternion rotation, float mlp)
		{
			Vector3 vector = effector.bone.position - base.transform.position;
			Vector3 a = rotation * vector;
			Vector3 a2 = a - vector;
			effector.positionOffset += a2 * mlp;
		}

		private FullBodyBipedIK ik;
	}
}
