using System;

namespace Zombie3D
{
	public interface IEnemySpawnTrigger
	{
		void DoTrigger();

		void DoLogic(float dt);

		void DoStop();
	}
}
