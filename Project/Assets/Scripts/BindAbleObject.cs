using System;
using UnityEngine;

[Serializable]
public class BindAbleObject : BindLevel
{
	private BindAbleObject()
	{
	}

	public BindAbleObject(GameObject obj)
	{
		this.bindObject = obj;
	}

	public GameObject bindObject;
}
