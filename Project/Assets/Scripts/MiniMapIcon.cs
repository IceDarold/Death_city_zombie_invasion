using System;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapIcon : MonoBehaviour
{
	public IconType iconType
	{
		get
		{
			return this.iconTypes[this.iconTypes.Count - 1];
		}
	}

	public void Start()
	{
		this.InitIconPrefab();
		MiniMapSystem.GetInstances().RegisterIcon(this);
	}

	public void SetIconType(IconType type)
	{
		this.iconTypes.Add(type);
		this.InitIconPrefab();
	}

	public void RemoveIconType()
	{
		this.iconTypes.Remove(this.iconTypes[this.iconTypes.Count - 1]);
		this.InitIconPrefab();
	}

	private void InitIconPrefab()
	{
		if (this.iconTypes.Count == 0)
		{
			UnityEngine.Debug.LogError(base.gameObject.name + "没有设置图标类型");
			return;
		}
		if (this.icon != null)
		{
			this.icon.SetActive(false);
			UnityEngine.Object.Destroy(this.icon);
		}
		this.icon = MiniMapSystem.GetInstances().GetMiniMapIconPrefab(this.iconType);
		this.icon.transform.parent = null;
		this.UpdatePosition();
	}

	public void OnDestroy()
	{
		if (MiniMapSystem.GetInstances())
		{
			MiniMapSystem.GetInstances().RemoveIcon(this);
		}
	}

	public void UpdatePosition()
	{
		this.icon.transform.rotation = base.transform.rotation;
		this.icon.transform.position = base.transform.position - new Vector3(0f, 1f, 0f);
	}

	public void CaculateOutTips(Transform player)
	{
		this.dir = base.transform.position - player.position;
	}

	public List<IconType> iconTypes = new List<IconType>();

	public bool isStatic;

	protected Vector3 dir;

	protected GameObject icon;
}
