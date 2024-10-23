using System;
using System.Collections.Generic;
using DataCenter;
using UnityEngine;

namespace Zombie3D
{
	public class Sniper : Weapon
	{
		public override WeaponType GetWeaponType()
		{
			return WeaponType.Sniper;
		}

		public override void Init()
		{
			base.Init();
			this.hitForce = 20f;
			this.firePointTransform = this.gun.transform.Find("gun_fire");
			this.gunfire = this.gun.transform.Find("gun_fire").gameObject;
			this.m_GunFireParticle = this.gunfire.GetComponentInChildren<ParticleSystem>();
			this.bulletsObjectPool = new ObjectPool();
			this.firelineObjectPool = new ObjectPool();
			this.sparksObjectPool = new ObjectPool();
			this.bulletsObjectPool.Init("Bullets", this.rConf.bullets, 6, 1f);
			this.firelineObjectPool.Init("Firelines", this.rConf.fireline, 4, 0.5f);
			this.sparksObjectPool.Init("Sparks", this.rConf.hitparticles, 3, 0.22f);
		}

		public override void LoadConfig()
		{
			base.LoadConfig();
		}

		public override void DoLogic(float deltaTime)
		{
			base.DoLogic(deltaTime);
		}

		public override void Fire(float deltaTime)
		{
			if (this.gameScene.PlayingMode == GamePlayingMode.SnipeMode)
			{
				this.SnipeFire(deltaTime);
			}
			else
			{
				this.NormalFire(deltaTime);
			}
		}

		public void NormalFire(float deltaTime)
		{
			if (this.sbulletCount == 0)
			{
				return;
			}
			bool critical = base.Critical;
			if (critical)
			{
				this.gameScene.SetUIDisplayEvnt(UIDisplayEvnt.CRITICAL, new float[0]);
			}
			this.gameScene.ShakeMainCamera(this.gunAtt.balance, this.gunAtt.shakeTime, this.gunAtt.shakeRange);
			this.gameScene.DoPlayerStatistics(PlayerStatistics.TotalShootTimes);
			base.PlayGunFireParticle();
			Ray ray = new Ray(this.cameraTransform.position, this.cameraTransform.forward);
			bool flag = false;
			this.hits = Physics.SphereCastAll(ray, this.gunAtt.shootRadius, 30f, 134777344);
			Array.Sort<RaycastHit>(this.hits, (RaycastHit a, RaycastHit b) => a.distance.CompareTo(b.distance));
			int i = 0;
			int num = this.hits?.Length ?? 0;
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
			if (flag)
			{
				this.player.HasShootedEnemy = true;
				this.gameScene.DoPlayerStatistics(PlayerStatistics.ShootHitTimes);
				Singleton<GameAudioManager>.Instance.PlayGlobalEffect(GlobalEffect.HitEnemy);
			}
			this.PlayShootAudio();
			base.ReduceBulletNum();
		}

		public void SnipeFire(float deltaTime)
		{
			if (this.sbulletCount == 0)
			{
				return;
			}
			this.hitInfo.Clear();
			this.gotHittedEnemyName.Clear();
			this.gameScene.DoPlayerStatistics(PlayerStatistics.TotalShootTimes);
			this.gameScene.ShakeMainCamera(this.gunAtt.balance, this.gunAtt.shakeTime, this.gunAtt.shakeRange);
			this.ReduceBulletNum();
			Ray ray = new Ray(this.cameraTransform.position, this.cameraTransform.forward);
			Vector3 a = this.cameraTransform.position + this.cameraTransform.forward * 100f;
			RaycastHit raycastHit;
			if (Physics.SphereCast(ray, this.gunAtt.shootRadius, out raycastHit, 100f, 134777344))
			{
				a = raycastHit.point;
			}
			Vector3 vector = this.cameraTransform.position - this.cameraTransform.up * 0.2f;
			Vector3 vector2 = a - vector;
			SnipeBullet snipeBullet;
			if (this.DetectMissionAfterFire(vector, vector2))
			{
				snipeBullet = UnityEngine.Object.Instantiate<SnipeBullet>(this.rConf.SnipeFinalBullet.GetComponent<SnipeFinalBullet>());
				this.gameScene.PlayingState = PlayingState.WaitForEnd;
				this.gameScene.FreezeAllEnemies();
			}
			else
			{
				snipeBullet = UnityEngine.Object.Instantiate<SnipeBullet>(this.rConf.SnipeNormalBullet.GetComponent<SnipeBullet>());
				this.PlayShootAudio();
			}
			snipeBullet.Init(vector, vector2, this.damage, this.gunAtt.hitForce, this.hitInfo);
		}

