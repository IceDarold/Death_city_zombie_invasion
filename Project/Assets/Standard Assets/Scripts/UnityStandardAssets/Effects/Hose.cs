using System;
using UnityEngine;

namespace UnityStandardAssets.Effects
{
	public class Hose : MonoBehaviour
	{
		private void Update()
		{
			this.m_Power = Mathf.Lerp(this.m_Power, (!Input.GetMouseButton(0)) ? this.minPower : this.maxPower, Time.deltaTime * this.changeSpeed);
			if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1))
			{
				this.systemRenderer.enabled = !this.systemRenderer.enabled;
			}
			foreach (ParticleSystem particleSystem in this.hoseWaterSystems)
			{
                ParticleSystem.MainModule e = particleSystem.main;
				e.startSpeed = this.m_Power;
				var _temp_val_581 = particleSystem.emission; _temp_val_581.enabled = (this.m_Power > this.minPower * 1.1f);
			}
		}

		public float maxPower = 20f;

		public float minPower = 5f;

		public float changeSpeed = 5f;

		public ParticleSystem[] hoseWaterSystems;

		public Renderer systemRenderer;

		private float m_Power;
	}
}
