using System;
using System.Collections;
using UnityEngine;

namespace Zombie3D
{
	public class Saw : Weapon
	{
		public Saw()
		{
			this.maxCapacity = 9999;
			base.IsSelectedForBattle = false;
		}

		public override WeaponType GetWeaponType()
		{
			return WeaponType.Saw;
		}

		public override void LoadConfig()
		{
			base.LoadConfig();
		}

		public override void Init()
		{
			base.Init();
			this.hitForce = 20f;
			this.fireTrail = GameObject.Find("muzzleFlash");
			this.gunfire = this.gun.transform.Find("gun_fire_new").gameObject;
			this.bulletsObjectPool = new ObjectPool();
			this.firelineObjectPool = new ObjectPool();
			this.sparksObjectPool = new ObjectPool();
			this.bulletsObjectPool.Init("Bullets", this.rConf.bullets, 6, 1f);
			this.firelineObjectPool.Init("Firelines", this.rConf.fireline, 4, 0.5f);
			this.sparksObjectPool.Init("Sparks", this.rConf.hitparticles, 3, 0.22f);
		}

		public override void changeReticle()
		{
		}

		public override void CreateGun()
		{
		}

		public override void DoLogic(float deltaTime)
		{
			this.bulletsObjectPool.AutoDestruct();
			this.firelineObjectPool.AutoDestruct();
			this.sparksObjectPool.AutoDestruct();
		}

		public override void FireUpdate(float deltaTime)
		{
		}

		public override bool HaveBullets()
		{
			return true;
		}

		public override void Fire(float deltaTime)
		{
			if (this.shootAudio != null && !this.shootAudio.isPlaying)
			{
				AudioPlayer.PlayAudio(this.shootAudio);
			}
			Hashtable enemies = this.gameScene.GetEnemies();
			IEnumerator enumerator = enemies.Values.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Enemy enemy = (Enemy)obj;
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			this.lastShootTime = Time.time;
		}

		public override void GunOn()
		{
			GameObject gameObject = this.gun.transform.Find("Saw01").gameObject;
			GameObject gameObject2 = this.gun.transform.Find("Saw02").gameObject;
			if (gameObject.GetComponent<Renderer>() != null)
			{
				gameObject.GetComponent<Renderer>().enabled = true;
			}
			if (gameObject2.GetComponent<Renderer>() != null)
			{
				gameObject2.GetComponent<Renderer>().enabled = true;
			}
		}

		public override void GunOff()
		{
			GameObject gameObject = this.gun.transform.Find("Saw01").gameObject;
			GameObject gameObject2 = this.gun.transform.Find("Saw02").gameObject;
			if (gameObject.GetComponent<Renderer>() != null)
			{
				gameObject.GetComponent<Renderer>().enabled = false;
			}
			if (gameObject2.GetComponent<Renderer>() != null)
			{
				gameObject2.GetComponent<Renderer>().enabled = false;
			}
			this.StopFire();
		}

		public override void StopFire()
		{
			if (this.shootAudio != null)
			{
			}
			if (this.gunfire != null)
			{
				this.gunfire.GetComponent<Renderer>().enabled = false;
			}
		}

		protected GameObject fireTrail;

		protected ObjectPool bulletsObjectPool;

		protected ObjectPool firelineObjectPool;

		protected ObjectPool sparksObjectPool;
	}
}
