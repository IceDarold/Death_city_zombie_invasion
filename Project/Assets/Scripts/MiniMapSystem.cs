using System;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapSystem : MonoBehaviour
{
	private void Awake()
	{
		MiniMapSystem.instances = this;
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public GameObject GetMiniMapIconPrefab(IconType type)
	{
		return UnityEngine.Object.Instantiate<GameObject>(this.minimapIconsPrefab[(int)type]);
	}

	public Transform PlayerTrans { get; set; }

	public static MiniMapSystem GetInstances()
	{
		if (MiniMapSystem.instances != null)
		{
			return MiniMapSystem.instances;
		}
		return null;
	}

	public void LateUpdate()
	{
		for (int i = 0; i < this.icons.Count; i++)
		{
			this.icons[i].UpdatePosition();
			if (this.PlayerTrans != null && this.icons[i].iconType != IconType.Role)
			{
				this.icons[i].CaculateOutTips(this.PlayerTrans);
			}
		}
	}

	public void RegisterIcon(MiniMapIcon icon)
	{
		this.icons.Add(icon);
	}

	public void RemoveIcon(MiniMapIcon icon)
	{
		this.icons.Remove(icon);
	}

	public GameObject[] minimapIconsPrefab = new GameObject[5];

	protected static MiniMapSystem instances;

	protected List<MiniMapIcon> icons = new List<MiniMapIcon>();
}
