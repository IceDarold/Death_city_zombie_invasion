using System;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using ui;
using UnityEngine;
using UnityEngine.AI;

namespace Zombie3D
{
	public class GameScene : IGameSceneControl, IDisposable
	{
		public GamePlayingMode PlayingMode
		{
			get
			{
				return this.playingMode;
			}
		}

		public GameControlMode ControlMode
		{
			get
			{
				return this.controlMode;
			}
			set
			{
				this.controlMode = value;
				if (this.gameUI != null)
				{
					this.gameUI.SetGameControlMode(this.controlMode);
				}
			}
		}

		public SceneLoader sceneLoader
		{
			get
			{
				return this.sLoader;
			}
		}

		public GamePlotManager gamePlotManager
		{
			get
			{
				return this.plotManager;
			}
		}

		public SceneMissions sceneMissions
		{
			get
			{
				return this.missions;
			}
		}

		public Cannon ControlledCannon
		{
			set
			{
				this.cannon = value;
			}
		}

		public IGameUIControl GameUI
		{
			get
			{
				return this.gameUI;
			}
		}

		public int EndLessTotalScore { get; set; }

		public int SelectedLevel
		{
			get
			{
				return this.selectedLevel;
			}
		}

		public bool IsInBulletTime
		{
			get
			{
				return this.bulletTimeDuration > 0f;
			}
		}

		public EnemyTarget DefaultTarget
		{
			get
			{
				return this.defaultTarget;
			}
		}

		public int PlayerKill
		{
			get
			{
				return this.playerKill;
			}
		}

		public int PlayerHeadShot
		{
			get
			{
				return this.playerHeadShot;
			}
		}

		public EnemySpawnManager GetEnemyManager()
		{
			return this.enemyManager;
		}

		public MissionPath GetMissionPath()
		{
			return this.missionPath;
		}

		public void EnablePlayer(bool canControl)
		{
			this.player.InputController.EnableMoveInput = canControl;
			this.player.InputController.EnableShootingInput = canControl;
			this.player.InputController.EnableTurningAround = canControl;
			if (!canControl)
			{
				this.player.SetPlayerAnimatorParameter(false, false, false);
			}
		}

		public GameObject ShowHitBlood(Vector3 pos, Quaternion rotation)
		{
			GameObject gameObject = this.hitBloodObjectPool.ShowObject(pos, rotation);
			if (gameObject != null)
			{
				ParticleSystem componentInChildren = gameObject.GetComponentInChildren<ParticleSystem>();
				if (componentInChildren == null)
				{
					UnityEngine.Debug.LogError(gameObject.name);
					return null;
				}
                ParticleSystem.MainModule e = componentInChildren.main;

                e.simulationSpeed = ((!this.IsInBulletTime) ? 1f : 0.5f);
			}
			return gameObject;
		}

		public void Init()
		{
			// if (MirraSDK.Device.IsMobile)
			// {
			// 	ControlMode = GameControlMode.AUTOFIRE;
			// 	Singleton<GlobalData>.Instance.ShootingMode = 0;
			// }
			// else
			// {
			// 	ControlMode = GameControlMode.MANUALFIRE;
			// 	Singleton<GlobalData>.Instance.ShootingMode = 1;
			// }
			Singleton<UiManager>.Instance.SetUIEnable(false);
			this.CreateSceneData();
			Transform spPoint = GameApp.GetInstance().GetGameScene().sceneLoader.GetPlayerSpawnPoint(this.SelectedLevel);
			PlayerSnipePoint snipePoint = spPoint.gameObject.GetComponent<PlayerSnipePoint>();
			PlayerPose _playerPose = (!(snipePoint == null)) ? snipePoint.playerPose : PlayerPose.FREE;
			this.playingMode = ((_playerPose != PlayerPose.FREE) ? GamePlayingMode.SnipeMode : GamePlayingMode.Normal);
			for (int i = 0; i < this.enemyObjectPool.Length; i++)
			{
				EnemyType enemyType = (EnemyType)i;
				this.enemyObjectPool[i] = AsyncPool.Init(enemyType.ToString(), "Prefabs/zombies/" + enemyType.ToString());
			}
			this.defaultTarget = (this.player = new Player());
			AvatarType aType = PlayerDataManager.GetRole();
			this.enemyList = new Hashtable();
			this.enemyID = 0;
			this.sceneLoader.LoadSceneObjects(this.selectedLevel, delegate
			{
				this.player.Init(aType, _playerPose, spPoint);
				this.camera.Init(snipePoint);
				this.gameUI.SetInstantiatedInterface(this, this.player);
				this.missionPath.Init(this.player.GetTransform());
				if (_playerPose != PlayerPose.FREE)
				{
					this.camera.SetFollowSnipe();
				}
				if (this.playingMode == GamePlayingMode.SnipeMode)
				{
					this.gameUI.SetUIDisplayMode(GamePlayingMode.SnipeMode);
					Singleton<UiControllers>.Instance.SnipeMode();
				}
				if (this.playingMode == GamePlayingMode.SnipeMode)
				{
					Singleton<GameAudioManager>.Instance.PlayMusic(Singleton<GameAudioManager>.Instance.SnipeBG);
				}
				else
				{
					Singleton<GameAudioManager>.Instance.PlayMusic(Singleton<GameAudioManager>.Instance.GameBgm);
					Singleton<UiControllers>.Instance.NormalMode();
				}
				this.startLogic = true;
			}, Singleton<AssetBundleManager>.Instance.CurSceneLoadFromAB);
		}

