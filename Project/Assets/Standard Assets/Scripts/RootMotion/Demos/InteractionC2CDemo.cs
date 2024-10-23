using System;
using RootMotion.FinalIK;
using UnityEngine;

namespace RootMotion.Demos
{
	public class InteractionC2CDemo : MonoBehaviour
	{
		private void OnGUI()
		{
			if (GUILayout.Button("Shake Hands", new GUILayoutOption[0]))
			{
				this.character1.StartInteraction(FullBodyBipedEffector.RightHand, this.handShake, true);
				this.character2.StartInteraction(FullBodyBipedEffector.RightHand, this.handShake, true);
			}
		}

		private void LateUpdate()
		{
			Vector3 position = Vector3.Lerp(this.character1.ik.solver.rightHandEffector.bone.position, this.character2.ik.solver.rightHandEffector.bone.position, 0.5f);
			this.handShake.transform.position = position;
		}

		public InteractionSystem character1;

		public InteractionSystem character2;

		public InteractionObject handShake;
	}
}
