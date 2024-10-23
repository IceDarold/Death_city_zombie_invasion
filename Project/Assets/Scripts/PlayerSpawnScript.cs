using System;
using System.Collections.Generic;
using UnityEngine;
using Zombie3D;

public class PlayerSpawnScript : MonoBehaviour, IBindAbleCollection
{
	public GameObject GetPlayerSpawn(int index)
	{
		GameObject result = null;
		int i = 0;
		int count = this.allPlayerSpawn.Count;
		while (i < count)
		{
			if (this.allPlayerSpawn[i].GetEnable(index))
			{
				result = this.allPlayerSpawn[i].bindObject;
				break;
			}
			i++;
		}
		return result;
	}

	public void SeekGameObjectInCollection(GameObject _obj, Action<string, BindLevel> Success)
	{
		for (int i = 0; i < this.allPlayerSpawn.Count; i++)
		{
			if (this.allPlayerSpawn[i].bindObject.name == _obj.name)
			{
				Success(this.allPlayerSpawn[i].bindObject.name, this.allPlayerSpawn[i]);
				break;
			}
		}
	}

	private void OnDrawGizmos()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(child.position, 0.3f);
		}
	}

	[HideInInspector]
	public List<BindAbleObject> allPlayerSpawn = new List<BindAbleObject>();
}
