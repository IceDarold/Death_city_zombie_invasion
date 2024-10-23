using System;
using UnityEngine;

public class MonoAudioPlayer : MonoBehaviour
{
	private void Awake()
	{
		this.audioSource = base.gameObject.GetComponent<AudioSource>();
		this.SetVolum(Singleton<GameAudioManager>.Instance.GameSound);
		Singleton<GameAudioManager>.Instance.RegistAudioPlayer(new Action<bool>(this.SetVolum));
	}

	private void SetVolum(bool enable)
	{
		this.audioSource.volume = (float)((!enable) ? 0 : 1);
	}

	public void OnDestroy()
	{
		Singleton<GameAudioManager>.Instance.RemoveAudioPlayer(new Action<bool>(this.SetVolum));
	}

	protected AudioSource audioSource;
}
