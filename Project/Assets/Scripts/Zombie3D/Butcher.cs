using System;
using UnityEngine;
using UnityEngine.AI;

namespace Zombie3D
{
	public class Butcher : Enemy
	{
		public override void Init(GameObject gObject, EnemyState state, int startAniID, bool armored = false, bool armed = false, bool redEye = false)
		{
			this.skillScheduler = new TimeScheduler(0.5f, new Action(this.DoCheckSkill), null);
			base.Init(gObject, state, startAniID, armored, armed, redEye);
			this.butcherAttr = (this.attr as ButcherAttr);
			this.rushThresholdMin = this.butcherAttr.rushDisMin;
			this.rushThresholdMax = this.butcherAttr.rushDisMax;
			this.audio.PlayAudio("Appear");
			this.skillCD = this.butcherAttr.skillCD;
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
			if (this.state != Enemy.RUSH_START && this.state != Enemy.RUSH_ATTACK && this.state != Enemy.RUSH_END)
			{
				this.skillCD -= deltaTime;
				this.skillCD = Mathf.Clamp(this.skillCD, 0f, this.butcherAttr.skillCD);
			}
		}

		public override void OnSpecialEventFrame(int id)
		{
		}

		public override void RemoveDeadBodyTimer(float dt)
		{
			if (Time.time - this.deadTime > 5f)
			{
				this.gameScene.GetEnemies().Remove(this.enemyObject.name);
				this.enemyObject.SetActive(false);
			}
		}

		public override void OnAttackBoxEnterTrigger(Collider other, int boxID)
		{
			if (boxID == 1 && other.gameObject.layer == 8)
			{
				base.SetAnimationEnd(E_ANIMATION.RUSH_ATTACK);
			}
		}

		public override void OnHit(DamageProperty dp, WeaponType weaponType, Vector3 pos, Bone _bone)
		{
			this.gotHitTime = Time.time;
			this.preHp = this.hp;
			this.hp -= dp.damage;
			this.hurtBone = _bone;
			this.lastHurtPos = pos;
			this.lastHurtWeapon = weaponType;
			this.lastDP = dp;
			if (this.state.key != StateKey.GOTHIT_STATE && this.state.key != StateKey.DIZZINESS)
			{
				this.dmg2Hurt += dp.damage;
			}
			this.lastHurtPos = pos;
			if (this.dmg2Hurt >= this.MaxHp * this.butcherAttr.change2Hurt)
			{
				this.dmg2Hurt = 0f;
				this.state.OnHit(this);
			}
			this.gameScene.ShowHitBlood(pos, Quaternion.identity);
			this.audio.PlayAudio("Hurt");
			this.SetHP();
		}

		public override void DoMove(float deltaTime)
		{
			if (this.gameScene.PlayingState != PlayingState.GamePlaying && this.gameScene.PlayingState != PlayingState.WaitForEnd)
			{
				return;
			}
			this.findPathScheduler.DoUpdate(deltaTime);
			this.checkTargetSchduler.DoUpdate(deltaTime);
			if (this.nav.path.corners.Length >= 2)
			{
				Vector3 vector = this.nav.path.corners[1] - this.enemyTransform.position;
				vector.y = 0f;
				if (this.CheckGetObstacle(vector))
				{
					return;
				}
				this.enemyTransform.rotation = Quaternion.Lerp(this.enemyTransform.rotation, Quaternion.LookRotation(vector), deltaTime * 10f);
			}
			this.skillScheduler.DoUpdate(deltaTime);
		}

		public virtual void DoCheckSkill()
		{
			if (this.skillCD == 0f && this.CheckDistances())
			{
				this.skillCD = this.butcherAttr.skillCD;
				base.SetState(Enemy.RUSH_START, true);
			}
		}

		public override bool CheckGetObstacle(Vector3 delta)
		{
			return base.CheckGetObstacle(delta);
		}

		public virtual bool CheckDistances()
		{
			float distancesFromPlayer = base.DistancesFromPlayer;
			if (distancesFromPlayer > this.rushThresholdMin && distancesFromPlayer < this.rushThresholdMax)
			{
				UnityEngine.Debug.Log("dis = " + distancesFromPlayer.ToString());
			}
			if (distancesFromPlayer <= this.rushThresholdMin || distancesFromPlayer >= this.rushThresholdMax)
			{
				return false;
			}
			NavMeshPath navMeshPath = new NavMeshPath();
			bool flag = NavMesh.CalculatePath(this.enemyTransform.position, this.player.GetTransform().position, 1, navMeshPath);
			return flag && navMeshPath.status == NavMeshPathStatus.PathComplete && navMeshPath.corners.Length <= 2;
		}

		public override void OnDead()
		{
			this.OnDieDropItems();
			base.OnEnemyAttackBoxDisable();
			this.nav.enabled = false;
			foreach (ParticleSystem particleSystem in this.attr.redEye)
			{
				particleSystem.gameObject.SetActive(false);
			}
			this.attr.hpSlider.localScale = new Vector3(0f, 1f, 1f);
			this.attr.hpGameObject.gameObject.SetActive(false);
			this.PlayDeadAudio();
			this.deadTime = Time.time;
			if (this.dieCallBack != null)
			{
				this.dieCallBack(this.name);
			}
			this.gameScene.DoKillEnemy(this.enemyType, this.lastHurtWeapon, false, this.keyEnemy);
			if (this.enemyType == EnemyType.E_DESPOT)
			{
				this.gameScene.SetUIDisplayEvnt(UIDisplayEvnt.BOSS_DEAD, new float[0]);
			}
			this.ResetBoneNames();
		}

		public override void OnAttack()
		{
			base.OnAttack();
			base.SetAnimation(E_ANIMATION.ATTACK);
			this.attacked = false;
			this.lastAttackTime = Time.time;
			if (this.animator.GetInteger("AttackID") == 0)
			{
				this.audio.PlayAudio("Attack");
			}
			else
			{
				this.audio.PlayAudio("Attack2");
			}
		}

		protected float dmg2Hurt;

		protected float aniRepeatTimes = 1f;

		protected float rushThresholdMin = 5f;

		protected float rushThresholdMax = 10f;

		protected ButcherAttr butcherAttr;

		protected NavMeshHit navHit;

		protected TimeScheduler skillScheduler;

		protected float skillCD;
	}
}
