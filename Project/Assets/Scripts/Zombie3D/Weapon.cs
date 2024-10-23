using System;
using System.Reflection;
using DataCenter;
using UnityEngine;
using ZombieMath;

namespace Zombie3D
{
	public abstract class Weapon
	{
		public Weapon()
		{
		}

		public int DamageLevel { get; set; }

		public int FrequencyLevel { get; set; }

		public int AccuracyLevel { get; set; }

		public int ReloadLevel { get; set; }

		public int ChargerLevel { get; set; }

		public bool IsSelected { get; set; }

		public WeaponConfig WConf { get; set; }

		public bool IsSelectedForBattle { get; set; }

		public abstract void Fire(float deltaTime);

		public abstract void StopFire();

		public WeaponExistState Exist { get; set; }

		public abstract WeaponType GetWeaponType();

		public abstract void changeReticle();

		public string Info
		{
			get
			{
				return this.Name;
			}
		}

		public float ScopePercent
		{
			get
			{
				return this.curScopePercent;
			}
		}

		public int SnipeScope
		{
			get
			{
				return this.snipeScopeIndex;
			}
		}

		public string Name { get; set; }

		public float FightPower { get; set; }

		public int Price
		{
			get
			{
				return this.price;
			}
		}

		public float Accuracy
		{
			get
			{
				return this.accuracy;
			}
			set
			{
				this.accuracy = value;
			}
		}

		public float Damage
		{
			get
			{
				return this.damage;
			}
			set
			{
				this.damage = value;
			}
		}

		public float AttackFrequency
		{
			get
			{
				return this.attackFrenquency;
			}
			set
			{
				this.attackFrenquency = value;
			}
		}

		public bool BulletAcross
		{
			get
			{
				return UnityEngine.Random.Range(0f, 1f) < this.bulletAcrossProbability;
			}
		}

		public bool NoConsumeBullet
		{
			get
			{
				return UnityEngine.Random.Range(0f, 1f) < this.noConsumeBulletProbability;
			}
		}

		public bool Critical
		{
			get
			{
				return UnityEngine.Random.Range(0f, 1f) < this.criticalProbability;
			}
		}

		public float Damage2Elite
		{
			get
			{
				return this.damage2EliteEnemy;
			}
		}

		public float Damage2Normal
		{
			get
			{
				return this.damage2NormalEnemy;
			}
		}

		public float DamageHeadShot
		{
			get
			{
				return this.damageHeadShot;
			}
		}

		public GunAtt GunAtt
		{
			get
			{
				return this.gunAtt;
			}
		}

		public float Reload
		{
			get
			{
				return this.reload;
			}
			set
			{
				this.reload = value;
			}
		}

		public int Charger
		{
			get
			{
				return this.charger;
			}
			set
			{
				this.charger = value;
			}
		}

		public int FirePower
		{
			get
			{
				return this.GetFirePower();
			}
		}

		private int GetFirePower()
		{
			if (this.Exist == WeaponExistState.Owned)
			{
				return (int)(this.damage / this.attackFrenquency * 1.5f + this.accuracy * 5f + (float)(this.ReloadLevel * 15) + (float)(this.ChargerLevel * 8)) * ((this.GetWeaponType() != WeaponType.ShotGun) ? 1 : 3);
			}
			return (int)(this.GetMaxDamage() / this.attackFrenquency * 1.5f + this.GetMaxAccuracy() * 5f + this.WConf.reloadConf.maxLevel * 15f + this.WConf.chargerConf.maxLevel * 8f) * ((this.GetWeaponType() != WeaponType.ShotGun) ? 1 : 3);
		}

		public virtual bool DetectMissionAfterFire(Vector3 origin, Vector3 direction)
		{
			return false;
		}

		public float GetGunSightFOV(float percent)
		{
			BaseCameraScript camera = GameApp.GetInstance().GetGameScene().GetCamera();
			return camera.snipeScopeFOV[0] - (camera.snipeScopeFOV[0] - camera.snipeScopeFOV[this.snipeScopeIndex]) * percent;
		}

		public float GetSnipeBaseFOV()
		{
			BaseCameraScript camera = GameApp.GetInstance().GetGameScene().GetCamera();
			return camera.snipeScopeFOV[0];
		}

		public float FireRange
		{
			get
			{
				return this.range;
			}
		}

