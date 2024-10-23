using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Effects
{
	public class WaterHoseParticles : MonoBehaviour
	{
		private void Start()
		{
			this.m_ParticleSystem = base.GetComponent<ParticleSystem>();
		}

		private void OnParticleCollision(GameObject other)
		{
			int collisionEvents = this.m_ParticleSystem.GetCollisionEvents(other, this.m_CollisionEvents);
			for (int i = 0; i < collisionEvents; i++)
			{
				if (Time.time > WaterHoseParticles.lastSoundTime + 0.2f)
				{
					WaterHoseParticles.lastSoundTime = Time.time;
				}
				Component colliderComponent = this.m_CollisionEvents[i].colliderComponent;
				Rigidbody component = colliderComponent.GetComponent<Rigidbody>();
				if (component != null)
				{
					Vector3 velocity = this.m_CollisionEvents[i].velocity;
					component.AddForce(velocity * this.force, ForceMode.Impulse);
				}
				other.BroadcastMessage("Extinguish", SendMessageOptions.DontRequireReceiver);
			}
		}

		public static float lastSoundTime;

		public float force = 1f;

		private List<ParticleCollisionEvent> m_CollisionEvents = new List<ParticleCollisionEvent>();

		private ParticleSystem m_ParticleSystem;
	}
}