		public override bool DetectMissionAfterFire(Vector3 origin, Vector3 direction)
		{
			this.hits = Physics.SphereCastAll(origin, this.gunAtt.shootRadius, direction, 200f, 134777344);
			Array.Sort<RaycastHit>(this.hits, (RaycastHit a, RaycastHit b) => a.distance.CompareTo(b.distance));
			List<WeaponHitInfo> list = new List<WeaponHitInfo>();
			for (int i = 0; i < this.hits.Length; i++)
			{
				GameObject gameObject = this.hits[i].collider.gameObject;
				if (this.CheckIsEnemyLayer(gameObject.layer) && gameObject.name.StartsWith("E_"))
				{
					string[] array = gameObject.name.Split(new char[]
					{
						'|'
					});
					
					Enemy enemyByID = this.gameScene.GetEnemyByID(array[0]);
					Bone bone = (Bone)Enum.Parse(typeof(Bone), array[1]);
					Vector3 point = this.hits[i].point;
					float num = this.damage;
					if (bone == Bone.Head)
					{
						num *= 1.5f;
					}
					if (!this.gotHittedEnemyName.Contains(array[0]))
					{
						WeaponHitInfo item = new WeaponHitInfo
						{
							hitBone = bone,
							enemy = enemyByID,
							hitPos = point,
							damage = num,
							hitDirection = direction,
							hitForce = this.gunAtt.hitForce,
							weaponType = WeaponType.Sniper
						};
						this.hitInfo.Add(item);
						list.Add(item);
						this.gotHittedEnemyName.Add(array[0]);
					}
				}
				else if (gameObject.layer == 19)
				{
					BreakAble component = gameObject.GetComponent<BreakAble>();
					List<WeaponHitInfo> collection = component.SimulateKillEnemy();
					list.AddRange(collection);
					this.hitInfo.Add(new WeaponHitInfo
					{
						breakableDrum = component,
						hitPos = this.hits[i].point
					});
				}
				else if (gameObject.layer == 11 || gameObject.layer == 15)
				{
					this.hitInfo.Add(new WeaponHitInfo
					{
						hitPos = this.hits[i].point
					});
					break;
				}
			}
			
			return this.gameScene.SimulateKillEnemy2CheckMission(list);
		}

		public override void ReduceBulletNum()
		{
			if (!base.NoConsumeBullet)
			{
				this.sbulletCount--;
				this.sbulletCount = Mathf.Clamp(this.sbulletCount, 0, this.maxCapacity);
				this.player.CheckBullet();
				base.SetBullet2UI();
			}
			this.lastShootTime = Time.time;
		}

		public override void StopFire()
		{
			base.StopGunFireParticle();
		}

		public override void changeReticle()
		{
		}

		protected List<WeaponHitInfo> hitInfo = new List<WeaponHitInfo>();

		protected List<string> gotHittedEnemyName = new List<string>();

		protected ObjectPool bulletsObjectPool;

		protected ObjectPool firelineObjectPool;

		protected ObjectPool sparksObjectPool;

		protected const string SNIPE_IN_NORMAL = "_NORMAL";
	}
}
