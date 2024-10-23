using System;
using DataCenter;
using UnityEngine;

namespace Zombie3D
{
	public class HandGun : Weapon
	{
		public HandGun()
		{
			base.IsSelectedForBattle = false;
			this.shotgunFireTimer = new Timer();
		}

		public override WeaponType GetWeaponType()
		{
			return WeaponType.HandGun;
		}

		public override void changeReticle()
		{
		}

		public override void LoadConfig()
		{
			base.LoadConfig();
		}

		public override void Init()
		{
			base.Init();
			this.gunfire = this.gun.transform.Find("gun_fire").gameObject;
			this.m_GunFireParticle = this.gunfire.GetComponentInChildren<ParticleSystem>();
			this.bulletsObjectPool = new ObjectPool();
			this.firelineObjectPool = new ObjectPool();
			this.sparksObjectPool = new ObjectPool();
			this.bulletsObjectPool.Init("HandGun_Bullets", this.rConf.bullets, 6, 1f);
			this.firelineObjectPool.Init("HandGun_Firelines", this.rConf.fireline, 4, 0.5f);
			this.sparksObjectPool.Init("HandGun_Sparks", this.rConf.hitparticles, 3, 0.22f);
		}

		public override void FireUpdate(float deltaTime)
		{
			base.FireUpdate(deltaTime);
			if (this.shotgunFireTimer.Ready())
			{
				if (this.gunfire != null)
				{
					this.gunfire.GetComponent<Renderer>().enabled = false;
				}
				this.shotgunFireTimer.Do();
			}
		}

		public override void Fire(float deltaTime)
		{
			if (this.sbulletCount == 0)
			{
				return;
			}
			bool bulletAcross = base.BulletAcross;
			bool critical = base.Critical;
			if (bulletAcross)
			{
				this.gameScene.SetUIDisplayEvnt(UIDisplayEvnt.BULLET_ACROSS, new float[0]);
			}
			if (critical)
			{
				this.gameScene.SetUIDisplayEvnt(UIDisplayEvnt.CRITICAL, new float[0]);
			}
			this.gameScene.ShakeMainCamera(this.gunAtt.balance, this.gunAtt.shakeTime, this.gunAtt.shakeRange);
			this.gameScene.DoPlayerStatistics(PlayerStatistics.TotalShootTimes);
			base.PlayGunFireParticle();
			Ray ray = new Ray(this.cameraTransform.position, this.cameraTransform.forward);
			this.gunAtt.RandomFireDetectedRay(ref ray, this.cameraTransform);
			bool flag = false;
			if (bulletAcross)
			{
				this.hits = Physics.SphereCastAll(ray, this.gunAtt.shootRadius, 30f, 134777344);
				Array.Sort<RaycastHit>(this.hits, (RaycastHit a, RaycastHit b) => a.distance.CompareTo(b.distance));
				int i = 0;
				int num = (this.hits != null) ? this.hits.Length : 0;
				while (i < num)
				{
					if (!this.CheckIsEnemyLayer(this.hits[i].collider.gameObject.layer))
					{
						break;
					}
					bool flag2 = this.DoCheckHit(this.hits[i], critical, ray, 0);
					if (!flag && flag2)
					{
						flag = true;
					}
					i++;
				}
			}
			else
			{
				bool flag3 = Physics.SphereCast(ray, this.gunAtt.shootRadius, out this.hit, 30f, 134777344);
				bool flag4 = this.DoCheckHit(this.hit, critical, ray, 0);
				if (!flag && flag4)
				{
					flag = true;
				}
			}
			if (flag)
			{
				this.player.HasShootedEnemy = true;
				this.gameScene.DoPlayerStatistics(PlayerStatistics.ShootHitTimes);
				Singleton<GameAudioManager>.Instance.PlayGlobalEffect(GlobalEffect.HitEnemy);
			}
			this.PlayShootAudio();
			this.ReduceBulletNum();
		}

		public override void DoHitWall(Vector3 pos, Vector3 dir)
		{
			base.DoHitWall(pos, dir);
			this.sparksObjectPool.CreateObject(pos, dir);
		}

		public override void StopFire()
		{
			base.StopGunFireParticle();
		}

		public override void DoLogic(float deltaTime)
		{
			if (this.isPlayGunFire)
			{
				if (Time.time - this.gunfire_time > 0.03f)
				{
					base.StopGunFireParticle();
					return;
				}
				this.m_GunFireParticle.Play(true);
			}
			this.bulletsObjectPool.AutoDestruct();
			this.firelineObjectPool.AutoDestruct();
			this.sparksObjectPool.AutoDestruct();
		}

		public override void GunOff()
		{
			base.GunOff();
		}

		protected Timer shotgunFireTimer;

		protected ObjectPool bulletsObjectPool;

		protected ObjectPool firelineObjectPool;

		protected ObjectPool sparksObjectPool;

		protected float lastPlayAudioTime;
	}
}
