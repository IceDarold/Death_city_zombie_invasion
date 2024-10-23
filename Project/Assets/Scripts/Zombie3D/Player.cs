using System;
using System.Collections.Generic;
using DataCenter;
using UnityEngine;
using UnityEngine.AI;
using ZombieMath;

namespace Zombie3D
{
	public class Player : EnemyTarget, IPlayerControl
	{
		public WayPointScript NearestWayPoint { get; set; }

		public Vector3 HitPoint { get; set; }

		public PlayerIK IK
		{
			get
			{
				return this.playerIK;
			}
		}

		public float MaxWeaponFightPower { get; set; }

		public AimState AimState
		{
			get
			{
				return this.playerAimState;
			}
		}

		public float BuffLastTime
		{
			get
			{
				return this.buffLastTime;
			}
		}

		public float WalkSpeed
		{
			get
			{
				return this.nav.speed;
			}
		}

		public InputController InputController
		{
			get
			{
				return this.inputController;
			}
		}

		public bool IsRunning
		{
			get
			{
				return this.isRunning;
			}
		}

		public bool IsFullBodyActionOver
		{
			get
			{
				return this.isFullBodyActionOver;
			}
		}

		public List<Weapon> WeaponList
		{
			get
			{
				return this.weaponList;
			}
		}

		public ArmorBox TriggeredArmorBox
		{
			get
			{
				return this.triggeredArmorBox;
			}
			set
			{
				this.triggeredArmorBox = value;
			}
		}

		public void RandomSawAnimation()
		{
			if (ZombieMath.Math.RandomRate(50f))
			{
				this.weaponNameEnd = "_Saw";
			}
			else
			{
				this.weaponNameEnd = "_Saw2";
			}
		}

		public string WeaponNameEnd
		{
			get
			{
				return this.weaponNameEnd;
			}
			set
			{
				this.weaponNameEnd = value;
			}
		}

		public void Move(Vector3 motion)
		{
			if (this.IsVisible())
			{
				this.nav.Move(motion);
			}
		}

		public GameObject PlayerObject
		{
			get
			{
				return this.playerObject;
			}
		}

		public float PowerBuff
		{
			get
			{
				return this.powerBuff;
			}
		}

		public float GetGuiHp()
		{
			return this.guiHp;
		}

		public float GetHp()
		{
			return this.hp;
		}

		public float GetMaxHp()
		{
			return this.maxHp;
		}

		public Transform GetTransform()
		{
			return this.playerTransform;
		}

		public bool IsVisible()
		{
			return this.playerMesh.enabled;
		}

		public void WrapToPosition(Transform trans)
		{
			this.nav.Warp(trans.position);
			this.playerTransform.rotation = trans.rotation;
		}

		public EnemyTargetType GetTargetType()
		{
			return EnemyTargetType.PLAYER;
		}

		public void SetVisible(bool visible)
		{
			this.playerMesh.enabled = visible;
			this.nav.enabled = visible;
			this.playerIK.pickUpCollider.gameObject.SetActive(visible);
			if (visible)
			{
				this.weapon.GunOn();
			}
			else
			{
				this.weapon.GunOff();
			}
		}

		public PlayerIK GetPlayerIK()
		{
			return this.playerIK;
		}

		public void SetActive(bool _active)
		{
			this.playerObject.SetActive(_active);
		}

		public Transform AvatarTransform()
		{
			return this.avatarTransform;
		}

		public Collider GetCollider()
		{
			return this.collider;
		}

		public Transform GetRespawnTransform()
		{
			return this.respawnTrans;
		}

		public void SetPlayerPosition()
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("Respawn");
			int num = UnityEngine.Random.Range(0, array.Length);
			GameObject gameObject = array[num];
			this.respawnTrans = gameObject.transform;
			this.playerObject.transform.position = gameObject.transform.position;
			this.playerObject.transform.rotation = gameObject.transform.rotation;
		}

