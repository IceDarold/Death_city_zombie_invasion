using System;
using UnityEngine;
using UnityEngine.AI;

namespace Zombie3D
{
	public class Spitter : Enemy
	{
		public override void Init(GameObject gObject, EnemyState state, int startAniID, bool armored = false, bool armed = false, bool redEye = false)
		{
			base.Init(gObject, state, startAniID, armored, armed, redEye);
			this.spitterAttr = (this.attr as SpitterAttr);
			this.audio.PlayAudio("Appear");
		}

		public override void OnSpecialEventFrame(int id)
		{
			this.DoSpit();
		}

		public void DoSpit()
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Singleton<ResourceConfigScript>.Instance.spitterBullet, this.enemyTransform.position, this.enemyTransform.rotation);
			gameObject.GetComponentInChildren<SpitterBullet>().Init(this.attackDamage, base.Name);
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
			this.PlayDeadAudio();
			this.state.DoExit(this);
			this.deadTime = Time.time;
			if (this.dieCallBack != null)
			{
				this.dieCallBack(this.name);
			}
			this.ResetBoneNames();
			this.CreateRagdoll(this.lastDP, Bone.MiddleSpine);
			this.gameScene.DoKillEnemy(this.enemyType, this.lastHurtWeapon, false, this.keyEnemy);
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

		public override bool CouldEnterAttackState()
		{
			if (Time.time - this.lastCheckAttackTime < 0.1f)
			{
				return false;
			}
			bool flag = base.CouldEnterAttackState();
			NavMeshPath navMeshPath = new NavMeshPath();
			bool flag2 = NavMesh.CalculatePath(this.enemyTransform.position, this.target.GetTransform().position, 1, navMeshPath);
			bool flag3 = flag2 && navMeshPath.status == NavMeshPathStatus.PathComplete && navMeshPath.corners.Length == 2;
			return flag && flag3;
		}

		public bool CheckAngelToTargte()
		{
			Vector3 forward = this.enemyTransform.forward;
			Vector3 rhs = this.player.GetTransform().position - this.enemyTransform.position;
			float num = Vector3.Dot(forward, rhs);
			return false;
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
			this.middleSpineRight = (this.attr.bones[9].transform.InverseTransformPoint(pos).z > 0f);
			this.gameScene.ShowHitBlood(pos, Quaternion.identity);
			this.state.OnHit(this);
			this.audio.PlayAudio("Hurt");
			this.SetHP();
		}

		public override void OnAttack()
		{
			base.OnAttack();
			base.SetAnimation(E_ANIMATION.ATTACK);
			this.attacked = false;
			this.lastAttackTime = Time.time;
			this.audio.PlayAudio("Attack");
		}

		protected SpitterAttr spitterAttr;

		protected float lastCheckAttackTime = -9999f;
	}
}