		public void CreateLimbsAndHitBlood()
		{
			for (int i = 0; i < 3; i++)
			{
				this.limbsPool[i] = new ObjectPool();
			}
			this.limbsPool[0].Init("limbsHead", EnemyFactory.GetInstance().limbsHead, 3, 3f);
			this.limbsPool[1].Init("limbsArm", EnemyFactory.GetInstance().limbsArm, 3, 3f);
			this.limbsPool[2].Init("limbsLeg", EnemyFactory.GetInstance().limbsLeg, 3, 3f);
			this.hitBloodObjectPool.Init("HitBlood", Singleton<ResourceConfigScript>.Instance.hitBlood, 10, 1.5f);
		}

		public void OnSceneSplashFadeOut()
		{
			this.player.DoSnipeCameraAction();
		}

		public EnemyTarget CheckTarget(Vector3 position, float attackRadius)
		{
			if (this.enemyTarget == null)
			{
				return this.defaultTarget;
			}
			if (!this.defaultTarget.IsVisible())
			{
				return this.enemyTarget;
			}
			float magnitude = (this.defaultTarget.GetTransform().position - position).magnitude;
			return (magnitude >= attackRadius) ? this.enemyTarget : this.defaultTarget;
		}

		public void SetEnemyTarget(EnemyTarget _target, bool force = false)
		{
			this.enemyTarget = _target;
			IEnumerator enumerator = this.enemyList.Keys.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					string key = (string)obj;
					(this.enemyList[key] as Enemy).SetTarget((this.enemyTarget != null) ? this.enemyTarget : this.defaultTarget);
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
		}

		public void SetEnemyTargetOnSensor(EnemyTarget sensor)
		{
			IEnumerator enumerator = this.enemyList.Keys.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					string key = (string)obj;
					(this.enemyList[key] as Enemy).SetTargetOnSensor(sensor);
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
		}

		public void ReleaseEnemyTargetOnSensor(GameObject sensor)
		{
			IEnumerator enumerator = this.enemyList.Keys.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					string key = (string)obj;
					(this.enemyList[key] as Enemy).ReleaseSensorTarget(sensor);
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
		}

		public void DoBulletTime(float duration)
		{
			this.bulletTimeDuration = duration;
			this.camera.colorCurves.saturation = 0.3f;
			object[] array = new object[this.enemyList.Count];
			this.enemyList.Keys.CopyTo(array, 0);
			int i = 0;
			int num = array.Length;
			while (i < num)
			{
				(this.enemyList[array[i]] as Enemy).DoChange2BulletTime(true);
				i++;
			}
		}

		public bool CaculatePath2Player(Vector3 position)
		{
			NavMeshPath navMeshPath = new NavMeshPath();
			bool flag = NavMesh.CalculatePath(position, this.player.GetTransform().position, 1, navMeshPath);
			return flag && navMeshPath.status == NavMeshPathStatus.PathComplete;
		}

		public int GetWaveScore()
		{
			return this.waveScore;
		}

		public void AddNetworkComponents()
		{
			//NetworkView networkView = this.player.PlayerObject.AddComponent<NetworkView>();
			this.player.PlayerObject.AddComponent<PlayerNetworkViewScript>();
		}

		public Player GetPlayer()
		{
			return this.player;
		}

