using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombie3D;

public class ScenePrefabs : SceneBatches, IBindAbleCollection
{
	public override void DoLoad(int curLevelIndex)
	{
	}

	public void DoLoad(int curLevelIndex, Action _loadOverCallback, bool _loadFromAB)
	{
		this.loadOverCallback = _loadOverCallback;
		string text = this.ResourcePath();
		if (_loadFromAB || string.IsNullOrEmpty(text))
		{
			this.DoLoadSceneFromABAsync(curLevelIndex);
		}
		else
		{
			this.DoLoadSceneAsync(curLevelIndex, text);
		}
	}

	public bool IsInRuntimeCombine(string name)
	{
		for (int i = 0; i < this.allRuntimeCombine.Count; i++)
		{
			if (this.allRuntimeCombine[i].ContainObject(name))
			{
				return true;
			}
		}
		return false;
	}

	private void DoLoadSceneAsync(int curLevel, string resourcePath)
	{
		this.startLoadScenePrefabsTime = Time.time;
		int i = 0;
		int count = this.lightInfos.Count;
		while (i < count)
		{
			if (this.lightInfos[i].GetEnable(curLevel))
			{
				this.asyncLoadCounter++;
				base.StartCoroutine(this.LoadScenePrefabAsync(resourcePath + "/" + this.lightInfos[i].GetPrefabName(), this.lightInfos[i]));
			}
			i++;
		}
	}

	private IEnumerator LoadScenePrefabAsync(string path, BakedLightInfo lightinfo)
	{
		yield return null;
		ResourceRequest resourceRequest = Resources.LoadAsync<GameObject>(path);
		while (!resourceRequest.isDone)
		{
			yield return null;
		}
		yield return new WaitForSeconds(0.1f);
		this.OnLoadOverCallBack(resourceRequest.asset as GameObject, lightinfo);
		yield break;
	}

	private void DoLoadSceneFromABAsync(int curlevel)
	{
		this.startLoadScenePrefabsTime = Time.time;
		//TODO: ASSET BUNDLES
		this.LoadLightInfoFromAB();
		int i = 0;
		int count = this.lightInfos.Count;
		while (i < count)
		{
			if (this.lightInfos[i].GetEnable(curlevel))
			{
				this.asyncLoadCounter++;
				base.StartCoroutine(this.LoadScenePrefabAsyncFromAB(this.lightInfos[i].GetPrefabName(), this.lightInfos[i]));
			}
			i++;
		}
	}

	private IEnumerator LoadScenePrefabAsyncFromAB(string path, BakedLightInfo lightinfo)
	{
		GameObject obj = null;
		AssetBundleRequest request = Singleton<AssetBundleManager>.Instance.MainBundle.LoadAssetAsync<GameObject>(path);
		while (!request.isDone)
		{
			yield return null;
		}
		obj = (request.asset as GameObject);
		this.OnLoadOverCallBack(obj, lightinfo);
		yield break;
	}

	private void OnLoadOverCallBack(GameObject _obj, BakedLightInfo lightinfo)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(_obj, base.transform);
		gameObject.transform.localPosition = lightinfo.position;
		gameObject.transform.localRotation = Quaternion.Euler(lightinfo.rotation);
		gameObject.transform.localScale = lightinfo.scale;
		gameObject.isStatic = true;
		Renderer component = gameObject.GetComponent<Renderer>();
		if (component == null)
		{
			BakedLightGroup component2 = gameObject.GetComponent<BakedLightGroup>();
			if (component2 == null)
			{
				UnityEngine.Debug.LogError("物体" + lightinfo.childname + "的光照信息查找不到  不符合标准");
			}
			else
			{
				component2.SetRenderLightInfo(lightinfo.renderlightInfo);
			}
		}
		else
		{
			component.lightmapIndex = lightinfo.lightMapIndex;
			component.lightmapScaleOffset = lightinfo.lightmapTillingOffset;
		}
		this.asyncLoadCounter--;
		if (this.asyncLoadCounter == 0)
		{
			if (Time.time - this.startLoadScenePrefabsTime < 1.5f)
			{
				base.StartCoroutine(this.DoLoadOverCallBack(this.startLoadScenePrefabsTime + 1.5f - Time.time));
			}
			else if (this.loadOverCallback != null)
			{
				this.loadOverCallback();
			}
			StaticBatchingUtility.Combine(base.gameObject);
		}
	}

	private IEnumerator DoLoadOverCallBack(float delay)
	{
		yield return new WaitForSeconds(delay);
		if (this.loadOverCallback != null)
		{
			this.loadOverCallback();
		}
		yield break;
	}

	private void LoadLightInfoFromAB()
	{
		List<Texture2D> list = new List<Texture2D>();
		int num = 0;
		for (;;)
		{
			Texture2D texture2D = Singleton<AssetBundleManager>.Instance.MainBundle.LoadAsset("Lightmap-" + num + "_comp_light.exr") as Texture2D;
			if (texture2D == null)
			{
				break;
			}
			num++;
			list.Add(texture2D);
		}
		LightmapSettings.lightmapsMode = LightmapsMode.NonDirectional;
		LightmapData[] array = new LightmapData[list.Count];
		LightmapSettings.lightmaps = null;
		int i = 0;
		int count = list.Count;
		while (i < count)
		{
			array[i] = new LightmapData();
			array[i].lightmapColor = list[i];
			i++;
		}
		LightmapSettings.lightmaps = array;
	}

	public void SeekGameObjectInCollection(GameObject _obj, Action<string, BindLevel> Success)
	{
		for (int i = 0; i < this.lightInfos.Count; i++)
		{
			if (this.lightInfos[i].childname == _obj.name)
			{
				Success(this.lightInfos[i].childname, this.lightInfos[i]);
				break;
			}
		}
	}

	[ContextMenu("删除光照贴图引用")]
	private void RemoveLightMaps()
	{
		LightmapData[] lightmaps = LightmapSettings.lightmaps;
		int i = 0;
		int num = lightmaps.Length;
		while (i < num)
		{
			lightmaps[i].lightmapColor = null;
			i++;
		}
	}

	[ContextMenu("ResourcePath")]
	private string ResourcePath()
	{
		Debug.Log(scenePrefabPath);
		int num = this.scenePrefabPath.LastIndexOf("Resources/");
		if (num == -1)
		{
			UnityEngine.Debug.LogError("ScenePrefabPath Do Not Contains Resource");
			return null;
		}
		int num2 = this.scenePrefabPath.LastIndexOf("Resources/") + "Resources/".Length;
		return this.scenePrefabPath.Substring(num2, this.scenePrefabPath.Length - num2);
	}

	[Space(10f)]
	public const string RESOURCES = "Resources/";

	//TODO: ASSET BUNDLES
	
	[HideInInspector]
	public string scenePrefabPath;

	protected int asyncLoadCounter;

	protected Action loadOverCallback;

	[HideInInspector]
	public List<BakedLightInfo> lightInfos = new List<BakedLightInfo>();

	public List<RunTimeMeshCombine> allRuntimeCombine = new List<RunTimeMeshCombine>();

	protected float startLoadScenePrefabsTime;
}
