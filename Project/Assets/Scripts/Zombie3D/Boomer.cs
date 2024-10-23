using System;
using UnityEngine;

namespace Zombie3D
{
	public class Boomer : Enemy
	{
		public override void Init(GameObject gObject, EnemyState state, int startAniID, bool armored = false, bool armed = false, bool redEye = false)
		{
			BoomerAttr component = gObject.GetComponent<BoomerAttr>();
			this.dizzinessThreshold = component.dizzinessThreshold;
			this.explosionRadius = component.explosionRadius;
			this.upperBody = component.upperBodyMesh;
			base.Init(gObject, state, startAniID, armored, armed, redEye);
			this.audio.PlayAudio("Appear");
		}

		public override void InitEnemyAttribute(GameObject _obj, bool armored = false, bool armed = false, bool redEye = false)
		{
			base.InitEnemyAttribute(_obj, armored, armed, redEye);
			this.upperBody.SetActive(true);
		}

		public override void DoLogic(float deltaTime)
		{
			base.DoLogic(deltaTime);
			if (this.hurtDuration > 0f)
			{
				this.hurtDuration -= deltaTime;
				this.SetGotHitTime();
			}
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
			this.deadTime = Time.time;
			if (this.dieCallBack != null)
			{
				this.dieCallBack(this.name);
			}
			this.PlayDeadAudio();
			this.gameScene.DoKillEnemy(this.enemyType, this.lastHurtWeapon, false, this.keyEnemy);
			this.state.DoExit(this);
			this.ResetBoneNames();
		}

		public override void RemoveDeadBodyTimer(float dt)
		{
			if (Time.time - this.deadTime > 5f)
			{
				this.gameScene.GetEnemies().Remove(this.enemyObject.name);
				this.enemyObject.SetActive(false);
			}
		}

		public override void OnHit(DamageProperty dp, WeaponType weaponType, Vector3 pos, Bone _bone)
		{
			if (this.hp <= 0f)
			{
				return;
			}
			this.gotHitTime = Time.time;
			this.preHp = this.hp;
			this.hp -= dp.damage;
			this.hurtBone = _bone;
			this.lastHurtPos = pos;
			this.lastHurtWeapon = weaponType;
			this.lastDP = dp;
			this.gameScene.ShowHitBlood(pos, Quaternion.identity);
			this.audio.PlayAudio("Hurt");
			this.hurtDuration = 0.5f;
			this.SetGotHitTime();
			this.SetHP();
		}

		public void SetGotHitTime()
		{
			this.animator.SetFloat("GotHitTime", this.hurtDuration);
		}

		public override void OnAttack()
		{
			base.OnAttack();
			base.SetAnimation(E_ANIMATION.ATTACK);
			this.attacked = false;
			this.lastAttackTime = Time.time;
			this.audio.PlayAudio("Attack");
		}

		public override void OnSpecialEventFrame(int id)
		{
			this.DoExplod();
		}

		public virtual void DoExplod()
		{
			this.upperBody.SetActive(false);
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.rConfig.woodExplode);
			gameObject.transform.position = this.enemyTransform.position;
			this.audio.PlayAudio("Bomb");
			this.gameScene.Bomb2Enemies(float.PositiveInfinity, this.enemyTransform.position, 400f, this.explosionRadius * 2f, WeaponType.NoGun, false, null);
			float magnitude = (this.gameScene.GetPlayer().GetTransform().position - this.enemyTransform.position).magnitude;
			if (magnitude <= this.explosionRadius)
			{
				this.player.OnHit(this.attackDamage * 0.5f, true, AttackType.BOMB);
			}
		}

		private float dizzinessThreshold;

		private float explosionRadius;

		public GameObject upperBody;

		private const string ONDIZZINESS = "OnDizziness";

		private const string GOTHITTIME = "GotHitTime";

		private float hurtDuration;
	}
}