		public void SetPlayer2Cannon(Cannon _cannon, Action callback = null)
		{
			this.PlayingState = PlayingState.GamePause;
			FadeAnimationScript.GetInstance().FadeInBlack(1f, delegate
			{
				this.ControlledCannon = _cannon;
				this.camera.SetFollowCannon(_cannon);
				this.player.SetState(Player.CANNON_STATE);
				this.player.SetVisible(false);
				this.playingMode = GamePlayingMode.Cannon;
				this.gameUI.SetUIDisplayMode(this.playingMode);
				this.SetEnemyTarget(this.cannon.BindNPC, false);
				this.camera.CameraTransform.position = this.cannon.cameraAnchor.position;
				this.camera.CameraTransform.rotation = this.cannon.cameraAnchor.rotation;
				FadeAnimationScript.GetInstance().FadeOutBlack(1f, delegate
				{
					if (callback != null)
					{
						callback();
					}
					this.PlayingState = PlayingState.GamePlaying;
				});
				UnityEngine.Debug.LogError("SetPlayer2Cannon");
			});
		}

		public void SetPlayer2CannonAfterPlotAction(Cannon _cannon)
		{
			this.ControlledCannon = _cannon;
			this.camera.SetFollowCannon(_cannon);
			this.player.SetState(Player.CANNON_STATE);
			this.player.SetVisible(false);
			this.playingMode = GamePlayingMode.Cannon;
			this.gameUI.SetUIDisplayMode(this.playingMode);
			this.SetEnemyTarget(this.cannon.BindNPC, false);
			this.camera.CameraTransform.position = this.cannon.cameraAnchor.position;
			this.camera.CameraTransform.rotation = this.cannon.cameraAnchor.rotation;
		}

		public void SetCannon2Player(Vector3 pos, Action callback = null)
		{
			this.PlayingState = PlayingState.GamePause;
			FadeAnimationScript.GetInstance().FadeInBlack(1f, delegate
			{
				this.camera.SetFollowPlayer();
				this.player.SetState(Player.IDLE_STATE);
				this.player.GetTransform().position = pos;
				this.player.SetVisible(true);
				this.ControlledCannon = null;
				this.playingMode = GamePlayingMode.Normal;
				this.gameUI.SetUIDisplayMode(this.playingMode);
				this.SetEnemyTarget(null, false);
				FadeAnimationScript.GetInstance().FadeOutBlack(1f, delegate
				{
					if (callback != null)
					{
						callback();
					}
					this.PlayingState = PlayingState.GamePlaying;
				});
			});
		}

		public void DoControlCannonShoot(bool isShooting)
		{
			if (this.cannon == null)
			{
				return;
			}
			this.cannon.SetShootState(isShooting);
		}

		public void SetAllEnemyAndPlayerVisiable(bool visible)
		{
			this.player.SetVisible(visible);
			object[] array = new object[this.enemyList.Count];
			this.enemyList.Keys.CopyTo(array, 0);
			int i = 0;
			int num = array.Length;
			while (i < num)
			{
				(this.enemyList[array[i]] as Enemy).SetVisible(visible);
				i++;
			}
		}

		public void SetPlayerAndCameraToTarget(Transform trans)
		{
			this.camera.SetCameraRotation(trans);
			this.player.WrapToPosition(trans);
			this.camera.ResetCameraPosition();
		}

		private Vector3[] GetPlayerMissionPath(Vector3 end)
		{
			if (!NavMesh.CalculatePath(this.player.GetTransform().position, end, 1, this.playerMissionPath))
			{
				return null;
			}
			return this.playerMissionPath.corners;
		}

		public Vector3[] GetPlayerMissionPath()
		{
			return this.GetPlayerMissionPath(this.missions.GetMissionTarget());
		}

		public SceneMissions GetSceneMissionMgr()
		{
			return this.missions;
		}

		public void SetReloadProgressEnable(bool enable, bool isCircle)
		{
			this.gameUI.SetProgressEnable(enable, isCircle);
		}

		public void SetReloadProgressPercent(float percent)
		{
			this.gameUI.SetReloadProgressPercent(percent);
		}

		public void SetReloadProgressPercent(int cur, int max)
		{
			this.gameUI.SetReloadProgressPercent(cur, max);
		}

		public void DoGameResult(bool win)
		{
			if (win)
			{
				this.GameWin();
			}
			else
			{
				this.LoseGame();
			}
		}

		public BaseCameraScript GetCamera()
		{
			return this.camera;
		}

		public void ShakeMainCamera(CameraShakeType type)
		{
			ShakeCamera.Instance.Shake(type);
		}

		public void SetGunShootShakeInfo(GunShootShakeInfo info, GunStability stability, WeaponType _weapontype)
		{
			ShakeCamera.Instance.SetGunShootShakeInfo(info, _weapontype);
			this.camera.SetGunStability(stability);
		}

