using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
	public bool MusicOn
	{
		get
		{
			return this.isMusicOn;
		}
		set
		{
			this.isMusicOn = value;
		}
	}

	public bool EffectOn
	{
		get
		{
			return this.isEffectOn;
		}
		set
		{
			this.isEffectOn = value;
		}
	}

	public void PlayEffect(AudioClip clip)
	{
		if (!this.isEffectOn || clip == null)
		{
			return;
		}
		AudioSource audioSource = this.FindAudioSources();
		audioSource.clip = clip;
		audioSource.loop = false;
		audioSource.Play();
	}

	public void SetMusic()
	{
	}

	public AudioSource PlayEffect(AudioClip clip, bool isLoop)
	{
		if (!this.isEffectOn || clip == null)
		{
			return null;
		}
		AudioSource audioSource = this.FindAudioSources();
		audioSource.clip = clip;
		audioSource.loop = true;
		audioSource.Play();
		return audioSource;
	}

	private AudioSource FindAudioSources()
	{
		for (int i = 0; i < this.allAudios.Count; i++)
		{
			if (!this.allAudios[i].isPlaying)
			{
				return this.allAudios[i];
			}
		}
		AudioSource audioSource = new GameObject
		{
			transform = 
			{
				parent = base.transform
			},
			name = "Audio_" + ++this.sourcesNum
		}.AddComponent<AudioSource>();
		audioSource.playOnAwake = false;
		audioSource.loop = false;
		this.allAudios.Add(audioSource);
		return audioSource;
	}

	public AudioClip uiMusic;

	public AudioClip hitWall;

	public AudioClip headShot;

	protected bool isMusicOn = true;

	protected bool isEffectOn = true;

	protected int sourcesNum;

	protected List<AudioSource> allAudios = new List<AudioSource>();
}
