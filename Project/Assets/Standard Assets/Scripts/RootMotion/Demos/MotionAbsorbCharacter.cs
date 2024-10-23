using System;
using UnityEngine;

namespace RootMotion.Demos
{
	public class MotionAbsorbCharacter : MonoBehaviour
	{
		private void Start()
		{
			this.cubeDefaultPosition = this.cube.position;
			this.cubeRigidbody = this.cube.GetComponent<Rigidbody>();
		}

		private void Update()
		{
			this.info = this.animator.GetCurrentAnimatorStateInfo(0);
			this.motionAbsorb.weight = this.motionAbsorbWeight.Evaluate(this.info.normalizedTime - (float)((int)this.info.normalizedTime));
		}

		private void SwingStart()
		{
			this.cubeRigidbody.MovePosition(this.cubeDefaultPosition + UnityEngine.Random.insideUnitSphere * this.cubeRandomPosition);
			this.cubeRigidbody.MoveRotation(Quaternion.identity);
			this.cubeRigidbody.velocity = Vector3.zero;
			this.cubeRigidbody.angularVelocity = Vector3.zero;
		}

		public Animator animator;

		public MotionAbsorb motionAbsorb;

		public Transform cube;

		public float cubeRandomPosition = 0.1f;

		public AnimationCurve motionAbsorbWeight;

		private Vector3 cubeDefaultPosition;

		private AnimatorStateInfo info;

		private Rigidbody cubeRigidbody;
	}
}
