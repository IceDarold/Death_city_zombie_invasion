using System;
using UnityEngine;
using UnityEngine.UI;
using Zombie3D;

public class SnipeScopeUI : MonoBehaviour, IMessageHandler
{
	public bool IsScopeShow { get; set; }

	public void Awake()
	{
		this.blurMat = this.blurMask.material;
		this.SetBlurValue(0f);
		Singleton<GlobalMessage>.Instance.RegisterMessageHandler(this);
	}

	public void OnDestroy()
	{
		Singleton<GlobalMessage>.Instance.RemoveMessageHandler(this);
	}

	public void DoShowScope(bool isShow)
	{
		if (isShow)
		{
			//this.animator.CrossFade("SnipeUI_In", 0.1f);
			this.setShootButtonEnable(true);
			this.animator.CrossFade("SnipeUI_Hold", 0.1f);
		}
		else
		{
			this.setShootButtonEnable(false);
			this.animator.CrossFade("SnipeUI_Out", 0.01f);
		}
		this.IsScopeShow = isShow;
	}

	public void SetShootBtnEnable(Action<bool> callback)
	{
		this.setShootButtonEnable = callback;
	}

	public void DoShakeScope()
	{
		this.setShootButtonEnable(false);
		this.animator.Play("SnipeUI_Shoot", 0, 0f);
	}

	public void OnScopeShakeOver()
	{
		if (!GameApp.GetInstance().GetGameScene().DoCheckSniperBullet())
		{
			this.setShootButtonEnable(true);
			this.animator.Play("SnipeUI_Hold");
		}
		else
		{
			this.DoReloadLogic();
		}
	}

	public void DoReloadLogic()
	{
		this.snipeUI.fovSlider.enabled = false;
		this.sliderValue = this.snipeUI.fovSlider.value;
		this.snipeUI.OnCameraFOVChanged(0f);
	}

	public void OnScopeShowOver()
	{
		this.blurTarget = 0f;
		this.blurValue = 0f;
		this.SetBlurValue(this.blurValue);
		this.animator.Play("SnipeUI_Hold");
		this.setShootButtonEnable(true);
	}

	public void OnScopeHideOver()
	{
		UnityEngine.Debug.LogError("OnScopeHideOver");
	}

	public void Update()
	{
		if (this.blurValue == this.blurTarget)
		{
			return;
		}
		float num = Mathf.Sign(this.blurTarget - this.blurValue);
		this.blurValue += num * this.blurSpeed * Time.deltaTime;
		this.blurValue = Mathf.Clamp(this.blurValue, 0f, 5f);
		this.SetBlurValue(this.blurValue);
	}

	public void SetPlayerEnable(int _enbale)
	{
		bool visible = _enbale == 1;
		GameApp.GetInstance().GetGameScene().GetPlayer().SetVisible(visible);
	}

	private void SetBlurValue(float _value)
	{
		this.blurValue = _value;
		this.blurMat.SetFloat("_Size", _value);
		this.blurMask.gameObject.SetActive(_value > 0f);
	}

	public float GetBlurValue()
	{
		return this.blurValue;
	}

	public void StartBlurAndShowScope()
	{
		this.blurTarget = 5f;
		this.DoShowScope(true);
	}

	public void StartBlurAndHideScope()
	{
		this.DoShowScope(false);
		this.SetBlurValue(5f);
		this.blurTarget = 0f;
	}

	public void OnBlurReachTarget()
	{
	}

	public void HandleMessage(MessageType type)
	{
		if (type == MessageType.PlayerChargeOver)
		{
			this.snipeUI.OnCameraFOVChanged(this.sliderValue);
			this.snipeUI.fovSlider.enabled = true;
		}
	}

	public Animator animator;

	protected Action<bool> setShootButtonEnable;

	public Image blurMask;

	[HideInInspector]
	public SnipeUI snipeUI;

	public const string SCOPE_IN = "SnipeUI_In";

	public const string SCOPE_OUT = "SnipeUI_Out";

	public const string SCOPE_SHAKE = "SnipeUI_Shoot";

	public const string SCOPE_HOLD = "SnipeUI_Hold";

	protected Material blurMat;

	protected float blurValue;

	protected float blurSpeed = 20f;

	protected float blurTarget;

	protected const float BLUR_MAX = 5f;

	protected const float BLUR_MIN = 0f;

	protected float sliderValue;
}
