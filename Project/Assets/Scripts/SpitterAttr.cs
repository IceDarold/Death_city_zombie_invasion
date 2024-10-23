using System;
using UnityEngine;

public class SpitterAttr : EnemyAttr
{
	[Space(2f)]
	[CNName("吐痰发出点")]
	public Transform spitPoint;

	[CNName("障碍物检测高度")]
	public float detectionHeight;
}
