using System;
using DG.Tweening;
using UnityEngine;
using Zombie3D;

public class ShakeCamera : MonoBehaviour
{
	private void Awake()
	{
		ShakeCamera.Instance = this;
	}

	public void Shake(CameraShakeType type)
	{
		float num = 0f;
		float shakeTime = 0f;
		if (type != CameraShakeType.SLIGHT)
		{
			if (type != CameraShakeType.MIDDLE)
			{
				if (type == CameraShakeType.DRASTIC)
				{
					shakeTime = 0.5f;
					num = 1.2f;
				}
			}
			else
			{
				shakeTime = 0.4f;
				num = 0.9f;
			}
		}
		else
		{
			shakeTime = 0.2f;
			num = 0.5f;
		}
		if (!this.isShake || num >= this.Amplitude)
		{
			this.time = (this.ShakeTime = shakeTime);
			this.amplitude = (this.Amplitude = num);
			this.isShake = true;
		}
	}

	public void Shake(float dur, float amp)
	{
		if (!this.isShake || amp > this.Amplitude)
		{
			this.ShakeTime = dur;
			this.time = dur;
			this.Amplitude = amp;
			this.amplitude = amp;
			this.isShake = true;
		}
	}

	public void ResetShootCurveInfo()
	{
		this.shootShakeInterval = ((this.weaponType != WeaponType.ShotGun) ? this.shootStateDuration : 0.08f);
	}

	public void SetGunShootShakeInfo(GunShootShakeInfo info, WeaponType _weapontype)
	{
		this.lerpSpeed1 = info.lerpSpeed1;
		this.lerpSpeed2 = info.lerpSpeed2;
		this.lerpSpeed3 = info.lerpSpeed3;
		this.shootStateDuration = info.shootStateDuration;
		this.shootRiseAngel = info.shootRiseAngel;
		this.shootBackRange = info.backRange;
		this.weaponType = _weapontype;
	}

	[ContextMenu("Shake")]
	public void Shake()
	{
		this.time = this.ShakeTime;
		this.amplitude = this.Amplitude;
		this.isShake = true;
	}

	public void Recoil(float balance, float dur, float amp)
	{
		if (this.isShake)
		{
			return;
		}
		balance = Mathf.Clamp(balance, 0f, 1f);
		this.RecoilSequence = DOTween.Sequence();
		this.RecoilSequence.Append(this.ChildTransform.DOLocalRotate(-new Vector3(amp, 0f, 0f), dur * balance, RotateMode.Fast).SetEase(Ease.OutQuint));
		this.RecoilSequence.Append(this.ChildTransform.DOLocalRotate(Vector3.zero, (1f - balance) * dur, RotateMode.Fast));
		this.RecoilSequence.Play<Sequence>();
	}

	private void Update()
	{
		if (this.isShake && Time.timeScale != 0f)
		{
			if (this.time > 0f)
			{
				this.time -= Time.deltaTime;
				this.amplitude = this.Amplitude * this.time / this.ShakeTime;
				base.transform.localEulerAngles = new Vector3(UnityEngine.Random.Range(0f, this.amplitude * 3f), UnityEngine.Random.Range(0f, this.amplitude), UnityEngine.Random.Range(0f, this.amplitude));
			}
			else
			{
				this.isShake = false;
				this.time = this.ShakeTime;
				this.amplitude = this.Amplitude;
				base.transform.localEulerAngles = Vector3.zero;
			}
		}
		float z;
		Quaternion localRotation;
		if (this.shootShakeInterval > 0f)
		{
			this.shootShakeInterval -= Time.deltaTime;
			float num = (this.weaponType != WeaponType.ShotGun) ? this.lerpSpeed1 : this.lerpSpeed4;
			z = Mathf.Lerp(this.ChildTransform.localPosition.z, this.shootBackRange, Time.deltaTime * num);
			localRotation = Quaternion.Lerp(this.ChildTransform.localRotation, Quaternion.Euler(this.shootRiseAngel, 0f, 0f), Time.deltaTime * num);
		}
		else
		{
			z = Mathf.Lerp(this.ChildTransform.localPosition.z, 0f, Time.deltaTime * this.lerpSpeed2);
			localRotation = Quaternion.Lerp(this.ChildTransform.localRotation, Quaternion.identity, Time.deltaTime * this.lerpSpeed3);
		}
		this.ChildTransform.localPosition = new Vector3(0f, 0f, z);
		this.ChildTransform.localRotation = localRotation;
	}

	public static ShakeCamera Instance;

	public Transform ChildTransform;

	[HideInInspector]
	public CameraShakeType Type;

	public float LimitDistance;

	[Header("震动时间")]
	public float ShakeTime;

	[Header("幅度")]
	public float Amplitude;

	private bool isShake;

	private Vector3 InitialRotation;

	private float time;

	private float amplitude;

	private Sequence RecoilSequence;

	[Space]
	[CNName("射击后移速度")]
	public float lerpSpeed1;

	[CNName("射击状态持续时间")]
	public float shootStateDuration;

	[Space]
	[CNName("射击后移幅度")]
	public float shootBackRange;

	[CNName("射击仰角幅度")]
	public float shootRiseAngel;

	[CNName("射击位移-回归速度")]
	public float lerpSpeed2;

	[CNName("射击仰角--回归速度")]
	public float lerpSpeed3;

	[CNName("霰弹枪仰角上升速度")]
	public float lerpSpeed4 = 10f;

	protected WeaponType weaponType;

	protected float shootShakeInterval;
}
