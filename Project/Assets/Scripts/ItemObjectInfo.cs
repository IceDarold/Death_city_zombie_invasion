using System;
using UnityEngine;

[Serializable]
public class ItemObjectInfo
{
	public int id;

	[CNName("类型")]
	public AllItemType type;

	[CNName("标题")]
	public string title;

	[CNName("描述")]
	public string desc;

	[CNName("重量")]
	public float weight;

	[CNName("兑换食物")]
	public int exchangeFood;

	[CNName("兑换水")]
	public int exchangeWater;

	[CNName("零件")]
	public int parts;

	public Sprite icon;
}