		public void ShakeMainCamera(float duration, float range)
		{
			if (duration == 0f || range == 0f)
			{
				return;
			}
			ShakeCamera.Instance.Shake(duration, range);
		}

		public void ShakeMainCamera(float balance, float duration, float range)
		{
			ShakeCamera.Instance.ResetShootCurveInfo();
			this.camera.DoShootOffset();
		}

		private void DoEnemyDie(WeaponType weaponType)
		{
			this.continuousKillTime = 1.5f;
			this.gameUI.SetUIDisplayEvnt(UIDisplayEvnt.CONTINOUS_KILL, new float[]
			{
				(float)(++this.continousKillNum)
			});
		}

		public void SetFillBulletEvnt(UIDisplayEvnt evnt, float percent)
		{
			this.gameUI.SetUIDisplayEvnt(evnt, new float[]
			{
				percent
			});
		}

		private void DestructContinousKill(float deltaTime)
		{
			if (this.continuousKillTime <= 0f)
			{
				return;
			}
			this.continuousKillTime -= deltaTime;
			if (this.continuousKillTime <= 0f)
			{
				this.continousKillNum = 0;
			}
		}

		private void DestructBulletTime(float deltaTime)
		{
			if (this.playingState != PlayingState.GamePlaying)
			{
				return;
			}
			if (this.bulletTimeDuration <= 0f)
			{
				return;
			}
			this.bulletTimeDuration -= deltaTime;
			if (this.bulletTimeDuration <= 0f)
			{
				object[] array = new object[this.enemyList.Count];
				this.enemyList.Keys.CopyTo(array, 0);
				int i = 0;
				int num = array.Length;
				while (i < num)
				{
					(this.enemyList[array[i]] as Enemy).DoChange2BulletTime(false);
					i++;
				}
				this.player.StopAudio("Adrenaline_loop");
				this.camera.colorCurves.saturation = 1f;
			}
		}

		public void SetUIDisplayEvnt(UIDisplayEvnt evnt, params float[] param)
		{
			this.gameUI.SetUIDisplayEvnt(evnt, param);
		}

		public void SetTouchShootThumb(bool isShoot)
		{
			this.gameUI.SetTouchShootThumb(isShoot);
		}

		public bool IsInShootThumb(Vector2 touchPos)
		{
			return this.gameUI.IsTouchInShootThumb(touchPos);
		}

		public bool GetGamePlotState()
		{
			return this.plotManager.isPlot();
		}

		public Hashtable GetEnemies()
		{
			return this.enemyList;
		}

		public void CreateSceneSpawnedEnemy(Enemy enemy)
		{
			this.sceneSpawnedEnemy.Add(enemy);
		}

		public void DeleteSceneSpawnedEnemy(Enemy enemy)
		{
			if (this.sceneSpawnedEnemy.Contains(enemy))
			{
				this.sceneSpawnedEnemy.Remove(enemy);
			}
			else
			{
				UnityEngine.Debug.LogError(enemy.Name + "is not in sceneSpawnedList");
			}
		}

		public void RemoveAllEnemiesInSceneWithCallback()
		{
			object[] array = new object[this.enemyList.Count];
			this.enemyList.Keys.CopyTo(array, 0);
			for (int i = 0; i < array.Length; i++)
			{
				(this.enemyList[array[i]] as Enemy).RemoveImmediatelyWithCallBack();
			}
		}

		public void RemoveAllEnemySpawnCurrentlyTriggered()
		{
			this.enemyManager.RemoveAllEnemySpawnInScene();
		}

		public int GetSpawnedEnemiesCount()
		{
			return this.enemyList.Count - this.sceneSpawnedEnemy.Count;
		}

		public int GetAllEnemiesCount()
		{
			return this.enemyList.Count;
		}

		public bool SimulateKillEnemy2CheckMission(List<WeaponHitInfo> info)
		{
			if (this.sceneMissions.CurMission == null)
			{
				return false;
			}
			bool flag = false;
			int curAmount = this.sceneMissions.CurMission.curAmount;
			for (int i = 0; i < info.Count; i++)
			{
				if (info[i].enemy == null)
				{
					break;
				}
				if (info[i].enemy.SimulateHitEnemy(info[i].damage))
				{
					flag = this.sceneMissions.SimulateKillEnemy(info[i].hitBone == Bone.Head, info[i].enemy.KeyEnemy, ref curAmount);
					if (flag)
					{
						break;
					}
				}
			}
			return flag;
		}

