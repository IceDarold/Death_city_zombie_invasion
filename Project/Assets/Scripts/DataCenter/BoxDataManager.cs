using System;
using System.Collections.Generic;
using UnityEngine;

namespace DataCenter
{
	public class BoxDataManager : MonoBehaviour
	{
		static BoxDataManager()
		{
			BoxDataManager.BoxList = DataManager.ParseXmlData<BoxData>("BoxData", "BoxDatas", "BoxData");
		}

		public static void CensusAllOpenCount(int type, int num)
		{
			PlayerPrefs.SetInt("BoxDatas" + type, BoxDataManager.getAllOpenCount(type) + num);
			PlayerPrefs.Save();
		}

		public static int getAllOpenCount(int num)
		{
			return PlayerPrefs.GetInt("BoxDatas" + num, 0);
		}

		private static List<BoxData> GetBoxDatasByType(int index, bool special = false)
		{
			List<BoxData> list = new List<BoxData>();
			for (int i = 0; i < BoxDataManager.BoxList.Count; i++)
			{
				if (special)
				{
					if (BoxDataManager.BoxList[i].LibraryTag[index] == 1 && BoxDataManager.BoxList[i].SpecialTag[index] == 1)
					{
						list.Add(BoxDataManager.BoxList[i]);
					}
				}
				else if (BoxDataManager.BoxList[i].LibraryTag[index] == 1)
				{
					list.Add(BoxDataManager.BoxList[i]);
				}
			}
			return list;
		}

		private static int GetWeightSum(List<BoxData> list, int index)
		{
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				num += list[i].Weight[index];
			}
			return num;
		}

		public static BoxData GetItemById(int id)
		{
			BoxData result = new BoxData();
			for (int i = 0; i < BoxDataManager.BoxList.Count; i++)
			{
				if (BoxDataManager.BoxList[i].ID == id)
				{
					result = BoxDataManager.BoxList[i];
				}
			}
			return result;
		}

		public static List<BoxData> GetBoxDatas(int index, bool special = false)
		{
			List<BoxData> boxDatasByType = BoxDataManager.GetBoxDatasByType(index, special);
			BoxDataManager.SetEffectiveLibrary(ref boxDatasByType);
			List<BoxData> list = new List<BoxData>();
			if (special)
			{
				while (list.Count < 4)
				{
					int index2 = UnityEngine.Random.Range(0, boxDatasByType.Count);
					if (list.Contains(boxDatasByType[index2]))
					{
						boxDatasByType.RemoveAt(index2);
					}
					else
					{
						list.Add(boxDatasByType[index2]);
					}
				}
			}
			else
			{
				if (index == 2)
				{
					List<BoxData> list2 = new List<BoxData>();
					for (int i = 0; i < boxDatasByType.Count; i++)
					{
						if (boxDatasByType[i].Quality == 4)
						{
							list2.Add(boxDatasByType[i]);
						}
					}
					if (list2.Count > 0)
					{
						int num = 0;
						int weightSum = BoxDataManager.GetWeightSum(list2, index);
						int num2 = UnityEngine.Random.Range(0, weightSum);
						for (int j = 0; j < list2.Count; j++)
						{
							num += list2[j].Weight[index];
							if (num2 <= num)
							{
								list.Add(list2[j]);
								break;
							}
						}
					}
				}
				while (list.Count < 4)
				{
					int weightSum2 = BoxDataManager.GetWeightSum(boxDatasByType, index);
					int num3 = UnityEngine.Random.Range(0, weightSum2);
					int num4 = 0;
					for (int k = 0; k < boxDatasByType.Count; k++)
					{
						num4 += boxDatasByType[k].Weight[index];
						if (num3 < num4)
						{
							if (list.Contains(boxDatasByType[k]))
							{
								boxDatasByType.RemoveAt(k);
							}
							else
							{
								list.Add(boxDatasByType[k]);
							}
							break;
						}
					}
				}
			}
			return list;
		}

		private static void SetEffectiveLibrary(ref List<BoxData> boxes)
		{
			for (int i = 0; i < boxes.Count; i++)
			{
				if (boxes[i].ItemID >= 2000 && boxes[i].ItemID < 3000)
				{
					WeaponData weaponData = WeaponDataManager.GetWeaponData(boxes[i].ItemID);
					if (weaponData == null || weaponData.State != WeaponState.未解锁)
					{
						boxes.RemoveAt(i);
						i--;
					}
				}
				else if (boxes[i].ItemID >= 3000 && boxes[i].ItemID < 4000)
				{
					EquipmentData equipmentData = EquipmentDataManager.GetEquipmentData(boxes[i].ItemID);
					if (equipmentData == null || equipmentData.State != EquipmentState.未解锁)
					{
						boxes.RemoveAt(i);
						i--;
					}
				}
				else if (boxes[i].ItemID >= 8000 && boxes[i].ItemID < 9000 && !DebrisDataManager.IsDebrisEffective(boxes[i].ItemID))
				{
					boxes.RemoveAt(i);
					i--;
				}
			}
		}

		public static int[] item = new int[]
		{
			1,
			2,
			3,
			4,
			2001,
			2002,
			4001,
			4002
		};

		private static List<BoxData> BoxList = new List<BoxData>();

		private const int CardCount = 4;
	}
}
