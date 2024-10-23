using System;
using RootMotion.FinalIK;
using UnityEngine;

namespace RootMotion.Demos
{
	public class Turret : MonoBehaviour
	{
		private void Update()
		{
			foreach (Turret.Part part in this.parts)
			{
				part.AimAt(this.target);
			}
		}

		public Transform target;

		public Turret.Part[] parts;

		[Serializable]
		public class Part
		{
			public void AimAt(Transform target)
			{
				this.transform.LookAt(target.position, this.transform.up);
				if (this.rotationLimit == null)
				{
					this.rotationLimit = this.transform.GetComponent<RotationLimit>();
					this.rotationLimit.Disable();
				}
				this.rotationLimit.Apply();
			}

			public Transform transform;

			private RotationLimit rotationLimit;
		}
	}
}
