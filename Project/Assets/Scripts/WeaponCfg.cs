using System;
using UnityEngine;

public class WeaponCfg : MonoBehaviour
{
	[ContextMenu("")]
	private void ResetData()
	{
	}

	[CNName("射击-扩张速度")]
	public float expandSpeed;

	[CNName("射击-收缩速度")]
	public float shrinkSpeed;
}
