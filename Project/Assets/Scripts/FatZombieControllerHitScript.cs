using System;
using UnityEngine;

public class FatZombieControllerHitScript : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (hit.collider.gameObject.layer == 13)
		{
			Rigidbody attachedRigidbody = hit.collider.attachedRigidbody;
			if (attachedRigidbody == null)
			{
				return;
			}
			float d = 20f;
			Vector3 a = new Vector3(hit.moveDirection.x, 0f, hit.moveDirection.z);
			attachedRigidbody.velocity = a * d;
		}
	}
}