		public bool ExpandReticle
		{
			get
			{
				return Time.time - this.gunfire_time < this.gameCamera.shoot_state_duration;
			}
		}

		public GameObject GunObject
		{
			get
			{
				return this.gun;
			}
		}

		public int BulletCount
		{
			get
			{
				return this.sbulletCount;
			}
			set
			{
				this.sbulletCount = value;
			}
		}

		public int MaxCapacity
		{
			get
			{
				return this.maxCapacity;
			}
		}

		public int ID
		{
			get
			{
				return this.id;
			}
		}

		public int MaxGunload
		{
			get
			{
				return this.maxGunLoad;
			}
		}

		public int MaxBulletNum
		{
			get
			{
				return this.maxBulletNum;
			}
		}

		public float GetFireArea()
		{
			return this.fireArea;
		}

		public virtual void DoLogic(float deltaTime)
		{
			if (this.isPlayGunFire)
			{
				if (Time.time - this.gunfire_time > 0.05f)
				{
					this.StopGunFireParticle();
					return;
				}
				this.m_GunFireParticle.Play(true);
			}
			if (this.continuousShoot)
			{
				this.baseOffset = Mathf.Lerp(this.baseOffset, this.gunAtt.shootOffsetMin, Time.deltaTime * 3f);
			}
			else
			{
				this.baseOffset = Mathf.Lerp(this.baseOffset, this.gunAtt.originalOffset, Time.deltaTime * 20f);
			}
		}

		public void SetContinuousShoot(bool isContinuous)
		{
			this.continuousShoot = isContinuous;
		}

		public float GetExpectReticleOffset(bool stable, bool isShooting, float deltaTime, float lerpSpeed)
		{
			this.curScaleRatio = Mathf.Lerp(this.curScaleRatio, (!stable) ? this.gunAtt.scaleRatio : 1f, Time.deltaTime * lerpSpeed);
			return (!this.ExpandReticle) ? (this.curScaleRatio * this.baseOffset) : (this.curScaleRatio * this.baseOffset + this.gunAtt.shootOffsetMax);
		}

		public void PlayGunFireParticle()
		{
			if (this.m_GunFireParticle)
			{
				this.gunfire_time = Time.time;
				this.m_GunFireParticle.Stop(true);
				this.isPlayGunFire = true;
			}
		}

		public void StopGunFireParticle()
		{
			if (this.m_GunFireParticle)
			{
				this.isPlayGunFire = false;
				this.m_GunFireParticle.Stop(true);
			}
		}

		public virtual void LoadConfig()
		{
		}

		public virtual void SetWeaponData(WeaponData data)
		{
			this.damage = (float)WeaponDataManager.GetCurrentAttribute(data, WeaponAttribute.DAMAGE);
			this.attackFrenquency = (float)WeaponDataManager.GetCurrentAttribute(data, WeaponAttribute.SHOOTSPEED);
			this.accuracy = (float)WeaponDataManager.GetCurrentAttribute(data, WeaponAttribute.PRECISE);
			this.reload = 0f;
			this.charger = WeaponDataManager.GetCurrentAttribute(data, WeaponAttribute.MAGAZINES);
			this.speedDrag = (float)data.Weight;
			this.range = (float)data.Range;
			this.maxCapacity = WeaponDataManager.GetCurrentAttribute(data, WeaponAttribute.BULLETS);
			this.maxGunLoad = this.maxCapacity * this.charger;
			this.id = data.ID;
			this.snipeScopeIndex = WeaponDataManager.GetCurrentAttribute(data, WeaponAttribute.SCOPETIMES);
			this.SetTalentData();
		}

		public virtual void SetTalentData()
		{
			this.bulletAcrossProbability = TalentDataManager.GetTalentValue(Talent.BULLET_ACROSS_PROBABILITY);
			this.noConsumeBulletProbability = TalentDataManager.GetTalentValue(Talent.NO_CONSUME_BULLET_PROBABILITY);
			this.damage2EliteEnemy = TalentDataManager.GetTalentValue(Talent.DAMAGE_2_ELITE);
			this.damage2NormalEnemy = TalentDataManager.GetTalentValue(Talent.DAMAGE_2_NORMAL);
			this.damageHeadShot = TalentDataManager.GetTalentValue(Talent.HEADSHOT_DAMAGE);
			this.criticalProbability = TalentDataManager.GetTalentValue(Talent.CRITICAL_PROBABILITY);
			this.bulletLimit = TalentDataManager.GetTalentValue(Talent.BULLET_LIMIT);
			this.maxBulletNum = (this.maxGunLoad = (int)((float)this.maxGunLoad * (1f + this.bulletLimit)));
			this.sbulletCount = this.maxCapacity;
			this.SetBullet2UI();
		}

