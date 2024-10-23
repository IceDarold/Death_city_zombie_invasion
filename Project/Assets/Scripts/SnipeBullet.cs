using System;
using System.Collections.Generic;
using UnityEngine;
using Zombie3D;

public class SnipeBullet : MonoBehaviour
{
	public virtual void Init(Vector3 _position, Vector3 _forward, float _damage, float _hitForce, List<WeaponHitInfo> _hitInfo)
	{
		base.transform.position = _position;
		this.startPos = _position;
		this.direction = _forward.normalized;
		base.transform.rotation = Quaternion.LookRotation(this.direction);
		this.damage = _damage;
		this.hitForce = _hitForce;
		this.hitInfo = _hitInfo;
		if (this.hitInfo.Count != 0)
		{
			this.targetPos = this.hitInfo[0].hitPos;
			this.shootLength = (this.targetPos - _position).magnitude;
		}
		this.life = 10f;
		this.gameScene = GameApp.GetInstance().GetGameScene();
		this.canStart = true;
	}

	public virtual void Update()
	{
		if (!this.canStart)
		{
			return;
		}
		this.life -= Time.deltaTime;
		if (this.life > 0f)
		{
			base.transform.position += base.transform.forward * this.speed * Time.deltaTime;
			float sqrMagnitude = (base.transform.position - this.startPos).sqrMagnitude;
			if (sqrMagnitude >= this.shootLength * this.shootLength)
			{
				base.transform.position = this.targetPos;
				this.OnArriveTargetPos(true);
			}
		}
		else
		{
			this.DestroyThis();
		}
	}

	public void OnArriveTargetPos(bool doHitForce = true)
	{
		if (this.hitInfo[0].enemy == null && this.hitInfo[0].breakableDrum == null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Singleton<ResourceConfigScript>.Instance.hitparticles);
			gameObject.transform.position = this.hitInfo[0].hitPos;
			gameObject.transform.rotation = Quaternion.LookRotation(-this.direction);
			RemoveTimerScript removeTimerScript = gameObject.AddComponent<RemoveTimerScript>();
			removeTimerScript.life = 3f;
		}
		else
		{
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < this.hitInfo.Count; i++)
			{
				WeaponHitInfo weaponHitInfo = this.hitInfo[i];
				if (weaponHitInfo.enemy == null && weaponHitInfo.breakableDrum == null)
				{
					break;
				}
				if (weaponHitInfo.enemy != null)
				{
					if (!flag && weaponHitInfo.hitBone == Bone.Head && weaponHitInfo.enemy.SimulateHitEnemy(weaponHitInfo.damage))
					{
						flag = true;
					}
					if (!flag2 && weaponHitInfo.enemy.SimulateHitEnemy(weaponHitInfo.damage) && this.gameScene.sceneMissions.CheckEnemyInMission(weaponHitInfo.hitBone == Bone.Head, weaponHitInfo.enemy.KeyEnemy))
					{
						flag2 = true;
					}
					Enemy enemy = weaponHitInfo.enemy;
					enemy.OnHit(new DamageProperty(weaponHitInfo.damage, (!doHitForce) ? 0f : weaponHitInfo.hitForce, weaponHitInfo.hitDirection), weaponHitInfo.weaponType, weaponHitInfo.hitPos, weaponHitInfo.hitBone);
				}
				else if (weaponHitInfo.breakableDrum != null)
				{
					weaponHitInfo.breakableDrum.OnHit(new DamageProperty(weaponHitInfo.damage), WeaponType.Sniper, Vector3.zero, Bone.None);
				}
			}
			if (flag)
			{
				Singleton<GlobalMessage>.Instance.SendGlobalMessage(MessageType.SniperHeadShot);
			}
			else if (flag2)
			{
				Singleton<GlobalMessage>.Instance.SendGlobalMessage(MessageType.SniperKillMissionTarget);
			}
		}
		this.DestroyThis();
	}

	public void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawSphere(this.startPos, 0.1f);
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(this.targetPos, 0.1f);
		Gizmos.DrawLine(this.startPos, this.targetPos);
		Gizmos.color = Color.red;
		Gizmos.DrawLine(this.startPos, this.startPos + this.direction * 100f);
	}

	public virtual void DestroyThis()
	{
		this.canStart = false;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public float speed = 60f;

	protected Vector3 direction;

	protected bool canStart;

	protected float damage;

	protected float hitForce;

	protected Vector3 targetPos;

	protected Vector3 startPos;

	protected float life = 10f;

	protected List<WeaponHitInfo> hitInfo;

	protected float shootLength = 1000f;

	protected GameScene gameScene;
}
