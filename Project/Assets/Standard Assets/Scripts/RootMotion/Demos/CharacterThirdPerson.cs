using System;
using UnityEngine;

namespace RootMotion.Demos
{
	public class CharacterThirdPerson : CharacterBase
	{
		public bool onGround { get; private set; }

		protected override void Start()
		{
			base.Start();
			this.animator = base.GetComponent<Animator>();
			if (this.animator == null)
			{
				this.animator = this.characterAnimation.GetComponent<Animator>();
			}
			this.wallNormal = -this.gravity.normalized;
			this.onGround = true;
			this.animState.onGround = true;
			if (this.cam != null)
			{
				this.cam.enabled = false;
			}
		}

		private void OnAnimatorMove()
		{
			this.Move(this.animator.deltaPosition, this.animator.deltaRotation);
		}

		public override void Move(Vector3 deltaPosition, Quaternion deltaRotation)
		{
			this.fixedDeltaPosition += deltaPosition;
			this.fixedDeltaRotation *= deltaRotation;
		}

		private void FixedUpdate()
		{
			this.gravity = base.GetGravity();
			this.verticalVelocity = V3Tools.ExtractVertical(this.r.velocity, this.gravity, 1f);
			this.velocityY = this.verticalVelocity.magnitude;
			if (Vector3.Dot(this.verticalVelocity, this.gravity) > 0f)
			{
				this.velocityY = -this.velocityY;
			}
			if (this.animator != null && this.animator.updateMode == AnimatorUpdateMode.AnimatePhysics)
			{
				this.smoothPhysics = false;
				this.characterAnimation.smoothFollow = false;
			}
			this.r.interpolation = ((!this.smoothPhysics) ? RigidbodyInterpolation.None : RigidbodyInterpolation.Interpolate);
			this.characterAnimation.smoothFollow = this.smoothPhysics;
			this.MoveFixed(this.fixedDeltaPosition);
			this.fixedDeltaPosition = Vector3.zero;
			base.transform.rotation *= this.fixedDeltaRotation;
			this.fixedDeltaRotation = Quaternion.identity;
			this.Rotate();
			this.GroundCheck();
			if (this.userControl.state.move == Vector3.zero && this.groundDistance < this.airborneThreshold * 0.5f)
			{
				base.HighFriction();
			}
			else
			{
				base.ZeroFriction();
			}
			if (this.gravityTarget != null)
			{
				this.r.useGravity = false;
				this.r.AddForce(this.gravity);
			}
			if (this.onGround)
			{
				this.animState.jump = this.Jump();
			}
			else
			{
				this.r.AddForce(this.gravity * this.gravityMultiplier);
			}
			base.ScaleCapsule((!this.userControl.state.crouch) ? 1f : this.crouchCapsuleScaleMlp);
			this.fixedFrame = true;
		}

		protected virtual void Update()
		{
			this.animState.onGround = this.onGround;
			this.animState.moveDirection = this.GetMoveDirection();
			this.animState.yVelocity = Mathf.Lerp(this.animState.yVelocity, this.velocityY, Time.deltaTime * 10f);
			this.animState.crouch = this.userControl.state.crouch;
			this.animState.isStrafing = (this.moveMode == CharacterThirdPerson.MoveMode.Strafe);
		}

		protected virtual void LateUpdate()
		{
			if (this.cam == null)
			{
				return;
			}
			this.cam.UpdateInput();
			if (!this.fixedFrame && this.r.interpolation == RigidbodyInterpolation.None)
			{
				return;
			}
			this.cam.UpdateTransform((this.r.interpolation != RigidbodyInterpolation.None) ? Time.deltaTime : Time.fixedDeltaTime);
			this.fixedFrame = false;
		}

