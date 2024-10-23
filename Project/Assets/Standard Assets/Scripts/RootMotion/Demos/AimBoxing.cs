using System;
using RootMotion.FinalIK;
using UnityEngine;

namespace RootMotion.Demos
{
	public class AimBoxing : MonoBehaviour
	{
		private void LateUpdate()
		{
			this.aimIK.solver.transform.LookAt(this.pin.position);
			this.aimIK.solver.IKPosition = base.transform.position;
		}

		public AimIK aimIK;

		public Transform pin;
	}
}
