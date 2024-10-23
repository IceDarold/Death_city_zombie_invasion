using System;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Aeroplane
{
	public class AeroplanePropellerAnimator : MonoBehaviour
	{
		private void Awake()
		{
			this.m_Plane = base.GetComponent<AeroplaneController>();
			this.m_PropellorModelRenderer = this.m_PropellorModel.GetComponent<Renderer>();
			this.m_PropellorBlurRenderer = this.m_PropellorBlur.GetComponent<Renderer>();
			this.m_PropellorBlur.parent = this.m_PropellorModel;
		}

		private void Update()
		{
			this.m_PropellorModel.Rotate(0f, this.m_MaxRpm * this.m_Plane.Throttle * Time.deltaTime * 60f, 0f);
			int num = 0;
			if (this.m_Plane.Throttle > this.m_ThrottleBlurStart)
			{
				float num2 = Mathf.InverseLerp(this.m_ThrottleBlurStart, this.m_ThrottleBlurEnd, this.m_Plane.Throttle);
				num = Mathf.FloorToInt(num2 * (float)(this.m_PropellorBlurTextures.Length - 1));
			}
			if (num != this.m_PropellorBlurState)
			{
				this.m_PropellorBlurState = num;
				if (this.m_PropellorBlurState == 0)
				{
					this.m_PropellorModelRenderer.enabled = true;
					this.m_PropellorBlurRenderer.enabled = false;
				}
				else
				{
					this.m_PropellorModelRenderer.enabled = false;
					this.m_PropellorBlurRenderer.enabled = true;
					this.m_PropellorBlurRenderer.material.mainTexture = this.m_PropellorBlurTextures[this.m_PropellorBlurState];
				}
			}
		}

		[SerializeField]
		private Transform m_PropellorModel;

		[SerializeField]
		private Transform m_PropellorBlur;

		[SerializeField]
		private Texture2D[] m_PropellorBlurTextures;

		[SerializeField]
		[Range(0f, 1f)]
		private float m_ThrottleBlurStart = 0.25f;

		[SerializeField]
		[Range(0f, 1f)]
		private float m_ThrottleBlurEnd = 0.5f;

		[SerializeField]
		private float m_MaxRpm = 2000f;

		private AeroplaneController m_Plane;

		private int m_PropellorBlurState = -1;

		private const float k_RpmToDps = 60f;

		private Renderer m_PropellorModelRenderer;

		private Renderer m_PropellorBlurRenderer;
	}
}
