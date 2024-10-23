using System;
using DataCenter;
using UnityEngine;

namespace Zombie3D
{
	public class BossButcher : Butcher
	{
		public override void Init(GameObject gObject, EnemyState state, int startAniID, bool armored = false, bool armed = false, bool redEye = false)
		{
			base.Init(gObject, state, startAniID, armored, armed, redEye);
			this.bossAttr = (this.attr as BossButcherAttr);
		}

		public override void DoLogic(float deltaTime)
		{
			this.state.NextState(this, deltaTime);
			if (this.nav.enabled)
			{
				this.nav.avoidancePriority = ((this.state != Enemy.RUSH_ATTACK) ? ((int)base.SqrDistanceFromPlayer) : 0);
			}
			if (this.state != Enemy.DEAD_STATE)
			{
				this.attr.hpGameObject.rotation = this.gameScene.GetCamera().CameraTransform.rotation;
				this.attr.hpGameObject.gameObject.SetActive(Time.time - this.gotHitTime < 2f);
			}
			if (this.state != Enemy.RUSH_START && this.state != Enemy.RUSH_ATTACK && this.state != Enemy.RUSH_END && this.state != Enemy.BUTCHER_ATTACK)
			{
				this.skillCD -= deltaTime;
				this.skillCD = Mathf.Clamp(this.skillCD, 0f, this.bossAttr.skillCD);
			}
		}

		public override void OnAttackBoxEnterTrigger(Collider other, int boxID)
		{
			base.OnAttackBoxEnterTrigger(other, boxID);
		}

		public override void OnHit(DamageProperty dp, WeaponType weaponType, Vector3 pos, Bone _bone)
		{
			if (CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.BOSS)
			{
				Singleton<DropItemManager>.Instance.ShowBossHitDrop(this.attr.bones[9].transform.position);
			}
			base.OnHit(dp, weaponType, pos, _bone);
		}

		public override void DoMove(float deltaTime)
		{
			base.DoMove(deltaTime);
		}

		public override void DoCheckSkill()
		{
			if (this.skillCD > 0f)
			{
				return;
			}
			float distancesFromPlayer = base.DistancesFromPlayer;
			if (distancesFromPlayer < this.attackRange)
			{
				return;
			}
			this.skillCD = this.bossAttr.skillCD;
			if (distancesFromPlayer > this.bossAttr.rushDisMax)
			{
				if (UnityEngine.Random.Range(0f, 1f) <= 0.5f)
				{
					base.SetState(Enemy.RUSH_START, true);
				}
				else
				{
					this.SetSkillID(1);
					base.SetState(Enemy.BUTCHER_ATTACK, true);
				}
			}
			else
			{
				this.SetSkillID(0);
				base.SetState(Enemy.BUTCHER_ATTACK, true);
			}
		}

		public override bool CheckGetObstacle(Vector3 delta)
		{
			return base.CheckGetObstacle(delta);
		}

		public override void OnDead()
		{
			base.OnDead();
		}

		public void DoHoldOver()
		{
			this.isHoldOver = true;
			this.animator.SetTrigger("OnHoldOver");
		}

		public void SetSkillID(int id)
		{
			this.skillHoldTime = this.bossAttr.skill_hold_time[id];
			this.isHoldOver = false;
			this.animator.SetInteger("SkillID", id);
		}

		public override void OnSpecialEventFrame(int id)
		{
			if (id != 0)
			{
				if (id != 1)
				{
					if (id != 10)
					{
						if (id != 11)
						{
							if (id == 999)
							{
								if (!this.ShouldGoToForceIdle())
								{
									base.SetState(Enemy.CATCHING_STATE, true);
								}
							}
						}
						else
						{
							this.bossSkillWarning = UnityEngine.Object.Instantiate<GameObject>(this.bossAttr.skill_1_warning, this.enemyTransform.position, this.enemyTransform.rotation);
						}
					}
					else
					{
						this.bossSkillWarning = UnityEngine.Object.Instantiate<GameObject>(this.bossAttr.skill_0_warning, this.enemyTransform.position, this.enemyTransform.rotation);
					}
				}
				else
				{
					this.bossSkillWarning.SetActive(false);
					UnityEngine.Object.Destroy(this.bossSkillWarning);
					this.bossSkillParticle = UnityEngine.Object.Instantiate<GameObject>(this.bossAttr.skill_1_effect, this.enemyTransform.position, this.enemyTransform.rotation);
					this.CheckAttackAreaInSector();
				}
			}
			else
			{
				this.bossSkillWarning.SetActive(false);
				UnityEngine.Object.Destroy(this.bossSkillWarning);
				this.bossSkillParticle = UnityEngine.Object.Instantiate<GameObject>(this.bossAttr.skill_0_effect, this.enemyTransform.position, this.enemyTransform.rotation);
				this.CheckAttackAreaInRect();
			}
		}

		public bool GetSkillHoldTimeOver(float dt)
		{
			this.skillHoldTime -= dt;
			return this.skillHoldTime <= 0f;
		}

		public void OnBossButcherAttackStateExit()
		{
		}

		private void CheckAttackAreaInRect()
		{
			Vector3 vector = this.enemyTransform.InverseTransformPoint(this.player.GetTransform().position);
			bool flag = Mathf.Abs(vector.x) < this.bossAttr.attackRect.x / 2f;
			bool flag2 = vector.z < this.bossAttr.attackRect.y;
			if (flag && flag2)
			{
				this.player.OnHit(this.bossAttr.damageRect * this.attackDamage, true, AttackType.NORMAL);
			}
		}

		private void CheckAttackAreaInSector()
		{
			Vector3 vector = this.enemyTransform.InverseTransformPoint(this.player.GetTransform().position);
			if (vector.magnitude > this.bossAttr.attackSector.y)
			{
				return;
			}
			if (vector.z <= 0f)
			{
				return;
			}
			float f = Mathf.Atan(Mathf.Abs(vector.x) / Mathf.Abs(vector.z)) * 57.29578f;
			if (Mathf.Abs(f) <= this.bossAttr.attackSector.x / 2f)
			{
				this.player.OnHit(this.bossAttr.damageSector * this.attackDamage, true, AttackType.NORMAL);
			}
		}

		protected const string SkillID = "SkillID";

		protected const string OnHoldOver = "OnHoldOver";

		protected BossButcherAttr bossAttr;

		protected float skillHoldTime;

		public bool isHoldOver;

		protected GameObject bossSkillParticle;

		protected GameObject bossSkillWarning;
	}
}
