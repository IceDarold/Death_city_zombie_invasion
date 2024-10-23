using System;
using UnityEngine;

public class ScenceControl : MonoBehaviour
{
	public void Awake()
	{
		ScenceControl.instance = this;
	}

	public void ShowUI()
	{
		this.Start.gameObject.SetActive(false);
		this.UI.SetActive(true);
	}

	public void ShowStart()
	{
		this.Start.gameObject.SetActive(true);
		this.UI.SetActive(false);
	}

	public GameObject UI;

	public GameObject Start;

	public static ScenceControl instance;
}
