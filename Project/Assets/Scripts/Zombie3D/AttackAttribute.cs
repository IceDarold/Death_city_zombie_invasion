using System;

namespace Zombie3D
{
	public class AttackAttribute
	{
		private AttackAttribute()
		{
		}

		public AttackAttribute(bool _oneShotKill, bool _isCritical)
		{
			this.oneShotKill = _oneShotKill;
			this.isCritical = _isCritical;
		}

		public bool oneShotKill;

		public bool isCritical;
	}
}