		private void MoveFixed(Vector3 deltaPosition)
		{
			this.WallRun();
			Vector3 vector = deltaPosition / Time.deltaTime;
			vector += V3Tools.ExtractHorizontal(this.platformVelocity, this.gravity, 1f);
			if (this.onGround)
			{
				if (this.velocityToGroundTangentWeight > 0f)
				{
					Quaternion b = Quaternion.FromToRotation(base.transform.up, this.normal);
					vector = Quaternion.Lerp(Quaternion.identity, b, this.velocityToGroundTangentWeight) * vector;
				}
			}
			else
			{
				Vector3 b2 = V3Tools.ExtractHorizontal(this.userControl.state.move * this.airSpeed, this.gravity, 1f);
				vector = Vector3.Lerp(this.r.velocity, b2, Time.deltaTime * this.airControl);
			}
			if (this.onGround && Time.time > this.jumpEndTime)
			{
				this.r.velocity = this.r.velocity - base.transform.up * this.stickyForce * Time.deltaTime;
			}
			Vector3 vector2 = V3Tools.ExtractVertical(this.r.velocity, this.gravity, 1f);
			Vector3 a = V3Tools.ExtractHorizontal(vector, this.gravity, 1f);
			if (this.onGround && Vector3.Dot(vector2, this.gravity) < 0f)
			{
				vector2 = Vector3.ClampMagnitude(vector2, this.maxVerticalVelocityOnGround);
			}
			this.r.velocity = a + vector2;
			float b3 = this.onGround ? base.GetSlopeDamper(-deltaPosition / Time.deltaTime, this.normal) : 1f;
			this.forwardMlp = Mathf.Lerp(this.forwardMlp, b3, Time.deltaTime * 5f);
		}

		private void WallRun()
		{
			bool flag = this.CanWallRun();
			if (this.wallRunWeight > 0f && !flag)
			{
				this.wallRunEndTime = Time.time;
			}
			if (Time.time < this.wallRunEndTime + 0.5f)
			{
				flag = false;
			}
			this.wallRunWeight = Mathf.MoveTowards(this.wallRunWeight, (!flag) ? 0f : 1f, Time.deltaTime * this.wallRunWeightSpeed);
			if (this.wallRunWeight <= 0f && this.lastWallRunWeight > 0f)
			{
				Vector3 forward = V3Tools.ExtractHorizontal(base.transform.forward, this.gravity, 1f);
				base.transform.rotation = Quaternion.LookRotation(forward, -this.gravity);
				this.wallNormal = -this.gravity.normalized;
			}
			this.lastWallRunWeight = this.wallRunWeight;
			if (this.wallRunWeight <= 0f)
			{
				return;
			}
			if (this.onGround && this.velocityY < 0f)
			{
				this.r.velocity = V3Tools.ExtractHorizontal(this.r.velocity, this.gravity, 1f);
			}
			Vector3 vector = V3Tools.ExtractHorizontal(base.transform.forward, this.gravity, 1f);
			RaycastHit raycastHit = default(RaycastHit);
			raycastHit.normal = -this.gravity.normalized;
			Physics.Raycast((!this.onGround) ? this.capsule.bounds.center : base.transform.position, vector, out raycastHit, 3f, this.wallRunLayers);
			this.wallNormal = Vector3.Lerp(this.wallNormal, raycastHit.normal, Time.deltaTime * this.wallRunRotationSpeed);
			this.wallNormal = Vector3.RotateTowards(-this.gravity.normalized, this.wallNormal, this.wallRunMaxRotationAngle * 0.0174532924f, 0f);
			Vector3 forward2 = base.transform.forward;
			Vector3 vector2 = this.wallNormal;
			Vector3.OrthoNormalize(ref vector2, ref forward2);
			base.transform.rotation = Quaternion.Slerp(Quaternion.LookRotation(vector, -this.gravity), Quaternion.LookRotation(forward2, this.wallNormal), this.wallRunWeight);
		}

