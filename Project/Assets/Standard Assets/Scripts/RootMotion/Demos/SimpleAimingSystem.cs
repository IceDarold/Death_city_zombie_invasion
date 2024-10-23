using System;
using RootMotion.FinalIK;
using UnityEngine;

namespace RootMotion.Demos
{
	public class SimpleAimingSystem : MonoBehaviour
	{
		private void Start()
		{
			this.aim.enabled = false;
			this.lookAt.enabled = false;
		}

		private void LateUpdate()
		{
			if (this.aim.solver.target == null)
			{
				UnityEngine.Debug.LogWarning("AimIK and LookAtIK need to have their 'Target' value assigned.", base.transform);
			}
			this.Pose();
			this.aim.solver.Update();
			if (this.lookAt != null)
			{
				this.lookAt.solver.Update();
			}
		}

		private void Pose()
		{
			this.LimitAimTarget();
			Vector3 direction = this.aim.solver.target.position - this.aim.solver.bones[0].transform.position;
			Vector3 localDirection = base.transform.InverseTransformDirection(direction);
			this.aimPose = this.aimPoser.GetPose(localDirection);
			if (this.aimPose != this.lastPose)
			{
				this.aimPoser.SetPoseActive(this.aimPose);
				this.lastPose = this.aimPose;
			}
			foreach (AimPoser.Pose pose in this.aimPoser.poses)
			{
				if (pose == this.aimPose)
				{
					this.DirectCrossFade(pose.name, 1f);
				}
				else
				{
					this.DirectCrossFade(pose.name, 0f);
				}
			}
		}

		private void LimitAimTarget()
		{
			Vector3 position = this.aim.solver.bones[0].transform.position;
			Vector3 b = this.aim.solver.target.position - position;
			b = b.normalized * Mathf.Max(b.magnitude, this.minAimDistance);
			this.aim.solver.target.position = position + b;
		}

		private void DirectCrossFade(string state, float target)
		{
			float value = Mathf.MoveTowards(this.animator.GetFloat(state), target, Time.deltaTime * (1f / this.crossfadeTime));
			this.animator.SetFloat(state, value);
		}

		[Tooltip("AimPoser is a tool that returns an animation name based on direction.")]
		public AimPoser aimPoser;

		[Tooltip("Reference to the AimIK component.")]
		public AimIK aim;

		[Tooltip("Reference to the LookAt component (only used for the head in this instance).")]
		public LookAtIK lookAt;

		[Tooltip("Reference to the Animator component.")]
		public Animator animator;

		[Tooltip("Time of cross-fading from pose to pose.")]
		public float crossfadeTime = 0.2f;

		[Tooltip("Will keep the aim target at a distance.")]
		public float minAimDistance = 0.5f;

		private AimPoser.Pose aimPose;

		private AimPoser.Pose lastPose;
	}
}
