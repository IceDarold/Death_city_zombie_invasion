using System;
using DataCenter;
using UnityEngine;
using Zombie3D;

public class Despot : Butcher
{
	public override void Init(GameObject gObject, EnemyState state, int startAniID, bool armored = false, bool armed = false, bool redEye = false)
	{
		base.Init(gObject, state, startAniID, armored, armed, redEye);
		this.despotAttr = (this.attr as DespotAttr);
		this.rushThresholdMin = this.despotAttr.rushDisMin;
		this.rushThresholdMax = this.despotAttr.rushDisMax;
		this.skillCD = this.despotAttr.skillCD;
		this.despotAttr.SetLateUpdateCallback(new Action(this.FixBulletPositionAndRotation));
		this.audio.PlayAudio("Appear");
		this.gameScene.SetUIDisplayEvnt(UIDisplayEvnt.BOSS_APPEAR, new float[0]);
	}

	public override void DoCheckSkill()
	{
		if (this.skillCD == 0f && this.CheckDistances())
		{
			this.skillCD = this.despotAttr.skillCD;
			if (UnityEngine.Random.Range(0, 2) == 0)
			{
				base.SetState(Enemy.RUSH_START, true);
			}
			else
			{
				base.SetState(Enemy.SKILL_STATE, true);
				this.audio.PlayAudio("ThrowStart");
			}
		}
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
		if (this.state == Enemy.SKILL_STATE)
		{
			if (this.bullet != null)
			{
				this.RotateEnemyToTarget();
			}
		}
		else
		{
			this.skillCD -= deltaTime;
			this.skillCD = Mathf.Clamp(this.skillCD, 0f, this.despotAttr.skillCD);
		}
	}

	public override void OnHit(DamageProperty dp, Zombie3D.WeaponType weaponType, Vector3 pos, Bone _bone)
	{
		if (CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.BOSS)
		{
			Singleton<DropItemManager>.Instance.ShowBossHitDrop(this.attr.bones[9].transform.position);
		}
		base.OnHit(dp, weaponType, pos, _bone);
		this.gameScene.SetUIDisplayEvnt(UIDisplayEvnt.BOSS_HP_PERCENT, new float[]
		{
			this.hp,
			this.MaxHp
		});
	}

	public override void OnAttackBoxEnterTrigger(Collider other, int boxID)
	{
		if (boxID == 0 && other.gameObject.layer == 8)
		{
			base.OnEnemyAttackBoxDisable();
			base.SetAnimationEnd(E_ANIMATION.RUSH_ATTACK);
		}
	}

	private void FixBulletPositionAndRotation()
	{
		if (this.bullet == null)
		{
			return;
		}
		this.bullet.transform.position = this.despotAttr.bulletAnchor.position;
		this.bullet.transform.rotation = this.despotAttr.bulletAnchor.rotation;
	}

	public override void OnSpecialEventFrame(int id)
	{
		if (id == 0)
		{
			this.bullet = UnityEngine.Object.Instantiate<GameObject>(this.rConfig.despotBullet, this.despotAttr.bulletAnchor.position, this.despotAttr.bulletAnchor.rotation);
			this.bulletDir = this.player.GetTransform().position - this.enemyTransform.position - new Vector3(0f, 2f, 0f);
		}
		else if (id == 1)
		{
			Vector3 direction = this.player.GetTransform().position - this.enemyTransform.position - new Vector3(0f, 2f, 0f);
			this.bullet.GetComponent<StraightLineBullet>().Init(direction, this.despotAttr.bulletSpeed, this.attackDamage);
			this.bullet = null;
		}
	}

	protected DespotAttr despotAttr;

	protected GameObject bullet;

	protected Vector3 bulletDir;

	protected new float skillCD;
}
