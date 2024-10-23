using System;
using System.Collections;
using UnityEngine;

namespace Zombie3D
{
	public class Zombie_Boomer : Enemy
	{
		public override void Init(GameObject gObject, EnemyState state, int startAniID, bool armored = false, bool armed = false, bool redEye = false)
		{
			base.Init(gObject, state, startAniID, armored, armed, redEye);
			this.particleEffect.SetActive(false);
		}

		public override void InitAudios()
		{
			base.InitAudios();
		}

		public override void CheckHit()
		{
			this.upMesh.SetActive(false);
			this.particleEffect.SetActive(true);
			float sqrMagnitude = (this.enemyTransform.position - this.target.GetTransform().position).sqrMagnitude;
			if (sqrMagnitude < 25f)
			{
			}
			GameScene gameScene = GameApp.GetInstance().GetGameScene();
			IEnumerator enumerator = gameScene.GetEnemies().Values.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Enemy enemy = (Enemy)obj;
					if ((this.enemyTransform.position - enemy.GetTransform().position).sqrMagnitude < 25f)
					{
						EnemyType enemyType = enemy.EnemyType;
						DamageProperty dp = new DamageProperty(enemy.HP);
						enemy.OnHit(dp, WeaponType.NoGun, Bone.None);
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
		}

		public override void DoLogic(float deltaTime)
		{
			base.DoLogic(deltaTime);
		}

		public override void OnAttack()
		{
			base.OnAttack();
			base.SetAnimation(E_ANIMATION.ATTACK);
			this.lastAttackTime = Time.time;
		}

		public override EnemyState EnterSpecialState(float deltaTime)
		{
			this.deadTime = Time.time;
			return Enemy.DEAD_STATE;
		}

		public override void OnDead()
		{
			base.OnDead();
		}

		public override void OnHit(DamageProperty dp, WeaponType weaponType, Vector3 pos, Bone _bone)
		{
			if (this.startExploed)
			{
				return;
			}
			UnityEngine.Object.Instantiate<GameObject>(this.rConfig.hitBlood, pos, Quaternion.identity);
			this.preHp = this.hp;
			this.hp -= dp.damage;
			if (this.state == Enemy.WAIT_STATE)
			{
				this.state.OnHit(this);
			}
			base.SetEnemyHpToAnimator();
		}

		private bool startExploed;

		private GameObject upMesh;

		private GameObject particleEffect;
	}
}
