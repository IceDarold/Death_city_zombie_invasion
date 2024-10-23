using System;
using UnityEngine;

namespace Zombie3D
{
	public class EnemySpawnType
	{
		protected EnemySpawnType()
		{
		}

		public EnemySpawnType(EnemyType tp, bool armored, bool armed, bool redEye)
		{
			this.eType = tp;
			this.isArmored = armored;
			this.isArmed = armed;
			this.isRedEye = redEye;
		}

		public EnemySpawnType(EnemyType randomMin, EnemyType ramdomMax, bool armored, bool armed, bool redEye) : this((EnemyType)UnityEngine.Random.Range((int)randomMin, (int)(ramdomMax + 1)), armored, armed, redEye)
		{
		}

		public EnemySpawnType(EnemyType tp) : this(tp, false, false, false)
		{
		}

		public EnemyType eType = EnemyType.E_NONE;

		public bool isArmored;

		public bool isArmed;

		public bool isRedEye;
	}
}
