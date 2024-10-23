using System;
using UnityEngine;

namespace Zombie3D
{
	public class Zombie_Throw : Enemy
	{
		public override void Init(GameObject gObject, EnemyState state, int startAniID, bool armored = false, bool armed = false, bool redEye = false)
		{
			base.Init(gObject, state, startAniID, armored, armed, redEye);
			this.GetAJerrican();
			this.SetStateSpeical(true);
		}

		public override void DoLogic(float deltaTime)
		{
			base.DoLogic(deltaTime);
		}

		public override bool CouldEnterAttackState()
		{
			if (!this.isSpecial)
			{
				return base.CouldEnterAttackState();
			}
			float num = Mathf.Sqrt(base.SqrDistanceFromPlayer);
			if (num < this.throwRadius && Mathf.Abs(this.enemyTransform.position.y - this.target.GetTransform().position.y) < 2f)
			{
				Vector3 a = this.target.GetTransform().position + new Vector3(0f, 0.5f, 0f);
				Ray ray = new Ray(this.enemyTransform.position + new Vector3(0f, 0.5f, 0f), a - (this.enemyTransform.position + new Vector3(0f, 0.5f, 0f)));
				if (Physics.Raycast(ray, out this.rayhit, num, 76032))
				{
					return this.rayhit.collider.gameObject.name == "Player";
				}
			}
			return false;
		}

		public override void OnAttack()
		{
			base.OnAttack();
			base.SetAnimation(E_ANIMATION.ATTACK);
			this.attacked = false;
			this.lastAttackTime = Time.time;
		}

		private void GetAJerrican()
		{
		}

		public override void RandomAttackAni()
		{
			if (!this.isSpecial)
			{
				base.RandomAttackAni();
			}
		}

		public void SetStateSpeical(bool _isSpecial)
		{
			this.isSpecial = _isSpecial;
			this.animator.SetBool("isSpecial", this.isSpecial);
		}

		public override void OnSpecialEventFrame(int id)
		{
		}

		public override void OnDead()
		{
			if (this.isSpecial)
			{
				this.SetStateSpeical(false);
				this.woodBox.OnHit(100000f);
				return;
			}
			base.OnDead();
		}

		public override void OnHeadShoot(DamageProperty dp, WeaponType weaponType, Vector3 pos)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.rConfig.hitBlood, pos, Quaternion.identity);
			this.hp -= dp.damage * 2f;
			if (this.hp <= 0f)
			{
				if (!QualityManager.isLow)
				{
					UnityEngine.Object.Instantiate<GameObject>(this.rConfig.headShootParticle, pos, Quaternion.identity);
				}
				Singleton<DynamicData>.Instance.DoHeadShot();
			}
			if (!this.isSpecial)
			{
				this.state.OnHit(this);
			}
			base.SetEnemyHpToAnimator();
		}

		public override void OnHit(DamageProperty dp, WeaponType weaponType, Vector3 pos, Bone _bone)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.rConfig.hitBlood, pos, Quaternion.identity);
			this.preHp = this.hp;
			this.hp -= dp.damage;
			if (!this.isSpecial)
			{
				this.state.OnHit(this);
			}
			base.SetEnemyHpToAnimator();
		}

		private bool isSpecial = true;

		private GameObject jerricanRoot;

		private GameObject jerricanObject;

		private WoodBoxScript woodBox;

		private float throwRadius;
	}
}
