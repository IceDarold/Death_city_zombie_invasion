using System;
using RootMotion.FinalIK;
using UnityEngine;

namespace RootMotion.Demos
{
	public class HitReactionTrigger : MonoBehaviour
	{
		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
				RaycastHit raycastHit = default(RaycastHit);
				if (Physics.Raycast(ray, out raycastHit, 100f))
				{
					this.hitReaction.Hit(raycastHit.collider, ray.direction * this.hitForce, raycastHit.point);
					this.colliderName = raycastHit.collider.name;
				}
			}
		}

		private void OnGUI()
		{
			GUILayout.Label("LMB to shoot the Dummy, RMB to rotate the camera.", new GUILayoutOption[0]);
			if (this.colliderName != string.Empty)
			{
				GUILayout.Label("Last Bone Hit: " + this.colliderName, new GUILayoutOption[0]);
			}
		}

		[SerializeField]
		private HitReaction hitReaction;

		[SerializeField]
		private float hitForce = 1f;

		private string colliderName;
	}
}
