using System;
using System.Collections.Generic;
using UnityEngine;
using Zombie3D;

public class SceneObjects : SceneBatches, IBindAbleCollection
{
	public override void DoLoad(int curLevelIndex)
	{
		GamePlotManager gamePlotManager = GameApp.GetInstance().GetGameScene().gamePlotManager;
		int i = 0;
		int count = this.allObjects.Count;
		while (i < count)
		{
			if (!(this.allObjects[i].obj == null))
			{
				if (this.allObjects[i].GetEnable(curLevelIndex))
				{
					this.allObjects[i].obj.SetActive(true);
				}
				else
				{
					this.allObjects[i].obj.SetActive(false);
				}
			}
			i++;
		}
	}

	public void SeekGameObjectInCollection(GameObject _obj, Action<string, BindLevel> Success)
	{
		for (int i = 0; i < this.allObjects.Count; i++)
		{
			if (this.allObjects[i].obj.name == _obj.name)
			{
				Success(this.allObjects[i].obj.name, this.allObjects[i]);
				break;
			}
		}
	}

	public List<SceneObjectInfo> allObjects = new List<SceneObjectInfo>();

	protected GameScene gameScene;
}
