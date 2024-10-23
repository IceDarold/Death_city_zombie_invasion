using System;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Car
{
	public class BrakeLight : MonoBehaviour
	{
		private void Start()
		{
			this.m_Renderer = base.GetComponent<Renderer>();
		}

		private void Update()
		{
			this.m_Renderer.enabled = (this.car.BrakeInput > 0f);
		}

		public CarController car;

		private Renderer m_Renderer;
	}
}
