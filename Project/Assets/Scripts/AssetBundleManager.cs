using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class AssetBundleManager : Singleton<AssetBundleManager>
{
	public bool CurSceneLoadFromAB { get; set; }

	public bool DownLoadFromLocal
	{
		get
		{
			return this.downLoadFromLocal;
		}
		set
		{
			this.downLoadFromLocal = value;
		}
	}

	public AssetBundle MainBundle
	{
		get
		{
			return this.mainBundle;
		}
	}

	private string AB_DOWNLOAD_URL
	{
		get
		{
			return "http://resgg.hvapi.com/100056";
		}
	}

	public void LoadABFromCatch(string bundleName, Action complete)
	{
		UnityEngine.Debug.LogError("LoadBundle ----" + bundleName);
		base.StartCoroutine(this.LoadAB(bundleName, complete));
	}

	public void UnloadRetainedAB()
	{
		if (this.mainBundle != null)
		{
			this.mainBundle.Unload(false);
		}
		if (this.commonBundle != null)
		{
			this.commonBundle.Unload(false);
		}
	}

	public bool IsAssetBundleExist(string bundleName)
	{
		return false;
		//TODO:Cash
		//return Caching.IsVersionCached(this.AB_DOWNLOAD_URL + "/" + this.GetBundleName(bundleName), this.GetCacheVersion(bundleName));
	}

	public void DownLoadAssetBundle(string bundleName, Action<float> refreshPercent = null, Action complete = null, Action fail = null)
	{
		this.downloadCoroutine = base.StartCoroutine(this.DownLoadAB(bundleName, refreshPercent, complete, fail));
		this.curDownLoadBundleName = bundleName;
	}

	public void CancleDownLoad()
	{
		if (this.downloadCoroutine != null)
		{
			base.StopCoroutine(this.downloadCoroutine);
			this.SetCacheVersion(this.curDownLoadBundleName, this.GetCacheVersion(this.curDownLoadBundleName) + 1);
		}
	}

	public string GetBundleName(string bundleName)
	{
		Debug.Log(bundleName);
		return string.Concat(new object[]
		{
			"v",
			1,
			"/",
			bundleName
		});
	}

	private int GetCacheVersion(string bundleName)
	{
		return PlayerPrefs.GetInt(bundleName, 0);
	}

	private void SetCacheVersion(string bundleName, int version)
	{
		PlayerPrefs.SetInt(bundleName, version);
		PlayerPrefs.Save();
	}

	private IEnumerator DownLoadAB(string bundleName, Action<float> refreshPercent, Action complete, Action fail)
	{
		int curVersion = this.GetCacheVersion(bundleName);
		string url = this.AB_DOWNLOAD_URL + "/" + this.GetBundleName(bundleName);
		WWW www = WWW.LoadFromCacheOrDownload(url, curVersion);
		AssetBundleDownloadProgress progress = this.DoRefreshProgress(www, refreshPercent);
		yield return www;
		if (string.IsNullOrEmpty(www.error))
		{
			if (complete != null)
			{
				complete();
			}
		}
		else
		{
			UnityEngine.Debug.LogError("#error:" + www.error);
			UnityEngine.Debug.LogError("#error: download version = " + curVersion);
			this.SetCacheVersion(bundleName, curVersion + 1);
			if (fail != null)
			{
				fail();
			}
		}
		progress.enabled = false;
		UnityEngine.Object.Destroy(progress.gameObject);
		www.Dispose();
		yield break;
	}

	private IEnumerator LoadAB(string bundle, Action complete)
	{
		//TODO:Cash
		// while (!Caching.ready)
		// {
		// 	yield return null;
		// }
		int curVersion = this.GetCacheVersion(bundle);
		string url = this.AB_DOWNLOAD_URL + "/" + this.GetBundleName(bundle);
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"url:",
			url,
			"DownLoadversion = ",
			curVersion
		}));
		//TODO:Cash
		//UnityEngine.Debug.Log(Caching.IsVersionCached(url, curVersion));
		WWW www = WWW.LoadFromCacheOrDownload(url, curVersion);
		yield return www;
		if (!string.IsNullOrEmpty(www.error))
		{
			UnityEngine.Debug.LogError(www.error);
		}
		if (!bundle.Equals("common"))
		{
			this.mainBundle = www.assetBundle;
		}
		else
		{
			this.commonBundle = www.assetBundle;
		}
		yield return null;
		if (complete != null)
		{
			complete();
		}
		www.Dispose();
		yield break;
	}

	private AssetBundleDownloadProgress DoRefreshProgress(WWW www, Action<float> refreshProgress)
	{
		GameObject gameObject = new GameObject("AssetBundleDownloadProgress");
		return gameObject.AddComponent<AssetBundleDownloadProgress>().SetCallBack(www, refreshProgress);
	}

	private void SaveAsset2LocalFile(string path, string name, byte[] info, int length)
	{
		FileInfo fileInfo = new FileInfo(path + "/" + name);
		if (fileInfo.Exists)
		{
			fileInfo.Delete();
		}
		Stream stream = fileInfo.Create();
		stream.Write(info, 0, length);
		stream.Flush();
		stream.Close();
		stream.Dispose();
		UnityEngine.Debug.Log(name + "成功保存到本地 :" + path);
	}

	public static string SceneName2BundleName(string _sceneName)
	{
		string result = string.Empty;
		for (int i = 0; i < AssetBundleManager.SceneName.Length; i++)
		{
			if (_sceneName.Equals(AssetBundleManager.SceneName[i]))
			{
				result = AssetBundleManager.BundleName[i];
			}
		}
		return result;
	}

	protected AssetBundle mainBundle;

	protected AssetBundle commonBundle;

	protected string loadedAssetBundleName;

	protected Coroutine downloadCoroutine;

	protected const string AB_DOWNLOAD_URL_GP = "http://resgg.hvapi.com/100056";

	public const string COMMON_BUNDLE = "common";

	private const int version = 1;

	private string curDownLoadBundleName;

	//TODO: ASSET BUNDLES
	
	public static string[] SceneName = new string[]
	{
		"Demo_YiYuan",
		"YiYuan_Scene_2",
		"YiYuan_Scene_4",
		"YiYuan_Scene_5",
		"YiYuan_Scene_6",
		"YiYuan_Scene_7"
	};

	public static string[] BundleName = new string[]
	{
		"yiyuan_scene_1_3",
		"yiyuan_scene_2",
		"yiyuan_scene_4",
		"yiyuan_scene_5",
		"yiyuan_scene_6",
		"yiyuan_scene_7"
	};

	protected bool downLoadFromLocal;
}
