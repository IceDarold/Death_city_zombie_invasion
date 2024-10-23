using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MessagePopup : GamePage
{
	public override void OnBack()
	{
	}

	public new void OnEnable()
	{
		this.dotween.DORestart(false);
		base.StartCoroutine(this.AutoClose());
	}

	private IEnumerator AutoClose()
	{
		yield return new WaitForSeconds(this.LastTime);
		this.Close();
		yield break;
	}

	public Text Message;

	public float LastTime;

	public DOTweenAnimation dotween;
}
