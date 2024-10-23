using System;

namespace DataCenter
{
	[Serializable]
	public class WeaponSaveData
	{
		public int ID;

		public bool isNew;

		public bool isEquip;

		public int DebrisCount;

		public WeaponState State;

		public int Level;
	}
}
