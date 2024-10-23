using System;

namespace DataCenter
{
	[Serializable]
	public class ItemData
	{
		public int ID;

		public ItemType ItemTag;

		public string Name;

		public string Describe;

		public string Icon;

		public string Prefab;

		public int[] DropID;

		public int Level;

		public int Quality;

		public int Count;
	}
}
