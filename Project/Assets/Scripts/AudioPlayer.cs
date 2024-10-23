using System;
using System.Collections;
using UnityEngine;

public class AudioPlayer
{
	public AudioPlayer()
	{
		Singleton<GameAudioManager>.Instance.RegistAudioPlayer(new Action<bool>(this.SetAudioVolum));
	}

	~AudioPlayer()
	{
	}

	public void AddAudio(Transform folderTrans, string name)
	{
		if (folderTrans != null)
		{
			Transform transform = folderTrans.Find(name);
			if (transform != null && !this.audioTable.Contains(name))
			{
				AudioInfo audioInfo = new AudioInfo();
				audioInfo.audio = transform.GetComponent<AudioSource>();
				this.audioTable.Add(name, audioInfo);
				audioInfo.audio.volume = (float)((!Singleton<GameAudioManager>.Instance.GameSound) ? 0 : 1);
			}
		}
	}

	public AudioClip GetAudio(string name)
	{
		AudioInfo audioInfo = this.audioTable[name] as AudioInfo;
		if (audioInfo != null)
		{
			AudioSource audio = audioInfo.audio;
			if (audio != null)
			{
				return audio.clip;
			}
		}
		return null;
	}

	public void PlayAudio(string name)
	{
		AudioInfo audioInfo = this.audioTable[name] as AudioInfo;
		if (audioInfo != null)
		{
			AudioSource audio = audioInfo.audio;
			if (audio.clip == null)
			{
				return;
			}
			if (audio != null && Time.time - audioInfo.lastPlayingTime > audio.clip.length)
			{
				audio.Play();
				audioInfo.lastPlayingTime = Time.time;
			}
		}
	}

	public void PauseAllAudios()
	{
		object[] array = new object[this.audioTable.Count];
		this.audioTable.Keys.CopyTo(array, 0);
		for (int i = 0; i < array.Length; i++)
		{
			AudioInfo audioInfo = this.audioTable[array[i]] as AudioInfo;
			if (audioInfo.audio.isPlaying && audioInfo.audio.loop)
			{
				audioInfo.audio.Pause();
			}
		}
	}

	public void ResumeAllAudios()
	{
		object[] array = new object[this.audioTable.Count];
		this.audioTable.Keys.CopyTo(array, 0);
		for (int i = 0; i < array.Length; i++)
		{
			AudioInfo audioInfo = this.audioTable[array[i]] as AudioInfo;
			if (audioInfo != null && audioInfo.audio.loop)
			{
				audioInfo.audio.UnPause();
			}
		}
	}

	public void StopAudio(string name)
	{
		AudioInfo audioInfo = this.audioTable[name] as AudioInfo;
		if (audioInfo != null)
		{
			AudioSource audio = audioInfo.audio;
			audio.Stop();
		}
	}

	public static void PlayAudio(AudioSource audio)
	{
		if (Singleton<GameAudioManager>.Instance.GameSound)
		{
			audio.Play();
		}
	}

	public void SetAudioVolum(bool enable)
	{
		object[] array = new object[this.audioTable.Count];
		this.audioTable.Keys.CopyTo(array, 0);
		for (int i = 0; i < array.Length; i++)
		{
			AudioInfo audioInfo = this.audioTable[array[i]] as AudioInfo;
			if (audioInfo != null && audioInfo.audio != null)
			{
				audioInfo.audio.volume = (float)((!enable) ? 0 : 1);
			}
		}
	}

	protected Hashtable audioTable = new Hashtable();

	protected float lastPlayingTime;
}
