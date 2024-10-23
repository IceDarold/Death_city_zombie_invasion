using System;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Car
{
	public class Suspension : MonoBehaviour
	{
		private void Start()
		{
			this.m_TargetOriginalPosition = this.wheel.transform.localPosition;
			this.m_Origin = base.transform.localPosition;
		}

		private void Update()
		{
			base.transform.localPosition = this.m_Origin + (this.wheel.transform.localPosition - this.m_TargetOriginalPosition);
		}

		public GameObject wheel;

		private Vector3 m_TargetOriginalPosition;

		private Vector3 m_Origin;
	}
}
