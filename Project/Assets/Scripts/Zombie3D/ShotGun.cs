using System;
using DataCenter;
using UnityEngine;

namespace Zombie3D
{
	public class ShotGun : Weapon
	{
		public ShotGun()
		{
			base.IsSelectedForBattle = false;
			this.shotgunFireTimer = new Timer();
		}

		public override WeaponType GetWeaponType()
		{
			return WeaponType.ShotGun;
		}

		public override void LoadConfig()
		{
			base.LoadConfig();
		}

		public override void Init()
		{
			base.Init();
			this.hitForce = 20f;
			this.gunfire = this.gun.transform.Find("gun_fire").gameObject;
			this.m_GunFireParticle = this.gunfire.GetComponentInChildren<ParticleSystem>();
			this.sparksObjectPool = new ObjectPool();
			this.sparksObjectPool.Init("SPAS12-sparks", this.rConf.hitparticles, 3, 0.22f);
			this.fireArea = 15f;
		}

		public void PlayPumpAnimation()
		{
		}

		public override void DoLogic(float deltaTime)
		{
			base.DoLogic(deltaTime);
			this.sparksObjectPool.AutoDestruct();
		}

		public override void changeReticle()
		{
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
			bool flag = false;
			float num = -(float)((this.gunAtt.oneShotBullets - 1) / 2) * this.gunAtt.angelOffset;
			int i = 0;
			int oneShotBullets = this.gunAtt.oneShotBullets;
			while (i < oneShotBullets)
			{
				Vector3 vector = Quaternion.AngleAxis(num, this.cameraTransform.up) * this.cameraTransform.forward;
				if (Mathf.Abs(num) >= 1f)
				{
					vector = Quaternion.AngleAxis(this.gunAtt.angelRandom * 0.5f - UnityEngine.Random.value * this.gunAtt.angelRandom, this.cameraTransform.right) * vector;
				}
				Ray ray = new Ray(this.cameraTransform.position - this.cameraTransform.forward * this.gunAtt.shotgunShootOffset + vector * this.gunAtt.shotgunShootOffset, vector);
				this.hits = Physics.RaycastAll(ray, 1000f, 134777344);
				Array.Sort<RaycastHit>(this.hits, (RaycastHit a, RaycastHit b) => a.distance.CompareTo(b.distance));
				int j = 0;
				int num2 = (this.hits != null) ? this.hits.Length : 0;
				while (j < num2)
				{
					if (!this.CheckIsEnemyLayer(this.hits[j].collider.gameObject.layer))
					{
						break;
					}
					bool flag2 = this.DoCheckHit(this.hits[j], critical, ray, j);
					if (!flag && flag2)
					{
						flag = true;
					}
					j++;
				}
				num += this.gunAtt.angelOffset;
				i++;
			}
			if (flag)
			{
				this.gameScene.DoPlayerStatistics(PlayerStatistics.ShootHitTimes);
				this.player.HasShootedEnemy = true;
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

		public override void GunOff()
		{
			base.GunOff();
		}

		protected Timer shotgunFireTimer;

		protected ObjectPool sparksObjectPool;
	}
}
