using System;
using UnityEngine;
using Zombie3D;

public class SceneBindAble : MonoBehaviour, IBindAble
{
	public virtual void DoActive(int levelIndex)
	{
	}

	public BindLevel GetBindInfo()
	{
		return this.bindLevelInfo;
	}

	public bool CanUse(int index)
	{
		return this.bindLevelInfo.GetEnable(index);
	}

	[HideInInspector]
	public BindLevel bindLevelInfo = new BindLevel();
}