		public float GetSpeedDrag()
		{
			return this.speedDrag;
		}

		public float GetDamage(bool isCritical, bool toElite = false, bool isHeadShot = false)
		{
			float num = this.damage * this.player.attackPower;
			num *= ((!toElite) ? (1f + this.damage2NormalEnemy) : (1f + this.damage2EliteEnemy));
			num *= ((!isHeadShot) ? 1f : (2f * (1f + this.damageHeadShot)));
			return num * ((!isCritical) ? 1f : 1.5f);
		}

		public int MaxGunLoad
		{
			get
			{
				return this.maxGunLoad;
			}
		}

		public void Upgrade(float power, float frequency, float accur, float _reload, int _charger)
		{
			if (power != 0f)
			{
				this.damage += power;
				int num = (int)(this.damage * 100f);
				this.damage = (float)((double)num * 1.0) / 100f;
				this.DamageLevel++;
			}
			if (frequency != 0f)
			{
				this.attackFrenquency -= frequency;
				int num = (int)(this.attackFrenquency * 100f);
				this.attackFrenquency = (float)((double)num * 1.0) / 100f;
				this.FrequencyLevel++;
			}
			if (accur != 0f)
			{
				this.accuracy += accur;
				int num = (int)(this.accuracy * 100f);
				this.accuracy = (float)((double)num * 1.0) / 100f;
				this.AccuracyLevel++;
			}
			if (_reload != 0f)
			{
				this.reload -= _reload;
				int num = (int)(this.reload * 100f);
				this.reload = (float)((double)num * 1.0) / 100f;
				this.ReloadLevel++;
			}
			if (_charger != 0)
			{
				this.charger += _charger;
				this.ChargerLevel++;
			}
		}

		private float CaculateUpgradeValue(UpgradeConfig conf, float sigin)
		{
			float baseData = conf.baseData;
			float upFactor = conf.upFactor;
			float maxLevel = conf.maxLevel;
			float num = baseData;
			int num2 = 0;
			while ((float)num2 < maxLevel)
			{
				num += baseData * upFactor * sigin;
				int num3 = (int)(num * 100f);
				num = (float)((double)num3 * 1.0) / 100f;
				num2++;
			}
			return num;
		}

		public float GetMaxDamage()
		{
			return this.CaculateUpgradeValue(this.WConf.damageConf, 1f);
		}

		public float GetMaxAccuracy()
		{
			return this.CaculateUpgradeValue(this.WConf.accuracyConf, 1f);
		}

		public float GetMaxFrenquency()
		{
			return this.CaculateUpgradeValue(this.WConf.attackRateConf, -1f);
		}

		public float GetMaxReload()
		{
			return this.CaculateUpgradeValue(this.WConf.reloadConf, -1f);
		}

		public int GetMaxCharger()
		{
			return (int)(this.WConf.chargerConf.baseData + this.WConf.chargerConf.maxLevel * this.WConf.chargerConf.upFactor);
		}

		public bool IsMaxLevelDamage()
		{
			return (float)this.DamageLevel >= this.WConf.damageConf.maxLevel;
		}

		public bool IsMaxLevelCD()
		{
			return (float)this.FrequencyLevel >= this.WConf.attackRateConf.maxLevel;
		}

		public bool IsMaxLevelAccuracy()
		{
			return (float)this.AccuracyLevel >= this.WConf.accuracyConf.maxLevel;
		}

		public bool IsMaxLevelReload()
		{
			return (float)this.ReloadLevel >= this.WConf.reloadConf.maxLevel;
		}

		public bool isMaxLevelCharger()
		{
			return (float)this.ChargerLevel >= this.WConf.chargerConf.maxLevel;
		}

		public int GetDamageUpgradePrice()
		{
			return this.CaculateUpgradePrice(this.WConf.damageConf, this.DamageLevel);
		}

