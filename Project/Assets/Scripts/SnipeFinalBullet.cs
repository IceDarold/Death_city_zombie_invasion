using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombie3D;

public class SnipeFinalBullet : SnipeBullet
{
	public override void Init(Vector3 _position, Vector3 _forward, float _damage, float _hitForce, List<WeaponHitInfo> _hitInfo)
	{
		this.gameCamera = GameApp.GetInstance().GetGameScene().GetCamera();
		base.Init(_position, _forward, _damage, _hitForce, _hitInfo);
		this.speed = this.shootLength / this.duration;
		BaseCameraScript camera = GameApp.GetInstance().GetGameScene().GetCamera();
		camera.cameraComponent.fieldOfView = 60f;
		camera.cameraComponent.nearClipPlane = 0.001f;
		camera.SetSnipeCameraFinalBulletAnchor(this.cameraAnchor);
		InGamePage inGamePage = Singleton<UiManager>.Instance.GetPage(PageName.InGamePage) as InGamePage;
		inGamePage.HideSnipeUIImmediatly();
		Weapon weapon = GameApp.GetInstance().GetGameScene().GetPlayer().GetWeapon();
		GameObject gameObject = WeaponFactory.GetInstance().CreateSnipeMode(weapon.Name);
		gameObject.transform.position = base.transform.position;
		gameObject.transform.rotation = base.transform.rotation;
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(Singleton<ResourceConfigScript>.Instance.SnipeLastShootGunFire);
		gameObject2.transform.position = base.transform.position;
		gameObject2.transform.rotation = base.transform.rotation;
		this.positionCurveTime = this.positionCurve.keys[this.positionCurve.length - 1].time;
		this.blurCurveTime = this.blurCurve.keys[this.blurCurve.length - 1].time;
		this.rotateCurveTime = this.rotateCurve.keys[this.rotateCurve.length - 1].time;
		this.gameCamera.SetRadialBlurFactor(0f);
		this.gameCamera.SetRadialBlurEnable(true);
		this.startTime = Time.time;
	}

	private IEnumerator DelayShowBullet(float delay)
	{
		yield return new WaitForSeconds(delay);
		this.canStart = true;
		this.bulletObject.SetActive(true);
		yield break;
	}

	public override void Update()
	{
		if (!this.canStart)
		{
			return;
		}
		float num = (Time.time - this.startTime) / this.duration;
		num = Mathf.Clamp01(num);
		base.transform.position = this.startPos + this.positionCurve.Evaluate(num * this.positionCurveTime) * this.shootLength * base.transform.forward;
		this.gameCamera.SetRadialBlurFactor(this.blurCurve.Evaluate(num * this.blurCurveTime) * 2f);
		float num2 = this.rotateCurve.Evaluate(num * this.rotateCurveTime);
		this.bulletObject.transform.localRotation = Quaternion.Euler(this.bulletObject.transform.localRotation.eulerAngles + new Vector3(0f, 0f, num2 * Time.deltaTime));
		if (num == 1f)
		{
			base.OnArriveTargetPos(false);
			GameApp.GetInstance().GetGameScene().ShakeMainCamera(this.shakeDuration, this.shakeRange);
		}
	}

	public override void DestroyThis()
	{
		this.canStart = false;
		this.bulletObject.SetActive(false);
		this.gameCamera.SetRadialBlurEnable(false);
	}

	public Transform cameraAnchor;

	public float duration;

	public GameObject bulletObject;

	public AnimationCurve positionCurve;

	public AnimationCurve blurCurve;

	public AnimationCurve rotateCurve;

	[CNName("震动幅度")]
	public float shakeRange = 1f;

	[CNName("震动时间")]
	public float shakeDuration = 0.5f;

	protected float startTime;

	protected float positionCurveTime;

	protected float blurCurveTime;

	protected float rotateCurveTime;

	protected BaseCameraScript gameCamera;
}
