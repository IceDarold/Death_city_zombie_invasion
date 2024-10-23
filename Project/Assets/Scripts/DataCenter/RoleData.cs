using System;

namespace DataCenter
{
	public class RoleData : ItemData
	{
		public CurrencyType PriceType;

		public int Price;

		public int TalentTree;

		public int HP;

		public int Speed;

		public int[] Attributes;

		public new int Level = 1;

		public bool Enable;

		public bool isNew;
	}
}
