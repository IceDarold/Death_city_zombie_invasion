using System;
using UnityEngine;

public class DespotAttr : ButcherAttr
{
	public void SetLateUpdateCallback(Action callback)
	{
		this.lateUpdateCallBack = callback;
	}

	public void LateUpdate()
	{
		if (this.lateUpdateCallBack != null)
		{
			this.lateUpdateCallBack();
		}
	}

	[CNName("石块锚点")]
	public Transform bulletAnchor;

	[CNName("子弹速度")]
	public float bulletSpeed = 5f;

	private Action lateUpdateCallBack;
}
