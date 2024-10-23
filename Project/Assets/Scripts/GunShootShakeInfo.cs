using System;
using UnityEngine;

[Serializable]
public class GunShootShakeInfo
{
	[Space]
	[CNName("射击后移速度")]
	public float lerpSpeed1 = 50f;

	[CNName("射击状态持续时间")]
	public float shootStateDuration = 0.02f;

	[Space]
	[CNName("射击后移幅度")]
	public float backRange = 0.03f;

	[CNName("射击仰角幅度")]
	public float shootRiseAngel = -0.8f;

	[CNName("射击位移-回归速度")]
	public float lerpSpeed2 = 3f;

	[CNName("射击仰角--回归速度")]
	public float lerpSpeed3 = 5f;
}
