using System;
using UnityEngine;

public class BossButcherAttr : ButcherAttr
{
	[MyArray(new string[]
	{
		"下劈技能蓄力时间",
		"横扫技能蓄力时间"
	})]
	public float[] skill_hold_time = new float[2];

	[CNName("扇形预警")]
	public GameObject skill_1_warning;

	[CNName("扇形地裂")]
	public GameObject skill_1_effect;

	[CNName("矩形预警")]
	public GameObject skill_0_warning;

	[CNName("矩形地裂")]
	public GameObject skill_0_effect;

	[CNName("矩形地裂伤害")]
	public float damageRect = 50f;

	[CNName("扇形地裂伤害")]
	public float damageSector = 50f;

	[CNName("测试技能")]
	public int testSkill;

	[CNName("矩形伤害--长，款")]
	public Vector2 attackRect;

	[CNName("扇形伤害--角度，半径")]
	public Vector2 attackSector;
}
