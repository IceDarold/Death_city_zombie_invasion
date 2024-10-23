using System;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Car
{
	public class CarSelfRighting : MonoBehaviour
	{
		private void Start()
		{
			this.m_Rigidbody = base.GetComponent<Rigidbody>();
		}

		private void Update()
		{
			if (base.transform.up.y > 0f || this.m_Rigidbody.velocity.magnitude > this.m_VelocityThreshold)
			{
				this.m_LastOkTime = Time.time;
			}
			if (Time.time > this.m_LastOkTime + this.m_WaitTime)
			{
				this.RightCar();
			}
		}

		private void RightCar()
		{
			base.transform.position += Vector3.up;
			base.transform.rotation = Quaternion.LookRotation(base.transform.forward);
		}

		[SerializeField]
		private float m_WaitTime = 3f;

		[SerializeField]
		private float m_VelocityThreshold = 1f;

		private float m_LastOkTime;

		private Rigidbody m_Rigidbody;
	}
}
