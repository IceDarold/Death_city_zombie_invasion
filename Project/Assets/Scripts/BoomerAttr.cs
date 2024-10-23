using System;
using UnityEngine;

public class BoomerAttr : EnemyAttr
{
	[Space(5f)]
	[CNName("自爆范围")]
	public float explosionRadius;

	[CNName(0f, 1f, "受伤僵直血量(百分比)")]
	public float dizzinessThreshold = 0.1f;

	[CNName("上半身MESH")]
	public GameObject upperBodyMesh;
}
