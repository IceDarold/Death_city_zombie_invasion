using System;
using UnityEngine;

namespace Zombie3D
{
	public class Hunter : Enemy
	{
		public override void OnAttack()
		{
			base.OnAttack();
			this.attacked = false;
			this.lastAttackTime = Time.time;
		}

		public override void OnDead()
		{
		}

		public override void CheckHit()
		{
			if (base.CouldMakeNextAttack())
			{
			}
		}

		public bool Jump(float deltaTime)
		{
			if ((Time.time - this.lastRushingTime > 0.5f && this.enemyTransform.position.y <= 0.2f) || Time.time - this.lastRushingTime > 4f)
			{
				this.CheckHit();
			}
			else
			{
				this.speed += Physics.gravity * deltaTime;
			}
			if (!this.jumpended)
			{
				this.jumpended = true;
			}
			return false;
		}

		public bool JumpEnded
		{
			get
			{
				return this.jumpended;
			}
		}

		public override void DoLogic(float deltaTime)
		{
			base.DoLogic(deltaTime);
			if (this.state == Enemy.DEAD_STATE)
			{
				this.speed = Physics.gravity * 10f;
			}
		}

		public bool ReadyForJump()
		{
			return false;
		}

		public void StartJump()
		{
		}

		public bool LookAroundTimOut()
		{
			return Time.time - this.lookAroundStartTime > 2f;
		}

		public override void DoMove(float deltaTime)
		{
			Vector3 a = this.enemyTransform.TransformDirection(Vector3.forward);
			a += Physics.gravity * 0.2f;
		}

		public override EnemyState EnterSpecialState(float deltaTime)
		{
			return null;
		}

		public new static EnemyState JUMP_STATE = new JumpState();

		public static EnemyState LOOKAROUND_STATE = new LookAroundState();

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

		public Vector3 speed;

		protected bool jumpended;
	}
}
