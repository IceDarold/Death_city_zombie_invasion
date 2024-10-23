using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class ItemPopup : MonoBehaviour
{
	public void OnEnable()
	{
		this.GetPos();
		this.RefreshPage();
	}

	private void GetPos()
	{
		for (int i = 0; i < this.PointsParent.childCount; i++)
		{
			this.points[i] = this.PointsParent.GetChild(i).position;
		}
	}

	public void RefreshPage()
	{
		this.iconImg.sprite = Singleton<UiManager>.Instance.GetSprite(this.iconName);
		base.transform.DOPath(this.points, 2f, PathType.CatmullRom, PathMode.Full3D, 10, null).OnComplete(delegate
		{
			UnityEngine.Object.DestroyImmediate(base.gameObject);
		});
	}

	public Image iconImg;

	public string iconName;

	public Transform modelPos;

	public Transform PointsParent;

	private Vector3[] points = new Vector3[9];
}
