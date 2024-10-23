using System;
using System.Collections.Generic;
using UnityEngine;
using Zombie3D;

public class EnemyAttr : BoneInfo
{
	public void SetEnemy(Enemy _enemy)
	{
		this.enemy = _enemy;
		for (int i = 0; i < this.attackBoxScript.Length; i++)
		{
			this.attackBoxScript[i].SetEnemy(this.enemy, i);
		}
	}

	[ContextMenu("合并网格")]
	public GameObject CombineMesh(params Bone[] exceptBones)
	{
		if (this.combineMesh != null)
		{
			this.combineMesh.SetActive(false);
			UnityEngine.Object.Destroy(this.combineMesh);
		}
		List<SkinnedMeshRenderer> list = new List<SkinnedMeshRenderer>();
		for (int i = 0; i < this.meshs.Length; i++)
		{
			list.Add(this.meshs[i]);
		}
		list[6] = null;
		list[8] = null;
		list[2] = null;
		list[4] = null;
		list[9] = null;
		if (exceptBones != null)
		{
			for (int j = 0; j < exceptBones.Length; j++)
			{
				list[(int)exceptBones[j]] = null;
			}
		}
		list.RemoveAll((SkinnedMeshRenderer render) => render == null);
		return base.CombineMesh(list.ToArray(), base.gameObject);
	}

	[ContextMenu("记录骨骼原始名称")]
	private void RememberBoneNames()
	{
		for (int i = 0; i < this.bones.Length; i++)
		{
			this.boneNames[i] = this.bones[i].gameObject.name;
			if (this.bones[i].gameObject.GetComponent<Collider>() != null)
			{
				this.bones[i].gameObject.GetComponent<Collider>().enabled = true;
			}
		}
		this.specialBoneNames = new string[this.specialBones.Length];
		for (int j = 0; j < this.specialBoneNames.Length; j++)
		{
			this.specialBoneNames[j] = this.specialBones[j].name;
		}
	}

	[ContextMenu("删除所有刚体和关节")]
	private void RemoveAllRigidBody()
	{
		CharacterJoint[] componentsInChildren = base.gameObject.GetComponentsInChildren<CharacterJoint>();
		foreach (CharacterJoint obj in componentsInChildren)
		{
			UnityEngine.Object.DestroyImmediate(obj);
		}
		Rigidbody[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<Rigidbody>();
		foreach (Rigidbody obj2 in componentsInChildren2)
		{
			UnityEngine.Object.DestroyImmediate(obj2);
		}
	}

	[Space]
	[CNName("需要合并网格")]
	public bool needCombineMesh = true;

	[CNName("可断肢")]
	public bool canBreakLimbs = true;

	[Space]
	[CNName("攻击范围")]
	public float _attackRange = 3f;

	[CNName("瞄准骨骼")]
	public Transform aimBone;

	[Header("红眼相关特效")]
	public ParticleSystem[] redEye;

	[Space]
	[SerializeField]
	[Header("动画统计")]
	[CNName("攻击动画数量")]
	public int attackAniNum;

	[CNName("移动动画数量")]
	public int walkAniNum;

	[CNName("攻击间隔")]
	public float attackInterval;

	[CNName("巡逻速度")]
	public float patrolSpeed = 1f;

	[CNName("行走速度")]
	public float walkSpeed = 2f;

	[CNName("奔跑速度")]
	public float runSpeed = 3f;

	[Space]
	[SerializeField]
	[Header("布娃娃参数")]
	[CNName("力度")]
	public float forceValue = 5f;

	[CNName("力类型")]
	public ForceMode fMode = ForceMode.Impulse;

	[CNName("血条根节点")]
	public Transform hpGameObject;

	[CNName("血条")]
	public Transform hpSlider;

	[Space]
	[SerializeField]
	[Header("粒子特效")]
	public ParticleSystem[] allParticles;

	[HideInInspector]
	public Collider[] attackBox;

	[HideInInspector]
	public EnemyAttackBox[] attackBoxScript;

	[HideInInspector]
	public bool[] oneShotKill;

	[HideInInspector]
	public bool[] isCritical;

	protected bool isCombined;

	protected Enemy enemy;
}
