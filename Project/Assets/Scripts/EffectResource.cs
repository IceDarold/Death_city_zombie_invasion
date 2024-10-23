using System;
using UnityEngine;

[Serializable]
public class EffectResource
{
	public GameObject resource
	{
		get
		{
			if (this.resRef == null)
			{
				this.resRef = Resources.Load<GameObject>("Prefabs/Effect/" + this.resName);
			}
			return this.resRef;
		}
	}

	protected const string EFFECT_PREFAB_PATH = "Prefabs/Effect/";

	public string resName;

	public GameObject resRef;
}
