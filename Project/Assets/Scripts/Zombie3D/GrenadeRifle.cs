using System;
using UnityEngine;

namespace Zombie3D
{
	public class GrenadeRifle : Weapon
	{
		public override WeaponType GetWeaponType()
		{
			return WeaponType.GrenadeRifle;
		}

		public override void Init()
		{
			base.Init();
			this.firePointTransform = this.gun.transform.Find("Fire_Point");
			this.gunfire = this.gun.transform.Find("gun_fire").gameObject;
			this.m_GunFireParticle = this.gunfire.GetComponentInChildren<ParticleSystem>();
			this.GrenadeLauncherBulletPool = new ObjectPool();
			this.GrenadeLauncherBulletPool.Init("GrenadeLauncherBullets", this.rConf.GrenadeLauncherBullet, 5, 5f);
		}

		public override void changeReticle()
		{
		}

		public override void LoadConfig()
		{
			base.LoadConfig();
		}

		public override void DoLogic(float deltaTime)
		{
			base.DoLogic(deltaTime);
			this.GrenadeLauncherBulletPool.AutoDestruct();
		}

		public override void Fire(float deltaTime)
		{
			if (this.sbulletCount == 0)
			{
				return;
			}
			this.gameScene.ShakeMainCamera(this.gunAtt.balance, this.gunAtt.shakeTime, this.gunAtt.shakeRange);
			base.PlayGunFireParticle();
			GrenadeLauncherBullet grenadeLauncherBullet = this.GrenadeLauncherBulletPool.CreateObject<GrenadeLauncherBullet>(this.firePointPosition, this.firePointRotation);
			grenadeLauncherBullet.Init(this.damage * this.player.attackPower);
			this.PlayShootAudio();
			this.ReduceBulletNum();
		}

		public override void StopFire()
		{
			base.StopGunFireParticle();
		}

		protected ObjectPool GrenadeLauncherBulletPool;

		protected float lastPlayAudioTime;
	}
}