		public void Init(AvatarType _avatarType, PlayerPose _playerPose, Transform spPoint)
		{
			this.gameScene = GameApp.GetInstance().GetGameScene();
			this.avatarType = _avatarType;
			this.playerPose = _playerPose;
			this.playerObject = AvatarFactory.GetInstance().CreateAvatar(this.avatarType);
			this.playerObject.transform.position = spPoint.position;
			this.playerObject.transform.rotation = spPoint.rotation;
			this.playerObject.name = "Player";
			this.playerTransform = this.playerObject.transform;
			this.avatarTransform = this.playerTransform;
			this.playerMesh = this.playerObject.GetComponentInChildren<SkinnedMeshRenderer>();
			this.playerIK = this.playerObject.GetComponent<PlayerIK>();
			this.playerIK.SetPlayer(this);
			this.playerIK.aimIK.solver.target = GameApp.GetInstance().GetGameScene().GetCamera().playerAimTarget;
			this.aimDuration = this.playerIK.aimDuration;
			this.nav = this.playerObject.GetComponent<NavMeshAgent>();
			this.nav.enabled = true;
			this.SetSpeed(this.playerIK.normalSpeed, this.playerIK.aimSpeed);
			this.hp = 40f;
			this.maxHp = this.hp;
			this.gameScene.SetUIDisplayEvnt(UIDisplayEvnt.PLAYER_HP, new float[]
			{
				this.hp,
				this.maxHp
			});
			this.gameCamera = GameApp.GetInstance().GetGameScene().GetCamera();
			this.animator = this.playerObject.GetComponent<Animator>();
			this.playerObject.GetComponent<PlayerAnimatorEventListener>().SetPlayer(this);
			this.collider = this.playerObject.GetComponent<Collider>();
			this.audioPlayer = new AudioPlayer();
			Transform folderTrans = this.playerTransform.Find("Audio");
			this.audioPlayer.AddAudio(folderTrans, "Dead");
			this.audioPlayer.AddAudio(folderTrans, "Walk");
			this.audioPlayer.AddAudio(folderTrans, "AimWalk");
			this.audioPlayer.AddAudio(folderTrans, "HeartBeat");
			this.audioPlayer.AddAudio(folderTrans, "Hurt");
			this.audioPlayer.AddAudio(folderTrans, "Critical");
			this.audioPlayer.AddAudio(folderTrans, "QTE");
			this.audioPlayer.AddAudio(folderTrans, "Revive");
			this.playerState = ((this.playerPose != PlayerPose.FREE) ? Player.SNIPE_STATE : Player.IDLE_STATE);
			this.InitWeapons();
			this.inputController = new TPSInputController();
			this.inputController.Init();
			this.playerIK.SetPlayerGameInfo(this.avatarType, this.playerPose);
			this.playerIK.SetPickUpRadius(1f + TalentDataManager.GetTalentValue(Talent.PICKUP_RANGE));
			this.LoadEquimentData();
		}

		public void SetSpeed(float _normalSpeed, float _aimSpeed)
		{
			this.normalSpeed = _normalSpeed;
			this.aimSpeed = _aimSpeed * (1f + TalentDataManager.GetTalentValue(Talent.AIM_MOVESPEED));
		}

		private void LoadEquimentData()
		{
			PlayerAttributeData equipmentAttribute = PlayerDataManager.GetEquipmentAttribute();
			this.hp += equipmentAttribute.attrHP;
			this.maxHp = this.hp;
			this.normalSpeed *= 1f + equipmentAttribute.moveSpeed;
			this.attackPower += equipmentAttribute.attack;
			this.dodgeProbability = equipmentAttribute.dodge;
			this.remoteDefense = equipmentAttribute.remoteDefense;
			this.bombDefense = equipmentAttribute.bombDefense;
		}

		private void InitSkills()
		{
		}

