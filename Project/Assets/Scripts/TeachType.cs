using System;
using UnityEngine;

public enum TeachType
{
	[Header("移动教学")]
	MOVE,
	[Header("射击教学")]
	SHOOT,
	[Header("手雷教学")]
	SHOULEI,
	[Header("血包--不可配置")]
	MEDIC,
	[Header("炮台--不可配置")]
	PAOTAI,
	[Header("切换武器")]
	WEAPON,
	[Header("空")]
	NONE = -1
}
