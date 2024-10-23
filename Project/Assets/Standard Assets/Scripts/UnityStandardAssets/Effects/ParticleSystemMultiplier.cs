using System;
using UnityEngine;

namespace UnityStandardAssets.Effects
{
	public class ParticleSystemMultiplier : MonoBehaviour
	{
		private void Start()
		{
			ParticleSystem[] componentsInChildren = base.GetComponentsInChildren<ParticleSystem>();
			foreach (ParticleSystem particleSystem in componentsInChildren)
			{
				ParticleSystem.MainModule main = particleSystem.main;
				main.startSizeMultiplier *= this.multiplier;
				main.startSpeedMultiplier *= this.multiplier;
				main.startLifetimeMultiplier *= Mathf.Lerp(this.multiplier, 1f, 0.5f);
				particleSystem.Clear();
				particleSystem.Play();
			}
		}

		public float multiplier = 1f;
	}
}
