using System;
using UnityEngine;
using Zombie3D;

[Serializable]
public class PlotAction
{
	public void OnActionBegin(float dt)
	{
		if (dt >= this.BeginTime - 0.1f && dt <= this.BeginTime + 0.1f && !this.isAction)
		{
			this.isAction = true;
			this.Begin();
		}
	}

	public void OnActionEnded(float dt)
	{
		if (dt >= this.EndedTime - 0.1f && dt <= this.EndedTime + 0.1f && this.isAction)
		{
			this.isAction = false;
			this.End();
		}
	}

	public void Begin()
	{
		switch (this.Type)
		{
		case PlotActionType.角色移动:
			UnityEngine.Debug.Log("角色移动...");
			break;
		case PlotActionType.音效:
			this.PlotAudioSource = Singleton<GameAudioManager>.Instance.PlaySoundInGame(this.SoundClip, this.IsLoop);
			break;
		case PlotActionType.动画:
			if (this.isCamera)
			{
				if (this.SetPause)
				{
					GameApp.GetInstance().GetGameScene().PlayingState = PlayingState.GamePause;
				}
				GameApp.GetInstance().GetGameScene().GetPlayer().SetPlayerIdle(false);
				if (this.isNeedBlackScreen)
				{
					Singleton<FadeAnimationScript>.Instance.FadeInBlack(1f, delegate
					{
						if (Singleton<UiManager>.Instance.GetPage(PageName.InGamePage) != null && Singleton<UiManager>.Instance.GetPage(PageName.InGamePage).gameObject.activeSelf)
						{
							Singleton<UiManager>.Instance.GetPage(PageName.InGamePage).Hide();
						}
						GameApp.GetInstance().GetGameScene().GetCamera().gameObject.SetActive(false);
						this.PlotAnimator.gameObject.SetActive(true);
						if (this.PlotAnimator != null)
						{
							this.PlotAnimator.Play(this.ActionName);
						}
						Singleton<FadeAnimationScript>.Instance.FadeOutBlack(0.5f, null);
					});
				}
				else
				{
					if (Singleton<UiManager>.Instance.GetPage(PageName.InGamePage) != null && Singleton<UiManager>.Instance.GetPage(PageName.InGamePage).gameObject.activeSelf)
					{
						Singleton<UiManager>.Instance.GetPage(PageName.InGamePage).Hide();
					}
					GameApp.GetInstance().GetGameScene().GetCamera().gameObject.SetActive(false);
					this.PlotAnimator.gameObject.SetActive(true);
					if (this.PlotAnimator != null)
					{
						this.PlotAnimator.Play(this.ActionName);
					}
				}
			}
			else if (this.PlotAnimator != null)
			{
				this.PlotAnimator.Play(this.ActionName);
			}
			break;
		case PlotActionType.对话:
			Singleton<UiManager>.Instance.ShowDialogue(2, this.DialogueTag, 0);
			break;
		case PlotActionType.特效:
			this.Particle.Play(true);
			break;
		case PlotActionType.显隐:
			if (this.TargetObject != null)
			{
				this.TargetObject.SetActive(this.Display);
			}
			else
			{
				GameApp.GetInstance().GetGameScene().SetAllEnemyAndPlayerVisiable(false);
			}
			break;
		case PlotActionType.关卡结束:
			GameApp.GetInstance().GetGameScene().PlayingState = PlayingState.GameWin;
			GameApp.GetInstance().GetGameScene().GetPlayer().SetPlayerIdle(false);
			Singleton<UiManager>.Instance.SetLevelFinish(true);
			break;
		case PlotActionType.切至炮台:
			GameApp.GetInstance().GetGameScene().SetPlayer2CannonAfterPlotAction(this.cannonCreater.GetCannon());
			break;
		case PlotActionType.爆炸:
			ShakeCamera.Instance.Shake(CameraShakeType.DRASTIC);
			break;
		case PlotActionType.移除场景怪物:
			GameApp.GetInstance().GetGameScene().RemoveAllEnemiesInSceneWithCallback();
			break;
		case PlotActionType.禁用触发器:
			GameApp.GetInstance().GetGameScene().RemoveAllEnemySpawnCurrentlyTriggered();
			break;
		case PlotActionType.快_慢动作:
			Time.timeScale = this.TargetValue;
			break;
		case PlotActionType.改变角色位置:
			if (this.TargetPoint != null)
			{
				GameApp.GetInstance().GetGameScene().SetPlayerAndCameraToTarget(this.TargetPoint);
			}
			break;
		}
	}

