using System;
using DataCenter;
using UnityEngine;
using UnityEngine.AI;

namespace Zombie3D
{
	public class BossBoomer : Boomer
	{
		public override void Init(GameObject gObject, EnemyState state, int startAniID, bool armored = false, bool armed = false, bool redEye = false)
		{
			this.bossAttr = gObject.GetComponent<BossBoomerAttr>();
			this.skillCount = this.bossAttr.skillCD;
			this.shoutTime = this.bossAttr.shoutTime;
			this.shoutHurtInterval = this.bossAttr.shoutHurtInterval;
			this.callZombieInterval = this.bossAttr.callZombieInterval;
			base.Init(gObject, state, startAniID, armored, armed, redEye);
		}

		public override void InitEnemyAttribute(GameObject _obj, bool armored = false, bool armed = false, bool redEye = false)
		{
			base.InitEnemyAttribute(_obj, armored, armed, redEye);
		}

		public override void DoLogic(float deltaTime)
		{
			base.DoLogic(deltaTime);
			this.DoCallZombies(deltaTime);
		}

		public override void OnDead()
		{
			base.OnDead();
		}

		public override void RemoveDeadBodyTimer(float dt)
		{
			base.RemoveDeadBodyTimer(dt);
		}

		public override void OnHit(DamageProperty dp, WeaponType weaponType, Vector3 pos, Bone _bone)
		{
			if (this.hp < 0f)
			{
				return;
			}
			base.OnHit(dp, weaponType, pos, _bone);
			if (CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.BOSS)
			{
				Singleton<DropItemManager>.Instance.ShowBossHitDrop(this.attr.bones[9].transform.position);
			}
			this.gethurtHp += dp.damage;
			if (this.state == Enemy.SHOUT_STATE || this.state == Enemy.DIZZINESS_STATE)
			{
				return;
			}
			if (this.CheckHp() && this.state != Enemy.SHOUT_STATE && this.state != Enemy.DEAD_STATE)
			{
				this.gethurtHp = 0f;
				this.shoutTime = this.bossAttr.shoutTime;
				this.shoutHurtInterval = this.bossAttr.shoutHurtInterval;
				base.SetState(Enemy.SHOUT_STATE, true);
				this.gameScene.SetUIDisplayEvnt(UIDisplayEvnt.SHOW_BOSS_SKILL_EFFECT, new float[]
				{
					1f
				});
			}
		}

		private bool CheckHp()
		{
			return this.gethurtHp / this.MaxHp >= this.bossAttr.shoutHurtThreadhold;
		}

		public override void OnAttack()
		{
			base.OnAttack();
		}

		public override void DoMove(float deltaTime)
		{
			base.DoMove(deltaTime);
			this.CheckSkill(deltaTime);
		}

		private void CheckSkill(float dt)
		{
			this.skillCount -= dt;
			if (this.skillCount <= 0f && base.DistancesFromPlayer > this.attackRange)
			{
				this.skillCount = this.bossAttr.skillCD;
				float distancesFromPlayer = base.DistancesFromPlayer;
				if (distancesFromPlayer >= this.bossAttr.oilRange)
				{
					if (UnityEngine.Random.Range(0f, 1f) >= 0.5f)
					{
						this.lockedPlayerPosition = this.player.GetTransform().position;
						base.SetState(Enemy.THROW_STATE, true);
					}
					else
					{
						this.StartCallZombies(true);
					}
				}
				else
				{
					this.StartCallZombies(true);
				}
			}
		}

		public override void DoDizziness(float deltaTime)
		{
			this.StartCallZombies(false);
		}

		private void StartCallZombies(bool caculateLimit = true)
		{
			if (caculateLimit && this.zombieInScene > this.bossAttr.callZombieLimit)
			{
				return;
			}
			if (caculateLimit)
			{
				this.callZombieNum = ((this.bossAttr.callZombieLimit - this.zombieInScene <= this.bossAttr.callNumOnce) ? (this.bossAttr.callZombieLimit - this.zombieInScene) : this.bossAttr.callNumOnce);
			}
			else
			{
				this.callZombieNum = this.bossAttr.callNumOnce;
			}
			this.callZombieInterval = this.bossAttr.callZombieInterval;
		}

