using System;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Car
{
	[RequireComponent(typeof(CarController))]
	public class CarAudio : MonoBehaviour
	{
		private void StartSound()
		{
			this.m_CarController = base.GetComponent<CarController>();
			this.m_HighAccel = this.SetUpEngineAudioSource(this.highAccelClip);
			if (this.engineSoundStyle == CarAudio.EngineAudioOptions.FourChannel)
			{
				this.m_LowAccel = this.SetUpEngineAudioSource(this.lowAccelClip);
				this.m_LowDecel = this.SetUpEngineAudioSource(this.lowDecelClip);
				this.m_HighDecel = this.SetUpEngineAudioSource(this.highDecelClip);
			}
			this.m_StartedSound = true;
		}

		private void StopSound()
		{
			foreach (AudioSource obj in base.GetComponents<AudioSource>())
			{
				UnityEngine.Object.Destroy(obj);
			}
			this.m_StartedSound = false;
		}

		private void Update()
		{
			float sqrMagnitude = (Camera.main.transform.position - base.transform.position).sqrMagnitude;
			if (this.m_StartedSound && sqrMagnitude > this.maxRolloffDistance * this.maxRolloffDistance)
			{
				this.StopSound();
			}
			if (!this.m_StartedSound && sqrMagnitude < this.maxRolloffDistance * this.maxRolloffDistance)
			{
				this.StartSound();
			}
			if (this.m_StartedSound)
			{
				float num = CarAudio.ULerp(this.lowPitchMin, this.lowPitchMax, this.m_CarController.Revs);
				num = Mathf.Min(this.lowPitchMax, num);
				if (this.engineSoundStyle == CarAudio.EngineAudioOptions.Simple)
				{
					this.m_HighAccel.pitch = num * this.pitchMultiplier * this.highPitchMultiplier;
					this.m_HighAccel.dopplerLevel = ((!this.useDoppler) ? 0f : this.dopplerLevel);
					this.m_HighAccel.volume = 1f;
				}
				else
				{
					this.m_LowAccel.pitch = num * this.pitchMultiplier;
					this.m_LowDecel.pitch = num * this.pitchMultiplier;
					this.m_HighAccel.pitch = num * this.highPitchMultiplier * this.pitchMultiplier;
					this.m_HighDecel.pitch = num * this.highPitchMultiplier * this.pitchMultiplier;
					float num2 = Mathf.Abs(this.m_CarController.AccelInput);
					float num3 = 1f - num2;
					float num4 = Mathf.InverseLerp(0.2f, 0.8f, this.m_CarController.Revs);
					float num5 = 1f - num4;
					num4 = 1f - (1f - num4) * (1f - num4);
					num5 = 1f - (1f - num5) * (1f - num5);
					num2 = 1f - (1f - num2) * (1f - num2);
					num3 = 1f - (1f - num3) * (1f - num3);
					this.m_LowAccel.volume = num5 * num2;
					this.m_LowDecel.volume = num5 * num3;
					this.m_HighAccel.volume = num4 * num2;
					this.m_HighDecel.volume = num4 * num3;
					this.m_HighAccel.dopplerLevel = ((!this.useDoppler) ? 0f : this.dopplerLevel);
					this.m_LowAccel.dopplerLevel = ((!this.useDoppler) ? 0f : this.dopplerLevel);
					this.m_HighDecel.dopplerLevel = ((!this.useDoppler) ? 0f : this.dopplerLevel);
					this.m_LowDecel.dopplerLevel = ((!this.useDoppler) ? 0f : this.dopplerLevel);
				}
			}
		}

		private AudioSource SetUpEngineAudioSource(AudioClip clip)
		{
			AudioSource audioSource = base.gameObject.AddComponent<AudioSource>();
			audioSource.clip = clip;
			audioSource.volume = 0f;
			audioSource.loop = true;
			audioSource.time = UnityEngine.Random.Range(0f, clip.length);
			audioSource.Play();
			audioSource.minDistance = 5f;
			audioSource.maxDistance = this.maxRolloffDistance;
			audioSource.dopplerLevel = 0f;
			return audioSource;
		}

		private static float ULerp(float from, float to, float value)
		{
			return (1f - value) * from + value * to;
		}

		public CarAudio.EngineAudioOptions engineSoundStyle = CarAudio.EngineAudioOptions.FourChannel;

		public AudioClip lowAccelClip;

		public AudioClip lowDecelClip;

		public AudioClip highAccelClip;

		public AudioClip highDecelClip;

		public float pitchMultiplier = 1f;

		public float lowPitchMin = 1f;

		public float lowPitchMax = 6f;

		public float highPitchMultiplier = 0.25f;

		public float maxRolloffDistance = 500f;

		public float dopplerLevel = 1f;

		public bool useDoppler = true;

		private AudioSource m_LowAccel;

		private AudioSource m_LowDecel;

		private AudioSource m_HighAccel;

		private AudioSource m_HighDecel;

		private bool m_StartedSound;

		private CarController m_CarController;

		public enum EngineAudioOptions
		{
			Simple,
			FourChannel
		}
	}
}
