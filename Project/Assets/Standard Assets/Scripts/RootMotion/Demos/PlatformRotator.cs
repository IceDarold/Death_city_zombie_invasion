using System;
using System.Collections;
using UnityEngine;

namespace RootMotion.Demos
{
	public class PlatformRotator : MonoBehaviour
	{
		private void Start()
		{
			this.defaultRotation = base.transform.rotation;
			this.targetPosition = base.transform.position + this.movePosition;
			this.r = base.GetComponent<Rigidbody>();
			base.StartCoroutine(this.SwitchRotation());
		}

		private void FixedUpdate()
		{
			this.r.MovePosition(Vector3.SmoothDamp(this.r.position, this.targetPosition, ref this.velocity, 1f, this.moveSpeed));
			if (Vector3.Distance(base.GetComponent<Rigidbody>().position, this.targetPosition) < 0.1f)
			{
				this.movePosition = -this.movePosition;
				this.targetPosition += this.movePosition;
			}
			this.r.MoveRotation(Quaternion.RotateTowards(this.r.rotation, this.targetRotation, this.rotationSpeed * Time.deltaTime));
		}

		private IEnumerator SwitchRotation()
		{
			for (;;)
			{
				float angle = UnityEngine.Random.Range(-this.maxAngle, this.maxAngle);
				Vector3 axis = UnityEngine.Random.onUnitSphere;
				this.targetRotation = Quaternion.AngleAxis(angle, axis) * this.defaultRotation;
				yield return new WaitForSeconds(this.switchRotationTime + UnityEngine.Random.value * this.random);
			}
			yield break;
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (collision.gameObject.layer == this.characterLayer)
			{
				CharacterThirdPerson component = collision.gameObject.GetComponent<CharacterThirdPerson>();
				if (component == null)
				{
					return;
				}
				if (component.smoothPhysics)
				{
					component.smoothPhysics = false;
				}
			}
		}

		private void OnCollisionExit(Collision collision)
		{
			if (collision.gameObject.layer == this.characterLayer)
			{
				CharacterThirdPerson component = collision.gameObject.GetComponent<CharacterThirdPerson>();
				if (component == null)
				{
					return;
				}
				component.smoothPhysics = true;
			}
		}

		public float maxAngle = 70f;

		public float switchRotationTime = 0.5f;

		public float random = 0.5f;

		public float rotationSpeed = 50f;

		public Vector3 movePosition;

		public float moveSpeed = 5f;

		public int characterLayer;

		private Quaternion defaultRotation;

		private Quaternion targetRotation;

		private Vector3 targetPosition;

		private Vector3 velocity;

		private Rigidbody r;
	}
}
