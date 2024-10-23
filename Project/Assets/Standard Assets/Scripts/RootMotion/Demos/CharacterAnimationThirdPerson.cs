using System;
using UnityEngine;

namespace RootMotion.Demos
{
	[RequireComponent(typeof(Animator))]
	public class CharacterAnimationThirdPerson : CharacterAnimationBase
	{
		protected override void Start()
		{
			base.Start();
			this.animator = base.GetComponent<Animator>();
			this.lastForward = base.transform.forward;
		}

		public override Vector3 GetPivotPoint()
		{
			return this.animator.pivotPosition;
		}

		public override bool animationGrounded
		{
			get
			{
				return this.animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded Directional") || this.animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded Strafe");
			}
		}

		protected virtual void Update()
		{
			if (Time.deltaTime == 0f)
			{
				return;
			}
			if (this.characterController.animState.jump)
			{
				float num = Mathf.Repeat(this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime + this.runCycleLegOffset, 1f);
				float value = (float)((num >= 0f) ? -1 : 1) * this.characterController.animState.moveDirection.z;
				this.animator.SetFloat("JumpLeg", value);
			}
			float num2 = -base.GetAngleFromForward(this.lastForward);
			this.lastForward = base.transform.forward;
			num2 *= this.turnSensitivity * 0.01f;
			num2 = Mathf.Clamp(num2 / Time.deltaTime, -1f, 1f);
			this.animator.SetFloat("Turn", Mathf.Lerp(this.animator.GetFloat("Turn"), num2, Time.deltaTime * this.turnSpeed));
			this.animator.SetFloat("Forward", this.characterController.animState.moveDirection.z);
			this.animator.SetFloat("Right", this.characterController.animState.moveDirection.x);
			this.animator.SetBool("Crouch", this.characterController.animState.crouch);
			this.animator.SetBool("OnGround", this.characterController.animState.onGround);
			this.animator.SetBool("IsStrafing", this.characterController.animState.isStrafing);
			if (!this.characterController.animState.onGround)
			{
				this.animator.SetFloat("Jump", this.characterController.animState.yVelocity);
			}
			if (this.characterController.animState.onGround && this.characterController.animState.moveDirection.z > 0f)
			{
				this.animator.speed = this.animSpeedMultiplier;
			}
			else
			{
				this.animator.speed = 1f;
			}
		}

		private void OnAnimatorMove()
		{
			this.characterController.Move(this.animator.deltaPosition, this.animator.deltaRotation);
		}

		public CharacterThirdPerson characterController;

		[SerializeField]
		private float turnSensitivity = 0.2f;

		[SerializeField]
		private float turnSpeed = 5f;

		[SerializeField]
		private float runCycleLegOffset = 0.2f;

		[Range(0.1f, 3f)]
		[SerializeField]
		private float animSpeedMultiplier = 1f;

		protected Animator animator;

		private Vector3 lastForward;

		private const string groundedDirectional = "Grounded Directional";

		private const string groundedStrafe = "Grounded Strafe";
	}
}
