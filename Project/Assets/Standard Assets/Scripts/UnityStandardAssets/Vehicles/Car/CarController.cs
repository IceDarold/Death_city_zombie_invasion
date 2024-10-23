using System;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Car
{
	public class CarController : MonoBehaviour
	{
		public bool Skidding { get; private set; }

		public float BrakeInput { get; private set; }

		public float CurrentSteerAngle
		{
			get
			{
				return this.m_SteerAngle;
			}
		}

		public float CurrentSpeed
		{
			get
			{
				return this.m_Rigidbody.velocity.magnitude * 2.23693633f;
			}
		}

		public float MaxSpeed
		{
			get
			{
				return this.m_Topspeed;
			}
		}

		public float Revs { get; private set; }

		public float AccelInput { get; private set; }

		private void Start()
		{
			this.m_WheelMeshLocalRotations = new Quaternion[4];
			for (int i = 0; i < 4; i++)
			{
				this.m_WheelMeshLocalRotations[i] = this.m_WheelMeshes[i].transform.localRotation;
			}
			this.m_WheelColliders[0].attachedRigidbody.centerOfMass = this.m_CentreOfMassOffset;
			this.m_MaxHandbrakeTorque = float.MaxValue;
			this.m_Rigidbody = base.GetComponent<Rigidbody>();
			this.m_CurrentTorque = this.m_FullTorqueOverAllWheels - this.m_TractionControl * this.m_FullTorqueOverAllWheels;
		}

		private void GearChanging()
		{
			float num = Mathf.Abs(this.CurrentSpeed / this.MaxSpeed);
			float num2 = 1f / (float)CarController.NoOfGears * (float)(this.m_GearNum + 1);
			float num3 = 1f / (float)CarController.NoOfGears * (float)this.m_GearNum;
			if (this.m_GearNum > 0 && num < num3)
			{
				this.m_GearNum--;
			}
			if (num > num2 && this.m_GearNum < CarController.NoOfGears - 1)
			{
				this.m_GearNum++;
			}
		}

		private static float CurveFactor(float factor)
		{
			return 1f - (1f - factor) * (1f - factor);
		}

		private static float ULerp(float from, float to, float value)
		{
			return (1f - value) * from + value * to;
		}

		private void CalculateGearFactor()
		{
			float num = 1f / (float)CarController.NoOfGears;
			float b = Mathf.InverseLerp(num * (float)this.m_GearNum, num * (float)(this.m_GearNum + 1), Mathf.Abs(this.CurrentSpeed / this.MaxSpeed));
			this.m_GearFactor = Mathf.Lerp(this.m_GearFactor, b, Time.deltaTime * 5f);
		}

		private void CalculateRevs()
		{
			this.CalculateGearFactor();
			float num = (float)this.m_GearNum / (float)CarController.NoOfGears;
			float from = CarController.ULerp(0f, this.m_RevRangeBoundary, CarController.CurveFactor(num));
			float to = CarController.ULerp(this.m_RevRangeBoundary, 1f, num);
			this.Revs = CarController.ULerp(from, to, this.m_GearFactor);
		}

		public void Move(float steering, float accel, float footbrake, float handbrake)
		{
			for (int i = 0; i < 4; i++)
			{
				Vector3 position;
				Quaternion rotation;
				this.m_WheelColliders[i].GetWorldPose(out position, out rotation);
				this.m_WheelMeshes[i].transform.position = position;
				this.m_WheelMeshes[i].transform.rotation = rotation;
			}
			steering = Mathf.Clamp(steering, -1f, 1f);
			accel = (this.AccelInput = Mathf.Clamp(accel, 0f, 1f));
			footbrake = (this.BrakeInput = -1f * Mathf.Clamp(footbrake, -1f, 0f));
			handbrake = Mathf.Clamp(handbrake, 0f, 1f);
			this.m_SteerAngle = steering * this.m_MaximumSteerAngle;
			this.m_WheelColliders[0].steerAngle = this.m_SteerAngle;
			this.m_WheelColliders[1].steerAngle = this.m_SteerAngle;
			this.SteerHelper();
			this.ApplyDrive(accel, footbrake);
			this.CapSpeed();
			if (handbrake > 0f)
			{
				float brakeTorque = handbrake * this.m_MaxHandbrakeTorque;
				this.m_WheelColliders[2].brakeTorque = brakeTorque;
				this.m_WheelColliders[3].brakeTorque = brakeTorque;
			}
			this.CalculateRevs();
			this.GearChanging();
			this.AddDownForce();
			this.CheckForWheelSpin();
			this.TractionControl();
		}

		private void CapSpeed()
		{
			float num = this.m_Rigidbody.velocity.magnitude;
			SpeedType speedType = this.m_SpeedType;
			if (speedType != SpeedType.MPH)
			{
				if (speedType == SpeedType.KPH)
				{
					num *= 3.6f;
					if (num > this.m_Topspeed)
					{
						this.m_Rigidbody.velocity = this.m_Topspeed / 3.6f * this.m_Rigidbody.velocity.normalized;
					}
				}
			}
			else
			{
				num *= 2.23693633f;
				if (num > this.m_Topspeed)
				{
					this.m_Rigidbody.velocity = this.m_Topspeed / 2.23693633f * this.m_Rigidbody.velocity.normalized;
				}
			}
		}

		private void ApplyDrive(float accel, float footbrake)
		{
			CarDriveType carDriveType = this.m_CarDriveType;
			if (carDriveType != CarDriveType.FourWheelDrive)
			{
				if (carDriveType != CarDriveType.FrontWheelDrive)
				{
					if (carDriveType == CarDriveType.RearWheelDrive)
					{
						float num = accel * (this.m_CurrentTorque / 2f);
						WheelCollider wheelCollider = this.m_WheelColliders[2];
						float motorTorque = num;
						this.m_WheelColliders[3].motorTorque = motorTorque;
						wheelCollider.motorTorque = motorTorque;
					}
				}
				else
				{
					float num = accel * (this.m_CurrentTorque / 2f);
					WheelCollider wheelCollider2 = this.m_WheelColliders[0];
					float motorTorque = num;
					this.m_WheelColliders[1].motorTorque = motorTorque;
					wheelCollider2.motorTorque = motorTorque;
				}
			}
			else
			{
				float num = accel * (this.m_CurrentTorque / 4f);
				for (int i = 0; i < 4; i++)
				{
					this.m_WheelColliders[i].motorTorque = num;
				}
			}
			for (int j = 0; j < 4; j++)
			{
				if (this.CurrentSpeed > 5f && Vector3.Angle(base.transform.forward, this.m_Rigidbody.velocity) < 50f)
				{
					this.m_WheelColliders[j].brakeTorque = this.m_BrakeTorque * footbrake;
				}
				else if (footbrake > 0f)
				{
					this.m_WheelColliders[j].brakeTorque = 0f;
					this.m_WheelColliders[j].motorTorque = -this.m_ReverseTorque * footbrake;
				}
			}
		}

		private void SteerHelper()
		{
			for (int i = 0; i < 4; i++)
			{
				WheelHit wheelHit;
				this.m_WheelColliders[i].GetGroundHit(out wheelHit);
				if (wheelHit.normal == Vector3.zero)
				{
					return;
				}
			}
			if (Mathf.Abs(this.m_OldRotation - base.transform.eulerAngles.y) < 10f)
			{
				float angle = (base.transform.eulerAngles.y - this.m_OldRotation) * this.m_SteerHelper;
				Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
				this.m_Rigidbody.velocity = rotation * this.m_Rigidbody.velocity;
			}
			this.m_OldRotation = base.transform.eulerAngles.y;
		}

		private void AddDownForce()
		{
			this.m_WheelColliders[0].attachedRigidbody.AddForce(-base.transform.up * this.m_Downforce * this.m_WheelColliders[0].attachedRigidbody.velocity.magnitude);
		}

		private void CheckForWheelSpin()
		{
			for (int i = 0; i < 4; i++)
			{
				WheelHit wheelHit;
				this.m_WheelColliders[i].GetGroundHit(out wheelHit);
				if (Mathf.Abs(wheelHit.forwardSlip) >= this.m_SlipLimit || Mathf.Abs(wheelHit.sidewaysSlip) >= this.m_SlipLimit)
				{
					this.m_WheelEffects[i].EmitTyreSmoke();
					if (!this.AnySkidSoundPlaying())
					{
						this.m_WheelEffects[i].PlayAudio();
					}
				}
				else
				{
					if (this.m_WheelEffects[i].PlayingAudio)
					{
						this.m_WheelEffects[i].StopAudio();
					}
					this.m_WheelEffects[i].EndSkidTrail();
				}
			}
		}

		private void TractionControl()
		{
			CarDriveType carDriveType = this.m_CarDriveType;
			if (carDriveType != CarDriveType.FourWheelDrive)
			{
				if (carDriveType != CarDriveType.RearWheelDrive)
				{
					if (carDriveType == CarDriveType.FrontWheelDrive)
					{
						WheelHit wheelHit;
						this.m_WheelColliders[0].GetGroundHit(out wheelHit);
						this.AdjustTorque(wheelHit.forwardSlip);
						this.m_WheelColliders[1].GetGroundHit(out wheelHit);
						this.AdjustTorque(wheelHit.forwardSlip);
					}
				}
				else
				{
					WheelHit wheelHit;
					this.m_WheelColliders[2].GetGroundHit(out wheelHit);
					this.AdjustTorque(wheelHit.forwardSlip);
					this.m_WheelColliders[3].GetGroundHit(out wheelHit);
					this.AdjustTorque(wheelHit.forwardSlip);
				}
			}
			else
			{
				for (int i = 0; i < 4; i++)
				{
					WheelHit wheelHit;
					this.m_WheelColliders[i].GetGroundHit(out wheelHit);
					this.AdjustTorque(wheelHit.forwardSlip);
				}
			}
		}

		private void AdjustTorque(float forwardSlip)
		{
			if (forwardSlip >= this.m_SlipLimit && this.m_CurrentTorque >= 0f)
			{
				this.m_CurrentTorque -= 10f * this.m_TractionControl;
			}
			else
			{
				this.m_CurrentTorque += 10f * this.m_TractionControl;
				if (this.m_CurrentTorque > this.m_FullTorqueOverAllWheels)
				{
					this.m_CurrentTorque = this.m_FullTorqueOverAllWheels;
				}
			}
		}

		private bool AnySkidSoundPlaying()
		{
			for (int i = 0; i < 4; i++)
			{
				if (this.m_WheelEffects[i].PlayingAudio)
				{
					return true;
				}
			}
			return false;
		}

		[SerializeField]
		private CarDriveType m_CarDriveType = CarDriveType.FourWheelDrive;

		[SerializeField]
		private WheelCollider[] m_WheelColliders = new WheelCollider[4];

		[SerializeField]
		private GameObject[] m_WheelMeshes = new GameObject[4];

		[SerializeField]
		private WheelEffects[] m_WheelEffects = new WheelEffects[4];

		[SerializeField]
		private Vector3 m_CentreOfMassOffset;

		[SerializeField]
		private float m_MaximumSteerAngle;

		[Range(0f, 1f)]
		[SerializeField]
		private float m_SteerHelper;

		[Range(0f, 1f)]
		[SerializeField]
		private float m_TractionControl;

		[SerializeField]
		private float m_FullTorqueOverAllWheels;

		[SerializeField]
		private float m_ReverseTorque;

		[SerializeField]
		private float m_MaxHandbrakeTorque;

		[SerializeField]
		private float m_Downforce = 100f;

		[SerializeField]
		private SpeedType m_SpeedType;

		[SerializeField]
		private float m_Topspeed = 200f;

		[SerializeField]
		private static int NoOfGears = 5;

		[SerializeField]
		private float m_RevRangeBoundary = 1f;

		[SerializeField]
		private float m_SlipLimit;

		[SerializeField]
		private float m_BrakeTorque;

		private Quaternion[] m_WheelMeshLocalRotations;

		private Vector3 m_Prevpos;

		private Vector3 m_Pos;

		private float m_SteerAngle;

		private int m_GearNum;

		private float m_GearFactor;

		private float m_OldRotation;

		private float m_CurrentTorque;

		private Rigidbody m_Rigidbody;

		private const float k_ReversingThreshold = 0.01f;
	}
}
