using System;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Aeroplane
{
	public class LandingGear : MonoBehaviour
	{
		private void Start()
		{
			this.m_Plane = base.GetComponent<AeroplaneController>();
			this.m_Animator = base.GetComponent<Animator>();
			this.m_Rigidbody = base.GetComponent<Rigidbody>();
		}

		private void Update()
		{
			if (this.m_State == LandingGear.GearState.Lowered && this.m_Plane.Altitude > this.raiseAtAltitude && this.m_Rigidbody.velocity.y > 0f)
			{
				this.m_State = LandingGear.GearState.Raised;
			}
			if (this.m_State == LandingGear.GearState.Raised && this.m_Plane.Altitude < this.lowerAtAltitude && this.m_Rigidbody.velocity.y < 0f)
			{
				this.m_State = LandingGear.GearState.Lowered;
			}
			this.m_Animator.SetInteger("GearState", (int)this.m_State);
		}

		public float raiseAtAltitude = 40f;

		public float lowerAtAltitude = 40f;

		private LandingGear.GearState m_State = LandingGear.GearState.Lowered;

		private Animator m_Animator;

		private Rigidbody m_Rigidbody;

		private AeroplaneController m_Plane;

		private enum GearState
		{
			Raised = -1,
			Lowered = 1
		}
	}
}
