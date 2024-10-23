using System;
using UnityEngine;

namespace Zombie3D
{
	public class AvatarFactory
	{
		public static AvatarFactory GetInstance()
		{
			if (AvatarFactory.instance == null)
			{
				AvatarFactory.instance = new AvatarFactory();
			}
			return AvatarFactory.instance;
		}

		public GameObject CreateAvatar(AvatarType aType)
		{
			GameObject result = null;
			if (aType != AvatarType.Sam)
			{
				if (aType != AvatarType.Laura)
				{
					if (aType == AvatarType.Johnson)
					{
						result = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Player/JOHNSON"), Vector3.zero, Quaternion.identity);
					}
				}
				else
				{
					result = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Player/LAURA"), Vector3.zero, Quaternion.identity);
				}
			}
			else
			{
				result = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Player/SAM"), Vector3.zero, Quaternion.identity);
			}
			return result;
		}

		protected static AvatarFactory instance;

		protected const string PLAYER_PREFAB_PATH = "Prefabs/Player/";
	}
}
