using System;
using UnityEngine;

namespace Zombie3D
{
	public class Zombie : Enemy
	{
		protected void RandomRunAnimation()
		{
			int num = UnityEngine.Random.Range(0, 10);
			if (num < 7)
			{
				this.runAnimationName = "Run";
			}
			else if (num == 7)
			{
				this.runAnimationName = "Forward01";
			}
			else if (num == 8)
			{
				this.runAnimationName = "Forward02";
			}
			else if (num == 9)
			{
				this.runAnimationName = "Forward03";
			}
		}

		public override void CheckHit()
		{
		}

		public override void DoLogic(float deltaTime)
		{
			base.DoLogic(deltaTime);
		}

		public override void OnAttack()
		{
			base.OnAttack();
			this.attacked = false;
			this.lastAttackTime = Time.time;
		}

		protected Collider leftHandCollider;

		protected Vector3 targetPosition;

		protected Vector3[] p = new Vector3[4];
	}
}