		private bool CanWallRun()
		{
			return Time.time >= this.jumpEndTime - 0.1f && Time.time <= this.jumpEndTime - 0.1f + this.wallRunMaxLength && this.velocityY >= this.wallRunMinVelocityY && this.userControl.state.move.magnitude >= this.wallRunMinMoveMag;
		}

		private Vector3 GetMoveDirection()
		{
			CharacterThirdPerson.MoveMode moveMode = this.moveMode;
			if (moveMode == CharacterThirdPerson.MoveMode.Directional)
			{
				this.moveDirection = Vector3.SmoothDamp(this.moveDirection, new Vector3(0f, 0f, this.userControl.state.move.magnitude), ref this.moveDirectionVelocity, this.smoothAccelerationTime);
				this.moveDirection = Vector3.MoveTowards(this.moveDirection, new Vector3(0f, 0f, this.userControl.state.move.magnitude), Time.deltaTime * this.linearAccelerationSpeed);
				return this.moveDirection * this.forwardMlp;
			}
			if (moveMode != CharacterThirdPerson.MoveMode.Strafe)
			{
				return Vector3.zero;
			}
			this.moveDirection = Vector3.SmoothDamp(this.moveDirection, this.userControl.state.move, ref this.moveDirectionVelocity, this.smoothAccelerationTime);
			this.moveDirection = Vector3.MoveTowards(this.moveDirection, this.userControl.state.move, Time.deltaTime * this.linearAccelerationSpeed);
			return base.transform.InverseTransformDirection(this.moveDirection);
		}

		protected virtual void Rotate()
		{
			if (this.gravityTarget != null)
			{
				base.transform.rotation = Quaternion.FromToRotation(base.transform.up, base.transform.position - this.gravityTarget.position) * base.transform.rotation;
			}
			if (this.platformAngularVelocity != Vector3.zero)
			{
				base.transform.rotation = Quaternion.Euler(this.platformAngularVelocity) * base.transform.rotation;
			}
			float num = base.GetAngleFromForward(this.GetForwardDirection());
			if (this.userControl.state.move == Vector3.zero)
			{
				num *= (1.01f - Mathf.Abs(num) / 180f) * this.stationaryTurnSpeedMlp;
			}
			base.RigidbodyRotateAround(this.characterAnimation.GetPivotPoint(), base.transform.up, num * Time.deltaTime * this.turnSpeed);
		}

		private Vector3 GetForwardDirection()
		{
			bool flag = this.userControl.state.move != Vector3.zero;
			CharacterThirdPerson.MoveMode moveMode = this.moveMode;
			if (moveMode != CharacterThirdPerson.MoveMode.Directional)
			{
				if (moveMode != CharacterThirdPerson.MoveMode.Strafe)
				{
					return Vector3.zero;
				}
				if (flag)
				{
					return this.userControl.state.lookPos - this.r.position;
				}
				return (!this.lookInCameraDirection) ? base.transform.forward : (this.userControl.state.lookPos - this.r.position);
			}
			else
			{
				if (flag)
				{
					return this.userControl.state.move;
				}
				return (!this.lookInCameraDirection) ? base.transform.forward : (this.userControl.state.lookPos - this.r.position);
			}
		}

		protected virtual bool Jump()
		{
			if (!this.userControl.state.jump)
			{
				return false;
			}
			if (this.userControl.state.crouch)
			{
				return false;
			}
			if (!this.characterAnimation.animationGrounded)
			{
				return false;
			}
			if (Time.time < this.lastAirTime + this.jumpRepeatDelayTime)
			{
				return false;
			}
			this.onGround = false;
			this.jumpEndTime = Time.time + 0.1f;
			Vector3 velocity = this.userControl.state.move * this.airSpeed;
			this.r.velocity = velocity;
			this.r.velocity += base.transform.up * this.jumpPower;
			return true;
		}