	public void End()
	{
		switch (this.Type)
		{
		case PlotActionType.音效:
			if (this.PlotAudioSource != null)
			{
				this.PlotAudioSource.Stop();
			}
			break;
		case PlotActionType.动画:
			if (this.isCamera)
			{
				if (GameApp.GetInstance().GetGameScene().PlayingState != PlayingState.GameWin && GameApp.GetInstance().GetGameScene().PlayingState != PlayingState.GameLose)
				{
					GameApp.GetInstance().GetGameScene().PlayingState = PlayingState.GamePlaying;
				}
				if (this.isNeedBlackScreen)
				{
					Singleton<FadeAnimationScript>.Instance.FadeInBlack(0.5f, delegate
					{
						if (GameApp.GetInstance().GetGameScene().PlayingState != PlayingState.GameWin && Singleton<UiManager>.Instance.GetPage(PageName.InGamePage) != null && !Singleton<UiManager>.Instance.GetPage(PageName.InGamePage).gameObject.activeSelf)
						{
							Singleton<UiManager>.Instance.GetPage(PageName.InGamePage).Show();
						}
						if (this.TargetPoint != null)
						{
							GameApp.GetInstance().GetGameScene().SetPlayerAndCameraToTarget(this.TargetPoint);
						}
						GameApp.GetInstance().GetGameScene().GetCamera().gameObject.SetActive(true);
						this.PlotAnimator.gameObject.SetActive(false);
						Singleton<FadeAnimationScript>.Instance.FadeOutBlack(0.5f, null);
					});
				}
				else
				{
					if (GameApp.GetInstance().GetGameScene().PlayingState != PlayingState.GameWin && Singleton<UiManager>.Instance.GetPage(PageName.InGamePage) != null && !Singleton<UiManager>.Instance.GetPage(PageName.InGamePage).gameObject.activeSelf)
					{
						Singleton<UiManager>.Instance.GetPage(PageName.InGamePage).Show();
					}
					if (this.TargetPoint != null)
					{
						GameApp.GetInstance().GetGameScene().SetPlayerAndCameraToTarget(this.TargetPoint);
					}
					GameApp.GetInstance().GetGameScene().GetCamera().gameObject.SetActive(true);
					this.PlotAnimator.gameObject.SetActive(false);
				}
			}
			break;
		case PlotActionType.显隐:
			if (this.TargetObject == null)
			{
				GameApp.GetInstance().GetGameScene().SetAllEnemyAndPlayerVisiable(true);
			}
			break;
		case PlotActionType.快_慢动作:
			Time.timeScale = 1f;
			break;
		}
	}

	public PlotActionType Type;

	public float BeginTime;

	public float EndedTime;

	public bool IsLoop;

	public GameObject TargetObject;

	public bool Display;

	public AudioClip SoundClip;

	public Transform TargetPoint;

	public Animator PlotAnimator;

	public string ActionName;

	public bool isCamera;

	public bool isNeedBlackScreen = true;

	public int DialogueTag;

	public ParticleSystem Particle;

	public bool isDisplay;

	private bool isAction;

	private AudioSource PlotAudioSource = new AudioSource();

	public CannonCreater cannonCreater;

	public bool SetPause = true;

	public float TargetValue = 1f;
}
