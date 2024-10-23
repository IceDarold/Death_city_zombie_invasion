using System;
using UnityEngine;

namespace RootMotion.Demos
{
	public class TransferMotion : MonoBehaviour
	{
		private void OnEnable()
		{
			this.lastPosition = base.transform.position;
		}

		private void Update()
		{
			Vector3 a = base.transform.position - this.lastPosition;
			this.to.position += a * this.transferMotion;
			this.lastPosition = base.transform.position;
		}

		[Tooltip("The Transform to transfer motion to.")]
		public Transform to;

		[Tooltip("The amount of motion to transfer.")]
		[Range(0f, 1f)]
		public float transferMotion = 0.9f;

		private Vector3 lastPosition;
	}
}