		private void InitWeapons()
		{
			this.weaponList = new List<Weapon>();
			WeaponData[] array = new WeaponData[2];
			bool flag = CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.WEAPON;
			array[0] = ((!flag) ? WeaponDataManager.GetWeaponData(PlayerDataManager.Player.Weapon1) : WeaponDataManager.GetTryWeapon());
			array[1] = ((!flag) ? WeaponDataManager.GetWeaponData(PlayerDataManager.Player.Weapon2) : null);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null)
				{
					Weapon weapon = WeaponFactory.GetInstance().CreateWeapon(array[i].Prefab);
					weapon.Name = array[i].Prefab;
					weapon.SetWeaponData(array[i]);
					weapon.IsSelectedForBattle = true;
					weapon.FightPower = (float)WeaponDataManager.GetCurrentFightingStrength(array[i]);
					this.weaponList.Add(weapon);
					this.weaponList[i].Init();
				}
			}
			if (this.gameScene.PlayingMode == GamePlayingMode.SnipeMode)
			{
				for (int j = 0; j < this.weaponList.Count; j++)
				{
					if (this.weaponList[j].GetWeaponType() == WeaponType.Sniper)
					{
						this.ChangeWeapon(this.weaponList[j]);
					}
				}
			}
			else
			{
				this.ChangeWeapon(this.weaponList[(!flag) ? Singleton<GlobalData>.Instance.InGameWeaponIndex : 0]);
			}
			this.SetMaxWeaponFightPower();
		}

		public void SetMaxWeaponFightPower()
		{
			float power = 0f;
			this.weaponList.ForEach(delegate(Weapon temp)
			{
				power = ((temp.FightPower <= power) ? power : temp.FightPower);
			});
			this.MaxWeaponFightPower = power;
		}

		public void LerpNavMeshAgentSpeed(float target, float deltaTime)
		{
			this.nav.speed = Mathf.Lerp(this.nav.speed, target, deltaTime * 5f);
		}

		public bool BulletEmpty()
		{
			return this.weaponList[0].IsBulletEmpty() && (this.weaponList.Count < 2 || this.weaponList[1] == null || this.weaponList[1].IsBulletEmpty());
		}

		public AvatarType GetAvatarType()
		{
			return this.avatarType;
		}

		public void SetState(PlayerState state)
		{
			if (this.playerState != null)
			{
				this.playerState.OnExit(this);
			}
			this.playerState = state;
			this.playerState.OnEnter(this);
			if (state == Player.AIM_STATE)
			{
				this.aimCount = this.aimDuration;
			}
		}

		public bool CanChangeToIdle(float deltaTime)
		{
			this.aimCount -= deltaTime;
			return this.aimCount <= 0f;
		}

		public PlayerState GetState()
		{
			return this.playerState;
		}

		public void SetPlayerAnimatorParameter(bool _isMoving, bool isShooting, bool forceIdle = false)
		{
			this.isShoot = isShooting;
			if (isShooting || forceIdle)
			{
				this.isAim = isShooting;
			}
			this.playerIK.SetPlayerAnimator(_isMoving, isShooting, forceIdle);
			this.isMoving = _isMoving;
		}

		public void SetIsAim(bool _isAim)
		{
			this.isAim = _isAim;
			this.playerIK.SetIsAim(_isAim);
		}

		public void SetPlayerMoveDirection(float directionX, float directionZ)
		{
			this.targetDirection.x = directionX;
			this.targetDirection.y = directionZ;
			this.targetDirection = this.targetDirection.normalized;
		}

		public void SetPlayerIdle(bool forceIdle = false)
		{
			this.SetPlayerAnimatorParameter(false, false, forceIdle);
			this.SetPlayerMoveDirection(0f, 0f);
		}

		private void SetMoveDir2Animator()
		{
			this.animator.SetFloat("X", this.curDirection.x);
			this.animator.SetFloat("Z", this.curDirection.y);
		}

		public void SetShootAniSpeed(bool isNormal)
		{
		}

		public void SetChangeGun(WeaponType type)
		{
			this.animator.SetInteger("WeaponType", (int)type);
			this.animator.SetTrigger("ChangeGun");
		}

		public void SetCharge()
		{
			this.animator.SetTrigger("Charge");
		}

		public void SetIsInCharge(bool isInCharge)
		{
			if (this.SetIsInChargeCallBack != null)
			{
				this.SetIsInChargeCallBack(isInCharge);
			}
		}

		public void SetIsChangeGun(bool isChangeGun)
		{
			this.IsChangeGun = isChangeGun;
		}

		public void SetChargePercent(float percent)
		{
			if (this.SetChargeSliderPercentCallBack != null)
			{
				this.SetChargeSliderPercentCallBack(percent);
			}
		}

		public bool IsChangeGun { get; set; }

		public bool IsShooting
		{
			get
			{
				return this.isShoot;
			}
		}

		public bool isAiming
		{
			get
			{
				return this.isAim;
			}
		}

		public bool IsMoving
		{
			get
			{
				return this.isMoving;
			}
		}

		public void ZoomIn(float deltaTime)
		{
		}

		public void ZoomOut(float deltaTime)
		{
		}

		public void AutoAim(float deltaTime)
		{
			this.weapon.AutoAim(deltaTime);
		}

		public void Fire(float deltaTime)
		{
			if (this.gameScene.PlayingState == PlayingState.GamePlaying || this.gameScene.PlayingState == PlayingState.WaitForEnd)
			{
				this.weapon.Fire(deltaTime);
			}
		}

		public void CheckBullet()
		{
			if (this.OnShootCallBack != null)
			{
				this.OnShootCallBack();
			}
		}

		public void OnGunOffOver()
		{
			this.playerIK.SetGunOn();
			if (this.weaponList.FindIndex((Weapon wp) => wp.Name == this.weapon.Name) == 0)
			{
				this.ChangeWeapon(this.weaponList[1]);
			}
			else
			{
				this.ChangeWeapon(this.weaponList[0]);
			}
		}

		public void OnGunOnOver()
		{
			if (this.animator.GetBool("isAim"))
			{
				this.SetState(Player.AIM_STATE);
			}
			else
			{
				this.SetState(Player.IDLE_STATE);
			}
			this.isFullBodyActionOver = true;
		}

		public void OnFullbodyActionOver()
		{
			this.isFullBodyActionOver = true;
			this.weapon.GunOn();
			if (this.weapon.BulletCount == 0 && !this.weapon.IsBulletEmpty() && this.playerState != Player.CHARGER_STATE)
			{
				this.DoReload();
			}
			else if (this.animator.GetBool("isAim"))
			{
				this.SetState(Player.AIM_STATE);
				this.playerIK.SetIKWeight(1f);
				this.playerIK.upperBodyLockedWeight = 1f;
			}
			else
			{
				this.SetState(Player.IDLE_STATE);
			}
			if (this.gameScene.GetCamera().fMode == FollowMode.Animation)
			{
				this.gameScene.GetCamera().SetFollowPlayer();
				this.playerIK.CamerQTEOver();
			}
		}

		public void OnChargeOver()
		{
			if (this.RefreshWeaponBullet != null)
			{
				this.RefreshWeaponBullet();
			}
			if (this.animator.GetBool("isAim"))
			{
				this.SetState(Player.AIM_STATE);
			}
			else
			{
				this.SetState(Player.IDLE_STATE);
			}
			if (!this.weapon.ReloadOneByOne())
			{
				this.weapon.ReloadBullet();
			}
			else
			{
				this.gameScene.SetReloadProgressEnable(false, false);
			}
		}

		public void DoChangeGun()
		{
			this.weapon.changeReticle();
			this.weapon.GunOn();
			this.gameCamera.isAngelVFixed = false;
		}

		public void StopFire()
		{
			this.weapon.StopFire();
		}

		public void SetBoneRotation(float angle)
		{
		}

		public void DoLogic(float deltaTime)
		{
			this.playerState.NextState(this, deltaTime);
			this.curDirection = Vector2.Lerp(this.curDirection, this.targetDirection, 20f * deltaTime);
			if (this.playerState == Player.AIM_STATE || this.playerState == Player.SHOOT_STATE || this.playerState == Player.RUNSHOOT_STATE)
			{
				this.LerpNavMeshAgentSpeed(this.aimSpeed, deltaTime);
			}
			else
			{
				this.LerpNavMeshAgentSpeed(this.normalSpeed, deltaTime);
			}
			this.SetMoveDir2Animator();
			foreach (Weapon weapon in this.weaponList)
			{
				if (weapon.IsSelectedForBattle)
				{
					weapon.DoLogic(deltaTime);
				}
			}
			if (this.hp < this.maxHp * 0.4f)
			{
				this.audioPlayer.PlayAudio("HeartBeat");
			}
			if (this.triggeredArmorBox != null)
			{
				this.CheckRaycast2Armorbox(deltaTime);
			}
			if (this.invincibleTime > 0f)
			{
				this.invincibleTime -= deltaTime;
			}
		}

		public float HP
		{
			get
			{
				return this.hp;
			}
		}

		public void DoReloadOnce()
		{
			this.playerIK.ReloadOnce();
			this.weapon.ReloadBulletOnce();
		}

		public void CheckRaycast2Armorbox(float dt)
		{
			bool flag = Physics.Raycast(this.playerTransform.position + Vector3.up, this.playerTransform.forward, 1f, 1048576);
			this.triggeredArmorBox.SetFocus(flag);
			if (flag)
			{
				this.triggeredArmorBox.DoFill(dt);
			}
		}

		public void DoReload()
		{
			if (this.playerState == Player.CHARGER_STATE)
			{
				return;
			}
			if (this.weapon.IsMaxGunLoadEmpty() || this.weapon.IsCapacityFull())
			{
				return;
			}
			bool flag = UnityEngine.Random.Range(0f, 1f) < TalentDataManager.GetTalentValue(Talent.FLASH_RELOAD_PROBABILITY);
			if (flag)
			{
				GameApp.GetInstance().GetGameScene().SetUIDisplayEvnt(UIDisplayEvnt.FLASH_RELOAD, new float[0]);
				this.weapon.ReloadBullet();
			}
			else
			{
				if (this.weapon.GunAtt.reloadOneByOne)
				{
					this.playerIK.SetReloadTimes(this.weapon.GetMaxReloadTimes());
				}
				this.SetState(Player.CHARGER_STATE);
				this.playerIK.upperBodyLockedWeight = 1f;
				this.playerIK.DoReload();
			}
			if (!flag && this.weapon.ReloadOneByOne())
			{
				this.weapon.OnPreReload();
			}
			if (this.gameScene.PlayingMode == GamePlayingMode.SnipeMode)
			{
				InGamePage inGamePage = Singleton<UiManager>.Instance.GetPage(PageName.InGamePage) as InGamePage;
				inGamePage.snipeUI.snipeScopeUI.DoReloadLogic();
			}
		}

		public bool DoQTE()
		{
			if (!this.isFullBodyActionOver)
			{
				return false;
			}
			this.playerIK.OnQTE();
			this.DoFullBodyState(false);
			this.weapon.GunOff();
			this.gameScene.GetCamera().SetFollowAnimation();
			this.audioPlayer.PlayAudio("QTE");
			return true;
		}

		public void PlayAudio(string audioName)
		{
			this.audioPlayer.PlayAudio(audioName);
		}

		public void StopAudio(string audioName)
		{
			this.audioPlayer.StopAudio(audioName);
		}

		public void AddHp(float _add)
		{
			this.hp += _add;
			this.hp = Mathf.Clamp(this.hp, 0f, this.maxHp);
			this.gameScene.SetUIDisplayEvnt(UIDisplayEvnt.PLAYER_HP, new float[]
			{
				this.hp,
				this.maxHp
			});
			this.playerIK.effectHp.Play();
			this.audioPlayer.PlayAudio("Medkit");
		}

		public void AddBullet()
		{
			for (int i = 0; i < this.weaponList.Count; i++)
			{
				this.weaponList[i].AddMaxGunLoad(this.weapon == this.weaponList[i]);
			}
		}

		public void AddBullet(float percent)
		{
			for (int i = 0; i < this.weaponList.Count; i++)
			{
				this.weaponList[i].AddMaxGunLoad(percent, this.weapon == this.weaponList[i]);
			}
		}

		public bool DoSkill(int skillID)
		{
			bool flag = this.isFullBodyActionOver;
			this.curUsedPropData = PropDataManager.GetPropData(skillID);
			switch (this.curUsedPropData.Type)
			{
			case PropType.MEDKIT:
				Singleton<GamePropManager>.Instance.UseProp(this.curUsedPropData.ID, this.playerIK.throwPoint.position, this.playerIK.throwPoint.forward);
				flag = true;
				break;
			case PropType.ADRENALINE:
				this.gameScene.DoBulletTime(10f);
				this.audioPlayer.PlayAudio("Adrenaline");
				this.audioPlayer.PlayAudio("Adrenaline_loop");
				flag = true;
				break;
			case PropType.LANDMINE:
			case PropType.TURRET:
				if (flag)
				{
					this.playerIK.OnSkill(SkillAction.ACTION_CROUCH);
					this.DoFullBodyState(false);
				}
				break;
			case PropType.GRENADE:
			case PropType.SNEER_BOMB:
				if (flag)
				{
					this.playerIK.OnSkill(SkillAction.ACTION_THROW);
					this.DoFullBodyState(false);
				}
				break;
			default:
				UnityEngine.Debug.LogError("Invalid PropType When Use Prop!");
				break;
			}
			return flag;
		}

		private void DoFullBodyState(bool forceIdle = false)
		{
			this.isFullBodyActionOver = false;
			this.SetState(Player.FULLBODY_ACTION);
			this.SetPlayerIdle(forceIdle);
			this.inputController.CameraRotation = Vector2.zero;
			this.playerIK.CancelCurrentAction();
			this.playerIK.SetIKWeight(0f);
			this.playerIK.upperBodyLockedWeight = 0f;
		}

		public void OnSpecialSkillKeyFrame()
		{
			switch (this.curUsedPropData.Type)
			{
			case PropType.LANDMINE:
				Singleton<GamePropManager>.Instance.UseProp(this.curUsedPropData.ID, this.playerTransform.position + this.playerTransform.forward * 0.6f, this.playerTransform.rotation.eulerAngles);
				break;
			case PropType.GRENADE:
			case PropType.SNEER_BOMB:
				Singleton<GamePropManager>.Instance.UseProp(this.curUsedPropData.ID, this.playerIK.throwPoint.position, this.playerIK.throwPoint.forward);
				break;
			case PropType.TURRET:
			{
				GameProp gameProp = Singleton<GamePropManager>.Instance.UseProp(this.curUsedPropData.ID, this.playerTransform.position + this.playerTransform.forward * 0.6f, this.playerTransform.rotation.eulerAngles);
				break;
			}
			}
		}

		public void ChangeGun()
		{
			this.SetState(Player.CHANGEGUN_STATE);
			this.playerIK.DoGunOff();
		}

		public void OnHit(float damage, bool isCritical, AttackType type)
		{
			if (this.gameScene.PlayingState != PlayingState.GamePlaying)
			{
				return;
			}
			if (!this.IsVisible())
			{
				return;
			}
			if (this.invincibleTime > 0f)
			{
				return;
			}
			if (type == AttackType.NORMAL)
			{
				bool flag = UnityEngine.Random.Range(0f, 1f) < this.dodgeProbability;
				if (flag)
				{
					this.gameScene.SetUIDisplayEvnt(UIDisplayEvnt.PLAYER_DODDGE, new float[0]);
					return;
				}
			}
			else if (type == AttackType.REMOTE)
			{
				damage *= 1f - this.remoteDefense;
			}
			else if (type == AttackType.BOMB)
			{
				damage *= 1f - this.bombDefense;
			}
			this.hp -= damage;
			this.hp = (float)((int)this.hp);
			this.hp = Mathf.Clamp(this.hp, 0f, this.maxHp);
			this.playerState.OnHit(this, damage, isCritical);
			this.gameScene.ShakeMainCamera(0.6f, 0.35f);
			if (isCritical)
			{
				this.audioPlayer.PlayAudio("Critical");
			}
			else
			{
				this.audioPlayer.PlayAudio("Hurt");
			}
			this.gameScene.SetUIDisplayEvnt(UIDisplayEvnt.PLAYER_HP, new float[]
			{
				this.hp,
				this.maxHp
			});
			this.gameScene.SetUIDisplayEvnt(UIDisplayEvnt.PLAYER_ONHIT, new float[0]);
		}

		public void DoHurt()
		{
			this.playerIK.OnHurt();
			this.DoFullBodyState(true);
		}

		public void DoDead()
		{
			this.playerIK.OnDead();
			this.DoFullBodyState(true);
		}

		public void DoRevive()
		{
			if (this.hp > 0f)
			{
				this.isFullBodyActionOver = true;
			}
			else
			{
				this.playerIK.OnRebirth();
				this.isFullBodyActionOver = false;
			}
			this.GetFullyHealed();
			this.invincibleTime = 6f;
			this.gameScene.SetUIDisplayEvnt(UIDisplayEvnt.PLAYER_HP, new float[]
			{
				this.hp,
				this.maxHp
			});
			this.audioPlayer.PlayAudio("Revive");
			this.weapon.FillCapacity();
		}

		public void DoPause()
		{
		}

		public void DoResume()
		{
		}

		public bool CouldGetAnotherHit()
		{
			return this.playerState != Player.CHARGER_STATE && this.isFullBodyActionOver;
		}

		public void OnDead()
		{
			this.audioPlayer.PlayAudio("Dead");
			this.weapon.StopFire();
			Transform transform = this.gameCamera.gameObject.transform.Find("Screen_Blood_Dead");
			if (transform != null)
			{
				transform.gameObject.active = true;
			}
			GameScene gameScene = GameApp.GetInstance().GetGameScene();
			gameScene.PlayingState = PlayingState.GameLose;
			this.DoDead();
			GameApp.GetInstance().GetGameScene().DoGameResult(false);
		}

		public void GetHealed(int point)
		{
			Transform transform = this.gameCamera.gameObject.transform.Find("Screen_Blood_Dead");
			if (transform != null)
			{
				transform.gameObject.SetActive(false);
			}
			this.hp += (float)point;
			this.hp = Mathf.Clamp(this.hp, 0f, this.maxHp);
		}

		public void GetFullyHealed()
		{
			this.hp = this.maxHp;
		}

		public AimState GetWeaponAimed()
		{
			if (this.playerState == Player.FULLBODY_ACTION)
			{
				this.playerAimState = AimState.Miss;
			}
			this.playerAimState = this.weapon.HasAimed();
			return this.playerAimState;
		}

		public bool HasShootedEnemy
		{
			get
			{
				return this.shootedEnemy;
			}
			set
			{
				this.shootedEnemy = value;
			}
		}

		public void ChangeWeapon(Weapon w)
		{
			if (w.IsSelectedForBattle)
			{
				if (this.weapon != null)
				{
					this.weapon.GunOff();
				}
				this.weapon = w;
				this.playerIK.SetGun(this.weapon.GunObject.transform, this.weapon);
				this.gameScene.SetGunShootShakeInfo(this.weapon.GunAtt.shootShakeInfo, this.weapon.GunAtt.stability, this.weapon.GetWeaponType());
				this.gameScene.GameUI.SetPlayerEquipedWeaponData(this.weapon);
				this.weapon.GunOn();
			}
		}

		public void OnPickUp(global::ItemType itemID)
		{
		}

		public void DoSnipeCameraAction()
		{
			this.playerIK.DoSnipeCameraAction(this.playerPose);
		}

		public Weapon GetWeapon()
		{
			return this.weapon;
		}

		public void SetTransparent(bool bTrue)
		{
			if (bTrue)
			{
				this.playerObject.transform.Find("Avatar_Suit").GetComponent<Renderer>().material.shader = Shader.Find("iPhone/AlphaBlend_Color");
				this.playerObject.transform.Find("Avatar_Suit").GetComponent<Renderer>().material.SetColor("_TintColor", new Color(1f, 1f, 1f, 0.1f));
			}
			else
			{
				this.playerObject.transform.Find("Avatar_Suit").GetComponent<Renderer>().material.shader = Shader.Find("iPhone/SolidTexture");
			}
		}

		public void PickOutCharger()
		{
			this.weapon.GunAtt.OnPickOutCharger((this.weapon.GetWeaponType() != WeaponType.Sniper) ? this.playerIK.leftHand : this.playerIK.rightHand, out this.pickOutCharger, out this.pickOnCharger);
		}

		public void DropCharger()
		{
			if (this.pickOutCharger == null)
			{
				return;
			}
			this.pickOutCharger.transform.parent = null;
			Rigidbody rigidbody = this.pickOutCharger.AddComponent<Rigidbody>();
			this.pickOutCharger.AddComponent<BoxCollider>();
			this.pickOutCharger.layer = 23;
			RemoveTimerScript removeTimerScript = this.pickOutCharger.AddComponent<RemoveTimerScript>();
			removeTimerScript.life = 5f;
			this.pickOutCharger = null;
		}

		public void ShowCharger()
		{
			if (this.pickOnCharger == null)
			{
				return;
			}
			this.pickOnCharger.SetActive(true);
		}

		public void PickOnCharger()
		{
			if (this.pickOnCharger == null)
			{
				return;
			}
			this.pickOnCharger.SetActive(false);
			UnityEngine.Object.Destroy(this.pickOnCharger);
			this.weapon.GunAtt.OnPickOnCharger();
		}

		public void PlayGunAnimation(int id)
		{
			this.weapon.GunAtt.PlayGunAni(id);
		}

		public void ShowAttackBox()
		{
			this.playerIK.ShowAttackBox();
		}

		public void Pause()
		{
			this.SetPlayerIdle(false);
			this.audioPlayer.PauseAllAudios();
		}

		public void Resume()
		{
			//TODO:Return
			this.audioPlayer.ResumeAllAudios();
		}

		public void UseProp()
		{
		}

		public void UseReBirth()
		{
		}

		public bool ChangeWeapon()
		{
			if (this.weaponList.Count == 1)
			{
				return false;
			}
			Singleton<GameAudioManager>.Instance.PlayGlobalEffect(GlobalEffect.ChangeWeapon);
			this.SetState(Player.CHANGEGUN_STATE);
			this.playerIK.DoGunOff();
			this.isFullBodyActionOver = false;
			return true;
		}

		public static PlayerState IDLE_STATE = new PlayerIdleState();

		public static PlayerState RUN_STATE = new PlayerRunState();

		public static PlayerState SHOOT_STATE = new PlayerShootState();

		public static PlayerState RUNSHOOT_STATE = new PlayerRunAndShootState();

		public static PlayerState GOTHIT_STATE = new PlayerGotHitState();

		public static PlayerState DEAD_STATE = new PlayerDeadState();

		public static PlayerState CHARGER_STATE = new PlayerChargerState();

		public static PlayerState CHANGEGUN_STATE = new PlayerChangeGunState();

		public static PlayerState AIM_STATE = new PlayerAimState();

		public static PlayerState CANNON_STATE = new PlayerCannonState();

		public static PlayerState FULLBODY_ACTION = new PlayerFullbodyActionState();

		public static PlayerState SNIPE_STATE = new PlayerSnipeState();

		protected GameObject playerObject;

		protected SkinnedMeshRenderer playerMesh;

		protected BaseCameraScript gameCamera;

		protected Transform playerTransform;

		protected Transform avatarTransform;

		protected CharacterController charController;

		protected NavMeshAgent nav;

		protected float normalSpeed;

		protected float aimSpeed;

		protected Animator animator;

		protected Collider collider;

		protected GameObject powerObj;

		protected Transform respawnTrans;

		protected PlayerConfig playerConfig;

		protected AudioPlayer audioPlayer;

		protected GameScene gameScene;

		protected AvatarType avatarType;

		protected PlayerPose playerPose;

		protected PlayerState playerState;

		protected InputController inputController;

		protected Weapon weapon;

		protected List<Weapon> weaponList;

		protected float maxHp;

		protected float hp;

		protected float guiHp;

		protected float walkSpeed;

		protected float powerBuff = 1f;

		protected float powerBuffStartTime;

		protected float lastUpdateNearestWayPointTime;

		protected int currentWeaponIndex;

		protected float gothitEndTime;

		protected string weaponNameEnd;

		protected bool isRunning;

		public float invincibleTime;

		public Action OnShootCallBack;

		public Action<bool> SetIsInChargeCallBack;

		public Action<float> SetChargeSliderPercentCallBack;

		public Action RefreshWeaponBullet;

		protected float buffLastTime;

		protected float gunShootInterval;

		protected PlayerIK playerIK;

		protected float aimDuration;

		protected float aimCount;

		protected Vector2 targetDirection = Vector2.zero;

		protected Vector2 curDirection = Vector2.zero;

		protected bool isShoot;

		protected bool isAim;

		protected bool isMoving;

		protected bool isFullBodyActionOver = true;

		protected PropData curUsedPropData;

		protected AimState playerAimState;

		protected ArmorBox triggeredArmorBox;

		public GameObject pickOutCharger;

		public GameObject pickOnCharger;

		public float attackPower = 1f;

		public float dodgeProbability;

		public float remoteDefense;

		public float bombDefense;

		protected bool shootedEnemy;
	}
}
