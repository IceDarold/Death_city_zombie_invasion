using System;
using UnityEngine;

namespace Zombie3D
{
	public class LaserGun : Weapon
	{
		public override void Init()
		{
			base.Init();
			this.maxCapacity = 9999;
			this.gunfire = this.gun.transform.Find("gun_fire").gameObject;
		}

		public override void CreateGun()
		{
		}

		public override void LoadConfig()
		{
			base.LoadConfig();
		}

		public new void PlayShootAudio()
		{
			if (this.shootAudio != null)
			{
				AudioPlayer.PlayAudio(this.shootAudio);
			}
		}

		public void SetShootTimeNow()
		{
			this.lastShootTime = Time.time;
		}

		public override void FireUpdate(float deltaTime)
		{
			if (this.laserObj != null)
			{
				Vector3 a = this.cameraComponent.ScreenToWorldPoint(new Vector3(this.gameCamera.ReticlePosition.x, (float)Screen.height - this.gameCamera.ReticlePosition.y, 50f));
				Ray ray = new Ray(this.cameraTransform.position, a - this.cameraTransform.position);
				RaycastHit raycastHit;
				if (Physics.Raycast(ray, out raycastHit, 1000f, 34816))
				{
					this.aimTarget = raycastHit.point;
				}
				else
				{
					this.aimTarget = this.cameraTransform.TransformPoint(0f, 0f, 1000f);
				}
				Vector3 normalized = (this.aimTarget - this.gunfire.transform.position).normalized;
				float magnitude = (this.aimTarget - this.gunfire.transform.position).magnitude;
				this.laserObj.transform.position = this.gunfire.transform.position;
				this.laserObj.transform.LookAt(this.aimTarget);
				if (raycastHit.collider != null)
				{
					this.laserObj.transform.localScale = new Vector3(this.laserObj.transform.localScale.x, this.laserObj.transform.localScale.y, magnitude * 0.5f / 30f);
				}
				if (Time.time - this.lastLaserHitInitiatTime <= 0.03f || (this.aimTarget - normalized - this.cameraTransform.position).sqrMagnitude > 9f)
				{
				}
				if (this.CouldMakeNextShoot())
				{
					if (this.shootAudio != null && !this.shootAudio.isPlaying)
					{
						AudioPlayer.PlayAudio(this.shootAudio);
					}
					this.lastShootTime = Time.time;
				}
			}
		}

		public override void Fire(float deltaTime)
		{
			this.gunfire.GetComponent<Renderer>().enabled = true;
			Vector3 a = this.cameraComponent.ScreenToWorldPoint(new Vector3(this.gameCamera.ReticlePosition.x, (float)Screen.height - this.gameCamera.ReticlePosition.y, 50f));
			Ray ray = new Ray(this.cameraTransform.position, a - this.cameraTransform.position);
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit, 1000f, 35328))
			{
				this.aimTarget = raycastHit.point;
			}
			else
			{
				this.aimTarget = this.cameraTransform.TransformPoint(0f, 0f, 1000f);
			}
			Vector3 normalized = (this.aimTarget - this.gunfire.transform.position).normalized;
			if (this.laserObj == null)
			{
			}
			this.lastShootTime = Time.time;
		}

		public override void StopFire()
		{
			if (this.laserObj != null)
			{
				UnityEngine.Object.Destroy(this.laserObj);
				this.laserObj = null;
			}
			if (this.shootAudio != null)
			{
				this.shootAudio.Stop();
			}
			if (this.gunfire != null)
			{
				this.gunfire.GetComponent<Renderer>().enabled = false;
			}
		}

		public override WeaponType GetWeaponType()
		{
			return WeaponType.LaserGun;
		}

		public override void changeReticle()
		{
		}

		protected float flySpeed;

		private GameObject laserObj;

		protected Vector3 laserStartScale;

		protected float lastLaserHitInitiatTime;
	}
}
