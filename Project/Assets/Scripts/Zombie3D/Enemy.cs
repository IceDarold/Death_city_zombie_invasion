using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DataCenter;
using UnityEngine;
using UnityEngine.AI;

namespace Zombie3D
{
	public abstract class Enemy
	{
		public NavMeshAgent Nav
		{
			get
			{
				return this.nav;
			}
		}

		public GameObject EnemyObject
		{
			get
			{
				return this.enemyObject;
			}
		}

		public string RunAnimationName
		{
			get
			{
				return this.runAnimationName;
			}
		}

		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
				this.SetBoneNames(this.name);
			}
		}

		public bool KeyEnemy
		{
			get
			{
				return this.keyEnemy;
			}
		}

		public float HP
		{
			get
			{
				return this.hp;
			}
		}

		public Transform GetTransform()
		{
			return this.enemyTransform;
		}

		public Transform GetAimBone()
		{
			if (this.attr.aimBone != null)
			{
				return this.attr.aimBone;
			}
			return this.attr.bones[9].transform;
		}

		public float DetectionRange
		{
			get
			{
				return this.detectionRange;
			}
		}

		public float GotHitTime
		{
			set
			{
				this.gotHitTime = value;
			}
		}

		public float WakeUpRange
		{
			get
			{
				return this.wakeupRange;
			}
			set
			{
				this.wakeupRange = value;
			}
		}

		public float AttackDamage
		{
			get
			{
				return this.attackDamage;
			}
		}

		public float AttackRange
		{
			get
			{
				return this.attackRange;
			}
		}

		public AttackAttribute AttAttribute
		{
			get
			{
				return this.attackAttribute;
			}
		}

		public EnemyType EnemyType
		{
			get
			{
				return this.enemyType;
			}
			set
			{
				this.enemyType = value;
			}
		}

		public Vector3 LastTarget
		{
			get
			{
				return this.lastTarget;
			}
		}

		public EnemyAttr Attr
		{
			get
			{
				return this.attr;
			}
		}

		public void SetTarget(EnemyTarget trans)
		{
			this.target = trans;
			this.DoCheckTarget();
		}

		public void SetTargetOnSensor(EnemyTarget sensor)
		{
			if (this.target.GetTargetType() == EnemyTargetType.SNEER_BOMB || this.target.GetTargetType() == EnemyTargetType.FOCUS_SENSOR)
			{
				return;
			}
			this.target = sensor;
			if (this.state == Enemy.WAIT_STATE || this.state == Enemy.PATROL_STATE)
			{
				this.WakeUp();
			}
		}

		public void ReleaseSensorTarget(GameObject sensor)
		{
			if (this.target != null && this.enemyTransform != null && this.target.GetTransform().gameObject == sensor)
			{
				this.target = null;
				this.DoCheckTarget();
			}
		}

		public void SetForceFocusTarget(EnemyTarget _target)
		{
			this.forceFocusTarget = _target;
			this.DoCheckTarget();
		}

		public EnemyTarget GetTarget()
		{
			return this.target;
		}

		public bool CouldMakeNextAttack()
		{
			return false;
		}

		public virtual bool CouldEnterAttackState()
		{
			return this.SqrDistanceFromPlayer < this.AttackRange * this.AttackRange;
		}

		public virtual bool ShouldGoToForceIdle()
		{
			bool flag = this.gameScene.PlayingState != PlayingState.GamePlaying && this.gameScene.PlayingState != PlayingState.WaitForEnd;
			if (flag)
			{
				this.SetState(Enemy.FORCE_IDLE, true);
			}
			return flag;
		}

		public virtual bool CheckAttackInterval(float deltaTime)
		{
			this.attackInterval -= deltaTime;
			return this.attackInterval <= 0f;
		}

		public bool PathEndAnimationEnds()
		{
			return false;
		}

		public bool GetAnimationEnds(E_ANIMATION state)
		{
			return !this.animationState[(int)state];
		}

		public void SetAnimationStart(E_ANIMATION state)
		{
			this.animationState[(int)state] = true;
		}

		public void SetAnimationEnd(E_ANIMATION state)
		{
			this.animationState[(int)state] = false;
		}

		public void SetInGrave(bool inGrave)
		{
		}

		public bool MoveFromGrave(float deltaTime)
		{
			return false;
		}

		public virtual void Init(GameObject gObject, EnemyState _state, int startAniID, bool armored = false, bool armed = false, bool redEye = false)
		{
			this.ClearAllAnimationStateTag();
			this.gameScene = GameApp.GetInstance().GetGameScene();
			this.player = this.gameScene.GetPlayer();
			this.enemyObject = gObject;
			this.enemyTransform = this.enemyObject.transform;
			this.animator = this.enemyObject.GetComponentInChildren<Animator>();
			this.findPathScheduler = new TimeScheduler(0.5f, 0f, new Action(this.DoFindPath), null);
			this.checkTargetSchduler = new TimeScheduler(1f, 0f, new Action(this.DoCheckTarget), () => this.forceFocusTarget == null);
			this.nav = this.enemyObject.GetComponent<NavMeshAgent>();
			this.nav.enabled = true;
			this.rConfig = GameApp.GetInstance().GetResourceConfig();
			this.detectionRange = 150f;
			this.InitEnemyAttribute(gObject, armored, armed, redEye);
			this.animator.SetBool("isElite", this.isRedEye);
			this.animator.SetBool("IsWeaponed", this.isArmed);
			this.SetEnemyActionID("StartAniID", startAniID);
			this.SetEnemyActionID("WalkID", UnityEngine.Random.Range(0, this.walkAniNum));
			this.InitAudios();
			this.InitAnimatorListener();
			this.SetEnemyHpToAnimator();
			this.OnEnemyAttackBoxDisable();
			this.DoCheckTarget();
			this.DoChange2BulletTime(this.gameScene.IsInBulletTime);
			this.SetState(_state, true);
			if (_state == Enemy.COMEOUT_STATE && startAniID == 9)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Singleton<ResourceConfigScript>.Instance.EnemyComeOutParticle, this.enemyTransform.position, this.enemyTransform.rotation, null);
			}
		}

		public virtual void SetIsKeyEnemy(bool _keyEnemy)
		{
			this.keyEnemy = _keyEnemy;
		}

		public void SetState(EnemyState newState, bool setAnimation = true)
		{
			if (this.state != null)
			{
				this.state.DoExit(this);
			}
			newState.DoEnter(this);
			this.state = newState;
			switch (this.state.key)
			{
			case StateKey.GOTHIT_STATE:
				this.SetAnimationStart(E_ANIMATION.HURT);
				break;
			case StateKey.ATTACK_STATE:
				this.SetAnimationStart(E_ANIMATION.ATTACK);
				break;
			case StateKey.DEAD_STATE:
				this.SetAnimationStart(E_ANIMATION.DIE);
				break;
			case StateKey.WAKEUP_STATE:
				this.SetAnimationStart(E_ANIMATION.WAKEUP);
				break;
			case StateKey.COMEOUT_STATE:
				this.SetAnimationStart(E_ANIMATION.COMEOUT);
				break;
			case StateKey.ANGERY_STATE:
				this.SetAnimationStart(E_ANIMATION.ANGERY);
				break;
			case StateKey.CLIMB_STATE:
				this.SetAnimationStart(E_ANIMATION.CLIMB);
				break;
			case StateKey.RUSH_START:
				this.SetAnimationStart(E_ANIMATION.RUSH_START);
				break;
			case StateKey.RUSH_ATTACK:
				this.SetAnimationStart(E_ANIMATION.RUSH_ATTACK);
				break;
			case StateKey.RUSH_END:
				this.SetAnimationStart(E_ANIMATION.RUSH_END);
				break;
			case StateKey.DIZZINESS:
				this.SetAnimationStart(E_ANIMATION.DIZZINESS);
				break;
			case StateKey.EXPLOD_STATE:
				this.SetAnimationStart(E_ANIMATION.EXPLOD);
				break;
			case StateKey.SKILL_STATE:
				this.SetAnimationStart(E_ANIMATION.SKILL);
				break;
			case StateKey.THROW_STATE:
				this.SetAnimationStart(E_ANIMATION.THROW);
				break;
			}
			if (setAnimation)
			{
				this.SetStateAnimation(this.state.key);
			}
			this.state.NextState(this, Time.deltaTime);
		}

		public virtual bool SetStateAnimation(StateKey key)
		{
			switch (key)
			{
			case StateKey.CATCHING_STATE:
			case StateKey.PATHWALK_STATE:
				this.SetAnimation(E_ANIMATION.WALK);
				return true;
			case StateKey.GOTHIT_STATE:
				this.SetAnimation(E_ANIMATION.HURT);
				return true;
			case StateKey.PATROL_STATE:
				this.SetAnimation(E_ANIMATION.PATROL);
				return true;
			case StateKey.ATTACK_STATE:
				this.OnAttack();
				return true;
			case StateKey.DEAD_STATE:
				this.SetAnimation(E_ANIMATION.DIE);
				return true;
			case StateKey.WAIT_STATE:
				this.SetAnimation(E_ANIMATION.WAIT);
				return true;
			case StateKey.WAKEUP_STATE:
				this.SetAnimation(E_ANIMATION.WAKEUP);
				return true;
			case StateKey.COMEOUT_STATE:
				this.SetAnimation(E_ANIMATION.COMEOUT);
				return true;
			case StateKey.ANGERY_STATE:
				this.SetAnimation(E_ANIMATION.ANGERY);
				return true;
			case StateKey.CLIMB_STATE:
				this.SetAnimation(E_ANIMATION.CLIMB);
				return true;
			case StateKey.RUSH_START:
				this.SetAnimation(E_ANIMATION.RUSH_START);
				return true;
			case StateKey.RUSH_END:
				this.SetAnimation(E_ANIMATION.RUSH_END);
				return true;
			case StateKey.DIZZINESS:
				this.SetAnimation(E_ANIMATION.DIZZINESS);
				return true;
			case StateKey.EXPLOD_STATE:
				this.SetAnimation(E_ANIMATION.EXPLOD);
				return true;
			case StateKey.SKILL_STATE:
			case StateKey.BOSS_ATTACK:
				this.SetAnimation(E_ANIMATION.SKILL);
				return true;
			case StateKey.FORCE_IDLE:
				this.SetAnimation(E_ANIMATION.FORCE_IDLE);
				return true;
			case StateKey.THROW_STATE:
				this.SetAnimation(E_ANIMATION.THROW);
				return true;
			case StateKey.SHOUT_STATE:
				this.SetAnimation(E_ANIMATION.SHOUT);
				return true;
			case StateKey.JUMP_STATE:
				this.SetAnimation(E_ANIMATION.JUMP);
				return true;
			}
			return false;
		}

		public EnemyState GetState()
		{
			return this.state;
		}

		public EnemyState GetPreState()
		{
			return this.preState;
		}

		public void SetPreState(EnemyState _state)
		{
			this.preState = _state;
		}

		public virtual void CheckHit()
		{
		}

		public virtual void CheckHit(bool isLeftHand)
		{
		}

		public virtual void OnAttackBoxEnterTrigger(Collider other, int boxID)
		{
		}

		public Transform GetAimedTransform()
		{
			return this.aimedTransform;
		}

		public Vector3 GetPosition()
		{
			return this.enemyTransform.position;
		}

		public float SqrDistanceFromPlayer
		{
			get
			{
				return (this.target.GetTransform().position - this.enemyTransform.position).sqrMagnitude;
			}
		}

		public float DistancesFromPlayer
		{
			get
			{
				return (this.target.GetTransform().position - this.enemyTransform.position).magnitude;
			}
		}

		public virtual void OnHeadShoot(DamageProperty dp, WeaponType weaponType, Vector3 pos)
		{
			if (this.hp < 0f)
			{
				return;
			}
			this.hp -= dp.damage;
			this.lastHurtPos = pos;
			if (this.hp <= 0f)
			{
				if (!QualityManager.isLow)
				{
					UnityEngine.Object.Instantiate<GameObject>(this.rConfig.headShootParticle, pos, Quaternion.identity);
				}
				if (this.gameScene.PlayingMode != GamePlayingMode.SnipeMode)
				{
					UnityEngine.Object.Instantiate<GameObject>(this.rConfig.headShotEffect, pos, Quaternion.identity);
				}
				this.gameScene.SetUIDisplayEvnt(UIDisplayEvnt.HEADSHOT, new float[0]);
			}
			else
			{
				this.gameScene.ShowHitBlood(pos, Quaternion.identity);
				this.audio.PlayAudio("Hurt");
			}
			if (this.state == Enemy.SNIPE_PATROL)
			{
				this.snipePatrolOnHitCallback(this);
			}
			this.lastHurtWeapon = weaponType;
			this.hurtBone = Bone.Head;
			this.lastDP = dp;
			this.SetEnemyHpToAnimator();
			this.state.OnHit(this);
			this.SetHP();
		}

		public virtual void OnHit(DamageProperty dp, WeaponType weaponType, Vector3 pos, Bone _bone)
		{
			if (this.hp <= 0f)
			{
				return;
			}
			this.gotHitTime = Time.time;
			if (_bone == Bone.Head)
			{
				this.OnHeadShoot(dp, weaponType, pos);
				return;
			}
			this.preHp = this.hp;
			this.hp -= dp.damage;
			this.hurtBone = _bone;
			this.lastHurtPos = pos;
			this.lastHurtWeapon = weaponType;
			this.lastDP = dp;
			if (this.hurtBone == Bone.MiddleSpine)
			{
				this.middleSpineRight = (this.attr.bones[9].transform.InverseTransformPoint(pos).z < 0f);
			}
			this.gameScene.ShowHitBlood(pos, Quaternion.identity);
			if (this.state == Enemy.WAIT_STATE || this.state == Enemy.PATROL_STATE)
			{
				this.WakeUp();
			}
			if (this.state == Enemy.SNIPE_PATROL)
			{
				this.snipePatrolOnHitCallback(this);
			}
			if (this.hurtBone != Bone.None)
			{
				this.state.OnHit(this);
			}
			this.audio.PlayAudio("Hurt");
			this.SetHP();
		}

		public bool SimulateHitEnemy(float _damage)
		{
			float num = this.hp;
			num -= _damage;
			return num <= 0f;
		}

		public virtual void SetHP()
		{
			float num = this.hp / this.MaxHp;
			num = Mathf.Clamp(num, 0f, 1f);
			this.attr.hpSlider.localScale = new Vector3(num, 1f, 1f);
			this.attr.hpGameObject.gameObject.SetActive(true);
		}

		private void CopyTransformsRecurse(BoneInfo _attr)
		{
			int i = 0;
			int num = this.attr.bones.Length;
			while (i < num)
			{
				_attr.bones[i].transform.localPosition = this.attr.bones[i].transform.localPosition;
				_attr.bones[i].transform.localRotation = this.attr.bones[i].transform.localRotation;
				i++;
			}
		}

		public void OnHit(DamageProperty dp, WeaponType weaponType, Bone _bone = Bone.None)
		{
			this.OnHit(dp, weaponType, this.enemyTransform.position + Vector3.up * 1f, _bone);
		}

		public virtual bool CanChangeToGotHitState()
		{
			return Time.time - this.gotHitTime > 1f;
		}

		private bool isBoneInDownBody(Bone bone)
		{
			return bone == Bone.LeftHips || bone == Bone.LeftKnee || bone == Bone.RightKnee || bone == Bone.RightHips;
		}

		public virtual void PlayDeadAudio()
		{
			Singleton<GameAudioManager>.Instance.PlaySoundInGame(this.audio.GetAudio("Dead"), false);
		}

		public virtual void OnAttack()
		{
			this.SetAnimationStart(E_ANIMATION.ATTACK);
			this.attackInterval = this.attr.attackInterval;
		}

		public virtual void PlayBloodEffect()
		{
			if (this.enemyObject && this.enemyObject.active)
			{
				UnityEngine.Object.Instantiate<GameObject>(this.rConfig.deadBlood, this.enemyTransform.position + new Vector3(0f, 1.35f, 0f), Quaternion.Euler(0f, 0f, 0f));
				float y = 0.01f;
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.rConfig.deadFoorblood, new Vector3(this.enemyTransform.position.x, y, this.enemyTransform.position.z), Quaternion.Euler(270f, 0f, 0f));
				gameObject.transform.rotation = this.deadRotation * gameObject.transform.rotation;
				gameObject.transform.position = this.deadPosition;
			}
		}

		public virtual void FindPath()
		{
		}

		public virtual void RemoveDeadBodyTimer(float dt)
		{
		}

		public virtual void OnEnemySpecialEvent()
		{
		}

		public virtual void OnDieDropItems()
		{
			if (this.gameScene.PlayingState == PlayingState.WaitForEnd || this.gameScene.PlayingMode == GamePlayingMode.SnipeMode)
			{
				return;
			}
			Singleton<DropItemManager>.Instance.DoDropItem(this.GetEnemyProbability(), this.attr.bones[9].transform.position, CheckpointDataManager.SelectCheckpoint.Type);
		}

		public virtual void OnDead()
		{
			this.OnDieDropItems();
			this.OnEnemyAttackBoxDisable();
			this.attr.hpSlider.localScale = new Vector3(0f, 1f, 1f);
			this.PlayDeadAudio();
			this.gameScene.DoKillEnemy(this.enemyType, this.lastHurtWeapon, this.hurtBone == Bone.Head, this.keyEnemy);
			this.state.DoExit(this);
			this.deadTime = Time.time;
			if (this.dieCallBack != null)
			{
				this.dieCallBack(this.name);
			}
			this.ResetBoneNames();
			this.CreateRagdoll(this.lastDP, Bone.MiddleSpine);
		}

		public virtual void CreateRagdoll(DamageProperty dp, Bone _bone)
		{
			this.gameScene.GetEnemies().Remove(this.enemyObject.name);
			this.nav.enabled = false;
			this.enemyObject.SetActive(false);
			if (this.gameScene.PlayingMode == GamePlayingMode.SnipeMode)
			{
				this.gameScene.DoCheckEnemyEnough2Mission();
			}
			GameObject deadBody = EnemyFactory.GetInstance().GetDeadBody(this.enemyType, this.enemyTransform.position, this.enemyTransform.rotation);
			BoneInfo component = deadBody.GetComponent<BoneInfo>();
			if (this.attr.canBreakLimbs && this.hurtBone == Bone.Head)
			{
				UnityEngine.Object.Instantiate<GameObject>(Singleton<ResourceConfigScript>.Instance.headShotBlood, component.bones[10].transform.position, Quaternion.identity);
				Singleton<GameAudioManager>.Instance.PlayGlobalEffect(GlobalEffect.Headshot);
			}
			component.SetArmorsAndWeapons(this.armorID, this.armID);
			this.CopyTransformsRecurse(component);
			component.bones[(int)_bone].gameObject.GetComponent<Rigidbody>().AddForce(dp.hitDir * dp.force, ForceMode.Force);
			if (this.attr.canBreakLimbs && !this.isArmored && !this.isArmed)
			{
				this.CreateLimbs(this.hurtBone, component);
			}
			component.enabled = (this.gameScene.PlayingMode != GamePlayingMode.SnipeMode);
		}

		public void CreateLimbs(Bone bone, BoneInfo ragdollBone)
		{
			GameObject gameObject = null;
			bool flag = false;
			switch (bone)
			{
			case Bone.LeftKnee:
				gameObject = this.gameScene.GetLimbs(2);
				goto IL_96;
			case Bone.RightKnee:
				gameObject = this.gameScene.GetLimbs(2);
				goto IL_96;
			case Bone.LeftElbow:
				gameObject = this.gameScene.GetLimbs(1);
				goto IL_96;
			case Bone.RightElbow:
				gameObject = this.gameScene.GetLimbs(1);
				goto IL_96;
			case Bone.Head:
				gameObject = this.gameScene.GetLimbs(0);
				goto IL_96;
			}
			flag = true;
			IL_96:
			if (flag)
			{
				return;
			}
			ragdollBone.bones[(int)bone].SetActive(false);
			ragdollBone.meshs[(int)bone].enabled = false;
			gameObject.transform.position = ragdollBone.bones[(int)bone].transform.position;
			gameObject.transform.rotation = ragdollBone.bones[(int)bone].transform.rotation;
			gameObject.GetComponent<Rigidbody>().AddForceAtPosition(Vector3.up, Vector3.zero, ForceMode.Impulse);
		}

		public virtual bool OnSpecialState(float deltaTime)
		{
			return false;
		}

		public virtual EnemyState EnterSpecialState(float deltaTime)
		{
			return null;
		}

		public virtual void DoPatrol(float deltaTime)
		{
			this.animator.SetFloat("PatrolSpeed", this.patrolInfo.patrolSpeed);
			this.nav.Move(this.enemyTransform.forward * this.patrolInfo.patrolSpeed * deltaTime);
			if (this.patrolInfo.InRest(deltaTime, this.enemyTransform.position))
			{
				this.patrolInfo.patrolSpeed = Mathf.Lerp(this.patrolInfo.patrolSpeed, 0f, deltaTime * 10f);
				return;
			}
			this.patrolInfo.patrolSpeed = Mathf.Lerp(this.patrolInfo.patrolSpeed, this.attr.patrolSpeed, deltaTime * 10f);
			Vector3 forward = this.patrolInfo.patrolTarget - this.enemyTransform.position;
			forward.y = 0f;
			this.enemyTransform.rotation = Quaternion.Lerp(this.enemyTransform.rotation, Quaternion.LookRotation(forward), deltaTime * 10f);
			if (forward.sqrMagnitude < 0.0100000007f)
			{
				this.patrolInfo.ResetPatrolRestTime();
			}
		}

		public void SetPatrolInfo(EnemyPatrolInfo _patrolInfo)
		{
			this.patrolInfo = _patrolInfo;
		}

		public void SetPatrolCenter()
		{
			this.patrolInfo.SetPatrolCenter(this.enemyTransform.position);
		}

		public void DoSnipePatrol(float deltatime)
		{
			if (this.gameScene.PlayingState != PlayingState.GamePlaying)
			{
				return;
			}
			if (this.patrolInPath)
			{
				this.SnipePathPatrol(deltatime);
			}
			else
			{
				this.SnipeRadiusPatrol(deltatime, this.enemyTransform.position);
			}
			this.ExtraRotation(deltatime);
		}

		private void SnipePathPatrol(float deltaTime)
		{
			if (this.allSnipePatrolPoints == null || this.allSnipePatrolPoints.Count == 0)
			{
				return;
			}
			if (this.curSnipePatrolPoint.nextPoint == null)
			{
				return;
			}
			if (this.curSnipePatrolPoint.InWait())
			{
				this.curSnipePatrolPoint.DoLogic(deltaTime);
				if (!this.curSnipePatrolPoint.InWait())
				{
					this.DoChangeSnipePatrolAction();
				}
				return;
			}
			Vector3 vector = this.enemyTransform.position - this.curSnipePatrolPoint.nextPoint.position;
			vector.y = 0f;
			if (vector.sqrMagnitude < 0.0100000007f)
			{
				if (this.curSnipePatrolPoint == this.escapePoint)
				{
					this.OnEscaped();
				}
				else
				{
					this.snipePatrolIndex++;
					if (this.snipePatrolIndex == this.allSnipePatrolPoints.Count)
					{
						this.snipePatrolIndex = 0;
					}
					this.curSnipePatrolPoint = this.allSnipePatrolPoints[this.snipePatrolIndex];
					this.curSnipePatrolPoint.Reset();
					this.DoChangeSnipePatrolAction();
				}
			}
		}

		private void SnipeRadiusPatrol(float deltaTime, Vector3 pos)
		{
			if (!this.InRest(deltaTime, pos))
			{
				Vector3 vector = this.enemyTransform.position - this.snipePatrolTraget;
				vector.y = 0f;
				if (vector.sqrMagnitude < 0.0100000007f)
				{
					this.ChangeToSnipePatrolRest();
					this.ResetPatrolRestTime();
				}
			}
		}

		private void ExtraRotation(float deltaTime)
		{
			Vector3 vector = this.nav.steeringTarget - this.enemyTransform.position;
			if (vector != Vector3.zero)
			{
				this.enemyTransform.rotation = Quaternion.Slerp(this.enemyTransform.rotation, Quaternion.LookRotation(vector), 10f * deltaTime);
			}
		}

		public void OnArrivePathEnd()
		{
			if (this.curSnipePatrolPoint.removeAndDetectMission)
			{
				this.OnEscaped();
			}
		}

		public void OnEscaped()
		{
			this.gameScene.GetEnemies().Remove(this.enemyObject.name);
			this.nav.enabled = false;
			this.enemyObject.SetActive(false);
			this.gameScene.CheckEnemyEscapedOrReachPathEnd();
		}

		public void SetSnipePatrolInfo(BaseEnemySpawnInfo _info)
		{
			this.allSnipePatrolPoints = _info.snipePatrolInfos;
			this.snipePatrolIndex = 0;
			if (this.allSnipePatrolPoints.Count > 0)
			{
				this.curSnipePatrolPoint = this.allSnipePatrolPoints[0];
				this.curSnipePatrolPoint.Reset();
			}
			this.escapePoint = this.allSnipePatrolPoints.Find((SnipePatrolPointInfo _snipePatrolPoint) => _snipePatrolPoint.isEscape);
			this.allSnipePatrolPoints.Remove(this.escapePoint);
			this.snipePatrolCenter = _info.trans.position;
			this.snipePatrolRadius = _info.snipePatrolRadius;
			this.patrolInPath = (this.allSnipePatrolPoints.Count > 0 && !this.allSnipePatrolPoints[0].isEscape);
		}

		public void DoChangeSnipePatrolAction()
		{
			if (this.patrolInPath)
			{
				if (this.curSnipePatrolPoint.InWait())
				{
					this.animator.CrossFade(this.curSnipePatrolPoint.waitAction, 0.05f);
					this.nav.speed = 0f;
					this.nav.enabled = true;
				}
				else if (this.curSnipePatrolPoint.removeAndDetectMission)
				{
					this.OnEscaped();
				}
				else
				{
					this.animator.CrossFade(this.curSnipePatrolPoint.patrolAnimationName, 0.05f);
					this.nav.speed = this.curSnipePatrolPoint.speed;
					this.nav.enabled = true;
					this.animator.speed = 1f;
					if (this.curSnipePatrolPoint.nextPoint != null && this.nav.enabled)
					{
						this.nav.SetDestination(this.curSnipePatrolPoint.nextPoint.position);
					}
				}
			}
			else
			{
				this.ResetPatrolRestTime();
				this.ChangeToSnipePatrolRest();
			}
		}

		public void DoChangeToEscapeAction()
		{
			if (this.escapePoint != null)
			{
				this.curSnipePatrolPoint = this.escapePoint;
				this.curSnipePatrolPoint.Reset();
				if (!this.patrolInPath)
				{
					this.patrolInPath = true;
				}
			}
		}

		public bool DoFindPatrolPoint(Vector3 currentPos)
		{
			Vector3 sourcePosition = this.snipePatrolCenter + UnityEngine.Random.insideUnitSphere * this.snipePatrolRadius;
			NavMeshHit navMeshHit;
			if (!NavMesh.SamplePosition(sourcePosition, out navMeshHit, this.snipePatrolRadius, 1))
			{
				return false;
			}
			Vector3 position = navMeshHit.position;
			float sqrMagnitude = (position - currentPos).sqrMagnitude;
			float num = this.snipePatrolRadius * 0.3333f * this.snipePatrolRadius * 0.33333f;
			if (sqrMagnitude < num)
			{
				return false;
			}
			this.snipePatrolTraget = position;
			return true;
		}

		public bool InRest(float deltaTime, Vector3 pos)
		{
			if (this.snipePatrolRestTime > 0f)
			{
				this.CaculatePatrolRestTime(deltaTime, pos);
				return true;
			}
			return false;
		}

		private void CaculatePatrolRestTime(float deltaTime, Vector3 pos)
		{
			if (this.findPatrolPointInRest)
			{
				this.snipePatrolRestTime -= deltaTime;
				if (this.snipePatrolRestTime <= 0f)
				{
					this.ChangeToSnipePatrolWalk();
				}
			}
			else
			{
				this.findPatrolPointInRest = this.DoFindPatrolPoint(pos);
			}
		}

		private void ResetPatrolRestTime()
		{
			this.snipePatrolRestTime = UnityEngine.Random.Range(1f, 6f);
			this.findPatrolPointInRest = false;
		}

		private void ChangeToSnipePatrolRest()
		{
			this.animator.CrossFade("SnipeActions.SnipeStand", 0.1f);
			this.nav.speed = 0f;
		}

		private void ChangeToSnipePatrolWalk()
		{
			this.animator.CrossFade("SnipeActions.SnipeWalk", 0.1f);
			this.nav.enabled = true;
			this.nav.speed = 1f;
			this.nav.SetDestination(this.snipePatrolTraget);
		}

		public virtual void DoMove(float deltaTime)
		{
			if (this.gameScene.PlayingState != PlayingState.GamePlaying && this.gameScene.PlayingState != PlayingState.WaitForEnd)
			{
				return;
			}
			this.findPathScheduler.DoUpdate(deltaTime);
			this.checkTargetSchduler.DoUpdate(deltaTime);
			this.nav.Move(this.enemyTransform.forward * this.walkSpeed * deltaTime);
			if (this.nav.path.corners.Length >= 2)
			{
				Vector3 vector = this.nav.path.corners[1] - this.enemyTransform.position;
				vector.y = 0f;
				if (this.CheckGetObstacle(vector))
				{
					if (this.toClimb != ClimbType.JUMP || (this.enemyTransform.position - this.nav.currentOffMeshLinkData.startPos).sqrMagnitude >= 0.0100000007f)
					{
						return;
					}
					this.SetState(Enemy.JUMP_STATE, true);
				}
				this.enemyTransform.rotation = Quaternion.Lerp(this.enemyTransform.rotation, Quaternion.LookRotation(vector), deltaTime * 10f);
			}
		}

		public void DoFindPath()
		{
			this.nav.SetDestination(this.target.GetTransform().position);
		}

		public virtual void DoCheckTarget()
		{
			if (this.target != null && this.target.GetTargetType() == EnemyTargetType.FOCUS_SENSOR)
			{
				return;
			}
			if (this.forceFocusTarget != null)
			{
				this.target = this.forceFocusTarget;
			}
			else
			{
				this.target = this.gameScene.CheckTarget(this.enemyTransform.position, this.attackRange);
			}
		}

		public virtual bool CheckGetObstacle(Vector3 delta)
		{
			if (this.nav.isOnOffMeshLink && this.nav.currentOffMeshLinkData.offMeshLink != null)
			{
				this.nav.ActivateCurrentOffMeshLink(false);
				int area = this.nav.currentOffMeshLinkData.offMeshLink.area;
				if (area != 9)
				{
					if (area != 10)
					{
						if (area == 12)
						{
							this.toClimb = ClimbType.JUMP;
						}
					}
					else
					{
						this.toClimb = ClimbType.DOWN;
					}
				}
				else
				{
					this.toClimb = ClimbType.UP;
				}
				if (this.toClimb == ClimbType.UP || this.toClimb == ClimbType.DOWN)
				{
					this.SetState(Enemy.CLIMB_STATE, true);
				}
				this.isStopFixOffset = false;
				return true;
			}
			return false;
		}

		public void StopFixOffset()
		{
			this.isStopFixOffset = true;
		}

		public virtual void DoClimb(float deltaTime)
		{
			Vector3 normalized = (this.nav.currentOffMeshLinkData.endPos - this.nav.currentOffMeshLinkData.startPos).normalized;
			Vector3 forward = this.nav.currentOffMeshLinkData.startPos + normalized * 1000f - this.enemyTransform.position;
			if (!this.isStopFixOffset)
			{
				this.climbOffset = this.nav.currentOffMeshLinkData.startPos - this.enemyTransform.position;
				this.climbOffset.y = 0f;
				Vector3 a = this.climbOffset;
				this.climbOffset = Vector3.Lerp(this.climbOffset, Vector3.zero, deltaTime * 5f);
				this.enemyObject.transform.Translate(a - this.climbOffset, Space.World);
			}
			if (Quaternion.Dot(this.enemyTransform.rotation, Quaternion.LookRotation(forward)) < 1f)
			{
				this.enemyTransform.rotation = Quaternion.Lerp(this.enemyTransform.rotation, Quaternion.LookRotation(forward), deltaTime * 10f);
			}
		}

		public virtual void DoJump(float deltaTime)
		{
			this.nav.SetDestination(this.nav.currentOffMeshLinkData.endPos);
			Vector3 normalized = (this.nav.currentOffMeshLinkData.endPos - this.nav.currentOffMeshLinkData.startPos).normalized;
			normalized.y = 0f;
			Vector3 vector = this.nav.currentOffMeshLinkData.startPos + normalized * 1000f - this.enemyTransform.position;
			if (Quaternion.Dot(this.enemyTransform.rotation, Quaternion.LookRotation(normalized)) < 1f)
			{
				this.enemyTransform.rotation = Quaternion.Lerp(this.enemyTransform.rotation, Quaternion.LookRotation(normalized), deltaTime * 10f);
			}
			if (this.animator.GetBool("DoJumpOffset"))
			{
				this.jumpSpeedLerp = Mathf.Lerp(this.jumpSpeedLerp, 3.5f, deltaTime * 5f);
				this.nav.Move((this.nav.currentOffMeshLinkData.endPos - this.enemyTransform.position).normalized * 3.5f * this.walkSpeed * deltaTime);
				if ((this.enemyTransform.position - this.nav.currentOffMeshLinkData.endPos).magnitude < 0.1f)
				{
					this.animator.SetTrigger("OnGround");
					this.nav.CompleteOffMeshLink();
					this.SetState(Enemy.CATCHING_STATE, true);
				}
			}
		}

		public void StartDoJumpOffset()
		{
			this.animator.SetBool("DoJumpOffset", true);
		}

		public void JumpStateReset()
		{
			this.animator.SetBool("DoJumpOffset", false);
		}

		public virtual void FixAndCompleteOffMeshLink()
		{
			if (this.nav.currentOffMeshLinkData.offMeshLink == null)
			{
				return;
			}
			float sqrMagnitude = (this.enemyTransform.position - this.nav.currentOffMeshLinkData.offMeshLink.startTransform.position).sqrMagnitude;
			float sqrMagnitude2 = (this.enemyTransform.position - this.nav.currentOffMeshLinkData.offMeshLink.endTransform.position).sqrMagnitude;
			Transform transform = (sqrMagnitude <= sqrMagnitude2) ? this.nav.currentOffMeshLinkData.offMeshLink.startTransform : this.nav.currentOffMeshLinkData.offMeshLink.endTransform;
			Vector3 position = transform.position;
			transform.position = this.enemyTransform.position;
			this.attr.StartCoroutine(this.CompleteCurOffMeshLink(transform, position, this.nav.currentOffMeshLinkData.offMeshLink));
			this.nav.currentOffMeshLinkData.offMeshLink.UpdatePositions();
		}

		private IEnumerator CompleteCurOffMeshLink(Transform trans, Vector3 originalPos, OffMeshLink link)
		{
			yield return null;
			this.nav.CompleteOffMeshLink();
			yield return null;
			trans.position = originalPos;
			link.UpdatePositions();
			yield break;
		}

		public virtual void DoLogic(float deltaTime)
		{
			this.state.NextState(this, deltaTime);
			if (this.nav.enabled)
			{
				this.nav.avoidancePriority = (int)this.SqrDistanceFromPlayer;
			}
			if (this.state != Enemy.DEAD_STATE)
			{
				this.attr.hpGameObject.rotation = this.gameScene.GetCamera().CameraTransform.rotation;
				this.attr.hpGameObject.gameObject.SetActive(Time.time - this.gotHitTime < 2f);
			}
		}

		public virtual void DoDizziness(float deltaTime)
		{
		}

		public void SetDieCallBack(Action<string> _dieCallBack)
		{
			this.dieCallBack = _dieCallBack;
		}

		public void SetWakeupCallback(Action callback)
		{
			this.wakeupCallback = callback;
		}

		public void SetSnipePatrolOnHitCallback(Action<Enemy> callback)
		{
			this.snipePatrolOnHitCallback = callback;
		}

		public bool CheckWakeUpRange()
		{
			return this.SqrDistanceFromPlayer <= this.wakeupRange * this.wakeupRange;
		}

		public virtual void RemoveImmediatelyWithCallBack()
		{
			if (this.dieCallBack != null)
			{
				this.dieCallBack(this.Name);
			}
			this.ResetBoneNames();
			this.gameScene.GetEnemies().Remove(this.enemyObject.name);
			this.nav.enabled = false;
			this.enemyObject.SetActive(false);
		}

		public void WakeUp()
		{
			StateKey key = this.state.key;
			if (key != StateKey.PATROL_STATE)
			{
				if (key == StateKey.WAIT_STATE)
				{
					this.SetState(Enemy.WAKEUP_STATE, true);
				}
			}
			else
			{
				this.SetState(Enemy.CATCHING_STATE, true);
			}
			if (this.wakeupCallback != null)
			{
				this.wakeupCallback();
			}
		}

		public virtual void RotateEnemyToTarget()
		{
			this.enemyObject.transform.forward = Vector3.Lerp(this.enemyObject.transform.forward, this.target.GetTransform().position - this.enemyObject.transform.position, 5f * Time.deltaTime);
		}

		public virtual void InitEnemyAttribute(GameObject _obj, bool armored = false, bool armed = false, bool redEye = false)
		{
			this.attr = _obj.GetComponent<EnemyAttr>();
			this.attr.SetEnemy(this);
			_obj.GetComponentInChildren<EnemyAnimatorEventListener>().enemy = this;
			this.dataType = this.EnemyType2EnemyDataType(this.enemyType);
			this.isArmored = armored;
			this.isArmed = armed;
			this.isRedEye = redEye;
			this.armorID = ((!this.isArmored) ? -1 : UnityEngine.Random.Range(0, this.attr.armors.Count));
			this.armID = ((!this.isArmed) ? -1 : UnityEngine.Random.Range(0, this.attr.weapons.Length));
			for (int i = 0; i < this.attr.armors.Count; i++)
			{
				for (int j = 0; j < this.attr.armors[i].armors.Length; j++)
				{
					this.attr.armors[i].armors[j].SetActive(i == this.armorID);
				}
			}
			for (int k = 0; k < this.attr.weapons.Length; k++)
			{
				this.attr.weapons[k].SetActive(k == this.armID);
			}
			for (int l = 0; l < this.attr.redEye.Length; l++)
			{
				this.attr.redEye[l].startColor = ((!this.isRedEye) ? ColorName.GREEN : ColorName.RED);
				this.attr.redEye[l].gameObject.SetActive(true);
			}
			float num = (float)CheckpointDataManager.SelectCheckpoint.RequireFighting;
			float num2 = this.player.MaxWeaponFightPower / num;
			float num3 = (num2 >= 0.8f) ? ((num2 < 1f) ? 3f : 1f) : 6f;
			this.hp = EnemyDataManager.GetEnemyHp(this.dataType) * num3;
			this.attackDamage = EnemyDataManager.GetEnemyDamage(this.dataType) * num3;
			this.MaxHp = this.hp;
			this.preHp = this.hp;
			this.attackRange = this.attr._attackRange;
			this.attackAniNum = this.attr.attackAniNum;
			this.walkAniNum = this.attr.walkAniNum;
			this.attr.hpSlider.localScale = Vector3.one;
			this.attr.hpGameObject.gameObject.SetActive(false);
			this.walkSpeed = ((!this.isRedEye) ? this.attr.walkSpeed : this.attr.runSpeed);
			if (this.attr.needCombineMesh && !this.isArmored && !this.isArmed)
			{
				this.attr.CombineMesh(new Bone[0]).layer = 9;
			}
		}

		public void DoChange2BulletTime(bool isInBulletTime)
		{
			SkinnedMeshRenderer[] componentsInChildren = this.enemyObject.GetComponentsInChildren<SkinnedMeshRenderer>();
			if (isInBulletTime)
			{
				Shader shader = Shader.Find("Custom/Cullmask");
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					Material[] materials = componentsInChildren[i].materials;
					foreach (Material material in materials)
					{
						material.shader = shader;
						material.SetColor("_maskColor", Color.red);
					}
				}
			}
			else
			{
				Shader shader2 = Shader.Find("Mobile/Diffuse");
				for (int k = 0; k < componentsInChildren.Length; k++)
				{
					Material[] materials2 = componentsInChildren[k].materials;
					foreach (Material material2 in materials2)
					{
						material2.shader = shader2;
					}
				}
			}
			this.animator.speed = ((!isInBulletTime) ? 1f : 0.5f);
		}

		public void Pause()
		{
			if (this.GetState() == Enemy.CATCHING_STATE)
			{
				this.SetState(Enemy.FORCE_IDLE, true);
			}
			this.audio.PauseAllAudios();
		}

		public void Resume()
		{
			if (this.GetState() == Enemy.FORCE_IDLE)
			{
				this.SetState(Enemy.CATCHING_STATE, true);
			}
			this.audio.ResumeAllAudios();
		}

		public void Freeze()
		{
			this.nav.speed = 0f;
			this.nav.enabled = false;
			this.animator.speed = 0f;
		}

		public void IncreaseEndlessModeHP(int waveNum)
		{
			this.MaxHp = (this.hp *= (float)(1 + waveNum));
		}

		public virtual void RandomAttackAni()
		{
			this.SetEnemyActionID("AttackID", UnityEngine.Random.Range(0, this.attackAniNum));
		}

		public virtual void RandomGotHitAni(Bone bone)
		{
			this.SetEnemyActionID("HurtBone", (int)bone);
		}

		public void RandomDeadAni()
		{
		}

		protected void SetBoneNames(string _name)
		{
			int i = 0;
			int num = this.attr.bones.Length;
			while (i < num)
			{
				StringBuilder stringBuilder = new StringBuilder(_name);
				stringBuilder.Append("|");
				StringBuilder stringBuilder2 = stringBuilder;
				Bone bone = (Bone)i;
				stringBuilder2.Append(bone.ToString());
				this.attr.bones[i].name = stringBuilder.ToString();
				i++;
			}
			int j = 0;
			int num2 = this.attr.specialBones.Length;
			while (j < num2)
			{
				StringBuilder stringBuilder = new StringBuilder(_name);
				stringBuilder.Append("|");
				stringBuilder.Append(Bone.MiddleSpine.ToString());
				this.attr.specialBones[j].name = stringBuilder.ToString();
				j++;
			}
		}

		public virtual void ResetBoneNames()
		{
			int i = 0;
			int num = this.attr.bones.Length;
			while (i < num)
			{
				this.attr.bones[i].name = this.attr.boneNames[i];
				i++;
			}
			int j = 0;
			int num2 = this.attr.specialBones.Length;
			while (j < num2)
			{
				this.attr.specialBones[j].name = this.attr.specialBoneNames[j];
				j++;
			}
		}

		public Transform GetBoneTransform(Bone bone)
		{
			return this.attr.bones[(int)bone].transform;
		}

		public void InitAnimatorParameter(string name, int id)
		{
			this.animator.SetInteger(name, id);
		}

		public void SetEnemyHpToAnimator()
		{
			this.animator.SetFloat("hp", this.hp);
		}

		public void SetEnemyActionID(string param, int id)
		{
			this.animator.SetInteger(param, id);
		}

		public virtual void InitAudios()
		{
			this.audio = new AudioPlayer();
			Transform folderTrans = this.enemyTransform.Find("Audio");
			this.audio.AddAudio(folderTrans, "Appear");
			this.audio.AddAudio(folderTrans, "Shout");
			this.audio.AddAudio(folderTrans, "Walk");
			this.audio.AddAudio(folderTrans, "Attack");
			this.audio.AddAudio(folderTrans, "Attack2");
			this.audio.AddAudio(folderTrans, "RushStart");
			this.audio.AddAudio(folderTrans, "Rush");
			this.audio.AddAudio(folderTrans, "Hurt");
			this.audio.AddAudio(folderTrans, "Dead");
			this.audio.AddAudio(folderTrans, "ThrowStart");
			this.audio.AddAudio(folderTrans, "Throw");
		}

		public void PlayAudio(string audioName)
		{
			this.audio.PlayAudio(audioName);
		}

		public void StopAudio(string audioName)
		{
			this.audio.StopAudio(audioName);
		}

		private void InitAnimatorListener()
		{
			BaseListener[] behaviours = this.animator.GetBehaviours<BaseListener>();
			for (int i = 0; i < behaviours.Length; i++)
			{
				behaviours[i].SetEnemy(this);
			}
		}

		public void SetAnimation(E_ANIMATION ani)
		{
			switch (ani)
			{
			case E_ANIMATION.COMEOUT:
				this.animator.SetInteger("ActionID", 1);
				break;
			case E_ANIMATION.WAIT:
				this.animator.SetInteger("ActionID", 2);
				break;
			case E_ANIMATION.WAKEUP:
				this.animator.SetTrigger("OnWakeup");
				break;
			case E_ANIMATION.WALK:
				this.animator.SetInteger("ActionID", 4);
				break;
			case E_ANIMATION.ATTACK:
				this.animator.SetInteger("AttackID", UnityEngine.Random.Range(0, this.attr.attackAniNum));
				this.animator.SetInteger("ActionID", 5);
				this.animator.SetTrigger("OnAttack");
				break;
			case E_ANIMATION.HURT:
				this.preHurtBone = this.hurtBone;
				this.animator.SetBool("MiddleSpineRight", this.middleSpineRight);
				this.animator.SetInteger("HurtBone", (int)this.hurtBone);
				this.animator.SetTrigger("OnHurt");
				break;
			case E_ANIMATION.DIE:
				this.animator.SetTrigger("OnDie");
				break;
			case E_ANIMATION.ANGERY:
				this.animator.SetBool("Angery", true);
				this.animator.SetTrigger("OnAngery");
				break;
			case E_ANIMATION.CLIMB:
				if (this.toClimb == ClimbType.UP)
				{
					this.animator.SetTrigger("OnClimb");
				}
				else
				{
					this.animator.SetTrigger("OnStepOut");
				}
				break;
			case E_ANIMATION.RUSH_START:
				this.animator.SetTrigger("OnRush");
				break;
			case E_ANIMATION.RUSH_END:
				this.animator.SetTrigger("OnRushEnd");
				break;
			case E_ANIMATION.DIZZINESS:
				this.animator.SetTrigger("OnDizziness");
				break;
			case E_ANIMATION.EXPLOD:
				this.animator.SetTrigger("OnExplod");
				break;
			case E_ANIMATION.SKILL:
				this.animator.SetTrigger("OnSkill");
				break;
			case E_ANIMATION.FORCE_IDLE:
				this.animator.SetInteger("ActionID", 16);
				this.animator.SetTrigger("ForceIdle");
				break;
			case E_ANIMATION.THROW:
				this.animator.SetTrigger("Throw");
				break;
			case E_ANIMATION.SHOUT:
				this.animator.SetTrigger("Shout");
				break;
			case E_ANIMATION.PATROL:
				this.animator.SetInteger("ActionID", 19);
				break;
			case E_ANIMATION.JUMP:
				this.animator.SetInteger("ActionID", 20);
				break;
			}
		}

		public virtual void OnAttackEventFrame(bool isLeftHand)
		{
			this.CheckHit(isLeftHand);
		}

		public virtual void OnAttackEventFrame()
		{
			this.CheckHit();
		}

		public void OnEnemyAttackBoxEnable(int boxID)
		{
			if (boxID == -1)
			{
				return;
			}
			if (this.attr.attackBox == null || boxID >= this.attr.attackBox.Length)
			{
				return;
			}
			this.attackAttribute = new AttackAttribute(this.attr.oneShotKill[boxID], this.attr.isCritical[boxID]);
			this.attr.attackBox[boxID].enabled = true;
		}

		public void OnEnemyAttackBoxDisable()
		{
			if (this.attr.attackBox == null)
			{
				return;
			}
			for (int i = 0; i < this.attr.attackBox.Length; i++)
			{
				this.attr.attackBox[i].enabled = false;
			}
		}

		public void OnPlayParticle(int index)
		{
			this.attr.allParticles[index].Play();
		}

		public virtual void OnSpecialEventFrame(int id = 0)
		{
		}

		public void ShakeCamera(int _id)
		{
			this.gameScene.ShakeMainCamera((CameraShakeType)_id);
		}

		public static EnemyType GetEnemyTypeByName(string name)
		{
			IEnumerator enumerator = Enum.GetValues(typeof(EnemyType)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					EnemyType result = (EnemyType)obj;
					if (name == result.ToString())
					{
						return result;
					}
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
			return EnemyType.E_NONE;
		}

		private void ClearAllAnimationStateTag()
		{
			for (int i = 0; i < this.animationState.Length; i++)
			{
				this.animationState[i] = false;
			}
		}

		public void SetVisible(bool visible)
		{
			if (!visible)
			{
				this.allRenders = this.enemyObject.GetComponentsInChildren<Renderer>();
			}
			if (this.allRenders == null)
			{
				return;
			}
			for (int i = 0; i < this.allRenders.Length; i++)
			{
				this.allRenders[i].enabled = visible;
			}
			if (visible)
			{
				this.allRenders = null;
			}
		}

		public global::EnemyProbability GetEnemyProbability()
		{
			global::EnemyProbability result = global::EnemyProbability.NORMAL;
			switch (this.enemyType)
			{
			case EnemyType.E_SPITTER:
			case EnemyType.E_BUTCHER:
			case EnemyType.E_BOMBER:
				result = global::EnemyProbability.ELITE;
				break;
			case EnemyType.E_DESPOT:
			case EnemyType.E_BOSS_FAT:
			case EnemyType.E_BOSS_BUTCHER:
				result = global::EnemyProbability.BOSS;
				break;
			}
			return result;
		}

		public static global::EnemyProbability GetEnemyProbabailityByEnemyType(EnemyType type)
		{
			global::EnemyProbability result = global::EnemyProbability.NORMAL;
			switch (type)
			{
			case EnemyType.E_SPITTER:
			case EnemyType.E_BUTCHER:
			case EnemyType.E_BOMBER:
				result = global::EnemyProbability.ELITE;
				break;
			case EnemyType.E_DESPOT:
			case EnemyType.E_BOSS_FAT:
			case EnemyType.E_BOSS_BUTCHER:
				result = global::EnemyProbability.BOSS;
				break;
			}
			return result;
		}

		public EnemyDataType EnemyType2EnemyDataType(EnemyType type)
		{
			EnemyDataType result;
			switch (type)
			{
			case EnemyType.E_SPITTER:
				result = EnemyDataType.SPITTER;
				break;
			case EnemyType.E_BUTCHER:
				result = EnemyDataType.BUTCHER;
				break;
			case EnemyType.E_BOMBER:
				result = EnemyDataType.BOMBER;
				break;
			case EnemyType.E_DESPOT:
				result = EnemyDataType.DESPOT;
				break;
			case EnemyType.E_BOSS_FAT:
				result = EnemyDataType.BOSS_BOMBER;
				break;
			case EnemyType.E_BOSS_BUTCHER:
				result = EnemyDataType.BOSS_BUTCHER;
				break;
			case EnemyType.E_BOMBER2:
				result = EnemyDataType.BOMBER2;
				break;
			default:
				result = ((!this.isArmored) ? ((!this.isArmed) ? EnemyDataType.NORMAL : EnemyDataType.ARMED) : EnemyDataType.ARMORED);
				break;
			}
			return result;
		}

		public static EnemyType EnemyDataType2EnemyType(EnemyDataType dataType)
		{
			EnemyType result = EnemyType.E_NONE;
			switch (dataType)
			{
			case EnemyDataType.NORMAL:
				result = EnemyType.NORMAL_RANDOM;
				break;
			case EnemyDataType.RED_EYE:
				result = EnemyType.REDEYE_RANDOM;
				break;
			case EnemyDataType.ARMORED:
				result = EnemyType.ARMORS_RAMDOM;
				break;
			case EnemyDataType.ARMED:
				result = EnemyType.WEAPONS_RANDOM;
				break;
			case EnemyDataType.BUTCHER:
				result = EnemyType.E_BUTCHER;
				break;
			case EnemyDataType.BOMBER:
				result = EnemyType.E_BOMBER;
				break;
			case EnemyDataType.SPITTER:
				result = EnemyType.E_SPITTER;
				break;
			case EnemyDataType.DESPOT:
				result = EnemyType.E_DESPOT;
				break;
			}
			return result;
		}

		public static EnemySpawnType GetSpawnType(EnemyType type)
		{
			EnemySpawnType result;
			switch (type)
			{
			case EnemyType.E_TSHIRT:
			case EnemyType.E_FAT1:
			case EnemyType.E_FAT2:
			case EnemyType.E_MAN1:
			case EnemyType.E_MAN2:
			case EnemyType.E_WOMAN1:
			case EnemyType.E_WOMAN2:
			case EnemyType.E_WOMAN3:
			case EnemyType.E_WOMAN4:
			case EnemyType.E_SPITTER:
			case EnemyType.E_BUTCHER:
			case EnemyType.E_BOMBER:
			case EnemyType.E_DESPOT:
			case EnemyType.E_BOSS_FAT:
			case EnemyType.E_BOSS_BUTCHER:
			case EnemyType.E_BOMBER2:
				result = new EnemySpawnType(type);
				break;
			default:
				switch (type)
				{
				case EnemyType.E_FAT_ARMOR:
					result = new EnemySpawnType(EnemyType.E_FAT1, EnemyType.E_FAT2, true, false, false);
					break;
				case EnemyType.E_FAT_WEAPON:
					result = new EnemySpawnType(EnemyType.E_FAT1, EnemyType.E_FAT2, false, true, false);
					break;
				case EnemyType.E_TSHIRT_ARMOR:
					result = new EnemySpawnType(EnemyType.E_TSHIRT, true, false, false);
					break;
				case EnemyType.E_TSHIRT_WEAPON:
					result = new EnemySpawnType(EnemyType.E_TSHIRT, false, true, false);
					break;
				case EnemyType.NORMAL_RANDOM:
					result = new EnemySpawnType(EnemyType.E_TSHIRT, EnemyType.E_WOMAN4, false, false, false);
					break;
				case EnemyType.ARMORS_RAMDOM:
					result = new EnemySpawnType(EnemyType.E_TSHIRT, EnemyType.E_FAT2, true, false, false);
					break;
				case EnemyType.WEAPONS_RANDOM:
					result = new EnemySpawnType(EnemyType.E_TSHIRT, EnemyType.E_FAT2, false, true, false);
					break;
				case EnemyType.REDEYE_NORMAL:
					result = new EnemySpawnType(EnemyType.E_TSHIRT, EnemyType.E_WOMAN4, false, false, true);
					break;
				case EnemyType.REDEYE_ARMOR:
					result = new EnemySpawnType(EnemyType.E_TSHIRT, EnemyType.E_FAT2, true, false, true);
					break;
				case EnemyType.REDEYE_WEAPON:
					result = new EnemySpawnType(EnemyType.E_TSHIRT, EnemyType.E_FAT2, false, true, true);
					break;
				case EnemyType.ELITE_RANDOM:
				{
					EnemyType type2 = (EnemyType)UnityEngine.Random.Range(119, 121);
					result = Enemy.GetSpawnType(type2);
					break;
				}
				case EnemyType.REDEYE_RANDOM:
				{
					EnemyType type3 = (EnemyType)UnityEngine.Random.Range(121, 124);
					result = Enemy.GetSpawnType(type3);
					break;
				}
				default:
					result = null;
					break;
				}
				break;
			}
			return result;
		}

		public static EnemyState IDLE_STATE = new IdleState();

		public static EnemyState CATCHING_STATE = new CatchingState();

		public static EnemyState GOTHIT_STATE = new GotHitState();

		public static EnemyState ATTACK_STATE = new AttackState();

		public static EnemyState DEAD_STATE = new DeadState();

		public static EnemyState WAIT_STATE = new WaitState();

		public static EnemyState WAKEUP_STATE = new WakeUpState();

		public static EnemyState COMEOUT_STATE = new ComeOutState();

		public static EnemyState CLIMB_STATE = new ClimbState();

		public static EnemyState FORCE_IDLE = new ForceIdleState();

		public static EnemyState PATROL_STATE = new PatrolState();

		public static EnemyState SNIPE_PATROL = new SnipePatrolState();

		public static EnemyState RUSH_START = new RushingStartState();

		public static EnemyState RUSH_ATTACK = new RushingAttackState();

		public static EnemyState RUSH_END = new RushingEndState();

		public static EnemyState DIZZINESS_STATE = new DizzinessState();

		public static EnemyState SKILL_STATE = new EnemySkillState();

		public static EnemyState JUMP_STATE = new JumpState();

		public static EnemyState THROW_STATE = new ThrowState();

		public static EnemyState SHOUT_STATE = new ShoutState();

		public static EnemyState BUTCHER_ATTACK = new BossAttack();

		protected bool[] animationState = new bool[21];

		protected GameObject enemyObject;

		protected Transform enemyTransform;

		protected Animator animator;

		protected Transform aimedTransform;

		protected ResourceConfigScript rConfig;

		protected EnemyType enemyType;

		protected EnemyDataType dataType;

		protected Vector3 lastTarget;

		protected GameScene gameScene;

		protected Player player;

		protected EnemyTarget target;

		protected AudioPlayer audio;

		protected EnemyAttr attr;

		protected Bone hurtBone;

		protected Vector3 lastHurtPos;

		protected WeaponType lastHurtWeapon;

		protected Bone preHurtBone = Bone.None;

		protected bool middleSpineRight;

		protected float hp;

		protected float preHp;

		protected float deadTime;

		protected string name;

		protected EnemyState state;

		protected EnemyState preState;

		protected float attackRange;

		protected float detectionRange;

		protected float wakeupRange;

		protected float attackDamage;

		protected AttackAttribute attackAttribute;

		protected int score;

		protected float lastAttackTime = -100f;

		protected float lookAroundStartTime;

		protected float comeoutTime;

		public float pathEndTime;

		protected int nextPoint = -1;

		protected string runAnimationName = "Run";

		protected GameObject targetObj;

		protected bool attacked;

		protected Quaternion deadRotation;

		protected Vector3 deadPosition;

		protected Vector3[] path;

		protected Action<string> dieCallBack;

		protected Action wakeupCallback;

		protected Action<Enemy> snipePatrolOnHitCallback;

		protected List<Transform> leadPath;

		protected int pathPointIndex;

		protected NavMeshAgent nav;

		protected ClimbType toClimb;

		protected Vector3 navPathTarget;

		protected Vector3 climbOffset;

		protected DamageProperty lastDP;

		protected TimeScheduler findPathScheduler;

		protected TimeScheduler checkTargetSchduler;

		protected bool isArmored;

		protected int armorID;

		protected bool isArmed;

		protected int armID;

		protected bool isRedEye;

		protected float attackInterval;

		protected float walkSpeed;

		protected Renderer[] allRenders;

		protected float gotHitTime = -999f;

		protected EnemyPatrolInfo patrolInfo;

		protected List<SnipePatrolPointInfo> allSnipePatrolPoints;

		protected SnipePatrolPointInfo escapePoint;

		protected SnipePatrolPointInfo curSnipePatrolPoint;

		protected bool keyEnemy;

		protected int snipePatrolIndex;

		protected float snipePatrolRestTime;

		protected float snipePatrolRadius = 3f;

		protected bool findPatrolPointInRest;

		protected Vector3 snipePatrolTraget = Vector3.zero;

		protected Vector3 snipePatrolCenter = Vector3.zero;

		protected bool patrolInPath;

		public RaycastHit rayhit;

		public float lastStateTime;

		[CNName("��ʾ��Χ")]
		public bool drawPath;

		protected int WalkID;

		protected int HurtID;

		protected int DieID;

		protected int attackAniNum;

		protected int walkAniNum;

		protected EnemyTarget forceFocusTarget;

		public float MaxHp;

		protected bool isStopFixOffset = true;

		protected float jumpSpeedLerp;
	}
}
