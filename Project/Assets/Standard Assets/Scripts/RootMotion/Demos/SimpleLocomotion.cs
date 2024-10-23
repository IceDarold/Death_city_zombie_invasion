using System;
using UnityEngine;

namespace RootMotion.Demos
{
	[RequireComponent(typeof(Animator))]
	public class SimpleLocomotion : MonoBehaviour
	{
		public bool isGrounded { get; private set; }

		private void Start()
		{
			this.animator = base.GetComponent<Animator>();
			this.characterController = base.GetComponent<CharacterController>();
			this.cameraController.enabled = false;
		}

		private void Update()
		{
			this.isGrounded = (base.transform.position.y < 0.1f);
			this.Rotate();
			this.Move();
		}

		private void LateUpdate()
		{
			this.cameraController.UpdateInput();
			this.cameraController.UpdateTransform();
		}

		private void Rotate()
		{
			if (!this.isGrounded)
			{
				return;
			}
			Vector3 inputVector = this.GetInputVector();
			if (inputVector == Vector3.zero)
			{
				return;
			}
			Vector3 vector = base.transform.forward;
			SimpleLocomotion.RotationMode rotationMode = this.rotationMode;
			if (rotationMode != SimpleLocomotion.RotationMode.Smooth)
			{
				if (rotationMode == SimpleLocomotion.RotationMode.Linear)
				{
					Vector3 inputVectorRaw = this.GetInputVectorRaw();
					if (inputVectorRaw != Vector3.zero)
					{
						this.linearTargetDirection = this.cameraController.transform.rotation * inputVectorRaw;
					}
					vector = Vector3.RotateTowards(vector, this.linearTargetDirection, Time.deltaTime * (1f / this.turnTime), 1f);
					vector.y = 0f;
					base.transform.rotation = Quaternion.LookRotation(vector);
				}
			}
			else
			{
				Vector3 vector2 = this.cameraController.transform.rotation * inputVector;
				float current = Mathf.Atan2(vector.x, vector.z) * 57.29578f;
				float target = Mathf.Atan2(vector2.x, vector2.z) * 57.29578f;
				float angle = Mathf.SmoothDampAngle(current, target, ref this.angleVel, this.turnTime);
				base.transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
			}
		}

		private void Move()
		{
			float target = (!this.walkByDefault) ? ((!Input.GetKey(KeyCode.LeftShift)) ? 1f : 0.5f) : ((!Input.GetKey(KeyCode.LeftShift)) ? 0.5f : 1f);
			this.speed = Mathf.SmoothDamp(this.speed, target, ref this.speedVel, this.accelerationTime);
			float num = this.GetInputVector().magnitude * this.speed;
			this.animator.SetFloat("Speed", num);
			bool flag = !this.animator.hasRootMotion && this.isGrounded;
			if (flag)
			{
				Vector3 a = base.transform.forward * num * this.moveSpeed;
				if (this.characterController != null)
				{
					this.characterController.SimpleMove(a);
				}
				else
				{
					base.transform.position += a * Time.deltaTime;
				}
			}
		}

		private Vector3 GetInputVector()
		{
			Vector3 result = new Vector3(UnityEngine.Input.GetAxis("Horizontal"), 0f, UnityEngine.Input.GetAxis("Vertical"));
			result.z += Mathf.Abs(result.x) * 0.05f;
			result.x -= Mathf.Abs(result.z) * 0.05f;
			return result;
		}

		private Vector3 GetInputVectorRaw()
		{
			return new Vector3(UnityEngine.Input.GetAxisRaw("Horizontal"), 0f, UnityEngine.Input.GetAxisRaw("Vertical"));
		}

		[Tooltip("The component that updates the camera.")]
		[SerializeField]
		private CameraController cameraController;

		[Tooltip("Acceleration of movement.")]
		[SerializeField]
		private float accelerationTime = 0.2f;

		[Tooltip("Turning speed.")]
		[SerializeField]
		private float turnTime = 0.2f;

		[Tooltip("If true, will run on left shift, if not will walk on left shift.")]
		[SerializeField]
		private bool walkByDefault = true;

		[Tooltip("Smooth or linear rotation.")]
		[SerializeField]
		private SimpleLocomotion.RotationMode rotationMode;

		[Tooltip("Procedural motion speed (if not using root motion).")]
		[SerializeField]
		private float moveSpeed = 3f;

		private Animator animator;

		private float speed;

		private float angleVel;

		private float speedVel;

		private Vector3 linearTargetDirection;

		private CharacterController characterController;

		[Serializable]
		public enum RotationMode
		{
			Smooth,
			Linear
		}
	}
}