		public void CheckEnemyEscapedOrReachPathEnd()
		{
			if (this.playingState != PlayingState.GamePlaying)
			{
				return;
			}
			if (this.playingMode != GamePlayingMode.SnipeMode)
			{
				return;
			}
			Mission curMission = this.sceneMissions.CurMission;
			if (curMission == null || curMission.mType != EMission.KILL_ENEMY)
			{
				return;
			}
			if (this.CheckMissionFail(curMission))
			{
				this.DoGameResult(false);
			}
		}

		public bool DoCheckSniperBullet()
		{
			if (this.player.GetWeapon().BulletCount == 0)
			{
				if (this.player.GetWeapon().MaxGunload > 0)
				{
					this.player.DoReload();
				}
				else
				{
					this.DoGameResult(false);
				}
				return true;
			}
			return false;
		}

		public void DoCheckEnemyEnough2Mission()
		{
			Mission curMission = this.sceneMissions.CurMission;
			if (curMission == null || curMission.mType != EMission.KILL_ENEMY)
			{
				return;
			}
			if (this.CheckMissionFail(curMission))
			{
				this.DoGameResult(false);
			}
		}

		private bool CheckMissionFail(Mission ms)
		{
			int num = ms.targetAmount - ms.curAmount;
			bool result;
			if (ms.needKeyEnemy)
			{
				float num2 = 0f;
				object[] array = new object[this.enemyList.Count];
				this.enemyList.Keys.CopyTo(array, 0);
				for (int i = 0; i < array.Length; i++)
				{
					if ((this.enemyList[array[i]] as Enemy).KeyEnemy)
					{
						num2 += 1f;
					}
				}
				result = (num2 < (float)num);
			}
			else
			{
				result = (this.enemyList.Count < num);
			}
			return result;
		}

		public void DoKillEnemy(EnemyType type, WeaponType weapon, bool headShot, bool _keyEnemy)
		{
			this.sceneMissions.DoKillEnemy(headShot, _keyEnemy);
			if (weapon != WeaponType.AssaultRifle && weapon != WeaponType.HandGun && weapon != WeaponType.ShotGun && weapon != WeaponType.GrenadeRifle && weapon != WeaponType.Sniper && weapon != WeaponType.PLAYER_CANNON && weapon != WeaponType.PLAYER_BOMBER && weapon != WeaponType.PLAYER_QTE)
			{
				return;
			}
			this.playerKill++;
			this.DoEnemyDie(weapon);
			DailyMissionSystem.SetDailyMission(DailyMissionType.KILL_ZOMBIE, 1);
			AchievementDataManager.SetAchievementValue(DataCenter.AchievementType.KILL_ENEMY, 1);
			PlayerDataManager.SetStatisticsDatas(PlayerStatistics.KillZombieCounts, 1);
			if (type == EnemyType.E_BOMBER)
			{
				DailyMissionSystem.SetDailyMission(DailyMissionType.KILL_ELITE, 1);
				AchievementDataManager.SetAchievementValue(DataCenter.AchievementType.KILL_INFERNALMOBS_BOMBER, 1);
			}
			else if (type == EnemyType.E_SPITTER)
			{
				DailyMissionSystem.SetDailyMission(DailyMissionType.KILL_ELITE, 1);
				AchievementDataManager.SetAchievementValue(DataCenter.AchievementType.KILL_INFERNALMOBS_SPITTER, 1);
			}
			else if (type == EnemyType.E_BUTCHER)
			{
				DailyMissionSystem.SetDailyMission(DailyMissionType.KILL_ELITE, 1);
				AchievementDataManager.SetAchievementValue(DataCenter.AchievementType.KILL_INFERNALMOBS_BUTCHER, 1);
			}
			if (headShot)
			{
				DailyMissionSystem.SetDailyMission(DailyMissionType.HEAD_SHOOT, 1);
				AchievementDataManager.SetAchievementValue(DataCenter.AchievementType.HEADSHOT, 1);
				PlayerDataManager.SetStatisticsDatas(PlayerStatistics.ShootHeadTimes, 1);
				this.playerHeadShot++;
			}
			if (weapon != WeaponType.HandGun)
			{
				if (weapon != WeaponType.ShotGun)
				{
					if (weapon == WeaponType.AssaultRifle)
					{
						AchievementDataManager.SetAchievementValue(DataCenter.AchievementType.MACHINEGUN_KILL, 1);
					}
				}
				else
				{
					AchievementDataManager.SetAchievementValue(DataCenter.AchievementType.SHOTGUN_KILL, 1);
				}
			}
			else
			{
				AchievementDataManager.SetAchievementValue(DataCenter.AchievementType.PISTOL_KILL, 1);
			}
		}

