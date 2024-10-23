using System;
using UnityEngine;

public class ButcherAttr : EnemyAttr
{
	[Space(5f)]
	[SerializeField]
	[Header("屠夫属性")]
	[CNName("冲刺最小距离，绿圈")]
	public float rushDisMin;

	[CNName("冲刺最大距离，黄圈")]
	public float rushDisMax;

	[CNName("技能CD")]
	public float skillCD;

	[CNName("受伤僵直阈值")]
	public float change2Hurt = 0.1f;
}
