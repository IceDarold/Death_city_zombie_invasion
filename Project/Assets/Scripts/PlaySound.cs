using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlaySound : MonoBehaviour
{
	private void Awake()
	{
		base.gameObject.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnBtnPressed));
	}

	private void OnBtnPressed()
	{
	}

	public AudioClip clip;
}
