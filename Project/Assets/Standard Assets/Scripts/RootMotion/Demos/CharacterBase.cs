using System;
using UnityEngine;

namespace RootMotion.Demos
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	public abstract class CharacterBase : MonoBehaviour
	{
		public abstract void Move(Vector3 deltaPosition, Quaternion deltaRotation);

		protected Vector3 GetGravity()
		{
			if (this.gravityTarget != null)
			{
				return (this.gravityTarget.position - base.transform.position).normalized * Physics.gravity.magnitude;
			}
			return Physics.gravity;
		}

		protected virtual void Start()
		{
			this.capsule = (base.GetComponent<Collider>() as CapsuleCollider);
			this.r = base.GetComponent<Rigidbody>();
			this.originalHeight = this.capsule.height;
			this.originalCenter = this.capsule.center;
			this.zeroFrictionMaterial = new PhysicMaterial();
			this.zeroFrictionMaterial.dynamicFriction = 0f;
			this.zeroFrictionMaterial.staticFriction = 0f;
			this.zeroFrictionMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
			this.zeroFrictionMaterial.bounciness = 0f;
			this.zeroFrictionMaterial.bounceCombine = PhysicMaterialCombine.Minimum;
			this.highFrictionMaterial = new PhysicMaterial();
			this.r.constraints = RigidbodyConstraints.FreezeRotation;
		}

		protected virtual RaycastHit GetSpherecastHit()
		{
			Vector3 up = base.transform.up;
			Ray ray = new Ray(this.r.position + up * this.airborneThreshold, -up);
			RaycastHit result = default(RaycastHit);
			result.point = base.transform.position - base.transform.transform.up * this.airborneThreshold;
			result.normal = base.transform.up;
			Physics.SphereCast(ray, this.spherecastRadius, out result, this.airborneThreshold * 2f, this.groundLayers);
			return result;
		}

		public float GetAngleFromForward(Vector3 worldDirection)
		{
			Vector3 vector = base.transform.InverseTransformDirection(worldDirection);
			return Mathf.Atan2(vector.x, vector.z) * 57.29578f;
		}

		protected void RigidbodyRotateAround(Vector3 point, Vector3 axis, float angle)
		{
			Quaternion quaternion = Quaternion.AngleAxis(angle, axis);
			Vector3 point2 = base.transform.position - point;
			this.r.MovePosition(point + quaternion * point2);
			this.r.MoveRotation(quaternion * base.transform.rotation);
		}

		protected void ScaleCapsule(float mlp)
		{
			if (this.capsule.height != this.originalHeight * mlp)
			{
				this.capsule.height = Mathf.MoveTowards(this.capsule.height, this.originalHeight * mlp, Time.deltaTime * 4f);
				this.capsule.center = Vector3.MoveTowards(this.capsule.center, this.originalCenter * mlp, Time.deltaTime * 2f);
			}
		}

		protected void HighFriction()
		{
			this.capsule.material = this.highFrictionMaterial;
		}

		protected void ZeroFriction()
		{
			this.capsule.material = this.zeroFrictionMaterial;
		}

		protected float GetSlopeDamper(Vector3 velocity, Vector3 groundNormal)
		{
			float num = 90f - Vector3.Angle(velocity, groundNormal);
			num -= this.slopeStartAngle;
			float num2 = this.slopeEndAngle - this.slopeStartAngle;
			return 1f - Mathf.Clamp(num / num2, 0f, 1f);
		}

		[Header("Base Parameters")]
		[Tooltip("If specified, will use the direction from the character to this Transform as the gravity vector instead of Physics.gravity. Physics.gravity.magnitude will be used as the magnitude of the gravity vector.")]
		public Transform gravityTarget;

		[Tooltip("Multiplies gravity applied to the character even if 'Individual Gravity' is unchecked.")]
		[SerializeField]
		protected float gravityMultiplier = 2f;

		[SerializeField]
		protected float airborneThreshold = 0.6f;

		[SerializeField]
		private float slopeStartAngle = 50f;

		[SerializeField]
		private float slopeEndAngle = 85f;

		[SerializeField]
		private float spherecastRadius = 0.1f;

		[SerializeField]
		private LayerMask groundLayers;

		private PhysicMaterial zeroFrictionMaterial;

		private PhysicMaterial highFrictionMaterial;

		protected Rigidbody r;

		protected const float half = 0.5f;

		protected float originalHeight;

		protected Vector3 originalCenter;

		protected CapsuleCollider capsule;
	}
}
