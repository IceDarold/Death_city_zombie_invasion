using System;
using System.Collections.Generic;
using UnityEngine;
using Zombie3D;

public class SceneColliders : SceneBatches, IBindAbleCollection
{
	public override void DoLoad(int curLevelIndex)
	{
		int i = 0;
		int count = this.allColliders.Count;
		while (i < count)
		{
			if (this.allColliders[i].GetEnable(curLevelIndex))
			{
				this.allColliders[i].CreateSceneColliders();
			}
			i++;
		}
	}

	public void CreateSceneColliders(SceneColliderInfo sceneCollider)
	{
		sceneCollider.CreateSceneColliders();
	}

	public void SeekGameObjectInCollection(GameObject _obj, Action<string, BindLevel> Success)
	{
		for (int i = 0; i < this.allColliders.Count; i++)
		{
			if (this.allColliders[i].obj.name == _obj.name)
			{
				Success(this.allColliders[i].obj.name, this.allColliders[i]);
				break;
			}
		}
	}

	[HideInInspector]
	public List<SceneColliderInfo> allColliders = new List<SceneColliderInfo>();

	public List<ColliderInfo> colliderInfo = new List<ColliderInfo>();

	[HideInInspector]
	public int curLevelIndex;
}
