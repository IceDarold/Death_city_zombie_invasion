using System;

namespace DataCenter
{
	public class EquipmentData : ItemData
	{
		public int Part;

		public int SetID;

		public ItemUnlockType UnlockType;

		public int UnlockPrice;

		public int[] RequiredPrice;

		public int[] RequiredTime;

		public EquipmentAttribute AttributeType;

		public int[] AttributeValue;

		public EquipmentState State;

		public bool isNew;
	}
}
