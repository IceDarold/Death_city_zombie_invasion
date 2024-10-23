using System;
using UnityEngine;

namespace Zombie3D
{
	public class Swat : Enemy
	{
		protected void RandomRunAnimation()
		{
			this.runAnimationName = "Run";
		}

		public override void DoLogic(float deltaTime)
		{
			base.DoLogic(deltaTime);
			if (TimerManager.GetInstance().Ready(1))
			{
				TimerManager.GetInstance().Do(1);
			}
		}

		public override void CheckHit()
		{
		}

		protected GameObject hitParticles;

		protected LineRenderer lineR;
	}
}
