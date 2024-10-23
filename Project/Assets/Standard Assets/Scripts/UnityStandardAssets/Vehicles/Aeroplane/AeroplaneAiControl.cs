using System;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Aeroplane
{
	[RequireComponent(typeof(AeroplaneController))]
	public class AeroplaneAiControl : MonoBehaviour
	{
		private void Awake()
		{
			this.m_AeroplaneController = base.GetComponent<AeroplaneController>();
			this.m_RandomPerlin = UnityEngine.Random.Range(0f, 100f);
		}

		public void Reset()
		{
			this.m_TakenOff = false;
		}

		private void FixedUpdate()
		{
			if (this.m_Target != null)
			{
				Vector3 position = this.m_Target.position + base.transform.right * (Mathf.PerlinNoise(Time.time * this.m_LateralWanderSpeed, this.m_RandomPerlin) * 2f - 1f) * this.m_LateralWanderDistance;
				Vector3 vector = base.transform.InverseTransformPoint(position);
				float num = Mathf.Atan2(vector.x, vector.z);
				float num2 = -Mathf.Atan2(vector.y, vector.z);
				num2 = Mathf.Clamp(num2, -this.m_MaxClimbAngle * 0.0174532924f, this.m_MaxClimbAngle * 0.0174532924f);
				float num3 = num2 - this.m_AeroplaneController.PitchAngle;
				float num4 = num3 * this.m_PitchSensitivity;
				float num5 = Mathf.Clamp(num, -this.m_MaxRollAngle * 0.0174532924f, this.m_MaxRollAngle * 0.0174532924f);
				float num6 = 0f;
				float num7 = 0f;
				if (!this.m_TakenOff)
				{
					if (this.m_AeroplaneController.Altitude > this.m_TakeoffHeight)
					{
						this.m_TakenOff = true;
					}
				}
				else
				{
					num6 = num;
					num7 = -(this.m_AeroplaneController.RollAngle - num5) * this.m_RollSensitivity;
				}
				float num8 = 1f + this.m_AeroplaneController.ForwardSpeed * this.m_SpeedEffect;
				num7 *= num8;
				num4 *= num8;
				num6 *= num8;
				this.m_AeroplaneController.Move(num7, num4, num6, 0.5f, false);
			}
			else
			{
				this.m_AeroplaneController.Move(0f, 0f, 0f, 0f, false);
			}
		}

		public void SetTarget(Transform target)
		{
			this.m_Target = target;
		}

		[SerializeField]
		private float m_RollSensitivity = 0.2f;

		[SerializeField]
		private float m_PitchSensitivity = 0.5f;

		[SerializeField]
		private float m_LateralWanderDistance = 5f;

		[SerializeField]
		private float m_LateralWanderSpeed = 0.11f;

		[SerializeField]
		private float m_MaxClimbAngle = 45f;

		[SerializeField]
		private float m_MaxRollAngle = 45f;

		[SerializeField]
		private float m_SpeedEffect = 0.01f;

		[SerializeField]
		private float m_TakeoffHeight = 20f;

		[SerializeField]
		private Transform m_Target;

		private AeroplaneController m_AeroplaneController;

		private float m_RandomPerlin;

		private bool m_TakenOff;
	}
}
