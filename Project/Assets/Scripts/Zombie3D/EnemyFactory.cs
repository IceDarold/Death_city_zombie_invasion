using System;
using UnityEngine;

namespace Zombie3D
{
	public class EnemyFactory
	{
		public static EnemyFactory GetInstance()
		{
			if (EnemyFactory.instance == null)
			{
				EnemyFactory.instance = new EnemyFactory();
			}
			return EnemyFactory.instance;
		}

		public GameObject GetEnemyPrefab(EnemyType type)
		{
			if (this.enemy[(int)type] == null)
			{
				this.enemy[(int)type] = (Resources.Load("Prefabs/zombies/" + type.ToString()) as GameObject);
				if (this.enemy[(int)type] == null)
				{
					UnityEngine.Debug.LogError("Prefabs/zombies/" + type.ToString() + "资源没有加载到");
				}
			}
			return this.enemy[(int)type];
		}

		public GameObject GetDeadBody(EnemyType type, Vector3 position, Quaternion rotation)
		{
			if (this.deadbody[(int)type] == null)
			{
				this.deadbody[(int)type] = (Resources.Load("Prefabs/DeadBody/" + type.ToString() + "_RAGDOLL") as GameObject);
				if (this.deadbody[(int)type] == null)
				{
					UnityEngine.Debug.LogError("Prefabs/DeadBody/" + type.ToString() + "_RAGDOLL资源没有加载到");
				}
			}
			return UnityEngine.Object.Instantiate<GameObject>(this.deadbody[(int)type], position, rotation);
		}

		public GameObject limbsHead
		{
			get
			{
				return Resources.Load("Prefabs/DeadBody/head") as GameObject;
			}
		}

		public GameObject limbsArm
		{
			get
			{
				return Resources.Load("Prefabs/DeadBody/arm") as GameObject;
			}
		}

		public GameObject limbsLeg
		{
			get
			{
				return Resources.Load("Prefabs/DeadBody/leg") as GameObject;
			}
		}

		protected static EnemyFactory instance;

		public GameObject[] enemy = new GameObject[16];

		public GameObject[] deadbody = new GameObject[16];
	}
}
