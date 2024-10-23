using System;
using UnityEngine;

namespace Zombie3D
{
	public class Tank : Enemy
	{
		public override void OnAttack()
		{
			base.OnAttack();
			this.startAttacking = true;
			this.lastAttackTime = Time.time;
		}

		public override void CheckHit()
		{
		}

		public override void OnDead()
		{
			this.deadTime = Time.time;
			this.PlayBloodEffect();
			this.enemyObject.layer = 18;
			this.enemyObject.SendMessage("OnLoot");
		}

		public bool Rush(float deltaTime)
		{
			return false;
		}

		public bool RushAttack(float deltaTime)
		{
			return false;
		}

		public override void DoMove(float deltaTime)
		{
			Vector3 a = this.enemyTransform.TransformDirection(Vector3.forward);
			a += Physics.gravity * 0.5f;
		}

		public static EnemyState RUSHINGSTART_STATE = new RushingStartState();

		public static EnemyState RUSHING_STATE = new RushingEndState();

		public static EnemyState RUSHINGATTACK_STATE = new RushingAttackState();

		protected Collider leftHandCollider;

		protected Vector3 targetPosition;

		protected Vector3[] p = new Vector3[4];

		protected bool startAttacking;

		protected float rushingInterval;

		protected float rushingSpeed;

		protected float rushingDamage;

		protected float rushingAttackDamage;

		protected float lastRushingTime;

		protected string rndRushingAnimationName;

		protected Vector3 rushingTarget;

		protected bool rushingCollided;

		protected bool rushingAttacked;
	}
}
