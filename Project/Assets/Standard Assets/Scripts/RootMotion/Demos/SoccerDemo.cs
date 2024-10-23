using System;
using System.Collections;
using UnityEngine;

namespace RootMotion.Demos
{
	[RequireComponent(typeof(Animator))]
	public class SoccerDemo : MonoBehaviour
	{
		private void Start()
		{
			this.animator = base.GetComponent<Animator>();
			this.defaultPosition = base.transform.position;
			this.defaultRotation = base.transform.rotation;
			base.StartCoroutine(this.ResetDelayed());
		}

		private IEnumerator ResetDelayed()
		{
			for (;;)
			{
				yield return new WaitForSeconds(3f);
				base.transform.position = this.defaultPosition;
				base.transform.rotation = this.defaultRotation;
				this.animator.CrossFade("SoccerKick", 0f, 0, 0f);
				yield return null;
			}
			yield break;
		}

		private Animator animator;

		private Vector3 defaultPosition;

		private Quaternion defaultRotation;
	}
}
