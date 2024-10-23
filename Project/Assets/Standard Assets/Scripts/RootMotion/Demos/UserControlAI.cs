using System;
using UnityEngine;

namespace RootMotion.Demos
{
	public class UserControlAI : UserControlThirdPerson
	{
		protected override void Update()
		{
			float d = (!this.walkByDefault) ? 1f : 0.5f;
			Vector3 a = this.moveTarget.position - base.transform.position;
			float magnitude = a.magnitude;
			Vector3 up = base.transform.up;
			Vector3.OrthoNormalize(ref up, ref a);
			float num = (!(this.state.move != Vector3.zero)) ? (this.stoppingDistance * this.stoppingThreshold) : this.stoppingDistance;
			this.state.move = ((magnitude <= num) ? Vector3.zero : (a * d));
		}

		public Transform moveTarget;

		public float stoppingDistance = 0.5f;

		public float stoppingThreshold = 1.5f;
	}
}
