using System;
using System.Collections;
using DataCenter;
using UnityEngine;
using Zombie3D;

public class MedkitProp : GameProp
{
	protected override void Init()
	{
		base.Init();
		if (!this.isDebug)
		{
			PropData propData = PropDataManager.GetPropData(this.ID);
			this.Type = propData.Type;
			this.Value = (float)propData.Value;
			this.Range = propData.Range;
			this.Delay = propData.Delay;
			this.Rate = propData.Rate;
			this.Duration = propData.Duration;
		}
	}

	public override void Activate(Vector3 direction = default(Vector3))
	{
		this.Init();
		base.gameObject.SetActive(true);
		PropData propData = PropDataManager.GetPropData(this.ID);
		GameApp.GetInstance().GetGameScene().GetPlayer().AddHp((float)propData.Value);
		base.StartCoroutine(this.Suicide());
	}

	private IEnumerator Suicide()
	{
		yield return new WaitForSeconds(this.Particle.main.duration);
		base.gameObject.SetActive(false);
		yield break;
	}

	[Header("使用音效")]
	public AudioClip Sound;

	[Header("使用特效")]
	public ParticleSystem Particle;
}
