using System;
using System.Collections.Generic;
using UnityEngine;

namespace DataCenter
{
	public static class PropDataManager
	{
		static PropDataManager()
		{
			PropDataManager.init();
		}

		public static List<PropData> Props
		{
			get
			{
				return PropDataManager.props;
			}
		}

		public static void init()
		{
			PropDataManager.props = DataManager.ParseXmlData<PropData>("PropData", "PropDatas", "PropData");
			if (PlayerPrefs.GetString("InitPropData", "true") == "true")
			{
				PlayerPrefs.SetString("InitPropData", "false");
				for (int i = 0; i < PropDataManager.props.Count; i++)
				{
					if (PropDataManager.props[i].ID == 4001 || PropDataManager.props[i].ID == 4003)
					{
						PropDataManager.Unlock(PropDataManager.props[i]);
						PropDataManager.CollectProp(PropDataManager.props[i].ID, 5);
					}
				}
				PropDataManager.save();
			}
			else
			{
				PropDataManager.read();
			}
		}

		public static PropData GetPropData(int id)
		{
			for (int i = 0; i < PropDataManager.props.Count; i++)
			{
				if (PropDataManager.props[i].ID == id)
				{
					return PropDataManager.props[i];
				}
			}
			return null;
		}

		public static void Unlock(int id)
		{
			PropData propData = PropDataManager.GetPropData(id);
			if (propData != null)
			{
				propData.State = PropState.待制作;
				PropDataManager.save();
			}
		}

		public static void Unlock(PropData prop)
		{
			prop.State = PropState.待制作;
			PropDataManager.save();
		}

		public static void StartFabricate(int id)
		{
			PropData propData = PropDataManager.GetPropData(id);
			if (propData != null)
			{
				propData.State = PropState.制作中;
				PropDataManager.save();
			}
		}

		public static void FinishFabricate(int id)
		{
			PropData propData = PropDataManager.GetPropData(id);
			if (propData != null)
			{
				propData.State = PropState.待领取;
				PropDataManager.save();
			}
		}

		public static void CollectProp(int id, int num)
		{
			PropData propData = PropDataManager.GetPropData(id);
			if (propData != null)
			{
				propData.Count += num;
				if (propData.State == PropState.待领取)
				{
					propData.State = PropState.待制作;
					propData.isNew = true;
				}
				PropDataManager.save();
			}
		}

		public static void RemoveNewTag(int id)
		{
			PropData propData = PropDataManager.GetPropData(id);
			if (propData != null)
			{
				propData.isNew = false;
				PropDataManager.save();
			}
		}

		public static void UseProp(int id)
		{
			PropData propData = PropDataManager.GetPropData(id);
			if (propData != null && propData.Count > 0)
			{
				propData.Count--;
				PropDataManager.save();
			}
		}

		public static void GetSameStateProp(ref List<PropData> list, PropState state)
		{
			for (int i = 0; i < PropDataManager.props.Count; i++)
			{
				if (PropDataManager.props[i].State == state)
				{
					list.Add(PropDataManager.props[i]);
				}
			}
		}

		private static void save()
		{
			PropDataManager.archive.Clear();
			for (int i = 0; i < PropDataManager.props.Count; i++)
			{
				PropSaveData propSaveData = new PropSaveData();
				propSaveData.ID = PropDataManager.props[i].ID;
				propSaveData.isNew = PropDataManager.props[i].isNew;
				propSaveData.State = PropDataManager.props[i].State;
				propSaveData.Count = PropDataManager.props[i].Count;
				PropDataManager.archive.Add(propSaveData);
			}
			DataManager.SaveToJson<PropSaveData>("PropDatas", PropDataManager.archive);
		}

		private static void read()
		{
			PropDataManager.archive = DataManager.ParseJson<PropSaveData>("PropDatas");
			for (int i = 0; i < PropDataManager.props.Count; i++)
			{
				for (int j = 0; j < PropDataManager.archive.Count; j++)
				{
					if (PropDataManager.props[i].ID == PropDataManager.archive[j].ID)
					{
						PropDataManager.props[i].isNew = PropDataManager.archive[j].isNew;
						PropDataManager.props[i].State = PropDataManager.archive[j].State;
						PropDataManager.props[i].Count = PropDataManager.archive[j].Count;
					}
				}
			}
		}

		private static List<PropData> props = new List<PropData>();

		private static List<PropSaveData> archive = new List<PropSaveData>();
	}
}
