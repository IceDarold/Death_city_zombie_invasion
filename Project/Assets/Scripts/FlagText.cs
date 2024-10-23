using System;
using UnityEngine;
using UnityEngine.UI;

public class FlagText : MonoBehaviour
{
	public void SetString(string s)
	{
		this.content.text = s;
		this.DoAction();
	}

	[ContextMenu("测试")]
	private void DoAction()
	{
	}

	private void OnMoveComplete()
	{
		base.gameObject.SetActive(false);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public Text content;

	public CanvasGroup canvasGroup;

	[CNName("上移距离")]
	public float deltaY = 30f;

	[CNName("上移时间")]
	public float _time = 2f;

	[CNName("淡出延时")]
	public float _delay = 1f;
}