		public void DoCallZombies(float deltaTime)
		{
			if (this.gameScene.PlayingState != PlayingState.GamePlaying)
			{
				return;
			}
			if (this.callZombieNum <= 0)
			{
				return;
			}
			this.callZombieInterval -= deltaTime;
			if (this.callZombieInterval > 0f)
			{
				return;
			}
			this.callZombieInterval = this.bossAttr.callZombieInterval;
			Vector3 pos = Vector3.zero;
			if (!this.RandomPoint(this.enemyTransform.position, this.attackRange, out pos))
			{
				return;
			}
			this.callZombieNum--;
			EnemySpawnType spawnType = Enemy.GetSpawnType(EnemyType.REDEYE_NORMAL);
			GameApp.GetInstance().GetGameScene().GetEnemyPool(spawnType.eType).CreateObject(pos, this.enemyTransform.rotation, delegate(GameObject enemyObject)
			{
				this.OnEnemyCreated(enemyObject, spawnType, pos, Enemy.COMEOUT_STATE, 1, null);
			});
		}

		public bool DoShout(float deltaTime)
		{
			this.shoutTime -= deltaTime;
			this.shoutHurtInterval -= deltaTime;
			if (this.shoutHurtInterval <= 0f)
			{
				this.shoutHurtInterval = this.bossAttr.shoutHurtInterval;
				this.DoShout();
			}
			return this.shoutTime <= 0f;
		}

		private bool RandomPoint(Vector3 center, float range, out Vector3 result)
		{
			Vector3 sourcePosition = center + UnityEngine.Random.insideUnitSphere * range;
			NavMeshHit navMeshHit;
			if (NavMesh.SamplePosition(sourcePosition, out navMeshHit, 1f, -1))
			{
				result = navMeshHit.position;
				return true;
			}
			result = Vector3.zero;
			return false;
		}

		private void OnEnemyCreated(GameObject curEnemy, EnemySpawnType spawnType, Vector3 pos, EnemyState state, int startAniID, Action<Enemy> afterCreate)
		{
			curEnemy.name = "E_" + GameApp.GetInstance().GetGameScene().GetNextEnemyID().ToString();
			Enemy enemy = BaseEnemySpawn.GetEnemy(spawnType.eType);
			enemy.EnemyType = spawnType.eType;
			enemy.Init(curEnemy, state, startAniID, spawnType.isArmored, spawnType.isArmed, spawnType.isRedEye);
			enemy.Name = curEnemy.name;
			enemy.SetTarget(GameApp.GetInstance().GetGameScene().CheckTarget(pos, enemy.AttackRange));
			if (afterCreate != null)
			{
				afterCreate(enemy);
			}
			GameApp.GetInstance().GetGameScene().GetEnemies().Add(enemy.Name, enemy);
			this.zombieInScene++;
			enemy.SetDieCallBack(delegate(string name)
			{
				this.zombieInScene--;
			});
		}

		public override void OnSpecialEventFrame(int id)
		{
			if (id != 0)
			{
				if (id != 1)
				{
					if (id == 2)
					{
						this.DoThrowOilDrum(this.lockedPlayerPosition);
						this.DoThrowOilDrum(this.lockedPlayerPosition + this.enemyTransform.right * this.bossAttr.bombAttackRange * 2f);
						this.DoThrowOilDrum(this.lockedPlayerPosition - this.enemyTransform.right * this.bossAttr.bombAttackRange * 2f);
					}
				}
			}
		}

		private void DoThrowOilDrum(Vector3 position)
		{
			BossOilDrum bossOilDrum = UnityEngine.Object.Instantiate<BossOilDrum>(Resources.Load<BossOilDrum>("Prefabs/SceneObject/BossOilDrum"), this.bossAttr.drumRoot.position, this.bossAttr.drumRoot.rotation);
			GameObject warning = UnityEngine.Object.Instantiate<GameObject>(this.bossAttr.skillBombWarning, position, Quaternion.identity);
			bossOilDrum.Init(position, this.bossAttr.damageBomb * this.attackDamage, this.bossAttr.bombAttackRange, this.bossAttr.ctrlOffset, warning, this.gameScene.GetCamera().cameraComponent);
		}

		private void DoShout()
		{
			float damage = this.player.GetMaxHp() * 0.05f;
			this.player.OnHit(damage, false, AttackType.NORMAL);
		}

		public override void DoExplod()
		{
			Array.ForEach<GameObject>(this.bossAttr.drums, delegate(GameObject obj)
			{
				obj.SetActive(false);
			});
			base.DoExplod();
		}

		private BossBoomerAttr bossAttr;

		private float skillCount;

		private Vector3 lockedPlayerPosition;

		private float shoutTime;

		private float shoutHurtInterval;

		private float gethurtHp;

		private float callZombieInterval;

		private int callZombieNum;

		protected int zombieInScene;

		private enum BossBoomerSpecialAction
		{
			EXPLOD,
			SHOUT,
			THROW
		}
	}
}
