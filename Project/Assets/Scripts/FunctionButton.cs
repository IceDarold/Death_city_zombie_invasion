using System;
using UnityEngine;
using UnityEngine.UI;

public class FunctionButton : MonoBehaviour
{
	public void SetUnlock(bool key)
	{
		this.Current.interactable = key;
		this.Lock.gameObject.SetActive(!key);
	}

	public Button Current;

	public Image Lock;

	public Image Tip;
}
