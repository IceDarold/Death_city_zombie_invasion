using System;
using UnityEngine;

namespace RootMotion.Demos
{
	public class MechSpiderController : MonoBehaviour
	{
		public Vector3 inputVector
		{
			get
			{
				return new Vector3(UnityEngine.Input.GetAxis("Horizontal"), 0f, UnityEngine.Input.GetAxis("Vertical"));
			}
		}

		private void Update()
		{
			Vector3 forward = this.cameraTransform.forward;
			Vector3 up = base.transform.up;
			Vector3.OrthoNormalize(ref up, ref forward);
			Quaternion quaternion = Quaternion.LookRotation(forward, base.transform.up);
			base.transform.Translate(quaternion * this.inputVector.normalized * Time.deltaTime * this.speed * this.mechSpider.scale, Space.World);
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion, Time.deltaTime * this.turnSpeed);
		}

		public MechSpider mechSpider;

		public Transform cameraTransform;

		public float speed = 6f;

		public float turnSpeed = 30f;
	}
}
