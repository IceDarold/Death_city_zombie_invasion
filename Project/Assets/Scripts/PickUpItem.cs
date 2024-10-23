using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PickUpItem : MonoBehaviour
{
	public void OnEnable()
	{
		base.transform.localPosition = Vector3.zero;
		this.dotween.DORestart(false);
	}

	public Text Num;

	public Image Icon;

	public DOTweenAnimation dotween;

	public CanvasGroup canvas;

	public PickUpData pickData;
}