		public int GetFrequencyUpgradePrice()
		{
			return this.CaculateUpgradePrice(this.WConf.attackRateConf, this.FrequencyLevel);
		}

		public int GetAccuracyUpgradePrice()
		{
			return this.CaculateUpgradePrice(this.WConf.accuracyConf, this.AccuracyLevel);
		}

		public int GetReloadUpgradePrice()
		{
			return this.CaculateUpgradePrice(this.WConf.reloadConf, this.ReloadLevel);
		}

		public int GetChargerUpgradePrice()
		{
			return this.CaculateUpgradePrice(this.WConf.chargerConf, this.ChargerLevel);
		}

		private int CaculateUpgradePrice(UpgradeConfig conf, int level)
		{
			return (int)(conf.basePrice * Mathf.Pow(1f + conf.upPriceFactor, (float)level));
		}

		public float GetNextLevelDamage()
		{
			float f = this.damage + this.damage * this.WConf.damageConf.upFactor;
			return ZombieMath.Math.SignificantFigures(f, 4);
		}

		public float GetNextLevelFrequency()
		{
			float f = this.attackFrenquency - this.attackFrenquency * this.WConf.attackRateConf.upFactor;
			return ZombieMath.Math.SignificantFigures(f, 4);
		}

		public float GetNextLevelAccuracy()
		{
			float f = this.accuracy + this.accuracy * this.WConf.accuracyConf.upFactor;
			return ZombieMath.Math.SignificantFigures(f, 4);
		}

		public float GetNextLevelReload()
		{
			float f = this.reload - this.reload * this.WConf.reloadConf.upFactor;
			return ZombieMath.Math.SignificantFigures(f, 4);
		}

		public int GetNextLevelCharger()
		{
			return (int)((float)this.charger + this.WConf.chargerConf.upFactor);
		}

		public float GetLastShootTime()
		{
			return this.lastShootTime;
		}

		public virtual void Init()
		{
			this.gameScene = GameApp.GetInstance().GetGameScene();
			this.rConf = GameApp.GetInstance().GetResourceConfig();
			this.gameCamera = this.gameScene.GetCamera();
			this.cameraComponent = this.gameCamera.MainCamera;
			this.cameraTransform = this.gameCamera.CameraTransform;
			this.player = this.gameScene.GetPlayer();
			this.aimTarget = default(Vector3);
			this.hitParticles = this.rConf.hitparticles;
			this.hitForce = 0f;
			if (this.GetWeaponType() == WeaponType.Sniper)
			{
				this.weaponBoneTrans = this.player.AvatarTransform().Find("BipDummy/Bip002 Pelvis/Bip002 Spine/Bip002 Spine1/Bip002 Spine2/Bip002 Neck/Bip002 L Clavicle/Bip002 L UpperArm/Bip002 L Forearm/Bip002 L Hand/qiang_guadian01");
			}
			else
			{
				this.weaponBoneTrans = this.player.AvatarTransform().Find("BipDummy/Bip002 Pelvis/Bip002 Spine/Bip002 Spine1/Bip002 Spine2/Bip002 Neck/Bip002 R Clavicle/Bip002 R UpperArm/Bip002 R Forearm/Bip002 R Hand/qiang_guadian02");
			}
			this.CreateGun();
			this.gunAtt = this.gun.GetComponent<GunAtt>();
			this.damage /= (float)this.gunAtt.oneShotBullets;
			this.aimSpereRadius = this.gunAtt.aimRadius;
			this.gun.transform.parent = this.weaponBoneTrans;
			this.gun.transform.localPosition = Vector3.zero;
			this.gun.transform.localRotation = Quaternion.Euler(0f, 270f, 0f);
			this.gun.transform.localScale = Vector3.one;
			this.BindGunAndFire();
			this.shootAudio = this.gun.GetComponent<AudioSource>();
			this.GunOff();
			this.SetBullet2UI();
			this.baseOffset = this.gunAtt.originalOffset;
		}

