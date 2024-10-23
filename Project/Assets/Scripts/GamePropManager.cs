using System;
using System.Collections.Generic;
using DataCenter;
using UnityEngine;

public class GamePropManager : Singleton<GamePropManager>
{
	public GameProp UseProp(int id, Vector3 positon, Vector3 dir = default(Vector3))
	{
		GameProp gameProp = this.GetIdleProp(id);
		if (gameProp == null)
		{
			string prefab = PropDataManager.GetPropData(id).Prefab;
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("GameProp/" + prefab)) as GameObject;
			gameObject.transform.SetParent(base.transform);
			gameProp = gameObject.GetComponent<GameProp>();
			this.PropPool.Add(gameProp);
		}
		gameProp.transform.position = positon;
		gameProp.Activate(dir);
		return gameProp;
	}

	private GameProp GetIdleProp(int id)
	{
		for (int i = 0; i < this.PropPool.Count; i++)
		{
			if (this.PropPool[i].ID == id && !this.PropPool[i].gameObject.activeSelf)
			{
				return this.PropPool[i];
			}
		}
		return null;
	}

	public void Recycle()
	{
		for (int i = 0; i < this.PropPool.Count; i++)
		{
			this.PropPool[i].gameObject.SetActive(false);
		}
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.Q))
		{
			this.Recycle();
		}
	}

	public List<GameProp> PropPool = new List<GameProp>();
}
