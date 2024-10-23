using System;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Aeroplane
{
	[RequireComponent(typeof(Rigidbody))]
	public class AeroplaneController : MonoBehaviour
	{
		public float Altitude { get; private set; }

		public float Throttle { get; private set; }

		public bool AirBrakes { get; private set; }

		public float ForwardSpeed { get; private set; }

		public float EnginePower { get; private set; }

		public float MaxEnginePower
		{
			get
			{
				return this.m_MaxEnginePower;
			}
		}

		public float RollAngle { get; private set; }

		public float PitchAngle { get; private set; }

		public float RollInput { get; private set; }

		public float PitchInput { get; private set; }

		public float YawInput { get; private set; }

		public float ThrottleInput { get; private set; }

		private void Start()
		{
			this.m_Rigidbody = base.GetComponent<Rigidbody>();
			this.m_OriginalDrag = this.m_Rigidbody.drag;
			this.m_OriginalAngularDrag = this.m_Rigidbody.angularDrag;
			for (int i = 0; i < base.transform.childCount; i++)
			{
				foreach (WheelCollider wheelCollider in base.transform.GetChild(i).GetComponentsInChildren<WheelCollider>())
				{
					wheelCollider.motorTorque = 0.18f;
				}
			}
		}

		public void Move(float rollInput, float pitchInput, float yawInput, float throttleInput, bool airBrakes)
		{
			this.RollInput = rollInput;
			this.PitchInput = pitchInput;
			this.YawInput = yawInput;
			this.ThrottleInput = throttleInput;
			this.AirBrakes = airBrakes;
			this.ClampInputs();
			this.CalculateRollAndPitchAngles();
			this.AutoLevel();
			this.CalculateForwardSpeed();
			this.ControlThrottle();
			this.CalculateDrag();
			this.CaluclateAerodynamicEffect();
			this.CalculateLinearForces();
			this.CalculateTorque();
			this.CalculateAltitude();
		}

		private void ClampInputs()
		{
			this.RollInput = Mathf.Clamp(this.RollInput, -1f, 1f);
			this.PitchInput = Mathf.Clamp(this.PitchInput, -1f, 1f);
			this.YawInput = Mathf.Clamp(this.YawInput, -1f, 1f);
			this.ThrottleInput = Mathf.Clamp(this.ThrottleInput, -1f, 1f);
		}

		private void CalculateRollAndPitchAngles()
		{
			Vector3 forward = base.transform.forward;
			forward.y = 0f;
			if (forward.sqrMagnitude > 0f)
			{
				forward.Normalize();
				Vector3 vector = base.transform.InverseTransformDirection(forward);
				this.PitchAngle = Mathf.Atan2(vector.y, vector.z);
				Vector3 direction = Vector3.Cross(Vector3.up, forward);
				Vector3 vector2 = base.transform.InverseTransformDirection(direction);
				this.RollAngle = Mathf.Atan2(vector2.y, vector2.x);
			}
		}

		private void AutoLevel()
		{
			this.m_BankedTurnAmount = Mathf.Sin(this.RollAngle);
			if (this.RollInput == 0f)
			{
				this.RollInput = -this.RollAngle * this.m_AutoRollLevel;
			}
			if (this.PitchInput == 0f)
			{
				this.PitchInput = -this.PitchAngle * this.m_AutoPitchLevel;
				this.PitchInput -= Mathf.Abs(this.m_BankedTurnAmount * this.m_BankedTurnAmount * this.m_AutoTurnPitch);
			}
		}

		private void CalculateForwardSpeed()
		{
			this.ForwardSpeed = Mathf.Max(0f, base.transform.InverseTransformDirection(this.m_Rigidbody.velocity).z);
		}

		private void ControlThrottle()
		{
			if (this.m_Immobilized)
			{
				this.ThrottleInput = -0.5f;
			}
			this.Throttle = Mathf.Clamp01(this.Throttle + this.ThrottleInput * Time.deltaTime * this.m_ThrottleChangeSpeed);
			this.EnginePower = this.Throttle * this.m_MaxEnginePower;
		}

		private void CalculateDrag()
		{
			float num = this.m_Rigidbody.velocity.magnitude * this.m_DragIncreaseFactor;
			this.m_Rigidbody.drag = ((!this.AirBrakes) ? (this.m_OriginalDrag + num) : ((this.m_OriginalDrag + num) * this.m_AirBrakesEffect));
			this.m_Rigidbody.angularDrag = this.m_OriginalAngularDrag * this.ForwardSpeed;
		}

		private void CaluclateAerodynamicEffect()
		{
			if (this.m_Rigidbody.velocity.magnitude > 0f)
			{
				this.m_AeroFactor = Vector3.Dot(base.transform.forward, this.m_Rigidbody.velocity.normalized);
				this.m_AeroFactor *= this.m_AeroFactor;
				Vector3 velocity = Vector3.Lerp(this.m_Rigidbody.velocity, base.transform.forward * this.ForwardSpeed, this.m_AeroFactor * this.ForwardSpeed * this.m_AerodynamicEffect * Time.deltaTime);
				this.m_Rigidbody.velocity = velocity;
				this.m_Rigidbody.rotation = Quaternion.Slerp(this.m_Rigidbody.rotation, Quaternion.LookRotation(this.m_Rigidbody.velocity, base.transform.up), this.m_AerodynamicEffect * Time.deltaTime);
			}
		}

		private void CalculateLinearForces()
		{
			Vector3 vector = Vector3.zero;
			vector += this.EnginePower * base.transform.forward;
			Vector3 normalized = Vector3.Cross(this.m_Rigidbody.velocity, base.transform.right).normalized;
			float num = Mathf.InverseLerp(this.m_ZeroLiftSpeed, 0f, this.ForwardSpeed);
			float d = this.ForwardSpeed * this.ForwardSpeed * this.m_Lift * num * this.m_AeroFactor;
			vector += d * normalized;
			this.m_Rigidbody.AddForce(vector);
		}

		private void CalculateTorque()
		{
			Vector3 a = Vector3.zero;
			a += this.PitchInput * this.m_PitchEffect * base.transform.right;
			a += this.YawInput * this.m_YawEffect * base.transform.up;
			a += -this.RollInput * this.m_RollEffect * base.transform.forward;
			a += this.m_BankedTurnAmount * this.m_BankedTurnEffect * base.transform.up;
			this.m_Rigidbody.AddTorque(a * this.ForwardSpeed * this.m_AeroFactor);
		}

		private void CalculateAltitude()
		{
			Ray ray = new Ray(base.transform.position - Vector3.up * 10f, -Vector3.up);
			RaycastHit raycastHit;
			this.Altitude = ((!Physics.Raycast(ray, out raycastHit)) ? base.transform.position.y : (raycastHit.distance + 10f));
		}

		public void Immobilize()
		{
			this.m_Immobilized = true;
		}

		public void Reset()
		{
			this.m_Immobilized = false;
		}

		[SerializeField]
		private float m_MaxEnginePower = 40f;

		[SerializeField]
		private float m_Lift = 0.002f;

		[SerializeField]
		private float m_ZeroLiftSpeed = 300f;

		[SerializeField]
		private float m_RollEffect = 1f;

		[SerializeField]
		private float m_PitchEffect = 1f;

		[SerializeField]
		private float m_YawEffect = 0.2f;

		[SerializeField]
		private float m_BankedTurnEffect = 0.5f;

		[SerializeField]
		private float m_AerodynamicEffect = 0.02f;

		[SerializeField]
		private float m_AutoTurnPitch = 0.5f;

		[SerializeField]
		private float m_AutoRollLevel = 0.2f;

		[SerializeField]
		private float m_AutoPitchLevel = 0.2f;

		[SerializeField]
		private float m_AirBrakesEffect = 3f;

		[SerializeField]
		private float m_ThrottleChangeSpeed = 0.3f;

		[SerializeField]
		private float m_DragIncreaseFactor = 0.001f;

		private float m_OriginalDrag;

		private float m_OriginalAngularDrag;

		private float m_AeroFactor;

		private bool m_Immobilized;

		private float m_BankedTurnAmount;

		private Rigidbody m_Rigidbody;

		private WheelCollider[] m_WheelColliders;
	}
}
