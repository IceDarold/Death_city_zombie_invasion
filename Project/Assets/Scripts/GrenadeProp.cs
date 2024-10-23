using System;
using System.Collections;
using DataCenter;
using UnityEngine;

public class GrenadeProp : GameProp
{
	protected override void Init()
	{
		base.Init();
		if (!this.isDebug)
		{
			PropData propData = PropDataManager.GetPropData(this.ID);
			this.Type = propData.Type;
			this.Value = (float)propData.Value * Mathf.Pow(1.132f, (float)CheckpointDataManager.GetCurrentCheckpointIndex()) * (TalentDataManager.GetTalentValue(Talent.BOMB_DAMAGE) + 1f);
			this.Range = propData.Range * (TalentDataManager.GetTalentValue(Talent.BOMB_RANGE) + 1f);
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
		base.gameObject.SetActive(true);
		base.GetComponent<Rigidbody>().AddForce(direction.normalized * this.Force, ForceMode.Impulse);
		this.isDelay = true;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 9 || other.gameObject.layer == 27)
		{
			this.isDelay = false;
			base.transform.position -= new Vector3(0f, base.transform.position.y, 0f);
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
				base.Explode();
				ShakeCamera.Instance.Shake(CameraShakeType.DRASTIC);
				base.StartCoroutine(this.Suicide());
			}
		}
	}

	private IEnumerator Suicide()
	{
		this.GrenadeBody.gameObject.SetActive(false);
		this.ExplodeParticle.gameObject.SetActive(true);
		yield return new WaitForSeconds(this.ExplodeParticle.main.duration);
		base.gameObject.SetActive(false);
		this.GrenadeBody.gameObject.SetActive(true);
		this.ExplodeParticle.gameObject.SetActive(false);
		yield break;
	}

	private void Update()
	{
		this.OnWork();
	}

	[Header("手雷本体")]
	public Transform GrenadeBody;

	[Header("爆炸特效")]
	public ParticleSystem ExplodeParticle;

	[Header("抛力")]
	public float Force;
}
