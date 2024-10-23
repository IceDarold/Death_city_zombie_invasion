using System;
using UnityEngine;
using Zombie3D;

public class Boomer2 : Enemy
{
	public override void Init(GameObject gObject, EnemyState _state, int startAniID, bool armored = false, bool armed = false, bool redEye = false)
	{
		base.Init(gObject, _state, startAniID, armored, armed, redEye);
		this.controlledMesh = this.enemyTransform.Find("Mod_Pao").gameObject;
		this.controlledMesh.SetActive(false);
		this.nav.areaMask |= 512;
		this.nav.areaMask |= 1024;
		this.nav.areaMask |= 4096;
	}

	public override void InitEnemyAttribute(GameObject _obj, bool armored = false, bool armed = false, bool redEye = false)
	{
		base.InitEnemyAttribute(_obj, armored, armed, redEye);
	}

	public override void DoLogic(float deltaTime)
	{
		base.DoLogic(deltaTime);
	}

	public override void DoMove(float deltaTime)
	{
		if (this.gameScene.PlayingState != PlayingState.GamePlaying && this.gameScene.PlayingState != PlayingState.WaitForEnd)
		{
			return;
		}
		if (!this.isExploded)
		{
			this.findPathScheduler.DoUpdate(deltaTime);
		}
		this.checkTargetSchduler.DoUpdate(deltaTime);
		this.nav.Move(this.enemyTransform.forward * this.walkSpeed * deltaTime);
		if (this.nav.path.corners.Length >= 2)
		{
			Vector3 vector = this.nav.path.corners[1] - this.enemyTransform.position;
			vector.y = 0f;
			if (!this.isExploded || vector.sqrMagnitude > 0.0100000007f)
			{
				this.enemyTransform.rotation = Quaternion.Lerp(this.enemyTransform.rotation, Quaternion.LookRotation(vector), deltaTime * 10f);
			}
			if (!this.isExploded && this.CheckGetObstacle(vector) && this.toClimb == ClimbType.JUMP && (this.enemyTransform.position - this.nav.currentOffMeshLinkData.startPos).sqrMagnitude < 0.0100000007f)
			{
				base.SetState(Enemy.JUMP_STATE, true);
			}
		}
	}

	public override void OnDead()
	{
		base.OnDead();
	}

	public override void OnHeadShoot(DamageProperty dp, WeaponType weaponType, Vector3 pos)
	{
		if (this.isExploded)
		{
			return;
		}
		this.lastHurtPos = pos;
		this.gameScene.ShowHitBlood(pos, Quaternion.identity);
		this.audio.PlayAudio("Hurt");
		this.lastHurtWeapon = weaponType;
		this.hurtBone = Bone.Head;
		this.lastDP = dp;
		this.DoCheckHP(dp.damage);
	}

	public override void OnHit(DamageProperty dp, WeaponType weaponType, Vector3 pos, Bone _bone)
	{
		if (this.isExploded)
		{
			return;
		}
		this.gotHitTime = Time.time;
		if (_bone == Bone.Head)
		{
			this.OnHeadShoot(dp, weaponType, pos);
			return;
		}
		this.hurtBone = _bone;
		this.lastHurtPos = pos;
		this.lastHurtWeapon = weaponType;
		this.lastDP = dp;
		if (this.hurtBone == Bone.MiddleSpine)
		{
			this.middleSpineRight = (this.attr.bones[9].transform.InverseTransformPoint(pos).z < 0f);
		}
		this.gameScene.ShowHitBlood(pos, Quaternion.identity);
		if (this.state == Enemy.WAIT_STATE || this.state == Enemy.PATROL_STATE)
		{
			base.WakeUp();
		}
		this.audio.PlayAudio("Hurt");
		this.DoCheckHP(dp.damage);
	}

	public void DoCheckHP(float damage)
	{
		this.preHp = this.hp;
		this.hp -= damage;
		this.hp = Mathf.Clamp(this.hp, 1f, this.MaxHp);
		this.SetHP();
		if (!this.isExploded && this.hp <= 1f)
		{
			if (this.state == Enemy.CATCHING_STATE)
			{
				this.DoAttack();
			}
			else
			{
				this.hp = 0f;
				this.isExploded = true;
				this.PlayBombEffect(this.attr.bones[9].transform.position);
			}
		}
	}

	public override void SetHP()
	{
		float num = this.hp / this.MaxHp;
		num = Mathf.Clamp(num, 0f, 1f);
		this.attr.hpSlider.localScale = new Vector3(num, 1f, 1f);
		this.attr.hpGameObject.gameObject.SetActive(this.hp > 1f);
	}

	private void DoAttack()
	{
		this.isExploded = true;
		base.SetAnimation(E_ANIMATION.ATTACK);
		this.nav.areaMask &= -513;
		this.nav.areaMask &= -1025;
		this.nav.areaMask &= -4097;
		this.controlledMesh.SetActive(true);
		this.gotHitTime = 0f;
		this.nav.ResetPath();
		base.DoFindPath();
	}

	public override bool CouldEnterAttackState()
	{
		if (!this.isExploded && base.CouldEnterAttackState())
		{
			if (this.lastDP == null)
			{
				this.lastDP = new DamageProperty(float.PositiveInfinity);
			}
			this.DoAttack();
		}
		return false;
	}

	public override void OnSpecialEventFrame(int id = 0)
	{
		this.PlayBombEffect(this.enemyTransform.position + Vector3.up * 2f);
		this.hp = 0f;
	}

	private void PlayBombEffect(Vector3 position)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.rConfig.Bomber2Explod);
		gameObject.transform.position = position;
		this.audio.PlayAudio("Bomb");
		this.gameScene.Bomb2Enemies(float.PositiveInfinity, this.enemyTransform.position, 400f, this.attackRange, WeaponType.NoGun, false, null);
		float magnitude = (this.gameScene.GetPlayer().GetTransform().position - this.enemyTransform.position).magnitude;
		bool flag = magnitude <= this.attackRange;
		if (flag)
		{
			this.player.OnHit(this.attackDamage, true, AttackType.BOMB);
		}
		Camera cameraComponent = this.gameScene.GetCamera().cameraComponent;
		Vector3 point = cameraComponent.WorldToViewportPoint(position);
		Rect rect = new Rect(0.1f, 0.1f, 0.9f, 0.9f);
		bool flag2 = rect.Contains(point);
		if (flag && flag2)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.rConfig.Bomber2Screen, this.gameScene.GetCamera().transform, false);
		}
	}

	public bool isExploded;

	public const string MESH_NAME = "Mod_Pao";

	protected GameObject controlledMesh;
}