		public void DoPlayerStatistics(PlayerStatistics evnt)
		{
			PlayerDataManager.SetStatisticsDatas(evnt, 1);
		}

		public void SetCameraFOV(float _fov)
		{
			this.camera.SetMainCameraFOV(_fov);
		}

		public float GetCameraFOV()
		{
			return this.camera.cameraComponent.fieldOfView;
		}

		public float GetPlayerWeaponFOV(float _percent)
		{
			return this.player.GetWeapon().GetGunSightFOV(_percent);
		}

		public void Bomb2Enemies(float _damage, Vector3 bombPos, float _force, float radius, WeaponType wType = WeaponType.NoGun, bool containsElite = false, BreakAble _self = null)
		{
			object[] array = new object[this.enemyList.Count];
			this.enemyList.Keys.CopyTo(array, 0);
			int i = 0;
			int num = array.Length;
			while (i < num)
			{
				Enemy enemy = this.enemyList[array[i]] as Enemy;
				if (containsElite || enemy.GetEnemyProbability() == global::EnemyProbability.NORMAL)
				{
					Vector3 vector = enemy.GetTransform().position - bombPos;
					if (vector.magnitude <= radius)
					{
						enemy.OnHit(new DamageProperty(_damage, _force, vector.normalized), wType, bombPos, Bone.MiddleSpine);
					}
				}
				i++;
			}
			for (int j = 0; j < this.allBossOilDrums.Count; j++)
			{
				if ((bombPos - this.allBossOilDrums[j].transform.position).magnitude <= radius)
				{
					this.allBossOilDrums[j].OnHit(new DamageProperty(_damage), wType, Vector3.zero, Bone.None);
				}
			}
			this.CheckAllBreakableDrums(bombPos, radius, _self);
			this.ShakeMainCamera(CameraShakeType.DRASTIC);
		}

		public void Bomb2Enemies(float dmg2Normal, float dmg2Elite, float radius, Vector3 bombPos, float _force, WeaponType wType = WeaponType.NoGun)
		{
			object[] array = new object[this.enemyList.Count];
			this.enemyList.Keys.CopyTo(array, 0);
			int i = 0;
			int num = array.Length;
			while (i < num)
			{
				Enemy enemy = this.enemyList[array[i]] as Enemy;
				float damage;
				if (enemy.GetEnemyProbability() == global::EnemyProbability.NORMAL)
				{
					damage = enemy.MaxHp * dmg2Normal;
				}
				else
				{
					damage = enemy.MaxHp * dmg2Elite;
				}
				Vector3 vector = enemy.GetTransform().position - bombPos;
				if (vector.magnitude <= radius)
				{
					enemy.OnHit(new DamageProperty(damage, _force, vector.normalized), wType, bombPos, Bone.MiddleSpine);
				}
				i++;
			}
		}

		public void CheckAllBreakableDrums(Vector3 bombPos, float radius, BreakAble self)
		{
			for (int i = 0; i < this.allBreakAbleDrums.Count; i++)
			{
				if (this.allBreakAbleDrums[i] != self)
				{
					if ((bombPos - this.allBreakAbleDrums[i].GetTransform().position).sqrMagnitude <= radius * radius)
					{
						this.allBreakAbleDrums[i].OnHit(new DamageProperty(float.PositiveInfinity), WeaponType.NoGun, Vector3.zero, Bone.None);
					}
				}
			}
		}

		public int GetSceneEnemiesCount()
		{
			return this.sceneSpawnedEnemy.Count;
		}

		public Enemy GetEnemyByID(string enemyID)
		{
			return (Enemy)this.enemyList[enemyID];
		}

		public PlayingState PlayingState
		{
			get
			{
				return this.playingState;
			}
			set
			{
				this.playingState = value;
				Debug.Log($"CURRENT PLAY STATE - {playingState}");
				switch (this.playingState)
				{
				case PlayingState.GamePlaying:
					if (this.playingMode == GamePlayingMode.SnipeMode)
					{
						this.UnFreezeAllEnemies();
					}
					else
					{
						this.ResumeAllEnemy();
					}
					this.player.Resume();
					if (this.enemyTarget != null)
					{
						this.enemyTarget.DoResume();
					}
					
					//TODO:Cursor
					if (Singleton<UiControllers>.Instance.IsMobile)
					{
						Singleton<UiControllers>.Instance.EnableJoysticks();
					}
					else
					{
						Singleton<UiControllers>.Instance.HideCursor();
					}


					break;
				case PlayingState.GameWin:
				case PlayingState.GameLose:
				case PlayingState.GamePause:
				case PlayingState.GamePlot:
					if (this.playingMode == GamePlayingMode.SnipeMode)
					{
						this.FreezeAllEnemies();
					}
					else
					{
						this.StopAllEnemy();
					}
					this.player.Pause();
					if (this.enemyTarget != null)
					{
						this.enemyTarget.DoPause();
					}
					//TODO:Cursor
					if (Singleton<UiControllers>.Instance.IsMobile)
					{
						Singleton<UiControllers>.Instance.DisableJoysticks();
					}
					else
					{
						Singleton<UiControllers>.Instance.ShowCursor();
					}
					break;
				}
			}
		}

