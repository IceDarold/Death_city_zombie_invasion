using System;
using UnityEngine;

namespace Zombie3D
{
	public class MachineGun : Weapon
	{
		public override WeaponType GetWeaponType()
		{
			return WeaponType.MachineGun;
		}

		public override void Init()
		{
			base.Init();
			this.maxCapacity = 9999;
			this.attackFrenquency = 0.25f;
			this.hitForce = 20f;
			this.maxDeflection = 4f;
			this.gunfire = this.gun.transform.Find("gun_fire").gameObject;
			this.bulletsObjectPool = new ObjectPool();
			this.firelineObjectPool = new ObjectPool();
			this.sparksObjectPool = new ObjectPool();
			this.bulletsObjectPool.Init("Bullets", this.rConf.bullets, 6, 1f);
			this.firelineObjectPool.Init("Firelines", this.rConf.fireline, 20, 0.5f);
			this.sparksObjectPool.Init("Sparks", this.rConf.hitparticles, 3, 0.22f);
		}

		public override void changeReticle()
		{
		}

		public override void CreateGun()
		{
		}

		public override void LoadConfig()
		{
			base.LoadConfig();
		}

		public override void FireUpdate(float deltaTime)
		{
			this.deflection.x = this.deflection.x + UnityEngine.Random.Range(-0.5f, 0.5f);
			this.deflection.y = this.deflection.y + UnityEngine.Random.Range(-0.5f, 0.5f);
			this.deflection.x = Mathf.Clamp(this.deflection.x, -this.maxDeflection, this.maxDeflection);
			this.deflection.y = Mathf.Clamp(this.deflection.y, -this.maxDeflection, this.maxDeflection);
		}

		public override void Fire(float deltaTime)
		{
			this.gunfire.GetComponent<Renderer>().enabled = true;
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
			if (Physics.Raycast(ray, out raycastHit, 1000f, 559616))
			{
				this.aimTarget = raycastHit.point;
				Vector3 vector = Vector3.zero;
				Vector3 vector2 = this.player.GetTransform().InverseTransformPoint(this.aimTarget);
				if (vector2.z > 2f)
				{
					for (int i = -2; i <= 2; i++)
					{
						Vector3 a2 = Vector3.zero;
						if (i % 2 == 0)
						{
							a2 = this.gunfire.transform.TransformPoint(Vector3.left * (float)i * 1.5f);
						}
						else
						{
							a2 = this.gunfire.transform.TransformPoint(Vector3.up * (float)i * 1.5f);
						}
						vector = (this.aimTarget - this.gunfire.transform.position).normalized;
						GameObject gameObject = this.firelineObjectPool.CreateObject(a2 + vector * (float)(Mathf.Abs(i) + 2), vector);
						gameObject.transform.Rotate(180f, 0f, 0f);
						if (gameObject == null)
						{
							UnityEngine.Debug.Log("fire line obj null");
						}
						else
						{
							FireLineScript component = gameObject.GetComponent<FireLineScript>();
							component.beginPos = this.rightGun.position;
							component.endPos = raycastHit.point;
						}
					}
				}
				this.bulletsObjectPool.CreateObject(this.rightGun.position, vector);
				GameObject gameObject2 = raycastHit.collider.gameObject;
				if (gameObject2.name.StartsWith("E_"))
				{
					Enemy enemyByID = this.gameScene.GetEnemyByID(gameObject2.name);
					if (enemyByID.GetState() != Enemy.DEAD_STATE)
					{
						if (vector2.z > 2f)
						{
							this.sparksObjectPool.CreateObject(raycastHit.point, -ray.direction);
						}
						DamageProperty dp = new DamageProperty(this.damage * this.player.PowerBuff, ray.direction * this.hitForce);
						int num = UnityEngine.Random.Range(0, 100);
						float sqrMagnitude = (enemyByID.GetPosition() - this.player.GetTransform().position).sqrMagnitude;
						float num2 = this.range * this.range;
						if (sqrMagnitude < num2)
						{
							enemyByID.OnHit(dp, this.GetWeaponType(), Bone.None);
						}
						else if ((float)num < this.accuracy)
						{
							enemyByID.OnHit(dp, this.GetWeaponType(), Bone.None);
						}
					}
				}
				else
				{
					if (vector2.z > 2f)
					{
						this.sparksObjectPool.CreateObject(raycastHit.point, -ray.direction);
					}
					if (gameObject2.layer == 19)
					{
						WoodBoxScript component2 = gameObject2.GetComponent<WoodBoxScript>();
						component2.OnHit(this.damage * this.player.PowerBuff);
					}
				}
				if (this.shootAudio != null && !this.shootAudio.isPlaying)
				{
					AudioPlayer.PlayAudio(this.shootAudio);
				}
				this.sbulletCount--;
				this.sbulletCount = Mathf.Clamp(this.sbulletCount, 0, this.maxCapacity);
				this.lastShootTime = Time.time;
			}
			else
			{
				this.aimTarget = this.cameraTransform.TransformPoint(0f, 0f, 1000f);
			}
		}

		public override void DoLogic(float deltaTime)
		{
			this.bulletsObjectPool.AutoDestruct();
			this.firelineObjectPool.AutoDestruct();
			this.sparksObjectPool.AutoDestruct();
		}

		public override void StopFire()
		{
			this.deflection = Vector2.zero;
			if (this.shootAudio != null)
			{
			}
			if (this.gunfire != null)
			{
				this.gunfire.GetComponent<Renderer>().enabled = false;
			}
		}

		public override void GunOff()
		{
			base.GunOff();
		}

		protected ObjectPool bulletsObjectPool;

		protected ObjectPool firelineObjectPool;

		protected ObjectPool sparksObjectPool;
	}
}
