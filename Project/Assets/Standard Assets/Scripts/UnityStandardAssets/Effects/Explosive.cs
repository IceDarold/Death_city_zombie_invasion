using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.Utility;

namespace UnityStandardAssets.Effects
{
	public class Explosive : MonoBehaviour
	{
		private void Start()
		{
			this.m_ObjectResetter = base.GetComponent<ObjectResetter>();
		}

		private IEnumerator OnCollisionEnter(Collision col)
		{
			if (base.enabled && col.contacts.Length > 0)
			{
				float magnitude = Vector3.Project(col.relativeVelocity, col.contacts[0].normal).magnitude;
				if ((magnitude > this.detonationImpactVelocity || this.m_Exploded) && !this.m_Exploded)
				{
					UnityEngine.Object.Instantiate<Transform>(this.explosionPrefab, col.contacts[0].point, Quaternion.LookRotation(col.contacts[0].normal));
					this.m_Exploded = true;
					base.SendMessage("Immobilize");
					if (this.reset)
					{
						this.m_ObjectResetter.DelayedReset(this.resetTimeDelay);
					}
				}
			}
			yield return null;
			yield break;
		}

		public void Reset()
		{
			this.m_Exploded = false;
		}

		public Transform explosionPrefab;

		public float detonationImpactVelocity = 10f;

		public float sizeMultiplier = 1f;

		public bool reset = true;

		public float resetTimeDelay = 10f;

		private bool m_Exploded;

		private ObjectResetter m_ObjectResetter;
	}
}