		public int EnemyID
		{
			get
			{
				return this.enemyID;
			}
		}

		public int GetNextEnemyID()
		{
			this.enemyID++;
			return this.enemyID;
		}

		public AsyncPool GetEnemyPool(EnemyType eType)
		{
			return this.enemyObjectPool[(int)eType];
		}

		public GameObject GetLimbs(int part)
		{
			return this.limbsPool[part].CreateObject(Vector3.zero, Quaternion.identity);
		}

		public void DoLogic(float deltaTime)
		{
			if (!this.startLogic)
			{
				return;
			}
			this.player.DoLogic(deltaTime);
			this.enemyManager.DoLogic(deltaTime);
			if (this.cannon != null)
			{
				this.cannon.DoLogic(deltaTime);
			}
			object[] array = new object[this.enemyList.Count];
			this.enemyList.Keys.CopyTo(array, 0);
			int i = 0;
			int num = array.Length;
			while (i < num)
			{
				(this.enemyList[array[i]] as Enemy).DoLogic(deltaTime);
				i++;
			}
			this.hitBloodObjectPool.AutoDestruct();
			this.DestructContinousKill(deltaTime);
			this.DestructBulletTime(deltaTime);
			for (int j = 0; j < this.limbsPool.Length; j++)
			{
				this.limbsPool[j].AutoDestruct();
			}
			this.sceneMissions.DoLogic(deltaTime);
		}

		public void CreateSceneData()
		{
			this.controlMode = (GameControlMode)Singleton<GlobalData>.Instance.ShootingMode;
			this.missionPath = (UnityEngine.Object.Instantiate(Resources.Load("MissionItems/MissionPathManager")) as GameObject).GetComponent<MissionPath>();
			this.sLoader = UnityEngine.Object.FindObjectOfType<SceneLoader>();
			this.plotManager = UnityEngine.Object.FindObjectOfType<GamePlotManager>();
			this.missions = UnityEngine.Object.FindObjectOfType<SceneMissions>();
			this.enemyManager = UnityEngine.Object.FindObjectOfType<EnemySpawnManager>();
			this.camera = UnityEngine.Object.FindObjectOfType<TPSSimpleCameraScript>();
			Singleton<DropItemManager>.Instance.Init();
			CheckpointData checkpointData = CheckpointDataManager.SelectCheckpoint;
			if (checkpointData.SceneID == -1 && checkpointData.ID == -1)
			{
				checkpointData = CheckpointDataManager.GetCheckPointDataBySceneID(this.sceneLoader.curLevelIndex);
				CheckpointDataManager.SelectCheckpoint = checkpointData;
				UnityEngine.Debug.LogError("未检测到选择的关卡，未从UI进入游戏");
			}
			this.selectedLevel = checkpointData.SceneID;
			if (checkpointData.Type == CheckpointType.GOLD)
			{
				CheckpointData currentCheckpoint = CheckpointDataManager.GetCurrentCheckpoint();
				int id = Mathf.Clamp(currentCheckpoint.ID - 1, 1, currentCheckpoint.ID);
				int dataID = CheckpointDataManager.GetCheckpointData(id).DataID;
				EnemyDataManager.InitEnemyDataByLevel(dataID);
			}
			else if (checkpointData.Type >= CheckpointType.WIPE_OUT)
			{
				CheckpointData currentCheckpoint2 = CheckpointDataManager.GetCurrentCheckpoint();
				int id2 = Mathf.Clamp(currentCheckpoint2.ID - 2, 1, currentCheckpoint2.ID);
				int dataID2 = CheckpointDataManager.GetCheckpointData(id2).DataID;
				EnemyDataManager.InitEnemyDataByLevel(dataID2);
			}
			else if (checkpointData.Type == CheckpointType.WEAPON || checkpointData.Type == CheckpointType.BOSS)
			{
				EnemyDataManager.InitEnemyDataByLevel(CheckpointDataManager.SelectCheckpoint.DataID);
			}
			else if (checkpointData.Type == CheckpointType.MAINLINE_SNIPE || checkpointData.Type == CheckpointType.SNIPE)
			{
				EnemyDataManager.InitEnemyDataByLevel(CheckpointDataManager.SelectCheckpoint.DataID);
			}
			else if (checkpointData.Type != CheckpointType.MAINLINE)
			{
				EnemyDataManager.InitEnemyDataByMaxLevel();
			}
			else
			{
				EnemyDataManager.InitEnemyDataByLevel(CheckpointDataManager.SelectCheckpoint.DataID);
			}
			this.reviveCount = 0;
			this.gameUI = Singleton<UiManager>.Instance.ShowPage<InGamePage>(PageName.InGamePage, null);
			this.gameUI.SetGameControlMode(this.controlMode);
		}

