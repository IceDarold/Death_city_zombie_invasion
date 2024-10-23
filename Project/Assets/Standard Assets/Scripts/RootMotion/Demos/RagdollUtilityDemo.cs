using System;
using RootMotion.FinalIK;
using UnityEngine;

namespace RootMotion.Demos
{
	public class RagdollUtilityDemo : MonoBehaviour
	{
		private void OnGUI()
		{
			GUILayout.Label(" Press R to switch to ragdoll. \n Weigh in one of the FBBIK effectors to make kinematic changes to the ragdoll pose.\n A to blend back to animation", new GUILayoutOption[0]);
		}

		private void Update()
		{
			if (UnityEngine.Input.GetKeyDown(KeyCode.R))
			{
				this.ragdollUtility.EnableRagdoll();
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.A))
			{
				Vector3 b = this.pelvis.position - this.root.position;
				this.root.position += b;
				this.pelvis.transform.position -= b;
				this.ragdollUtility.DisableRagdoll();
			}
		}

		public RagdollUtility ragdollUtility;

		public Transform root;

		public Rigidbody pelvis;
	}
}
