using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GameAudioManager : Singleton<GameAudioManager>
{
	//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	protected event Action<bool> setAudioVolum;

	public bool EffectOn
	{
		get
		{
			return this.sound;
		}
	}

	public bool GameMusic
	{
		get
		{
			return this.music;
		}
		set
		{
			this.music = value;
			if (this.music)
			{
				PlayerPrefs.SetInt("SAVE_KEY_MUSIC", 1);
			}
			else
			{
				PlayerPrefs.SetInt("SAVE_KEY_MUSIC", 0);
			}
		}
	}

	public bool GameSound
	{
		get
		{
			return this.sound;
		}
		set
		{
			this.sound = value;
			if (this.sound)
			{
				PlayerPrefs.SetInt("SAVE_KEY_SOUND", 1);
			}
			else
			{
				PlayerPrefs.SetInt("SAVE_KEY_SOUND", 0);
			}
			if (this.setAudioVolum != null)
			{
				this.setAudioVolum(this.sound);
			}
		}
	}

	public void ClearAudio()
	{
		this.setAudioVolum = null;
	}

	private void Awake()
	{
		this.music = (PlayerPrefs.GetInt("SAVE_KEY_MUSIC", 1) == 1);
		this.sound = (PlayerPrefs.GetInt("SAVE_KEY_SOUND", 1) == 1);
	}

	public void RegistAudioPlayer(Action<bool> _setAudioVolum)
	{
		this.setAudioVolum += _setAudioVolum;
	}

	public void RemoveAudioPlayer(Action<bool> _setAudioVolum)
	{
		this.setAudioVolum -= _setAudioVolum;
	}

	public void PlayMusic(AudioClip clip)
	{
		if (this.music)
		{
			if (clip == null)
			{
				return;
			}
			this.MusicSource.clip = clip;
			this.MusicSource.Play();
		}
	}

	public void PlayGlobalEffect(GlobalEffect effect)
	{
		this.PlaySoundInGame(this.globalAudios[(int)effect], false);
	}

	public void PauseMusic()
	{
		if (this.MusicSource.isPlaying)
		{
			this.MusicSource.Pause();
		}
	}

	public void UnPauseMusic()
	{
		this.MusicSource.UnPause();
	}

	public void StopMusic()
	{
		if (this.MusicSource.isPlaying)
		{
			this.MusicSource.Stop();
		}
	}

	public void PlaySound(AudioClip clip, bool isloop = false)
	{
		if (this.sound)
		{
			if (clip == null)
			{
				return;
			}
			this.SoundSource.clip = clip;
			this.SoundSource.Play();
			this.SoundSource.loop = isloop;
		}
	}

	public void StopSound()
	{
		if (this.SoundSource.isPlaying)
		{
			this.SoundSource.Stop();
		}
	}

	public AudioSource PlaySoundInGame(AudioClip clip, bool loop = false)
	{
		if (this.sound && clip != null)
		{
			for (int i = 0; i < this.AudioList.Count; i++)
			{
				if (!this.AudioList[i].isPlaying)
				{
					this.AudioList[i].clip = clip;
					this.AudioList[i].loop = loop;
					this.AudioList[i].Play();
					return this.AudioList[i];
				}
			}
			GameObject gameObject = new GameObject();
			gameObject.name = "AudioSource_" + (this.AudioList.Count + 1).ToString("00");
			gameObject.transform.SetParent(base.transform);
			AudioSource audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.clip = clip;
			audioSource.loop = loop;
			audioSource.Play();
			this.AudioList.Add(audioSource);
			return audioSource;
		}
		return null;
	}

	public void PauseSoundInGame()
	{
		if (this.sound)
		{
			for (int i = 0; i < this.AudioList.Count; i++)
			{
				if (this.AudioList[i].isPlaying)
				{
					this.AudioList[i].Pause();
				}
			}
		}
	}

	public void UnpauseSoundInGame()
	{
		if (this.sound)
		{
			for (int i = 0; i < this.AudioList.Count; i++)
			{
				this.AudioList[i].UnPause();
			}
		}
	}

	public void StopAllSound()
	{
		for (int i = 0; i < this.AudioList.Count; i++)
		{
			if (this.AudioList[i].isPlaying)
			{
				this.AudioList[i].Stop();
			}
		}
	}

	public AudioSource MusicSource;

	public AudioSource SoundSource;

	public AudioClip ConfirmClip;

	public AudioClip CancelClip;

	public AudioClip EquipClip;

	public AudioClip EquipMentEquipClip;

	public AudioClip GameBgm;

	public AudioClip UIBgm;

	public AudioClip RacingBGM;

	public AudioClip TalentUpClip;

	public AudioClip EquipUpClip;

	public AudioClip WeaponUpClip;

	public AudioClip GameSoundClip;

	public AudioClip SnipeBG;

	[Space]
	[MyArray(new string[]
	{
		"击中墙壁",
		"爆头",
		"榴弹爆炸",
		"通用爆炸",
		"命中反馈",
		"填弹音效",
		"切换武器",
		"任务完成",
		"狙击击杀"
	})]
	public AudioClip[] globalAudios;

	[Space]
	public AudioClip PickUpClip;

	private List<AudioSource> AudioList = new List<AudioSource>();

	private bool music;

	private bool sound;
}