		public void LoseGame()
		{
			this.PlayingState = PlayingState.GameLose;
			this.gameUI.GameFalied();
		}

		public void GameWin()
		{
			this.PlayingState = PlayingState.GameWin;
			this.StopAllEnemy();
		}

		public void Revive()
		{
			this.PlayingState = PlayingState.GamePlaying;
			this.defaultTarget.DoRevive();
			this.sceneMissions.Revive();
			if (this.enemyTarget != null)
			{
				this.enemyTarget.DoRevive();
			}
		}

		private void StopAllEnemy()
		{
			object[] array = new object[this.enemyList.Count];
			this.enemyList.Keys.CopyTo(array, 0);
			for (int i = 0; i < array.Length; i++)
			{
				(this.enemyList[array[i]] as Enemy).Pause();
			}
		}

		private void ResumeAllEnemy()
		{
			object[] array = new object[this.enemyList.Count];
			this.enemyList.Keys.CopyTo(array, 0);
			for (int i = 0; i < array.Length; i++)
			{
				(this.enemyList[array[i]] as Enemy).Resume();
			}
		}

		public void FreezeAllEnemies()
		{
			object[] array = new object[this.enemyList.Count];
			this.enemyList.Keys.CopyTo(array, 0);
			for (int i = 0; i < array.Length; i++)
			{
				(this.enemyList[array[i]] as Enemy).Freeze();
			}
		}

		public void UnFreezeAllEnemies()
		{
			object[] array = new object[this.enemyList.Count];
			this.enemyList.Keys.CopyTo(array, 0);
			for (int i = 0; i < array.Length; i++)
			{
				(this.enemyList[array[i]] as Enemy).DoChangeSnipePatrolAction();
			}
		}

		public void Dispose()
		{
			this.enemyList.Clear();
			this.allBossOilDrums.Clear();
			this.sceneSpawnedEnemy.Clear();
			this.allBreakAbleDrums.Clear();
			this.enemyObjectPool = null;
			this.limbsPool = null;
		}

		protected Player player;

		protected EnemyTarget enemyTarget;

		protected EnemyTarget defaultTarget;

		protected BaseCameraScript camera;

		protected Hashtable enemyList;

		protected bool startLogic;

		protected ObjectPool hitBloodObjectPool = new ObjectPool();

		protected AsyncPool[] enemyObjectPool = new AsyncPool[16];

		protected ObjectPool[] limbsPool = new ObjectPool[3];

		protected PlayingState playingState = PlayingState.Changing;

		protected int enemyID;

		protected int waveScore;

		protected List<WaveEnemySpawnTrigger> allSpawnedWaves = new List<WaveEnemySpawnTrigger>();

		protected EnemySpawnManager enemyManager;

		protected SceneLoader sLoader;

		protected int selectedLevel;

		protected GamePlotManager plotManager;

		protected SceneMissions missions;

		protected IGameUIControl gameUI;

		protected List<Enemy> sceneSpawnedEnemy = new List<Enemy>();

		protected Cannon cannon;

		protected float continuousKillTime;

		protected int continousKillNum;

		protected float bulletTimeDuration;

		protected GamePlayingMode playingMode;

		protected GameControlMode controlMode;

		protected MissionPath missionPath;

		protected NavMeshPath playerMissionPath = new NavMeshPath();

		protected int playerKill;

		protected int playerHeadShot;

		public int reviveCount;

		public List<BossOilDrum> allBossOilDrums = new List<BossOilDrum>();

		public List<BreakAble> allBreakAbleDrums = new List<BreakAble>();
	}
}
