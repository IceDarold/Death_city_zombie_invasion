using System;
using System.Collections.Generic;

namespace DataCenter
{
	public class AssetDownloadManager
	{
		static AssetDownloadManager()
		{
			AssetDownloadManager.AssetDatas = DataManager.ParseXmlData<AssetDownloadData>("AssetData", "AssetDatas", "AssetData");
		}

		public static AssetDownloadData GetDownloadData(int chapter)
		{
			for (int i = 0; i < AssetDownloadManager.AssetDatas.Count; i++)
			{
				if (AssetDownloadManager.AssetDatas[i].Chapter == chapter)
				{
					return AssetDownloadManager.AssetDatas[i];
				}
			}
			return null;
		}

		public static string[] GetAssetPath(AssetDownloadData asset)
		{
			return asset.Path.Split(new char[]
			{
				','
			});
		}

		public static void SetDownloadingState(int chapter, bool state)
		{
			AssetDownloadData downloadData = AssetDownloadManager.GetDownloadData(chapter);
			if (downloadData != null)
			{
				for (int i = 0; i < AssetDownloadManager.AssetDatas.Count; i++)
				{
					if (AssetDownloadManager.AssetDatas[i].Scene == downloadData.Scene)
					{
						AssetDownloadManager.AssetDatas[i].Downloading = state;
					}
				}
			}
		}

		private static List<AssetDownloadData> AssetDatas = new List<AssetDownloadData>();
	}
}
