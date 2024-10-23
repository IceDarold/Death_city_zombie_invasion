using System;
using UnityEngine;

public class AssetBundleDownloadProgress : MonoBehaviour
{
	public AssetBundleDownloadProgress SetCallBack(WWW _www, Action<float> _refreshProgress)
	{
		this.www = _www;
		this.refreshProgress = _refreshProgress;
		return this;
	}

	private void Update()
	{
		this.refreshProgress(this.www.progress);
	}

	protected WWW www;

	protected Action<float> refreshProgress;
}
