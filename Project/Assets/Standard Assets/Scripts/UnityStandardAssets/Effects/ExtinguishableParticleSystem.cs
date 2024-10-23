using System;
using UnityEngine;

namespace UnityStandardAssets.Effects
{
	public class ExtinguishableParticleSystem : MonoBehaviour
	{
		private void Start()
		{
			this.m_Systems = base.GetComponentsInChildren<ParticleSystem>();
		}

		public void Extinguish()
		{
			foreach (ParticleSystem particleSystem in this.m_Systems)
			{
				var _temp_val_575 = particleSystem.emission; _temp_val_575.enabled = false;
			}
		}

		public float multiplier = 1f;

		private ParticleSystem[] m_Systems;
	}
}