		public virtual void ReduceBulletNum()
		{
			if (!this.NoConsumeBullet)
			{
				this.sbulletCount--;
				this.sbulletCount = Mathf.Clamp(this.sbulletCount, 0, this.maxCapacity);
				this.player.CheckBullet();
				this.SetBullet2UI();
			}
			if (this.sbulletCount == 0)
			{
				if (this.MaxGunload > 0 && this.player.IsFullBodyActionOver)
				{
					this.player.DoReload();
				}
				else if (this.maxGunLoad == 0 && !this.player.BulletEmpty())
				{
					this.player.ChangeWeapon();
				}
				else if (this.maxGunLoad == 0 && this.player.BulletEmpty())
				{
					this.gameScene.SetUIDisplayEvnt(UIDisplayEvnt.NOARMOR, new float[]
					{
						1f
					});
				}
			}
			this.lastShootTime = Time.time;
		}

		public virtual void AddMaxGunLoad(bool set2UI)
		{
			bool flag = this.player.BulletEmpty();
			int num = Mathf.CeilToInt((float)this.maxCapacity * 0.5f);
			this.maxGunLoad += num;
			this.maxGunLoad = Mathf.Clamp(this.maxGunLoad, 0, this.maxBulletNum);
			if (flag)
			{
				this.gameScene.SetUIDisplayEvnt(UIDisplayEvnt.NOARMOR, new float[1]);
			}
			this.gameScene.SetUIDisplayEvnt(UIDisplayEvnt.PLAYER_GET_BULLET, new float[]
			{
				(float)this.id,
				(float)num
			});
			if (set2UI)
			{
				this.SetBullet2UI();
			}
		}

		public virtual void AddMaxGunLoad(float percent, bool set2UI)
		{
			this.maxGunLoad += Mathf.CeilToInt((float)this.maxBulletNum * percent);
			this.maxGunLoad = Mathf.Clamp(this.maxGunLoad, 0, this.maxBulletNum);
			if (set2UI)
			{
				this.SetBullet2UI();
			}
		}

		public virtual void ReloadBullet()
		{
			int num = this.maxGunLoad;
			this.maxGunLoad -= this.maxCapacity - this.sbulletCount;
			this.maxGunLoad = Mathf.Clamp(this.maxGunLoad, 0, this.maxGunLoad);
			this.sbulletCount += num - this.maxGunLoad;
			this.SetBullet2UI();
		}

		public virtual void ReloadBulletOnce()
		{
			int num = (this.maxGunLoad <= 1) ? 1 : 2;
			if (num == 2)
			{
				num = ((this.maxCapacity - this.sbulletCount < 2) ? 1 : 2);
			}
			this.maxGunLoad -= num;
			this.sbulletCount += num;
			this.gameScene.SetReloadProgressPercent(this.sbulletCount, this.maxCapacity);
			this.SetBullet2UI();
			Singleton<GameAudioManager>.Instance.PlayGlobalEffect(GlobalEffect.AddBullet);
		}

		public bool IsBulletFull()
		{
			return this.sbulletCount == this.maxCapacity && this.maxGunLoad == this.maxBulletNum;
		}

		public bool IsCapacityFull()
		{
			return this.sbulletCount == this.maxCapacity;
		}

		public bool IsMaxGunLoadEmpty()
		{
			return this.maxGunLoad == 0;
		}

		public bool IsBulletEmpty()
		{
			return this.sbulletCount == 0 && this.maxGunLoad == 0;
		}

		public bool AddBullet2Total(int num)
		{
			bool flag = this.player.BulletEmpty();
			int num2 = this.maxCapacity - this.sbulletCount;
			int num3 = (num <= num2) ? num : num2;
			this.sbulletCount += num3;
			this.maxGunLoad += num - num3;
			this.maxGunLoad = Mathf.Clamp(this.maxGunLoad, 0, this.maxBulletNum);
			this.SetBullet2UI();
			if (flag && num > 0)
			{
				this.gameScene.SetUIDisplayEvnt(UIDisplayEvnt.NOARMOR, new float[1]);
			}
			return this.IsBulletFull();
		}

		public void FillCapacity()
		{
			this.sbulletCount = this.maxCapacity;
			this.maxGunLoad = this.maxBulletNum;
			this.SetBullet2UI();
		}

		public bool ReloadOneByOne()
		{
			bool result = false;
			string name = this.Name;
			if (name != null)
			{
				if (name == "SPAS12" || name == "KSG" || name == "SPAS12L" || name == "FN" || name == "UTS15")
				{
					result = true;
				}
			}
			return result;
		}

		public void OnPreReload()
		{
			this.gameScene.SetReloadProgressPercent(this.sbulletCount, this.maxCapacity);
			this.gameScene.SetReloadProgressEnable(true, false);
		}

