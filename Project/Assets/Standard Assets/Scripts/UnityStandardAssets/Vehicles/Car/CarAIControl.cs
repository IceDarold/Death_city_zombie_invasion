using System;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Car
{
	[RequireComponent(typeof(CarController))]
	public class CarAIControl : MonoBehaviour
	{
		private void Awake()
		{
			this.m_CarController = base.GetComponent<CarController>();
			this.m_RandomPerlin = UnityEngine.Random.value * 100f;
			this.m_Rigidbody = base.GetComponent<Rigidbody>();
		}

		private void FixedUpdate()
		{
			if (this.m_Target == null || !this.m_Driving)
			{
				this.m_CarController.Move(0f, 0f, -1f, 1f);
			}
			else
			{
				Vector3 to = base.transform.forward;
				if (this.m_Rigidbody.velocity.magnitude > this.m_CarController.MaxSpeed * 0.1f)
				{
					to = this.m_Rigidbody.velocity;
				}
				float num = this.m_CarController.MaxSpeed;
				CarAIControl.BrakeCondition brakeCondition = this.m_BrakeCondition;
				if (brakeCondition != CarAIControl.BrakeCondition.TargetDirectionDifference)
				{
					if (brakeCondition != CarAIControl.BrakeCondition.TargetDistance)
					{
						if (brakeCondition != CarAIControl.BrakeCondition.NeverBrake)
						{
						}
					}
					else
					{
						Vector3 vector = this.m_Target.position - base.transform.position;
						float b = Mathf.InverseLerp(this.m_CautiousMaxDistance, 0f, vector.magnitude);
						float value = this.m_Rigidbody.angularVelocity.magnitude * this.m_CautiousAngularVelocityFactor;
						float t = Mathf.Max(Mathf.InverseLerp(0f, this.m_CautiousMaxAngle, value), b);
						num = Mathf.Lerp(this.m_CarController.MaxSpeed, this.m_CarController.MaxSpeed * this.m_CautiousSpeedFactor, t);
					}
				}
				else
				{
					float b2 = Vector3.Angle(this.m_Target.forward, to);
					float a = this.m_Rigidbody.angularVelocity.magnitude * this.m_CautiousAngularVelocityFactor;
					float t2 = Mathf.InverseLerp(0f, this.m_CautiousMaxAngle, Mathf.Max(a, b2));
					num = Mathf.Lerp(this.m_CarController.MaxSpeed, this.m_CarController.MaxSpeed * this.m_CautiousSpeedFactor, t2);
				}
				Vector3 vector2 = this.m_Target.position;
				if (Time.time < this.m_AvoidOtherCarTime)
				{
					num *= this.m_AvoidOtherCarSlowdown;
					vector2 += this.m_Target.right * this.m_AvoidPathOffset;
				}
				else
				{
					vector2 += this.m_Target.right * (Mathf.PerlinNoise(Time.time * this.m_LateralWanderSpeed, this.m_RandomPerlin) * 2f - 1f) * this.m_LateralWanderDistance;
				}
				float num2 = (num >= this.m_CarController.CurrentSpeed) ? this.m_AccelSensitivity : this.m_BrakeSensitivity;
				float num3 = Mathf.Clamp((num - this.m_CarController.CurrentSpeed) * num2, -1f, 1f);
				num3 *= 1f - this.m_AccelWanderAmount + Mathf.PerlinNoise(Time.time * this.m_AccelWanderSpeed, this.m_RandomPerlin) * this.m_AccelWanderAmount;
				Vector3 vector3 = base.transform.InverseTransformPoint(vector2);
				float num4 = Mathf.Atan2(vector3.x, vector3.z) * 57.29578f;
				float steering = Mathf.Clamp(num4 * this.m_SteerSensitivity, -1f, 1f) * Mathf.Sign(this.m_CarController.CurrentSpeed);
				this.m_CarController.Move(steering, num3, num3, 0f);
				if (this.m_StopWhenTargetReached && vector3.magnitude < this.m_ReachTargetThreshold)
				{
					this.m_Driving = false;
				}
			}
		}

		private void OnCollisionStay(Collision col)
		{
			if (col.rigidbody != null)
			{
				CarAIControl component = col.rigidbody.GetComponent<CarAIControl>();
				if (component != null)
				{
					this.m_AvoidOtherCarTime = Time.time + 1f;
					if (Vector3.Angle(base.transform.forward, component.transform.position - base.transform.position) < 90f)
					{
						this.m_AvoidOtherCarSlowdown = 0.5f;
					}
					else
					{
						this.m_AvoidOtherCarSlowdown = 1f;
					}
					Vector3 vector = base.transform.InverseTransformPoint(component.transform.position);
					float f = Mathf.Atan2(vector.x, vector.z);
					this.m_AvoidPathOffset = this.m_LateralWanderDistance * -Mathf.Sign(f);
				}
			}
		}

		public void SetTarget(Transform target)
		{
			this.m_Target = target;
			this.m_Driving = true;
		}

		[SerializeField]
		[Range(0f, 1f)]
		private float m_CautiousSpeedFactor = 0.05f;

		[SerializeField]
		[Range(0f, 180f)]
		private float m_CautiousMaxAngle = 50f;

		[SerializeField]
		private float m_CautiousMaxDistance = 100f;

		[SerializeField]
		private float m_CautiousAngularVelocityFactor = 30f;

		[SerializeField]
		private float m_SteerSensitivity = 0.05f;

		[SerializeField]
		private float m_AccelSensitivity = 0.04f;

		[SerializeField]
		private float m_BrakeSensitivity = 1f;

		[SerializeField]
		private float m_LateralWanderDistance = 3f;

		[SerializeField]
		private float m_LateralWanderSpeed = 0.1f;

		[SerializeField]
		[Range(0f, 1f)]
		private float m_AccelWanderAmount = 0.1f;

		[SerializeField]
		private float m_AccelWanderSpeed = 0.1f;

		[SerializeField]
		private CarAIControl.BrakeCondition m_BrakeCondition = CarAIControl.BrakeCondition.TargetDistance;

		[SerializeField]
		private bool m_Driving;

		[SerializeField]
		private Transform m_Target;

		[SerializeField]
		private bool m_StopWhenTargetReached;

		[SerializeField]
		private float m_ReachTargetThreshold = 2f;

		private float m_RandomPerlin;

		private CarController m_CarController;

		private float m_AvoidOtherCarTime;

		private float m_AvoidOtherCarSlowdown;

		private float m_AvoidPathOffset;

		private Rigidbody m_Rigidbody;

		public enum BrakeCondition
		{
			NeverBrake,
			TargetDirectionDifference,
			TargetDistance
		}
	}
}
