using System;
using RootMotion.FinalIK;
using UnityEngine;

namespace RootMotion.Demos
{
	public class VRIKPlatform : MonoBehaviour
	{
		private void Start()
		{
			this.lastPosition = base.transform.position;
			this.lastRotation = base.transform.rotation;
		}

		private void Update()
		{
			this.ik.solver.AddPlatformMotion(base.transform.position - this.lastPosition, base.transform.rotation * Quaternion.Inverse(this.lastRotation), base.transform.position);
			this.lastRotation = base.transform.rotation;
			this.lastPosition = base.transform.position;
		}

		public VRIK ik;

		private Vector3 lastPosition;

		private Quaternion lastRotation = Quaternion.identity;
	}
}
