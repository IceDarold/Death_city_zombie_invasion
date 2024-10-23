using System;
using UnityEngine;

public class ParticalDepth : MonoBehaviour
{
	private void OnEnable()
	{
		Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>();
		int num = componentsInChildren.Length;
		for (int i = 0; i < num; i++)
		{
			Renderer renderer = componentsInChildren[i];
			renderer.sortingOrder = this.order;
		}
	}

	public int order;
}