		public virtual int GetMaxReloadTimes()
		{
			int num = this.maxCapacity - this.sbulletCount;
			int num2 = (this.maxGunLoad < num) ? this.maxGunLoad : num;
			return Mathf.CeilToInt((float)num2 / 2f);
		}

		public void SetBullet2UI()
		{
			if (!this.IsSelected)
			{
				return;
			}
			this.gameScene.SetUIDisplayEvnt(UIDisplayEvnt.BULLET_COUNT, new float[]
			{
				(float)this.sbulletCount,
				(float)this.maxCapacity,
				(float)this.maxGunLoad
			});
		}

		public virtual bool DoCheckHit(RaycastHit hit, bool critical, Ray ray, int index = 0)
		{
			if (hit.collider != null)
			{
				this.aimTarget = hit.point;
				Vector3 vector = this.player.GetTransform().InverseTransformPoint(this.aimTarget);
				GameObject gameObject = hit.collider.gameObject;
				if (this.CheckIsEnemyLayer(gameObject.layer) && gameObject.name.StartsWith("E_"))
				{
					string[] array = gameObject.name.Split(new char[]
					{
						'|'
					});
					Enemy enemyByID = this.gameScene.GetEnemyByID(array[0]);
					if (enemyByID.GetState() == Enemy.DEAD_STATE)
					{
						return false;
					}
					Bone bone = (Bone)Enum.Parse(typeof(Bone), array[1]);
					float num = this.GetDamage(critical, gameObject.layer == 27, bone == Bone.Head);
					float max = num;
					num -= num * 0.15f * (float)index;
					num = Mathf.Clamp(num, 0f, max);
					DamageProperty dp = new DamageProperty(num, this.gunAtt.hitForce, ray.direction.normalized);
					enemyByID.OnHit(dp, this.GetWeaponType(), hit.point, bone);
					return true;
				}
				else
				{
					if (gameObject.layer == 19)
					{
						OilDrum component = gameObject.GetComponent<OilDrum>();
						component.OnHit(new DamageProperty(this.damage, Vector3.zero), this.GetWeaponType(), Vector3.zero, Bone.None);
						this.DoHitWall(hit.point, -ray.direction);
						return true;
					}
					if (vector.z > 2f)
					{
						this.DoHitWall(hit.point, -ray.direction);
						return false;
					}
				}
			}
			return false;
		}

		public virtual void DoHitWall(Vector3 pos, Vector3 dir)
		{
			Singleton<GameAudioManager>.Instance.PlayGlobalEffect(GlobalEffect.Hitwall);
		}

		private void DoHitWoodBox(GameObject hitObject)
		{
			BossOilDrum component = hitObject.GetComponent<BossOilDrum>();
			if (component == null)
			{
				return;
			}
			component.OnHit(new DamageProperty(this.GetDamage(false, false, false)), this.GetWeaponType(), Vector3.zero, Bone.None);
		}

		public virtual void PlayShootAudio()
		{
			Singleton<GameAudioManager>.Instance.PlaySoundInGame(this.shootAudio.clip, false);
		}

		public virtual bool CheckIsEnemyLayer(int layer)
		{
			return layer == 9 || layer == 27;
		}

		public virtual AimState HasAimed()
		{
			Ray ray = new Ray(this.cameraTransform.position, this.cameraTransform.forward);
			return this.CheckRaycast(ray);
		}

		private AimState CheckRaycast(Ray ray)
		{
			if (this.sbulletCount == 0)
			{
				return AimState.Miss;
			}
			RaycastHit raycastHit;
			bool flag = Physics.SphereCast(ray, this.aimSpereRadius, out raycastHit, 100f, 134777344);
			if (flag)
			{
				int layer = raycastHit.collider.gameObject.layer;
				if ((layer == 9 || layer == 27) && raycastHit.collider.gameObject.name.StartsWith("E_"))
				{
					string enemyID = raycastHit.collider.gameObject.name.Split(new char[]
					{
						'|'
					})[0];
					Enemy enemyByID = this.gameScene.GetEnemyByID(enemyID);
					if (enemyByID.GetState() != Enemy.DEAD_STATE)
					{
						return ((enemyByID.GetPosition() - this.player.GetTransform().position).sqrMagnitude >= this.range * this.range) ? AimState.OutRange : AimState.Aimed;
					}
				}
				else if (layer == 19)
				{
					return AimState.Aimed;
				}
			}
			return AimState.Miss;
		}