		private void GroundCheck()
		{
			Vector3 b = Vector3.zero;
			this.platformAngularVelocity = Vector3.zero;
			float num = 0f;
			this.hit = this.GetSpherecastHit();
			this.normal = base.transform.up;
			this.groundDistance = Vector3.Project(this.r.position - this.hit.point, base.transform.up).magnitude;
			bool flag = Time.time > this.jumpEndTime && this.velocityY < this.jumpPower * 0.5f;
			if (flag)
			{
				bool onGround = this.onGround;
				this.onGround = false;
				float num2 = onGround ? this.airborneThreshold : (this.airborneThreshold * 0.5f);
				float magnitude = V3Tools.ExtractHorizontal(this.r.velocity, this.gravity, 1f).magnitude;
				if (this.groundDistance < num2)
				{
					num = this.groundStickyEffect * magnitude * num2;
					if (this.hit.rigidbody != null)
					{
						b = this.hit.rigidbody.GetPointVelocity(this.hit.point);
						this.platformAngularVelocity = Vector3.Project(this.hit.rigidbody.angularVelocity, base.transform.up);
					}
					this.onGround = true;
				}
			}
			this.platformVelocity = Vector3.Lerp(this.platformVelocity, b, Time.deltaTime * this.platformFriction);
			this.stickyForce = num;
			if (!this.onGround)
			{
				this.lastAirTime = Time.time;
			}
		}

		[Header("References")]
		public CharacterAnimationBase characterAnimation;

		public UserControlThirdPerson userControl;

		public CameraController cam;

		[Header("Movement")]
		public CharacterThirdPerson.MoveMode moveMode;

		public bool smoothPhysics = true;

		public float smoothAccelerationTime = 0.2f;

		public float linearAccelerationSpeed = 3f;

		public float platformFriction = 7f;

		public float groundStickyEffect = 4f;

		public float maxVerticalVelocityOnGround = 3f;

		public float velocityToGroundTangentWeight;

		[Header("Rotation")]
		public bool lookInCameraDirection;

		public float turnSpeed = 5f;

		public float stationaryTurnSpeedMlp = 1f;

		[Header("Jumping and Falling")]
		public float airSpeed = 6f;

		public float airControl = 2f;

		public float jumpPower = 12f;

		public float jumpRepeatDelayTime;

		[Header("Wall Running")]
		[SerializeField]
		private LayerMask wallRunLayers;

		public float wallRunMaxLength = 1f;

		public float wallRunMinMoveMag = 0.6f;

		public float wallRunMinVelocityY = -1f;

		public float wallRunRotationSpeed = 1.5f;

		public float wallRunMaxRotationAngle = 70f;

		public float wallRunWeightSpeed = 5f;

		[Header("Crouching")]
		public float crouchCapsuleScaleMlp = 0.6f;

		public CharacterThirdPerson.AnimState animState = default(CharacterThirdPerson.AnimState);

		protected Vector3 moveDirection;

		private Animator animator;

		private Vector3 normal;

		private Vector3 platformVelocity;

		private Vector3 platformAngularVelocity;

		private RaycastHit hit;

		private float jumpLeg;

		private float jumpEndTime;

		private float forwardMlp;

		private float groundDistance;

		private float lastAirTime;

		private float stickyForce;

		private Vector3 wallNormal = Vector3.up;

		private Vector3 moveDirectionVelocity;

		private float wallRunWeight;

		private float lastWallRunWeight;

		private Vector3 fixedDeltaPosition;

		private Quaternion fixedDeltaRotation;

		private bool fixedFrame;

		private float wallRunEndTime;

		private Vector3 gravity;

		private Vector3 verticalVelocity;

		private float velocityY;

		[Serializable]
		public enum MoveMode
		{
			Directional,
			Strafe
		}

		public struct AnimState
		{
			public Vector3 moveDirection;

			public bool jump;

			public bool crouch;

			public bool onGround;

			public bool isStrafing;

			public float yVelocity;
		}
	}
}
