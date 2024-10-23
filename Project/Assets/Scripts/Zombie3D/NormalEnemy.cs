using System;
using UnityEngine;

namespace Zombie3D
{
	public class NormalEnemy : Enemy
	{
		public override void Init(GameObject gObject, EnemyState state, int startAniID, bool armored = false, bool armed = false, bool redEye = false)
		{
			base.Init(gObject, state, startAniID, armored, armed, redEye);
			this.lastTarget = Vector3.zero;
			TimerManager.GetInstance().SetTimer(0, 8f, true);
		}

		public override void CheckHit(bool isLeftHand)
		{
		}

		public override void DoLogic(float deltaTime)
		{
			base.DoLogic(deltaTime);
		}

		public override void OnAttack()
		{
			base.OnAttack();
			base.SetAnimation(E_ANIMATION.ATTACK);
			this.attacked = false;
			this.lastAttackTime = Time.time;
			this.audio.PlayAudio("Attack");
		}
	}
}
