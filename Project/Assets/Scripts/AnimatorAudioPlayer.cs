using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorAudioPlayer : MonoBehaviour
{
	public void OnPlayAudioClip(int _index)
	{
		if (_index >= this.allAudios.Length)
		{
			UnityEngine.Debug.LogError(base.gameObject.name + "OnPlayAudioClip : _index = " + _index);
			return;
		}
		Singleton<GameAudioManager>.Instance.PlaySoundInGame(this.allAudios[_index], false);
	}

	public AudioClip[] allAudios;
}
