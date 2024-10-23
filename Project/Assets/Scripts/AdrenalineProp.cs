using System;
using DataCenter;
using UnityEngine;
using Zombie3D;

public class AdrenalineProp : GameProp
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
			this.Duration = propData.Duration * (1f + TalentDataManager.GetTalentValue(Talent.ADRENALINE_TIME));
		}
	}

	public override void Activate(Vector3 direction = default(Vector3))
	{
		this.Init();
		base.gameObject.SetActive(true);
		this.isEffect = true;
		GameApp.GetInstance().GetGameScene().DoBulletTime(this.Duration);
	}

	private void OnWork()
	{
		if (this.isEffect)
		{
			if (this.duration < this.Duration)
			{
				this.duration += Time.deltaTime;
			}
			else
			{
				this.isEffect = false;
				this.duration = 0f;
				base.gameObject.SetActive(false);
			}
		}
	}

	private void Update()
	{
		this.OnWork();
	}
}
