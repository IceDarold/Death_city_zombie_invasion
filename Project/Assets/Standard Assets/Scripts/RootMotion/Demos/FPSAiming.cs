using System;
using RootMotion.FinalIK;
using UnityEngine;

namespace RootMotion.Demos
{
	public class FPSAiming : MonoBehaviour
	{
		private void Start()
		{
			this.gunTargetDefaultLocalPosition = this.gunTarget.localPosition;
			this.gunTargetDefaultLocalRotation = this.gunTarget.localRotation;
			this.camDefaultLocalPosition = this.cam.transform.localPosition;
			this.cam.enabled = false;
			this.gunAim.enabled = false;
			this.ik.enabled = false;
			if (this.recoil != null && this.ik.solver.iterations == 0)
			{
				UnityEngine.Debug.LogWarning("FPSAiming with Recoil needs FBBIK solver iteration count to be at least 1 to maintain accuracy.");
			}
		}

		private void FixedUpdate()
		{
			this.updateFrame = true;
		}

		private void LateUpdate()
		{
			if (!this.animatePhysics)
			{
				this.updateFrame = true;
			}
			if (!this.updateFrame)
			{
				return;
			}
			this.updateFrame = false;
			this.cam.transform.localPosition = this.camDefaultLocalPosition;
			this.camRelativeToGunTarget = this.gunTarget.InverseTransformPoint(this.cam.transform.position);
			this.cam.LateUpdate();
			this.RotateCharacter();
			this.Aiming();
			this.LookDownTheSight();
		}

		private void Aiming()
		{
			if (this.aimWeight <= 0f)
			{
				return;
			}
			Quaternion rotation = this.cam.transform.rotation;
			this.gunAim.solver.IKPosition = this.cam.transform.position + this.cam.transform.forward * 10f;
			this.gunAim.solver.IKPositionWeight = this.aimWeight;
			this.gunAim.solver.Update();
			this.cam.transform.rotation = rotation;
		}

		private void LookDownTheSight()
		{
			float t = this.aimWeight * this.sightWeight;
			this.gunTarget.position = Vector3.Lerp(this.gun.position, this.gunTarget.parent.TransformPoint(this.gunTargetDefaultLocalPosition), t);
			this.gunTarget.rotation = Quaternion.Lerp(this.gun.rotation, this.gunTarget.parent.rotation * this.gunTargetDefaultLocalRotation, t);
			Vector3 position = this.gun.InverseTransformPoint(this.ik.solver.leftHandEffector.bone.position);
			Vector3 position2 = this.gun.InverseTransformPoint(this.ik.solver.rightHandEffector.bone.position);
			Quaternion rhs = Quaternion.Inverse(this.gun.rotation) * this.ik.solver.leftHandEffector.bone.rotation;
			Quaternion rhs2 = Quaternion.Inverse(this.gun.rotation) * this.ik.solver.rightHandEffector.bone.rotation;
			float d = 1f;
			this.ik.solver.leftHandEffector.positionOffset += (this.gunTarget.TransformPoint(position) - (this.ik.solver.leftHandEffector.bone.position + this.ik.solver.leftHandEffector.positionOffset)) * d;
			this.ik.solver.rightHandEffector.positionOffset += (this.gunTarget.TransformPoint(position2) - (this.ik.solver.rightHandEffector.bone.position + this.ik.solver.rightHandEffector.positionOffset)) * d;
			this.ik.solver.headMapping.maintainRotationWeight = 1f;
			if (this.recoil != null)
			{
				this.recoil.SetHandRotations(this.gunTarget.rotation * rhs, this.gunTarget.rotation * rhs2);
			}
			this.ik.solver.Update();
			if (this.recoil != null)
			{
				this.ik.references.leftHand.rotation = this.recoil.rotationOffset * (this.gunTarget.rotation * rhs);
				this.ik.references.rightHand.rotation = this.recoil.rotationOffset * (this.gunTarget.rotation * rhs2);
			}
			else
			{
				this.ik.references.leftHand.rotation = this.gunTarget.rotation * rhs;
				this.ik.references.rightHand.rotation = this.gunTarget.rotation * rhs2;
			}
			this.cam.transform.position = Vector3.Lerp(this.cam.transform.position, Vector3.Lerp(this.gunTarget.TransformPoint(this.camRelativeToGunTarget), this.gun.transform.TransformPoint(this.camRelativeToGunTarget), this.cameraRecoilWeight), t);
		}

		private void RotateCharacter()
		{
			if (this.maxAngle >= 180f)
			{
				return;
			}
			if (this.maxAngle <= 0f)
			{
				base.transform.rotation = Quaternion.LookRotation(new Vector3(this.cam.transform.forward.x, 0f, this.cam.transform.forward.z));
				return;
			}
			Vector3 vector = base.transform.InverseTransformDirection(this.cam.transform.forward);
			float num = Mathf.Atan2(vector.x, vector.z) * 57.29578f;
			if (Mathf.Abs(num) > Mathf.Abs(this.maxAngle))
			{
				float angle = num - this.maxAngle;
				if (num < 0f)
				{
					angle = num + this.maxAngle;
				}
				base.transform.rotation = Quaternion.AngleAxis(angle, base.transform.up) * base.transform.rotation;
			}
		}

		[Range(0f, 1f)]
		public float aimWeight = 1f;

		[Range(0f, 1f)]
		public float sightWeight = 1f;

		[Range(0f, 180f)]
		public float maxAngle = 80f;

		[SerializeField]
		private bool animatePhysics;

		[SerializeField]
		private Transform gun;

		[SerializeField]
		private Transform gunTarget;

		[SerializeField]
		private FullBodyBipedIK ik;

		[SerializeField]
		private AimIK gunAim;

		[SerializeField]
		private CameraControllerFPS cam;

		[SerializeField]
		private Recoil recoil;

		[SerializeField]
		[Range(0f, 1f)]
		private float cameraRecoilWeight = 0.5f;

		private Vector3 gunTargetDefaultLocalPosition;

		private Quaternion gunTargetDefaultLocalRotation;

		private Vector3 camDefaultLocalPosition;

		private Vector3 camRelativeToGunTarget;

		private bool updateFrame;
	}
}
