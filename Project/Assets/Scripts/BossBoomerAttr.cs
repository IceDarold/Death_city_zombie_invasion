using System;
using UnityEngine;

public class BossBoomerAttr : BoomerAttr
{
	[CNName("油桶爆炸伤害")]
	public float oilDrumDmg = 10f;

	[CNName("技能CD")]
	public float skillCD = 5f;

	[CNName("僵直时间")]
	public float dizzinessTime = 3f;

	[CNName("油桶根节点")]
	public Transform drumRoot;

	[CNName("咆哮时间")]
	public float shoutTime = 4f;

	[CNName("咆哮伤害间隔")]
	public float shoutHurtInterval = 0.5f;

	[CNName("咆哮血量阈值")]
	public float shoutHurtThreadhold = 0.3f;

	[CNName("召唤小怪间隔")]
	public float callZombieInterval = 0.5f;

	[CNName("召唤小怪数量")]
	public int callZombieNum = 4;

	[CNName("预警红圈")]
	public GameObject skillBombWarning;

	[CNName("油桶伤害")]
	public float damageBomb = 10f;

	[CNName("油桶爆炸范围")]
	public float bombAttackRange = 4f;

	[CNName("控制点高度")]
	public float ctrlOffset = 1f;

	[CNName("召唤小怪上限")]
	public int callZombieLimit = 4;

	[CNName("单次召唤上限")]
	public int callNumOnce = 4;

	[CNName("扔油桶范围")]
	public float oilRange = 4f;

	public GameObject[] drums;
}
