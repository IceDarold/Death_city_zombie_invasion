using System;
using UnityEngine;
using UnityEngine.AI;

public class Interactive : MonoBehaviour
{
	public void SetLinkEnable(bool _enable)
	{
		for (int i = 0; i < this.links.Length; i++)
		{
			this.links[i].activated = _enable;
		}
	}

	[ContextMenu("收集link")]
	private void ConnectOffMeshLink()
	{
		this.links = base.gameObject.GetComponentsInChildren<OffMeshLink>();
	}

	public ClimbType cType;

	public OffMeshLink[] links;

	public Animation ani;
}
