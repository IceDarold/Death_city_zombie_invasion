using System;

namespace Zombie3D
{
	public struct PlayerAttributeData
	{
		public PlayerAttributeData(float _attrHP, float _moveSpeed, float _attack, float _dodge, float _remoteDefense, float _bombDefense)
		{
			this.attrHP = _attrHP;
			this.moveSpeed = _moveSpeed;
			this.attack = _attack;
			this.dodge = _dodge;
			this.remoteDefense = _remoteDefense;
			this.bombDefense = _bombDefense;
		}

		public float attrHP;

		public float moveSpeed;

		public float attack;

		public float dodge;

		public float remoteDefense;

		public float bombDefense;
	}
}
