using System;
using UnityEngine;

namespace Zombie3D
{
	public class RocketLauncher : Weapon
	{
		public RocketLauncher()
		{
			this.maxCapacity = 9999;
			base.IsSelectedForBattle = false;
		}

		public override WeaponType GetWeaponType()
		{
			return WeaponType.RocketLauncher;
		}

		public override void LoadConfig()
		{
			base.LoadConfig();
		}

		public override void Init()
		{
			base.Init();
			this.hitForce = 30f;
		}

		public override void changeReticle()
		{
		}

		public override void CreateGun()
		{
		}

		public override void Fire(float deltaTime)
		{
			Ray ray = default(Ray);
			if (this.gameCamera.GetCameraType() == global::CameraType.TPSCamera)
			{
				Vector3 a = this.cameraComponent.ScreenToWorldPoint(new Vector3(this.gameCamera.ReticlePosition.x, (float)Screen.height - this.gameCamera.ReticlePosition.y, 50f));
				ray = new Ray(this.cameraTransform.position, a - this.cameraTransform.position);
			}
			else if (this.gameCamera.GetCameraType() == global::CameraType.TopWatchingCamera)
			{
				ray = new Ray(this.player.GetTransform().position + Vector3.up * 0.5f, this.player.GetTransform().TransformDirection(Vector3.forward));
			}
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit, 1000f, 35328))
			{
				this.aimTarget = raycastHit.point;
			}
			else
			{
				this.aimTarget = this.cameraTransform.TransformPoint(0f, 0f, 1000f);
			}
			Vector3 normalized = (this.aimTarget - this.rightGun.position).normalized;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.projectile, this.rightGun.position, Quaternion.LookRotation(normalized));
			ProjectileScript component = gameObject.GetComponent<ProjectileScript>();
			component.dir = normalized;
			component.flySpeed = this.rocketFlySpeed;
			component.explodeRadius = this.range;
			component.hitForce = this.hitForce;
			component.life = 8f;
			component.damage = this.damage;
			component.GunType = WeaponType.RocketLauncher;
			this.lastShootTime = Time.time;
			this.sbulletCount--;
			this.sbulletCount = Mathf.Clamp(this.sbulletCount, 0, this.maxCapacity);
		}

		public override void StopFire()
		{
		}

		protected const float shootLastingTime = 0.5f;

		protected float rocketFlySpeed;
	}
}
