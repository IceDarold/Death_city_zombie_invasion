using System;
using UnityEngine;

[ExecuteInEditMode]
public class BakedLightGroup : MonoBehaviour
{
	public void SetRenderLightInfo(RenderLightInfo[] info)
	{
		for (int i = 0; i < this.renders.Length; i++)
		{
			this.renders[i].lightmapIndex = info[i].lightMapIndex;
			this.renders[i].lightmapScaleOffset = info[i].lightMapTillingOffset;
		}
	}

	public MeshRenderer[] renders;
}
