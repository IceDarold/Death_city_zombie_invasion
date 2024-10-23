using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
	[RequireComponent(typeof(CarController))]
	public class CarUserControl : MonoBehaviour
	{
		private void Awake()
		{
			this.m_Car = base.GetComponent<CarController>();
		}

		private void FixedUpdate()
		{
			float axis = CrossPlatformInputManager.GetAxis("Horizontal");
			float axis2 = CrossPlatformInputManager.GetAxis("Vertical");
			this.m_Car.Move(axis, axis2, axis2, 0f);
		}

		private CarController m_Car;
	}
}
