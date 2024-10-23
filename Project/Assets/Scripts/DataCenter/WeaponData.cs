using System;

namespace DataCenter
{
	[Serializable]
	public class WeaponData : ItemData
	{
		public WeaponType Type;

		public int MaxLevel;

		public int DebrisID;

		public int RequiredDebris;

		public ItemUnlockType UnlockType;

		public int UnlockPrice;

		public int ProducePrice;

		public int ProduceTime;

		public int[] Parts;

		public int Damage;

		public int Precise;

		public int Speed;

		public int Magazines;

		public int Bullets;

		public int ReloadingTime;

		public int ScopeTimes;

		public int Weight;

		public int Range;

		public int BasicPower;

		public string UiModel;

		public bool isNew;

		public WeaponState State;
	}
}
