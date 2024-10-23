using System;
using System.Collections;
using DataCenter;
using UnityEngine;

public class LandmineProp : GameProp
{
	protected override void Init()
	{
		base.Init();
		if (!this.isDebug)
		{
			PropData propData = PropDataManager.GetPropData(this.ID);
			this.Type = propData.Type;
			this.Value = (float)propData.Value * Mathf.Pow(1.132f, (float)CheckpointDataManager.GetCurrentCheckpointIndex());
			this.Range = propData.Range;
			this.Rate = propData.Rate;
			this.Delay = propData.Delay;
			this.Duration = propData.Duration;
		}
		this.GrenadeBody.gameObject.SetActive(true);
		this.ExplodeParticle.gameObject.SetActive(false);
	}

	public override void Activate(Vector3 direction = default(Vector3))
	{
		this.Init();
		base.transform.rotation = Quaternion.Euler(direction);
		base.gameObject.SetActive(true);
		this.isEffect = true;
		Singleton<GameAudioManager>.Instance.PlaySoundInGame(this.WarningSound, true);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (this.isEffect && (other.gameObject.layer == 9 || other.gameObject.layer == 27))
		{
			this.isEffect = false;
			base.Explode();
			ShakeCamera.Instance.Shake(CameraShakeType.DRASTIC);
			base.StartCoroutine(this.Suicide());
		}
	}

	private void OnWork()
	{
		if (this.isDelay)
		{
			if (this.delay < this.Delay)
			{
				this.delay += Time.deltaTime;
			}
			else
			{
				this.isDelay = false;
				this.delay = 0f;
			}
		}
	}

	private IEnumerator Suicide()
	{
		this.GrenadeBody.gameObject.SetActive(false);
		this.ExplodeParticle.gameObject.SetActive(true);
		yield return new WaitForSeconds(this.ExplodeParticle.main.duration);
		base.gameObject.SetActive(false);
		yield break;
	}

	private void Update()
	{
	}

	[Header("地雷本体")]
	public Transform GrenadeBody;

	public ParticleSystem ExplodeParticle;

	[Header("警示音")]
	public AudioClip WarningSound;
}
