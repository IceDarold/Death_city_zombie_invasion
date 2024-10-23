using System;
using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
	public class ParticleSystemDestroyer : MonoBehaviour
	{
		private IEnumerator Start()
		{
			ParticleSystem[] systems = base.GetComponentsInChildren<ParticleSystem>();
			foreach (ParticleSystem particleSystem in systems)
			{
				this.m_MaxLifetime = Mathf.Max(particleSystem.main.startLifetime.constant, this.m_MaxLifetime);
			}
			float stopTime = Time.time + UnityEngine.Random.Range(this.minDuration, this.maxDuration);
			while (Time.time < stopTime || this.m_EarlyStop)
			{
				yield return null;
			}
			UnityEngine.Debug.Log("stopping " + base.name);
			foreach (ParticleSystem particleSystem2 in systems)
			{
				var _temp_val_757 = particleSystem2.emission; _temp_val_757.enabled = false;
			}
			base.BroadcastMessage("Extinguish", SendMessageOptions.DontRequireReceiver);
			yield return new WaitForSeconds(this.m_MaxLifetime);
			UnityEngine.Object.Destroy(base.gameObject);
			yield break;
		}

		public void Stop()
		{
			this.m_EarlyStop = true;
		}

		public float minDuration = 8f;

		public float maxDuration = 10f;

		private float m_MaxLifetime;

		private bool m_EarlyStop;
	}
}
