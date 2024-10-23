using System;
using System.Collections;
using RootMotion.FinalIK;
using UnityEngine;

namespace RootMotion.Demos
{
	public class ResetInteractionObject : MonoBehaviour
	{
		private void Start()
		{
			this.defaultPosition = base.transform.position;
			this.defaultRotation = base.transform.rotation;
			this.defaultParent = base.transform.parent;
			this.r = base.GetComponent<Rigidbody>();
		}

		private void OnPickUp(Transform t)
		{
			base.StopAllCoroutines();
			base.StartCoroutine(this.ResetObject(Time.time + this.resetDelay));
		}

		private IEnumerator ResetObject(float resetTime)
		{
			while (Time.time < resetTime)
			{
				yield return null;
			}
			Poser poser = base.transform.parent.GetComponent<Poser>();
			if (poser != null)
			{
				poser.poseRoot = null;
				poser.weight = 0f;
			}
			base.transform.parent = this.defaultParent;
			base.transform.position = this.defaultPosition;
			base.transform.rotation = this.defaultRotation;
			if (this.r != null)
			{
				this.r.isKinematic = false;
			}
			yield break;
		}

		public float resetDelay = 1f;

		private Vector3 defaultPosition;

		private Quaternion defaultRotation;

		private Transform defaultParent;

		private Rigidbody r;
	}
}
