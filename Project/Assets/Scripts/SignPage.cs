using System;
using UnityEngine;

public class SignPage : GamePage
{
	private new void Awake()
	{
	}

	public override void Close()
	{
		base.Close();
		MainPage.instance.RefleshPage();
	}

	public void OnclickClose()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		this.Close();
	}

	public override void OnBack()
	{
	}

	public void OnClickGet()
	{
		this.Close();
	}

	public GameObject btnclose;
}
