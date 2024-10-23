using System;
using System.Collections.Generic;
using UnityEngine;

namespace DataCenter
{
	public class EnemyDataManager
	{
		static EnemyDataManager()
		{
			List<EnemyData> list = DataManager.ParseXmlData<EnemyData>("EnemyData", "EnemyDatas", "EnemyData");
			List<EnemyProbability> list2 = DataManager.ParseXmlData<EnemyProbability>("EnemyWeight", "EnemyWeights", "EnemyWeight");
			for (int i = 0; i < list.Count; i++)
			{
				EnemyDataManager.allEnemyData.Add(list[i].Type, list[i]);
			}
		}

		public static EnemyData GetEnemyData(int id)
		{
			return null;
		}

		public static void InitEnemyDataByLevel(int levelIndex)
		{
			EnemyDataManager.dataID = levelIndex;
		}

		public static void InitEnemyDataByMaxLevel()
		{
			CheckpointData currentCheckpoint = CheckpointDataManager.GetCurrentCheckpoint();
			EnemyDataManager.InitEnemyDataByLevel(currentCheckpoint.DataID);
		}

		public static EnemyData GetEnemyData(string name)
		{
			return null;
		}

		public static float GetEnemyHp(EnemyDataType type)
		{
			if (type == EnemyDataType.BOSS_BOMBER || type == EnemyDataType.BOSS_BUTCHER)
			{
				UnityEngine.Debug.LogError(string.Concat(new object[]
				{
					"BOSS 血量:",
					EnemyDataManager.allEnemyData[type].HP[EnemyDataManager.dataID],
					"等级 ： ",
					EnemyDataManager.dataID
				}));
			}
			return (float)EnemyDataManager.allEnemyData[type].HP[EnemyDataManager.dataID];
		}

		public static float GetEnemyDamage(EnemyDataType type)
		{
			return (float)EnemyDataManager.allEnemyData[type].Damage[EnemyDataManager.dataID];
		}

		public static List<EnemyData> GetSameTypeEnemies(int type)
		{
			return new List<EnemyData>();
		}

		private static Dictionary<EnemyDataType, EnemyData> allEnemyData = new Dictionary<EnemyDataType, EnemyData>();

		private static int dataID = 0;
	}
}
