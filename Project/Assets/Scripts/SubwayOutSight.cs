using System;
using UnityEngine;

public class SubwayOutSight : MonoBehaviour
{
	private void Awake()
	{
		foreach (MeshRenderer meshRenderer in this.allMeshes)
		{
			meshRenderer.lightmapIndex = this.lightmapIndex;
			meshRenderer.lightmapScaleOffset = this.lightMapOffset;
		}
	}

	public void Update()
	{
		for (int i = 0; i < this.allTransforms.Length; i++)
		{
			this.allTransforms[i].Translate(new Vector3(0f, 0f, -this.backSpeed * Time.deltaTime), Space.Self);
			if (this.allTransforms[i].localPosition.x < -550f)
			{
				this.allTransforms[i].localPosition += new Vector3(441.12f, 0f, 0f);
			}
		}
	}

	public MeshRenderer[] allMeshes;

	public Transform[] allTransforms;

	public int lightmapIndex;

	public Vector4 lightMapOffset;

	public float backSpeed = 1f;
}
