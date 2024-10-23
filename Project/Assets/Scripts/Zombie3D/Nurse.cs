using System;
using UnityEngine;

namespace Zombie3D
{
	public class Nurse : Enemy
	{
		protected void RandomRunAnimation()
		{
			int num = UnityEngine.Random.Range(0, 100);
			if (num < 50)
			{
				this.runAnimationName = "Run";
			}
			else if (num < 75)
			{
				this.runAnimationName = "Forward01";
			}
			else
			{
				this.runAnimationName = "Forward02";
			}
		}

		public override void DoLogic(float deltaTime)
		{
			base.DoLogic(deltaTime);
			if (TimerManager.GetInstance().Ready(1))
			{
				TimerManager.GetInstance().Do(1);
			}
		}

		protected GameObject hitParticles;

		protected LineRenderer lineR;
	}
}
