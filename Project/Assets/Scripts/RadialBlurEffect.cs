using System;
using UnityEngine;

public class RadialBlurEffect : PostEffectBase
{
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (base._Material)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(source.width >> this.downSampleFactor, source.height >> this.downSampleFactor, 0, source.format);
			RenderTexture temporary2 = RenderTexture.GetTemporary(source.width >> this.downSampleFactor, source.height >> this.downSampleFactor, 0, source.format);
			Graphics.Blit(source, temporary);
			base._Material.SetFloat("_BlurFactor", this.blurFactor);
			base._Material.SetVector("_BlurCenter", this.blurCenter);
			Graphics.Blit(temporary, temporary2, base._Material, 0);
			base._Material.SetTexture("_BlurTex", temporary2);
			base._Material.SetFloat("_LerpFactor", this.lerpFactor);
			Graphics.Blit(source, destination, base._Material, 1);
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.ReleaseTemporary(temporary2);
		}
		else
		{
			Graphics.Blit(source, destination);
		}
	}

	[Range(0f, 0.1f)]
	public float blurFactor = 1f;

	[Range(0f, 2f)]
	public float lerpFactor = 0.5f;

	public int downSampleFactor = 2;

	public Vector2 blurCenter = new Vector2(0.5f, 0.5f);
}
