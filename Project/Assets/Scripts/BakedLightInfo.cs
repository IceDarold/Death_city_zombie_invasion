using System;
using UnityEngine;

[Serializable]
public class BakedLightInfo : BindLevel
{
	protected BakedLightInfo()
	{
	}

	public BakedLightInfo(string name, int lightIndex, Vector4 tillingOffset, string _prefabName = "")
	{
		this.childname = name;
		this.lightMapIndex = lightIndex;
		this.lightmapTillingOffset = tillingOffset;
		this.prefabName = _prefabName;
	}

	public BakedLightInfo(string name, int lightIndex, Vector4 tillingOffset, uint bindLevel0, uint bindLevel1) : this(name, lightIndex, tillingOffset, string.Empty)
	{
		this.bindLevel[0] = bindLevel0;
		this.bindLevel[1] = bindLevel1;
	}

	public string GetPrefabName()
	{
		if (string.IsNullOrEmpty(this.prefabName))
		{
			return this.childname;
		}
		return this.prefabName;
	}

	public string childname;

	public string prefabName;

	public int lightMapIndex;

	public Vector4 lightmapTillingOffset;

	public Vector3 position;

	public Vector3 rotation;

	public Vector3 scale;

	public RenderLightInfo[] renderlightInfo;
}