		public virtual void CreateGun()
		{
			this.gun = WeaponFactory.GetInstance().CreateWeaponModel(this.Name);
			this.SetLayerRecursively(this.gun.transform, 8);
		}

		public void SetLayerRecursively(Transform root, int _layer)
		{
			root.gameObject.layer = _layer;
			for (int i = 0; i < root.childCount; i++)
			{
				Transform child = root.GetChild(i);
				this.SetLayerRecursively(child, _layer);
			}
		}

		public virtual void FireUpdate(float deltaTime)
		{
		}

		public virtual void AutoAim(float deltaTime)
		{
		}

		public void BindGunAndFire()
		{
			this.rightGun = this.gun.transform;
		}

		public virtual bool HaveBullets()
		{
			if (this.BulletCount == 0)
			{
				this.StopFire();
				return false;
			}
			if (this.IsReload)
			{
				this.StopFire();
				return false;
			}
			return true;
		}

		public virtual bool CouldMakeNextShoot()
		{
			return Time.time - this.lastShootTime > this.WConf.attackRateConf.baseData;
		}

		public virtual void GunOff()
		{
			this.StopFire();
			this.gun.SetActive(false);
			if (this.GetWeaponType() != WeaponType.Sniper)
			{
				this.IsSelected = false;
			}
		}

		public virtual void GunOn()
		{
			this.IsSelected = true;
			this.gameScene.GameUI.SetUIPlayerWeapon(this.Name);
			this.gun.SetActive(true);
			this.SetBullet2UI();
		}

		public virtual void SetFirePointInfo(Vector3 pos, Quaternion rotate)
		{
			this.firePointPosition = pos;
			this.firePointRotation = rotate;
		}

		public int Capacity
		{
			get
			{
				return this.capacity;
			}
		}

		public Vector2 Deflection
		{
			get
			{
				return this.deflection;
			}
		}

		public int GunSpriteIndex
		{
			get
			{
				return 0;
			}
		}

		public int GetGunIndex()
		{
			FieldInfo field = typeof(GunIndex).GetField(this.Name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			object value = field.GetValue(null);
			return int.Parse(value.ToString());
		}

		public bool SnipeHasHandle()
		{
			return this.Name == "AWP" || this.Name == "SR02" || this.Name == "MSR";
		}

		protected GameObject hitParticles;

		protected GameObject projectile;

		protected Camera cameraComponent;

		protected Transform cameraTransform;

		protected Transform rightGun;

		protected BaseCameraScript gameCamera;

		protected GameObject gunfire;

		protected GameObject gun;

		protected Transform weaponBoneTrans;

		protected ResourceConfigScript rConf;

		protected AudioSource shootAudio;

		protected RaycastHit hit = default(RaycastHit);

		protected RaycastHit[] hits;

		protected GameScene gameScene;

		protected Player player;

		protected Vector3 aimTarget;

		protected float hitForce;

		protected float range;

		protected float lastShootTime;

		protected int id;

		protected int maxCapacity;

		protected int maxGunLoad;

		protected int capacity;

		protected float maxDeflection;

		protected Vector2 deflection;

		protected float damage;

		protected float attackFrenquency;

		protected float accuracy;

		protected float reload;

		protected int charger;

		protected int sbulletCount;

		protected float speedDrag;

		protected Vector3 lastHitPosition;

		protected int price;

		protected GunAtt gunAtt;

		protected float aimSpereRadius;

		protected float bulletAcrossProbability;

		protected float noConsumeBulletProbability;

		protected float damage2EliteEnemy;

		protected float damage2NormalEnemy;

		protected float damageHeadShot;

		protected float criticalProbability;

		protected float bulletLimit;

		protected int maxBulletNum;

		public ParticleSystem m_GunFireParticle;

		public float gunfire_time = -1f;

		public bool isPlayGunFire;

		public Transform firePointTransform;

		public Vector3 firePointPosition;

		public Quaternion firePointRotation;

		public bool IsReload;

		public float fireArea;

		protected float curScaleRatio = 1f;

		protected float baseOffset;

		protected int snipeScopeIndex = 8;

		protected float curScopePercent;

		protected bool continuousShoot;
	}
}
