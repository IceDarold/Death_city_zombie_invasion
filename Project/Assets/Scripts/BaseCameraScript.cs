using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using Zombie3D;

public abstract class BaseCameraScript : MonoBehaviour
{
	public Camera MainCamera
	{
		get
		{
			return this.mainCamera;
		}
	}

	public Transform CameraTransform
	{
		get
		{
			return this.cameraTransform;
		}
	}

	public Vector2 ReticlePosition
	{
		get
		{
			return this.reticlePosition;
		}
		set
		{
			this.reticlePosition = value;
		}
	}

	public virtual void SetCameraRotation(Transform trans)
	{
	}

	public virtual void ResetCameraPosition()
	{
	}

	public virtual void SetCameraPosition(Transform trans)
	{
	}

	public virtual void DoShootOffset()
	{
	}

	public bool IsInViewport(Vector3 worldPos)
	{
		Vector3 point = this.cameraComponent.WorldToViewportPoint(worldPos);
		bool flag = point.z >= this.cameraComponent.nearClipPlane && point.z <= this.cameraComponent.farClipPlane;
		bool flag2 = this.camRect.Contains(point);
		return flag && flag2;
	}

	public abstract global::CameraType GetCameraType();

	public virtual void Init(PlayerSnipePoint snipeCameraCfg = null)
	{
		this.gameScene = GameApp.GetInstance().GetGameScene();
		this.player = this.gameScene.GetPlayer();
		this.cameraDistanceFromPlayer = this.cameraDistanceFromPlayerWhenIdle;
		base.StartCoroutine(this.InitCameraPositionAndRotation(snipeCameraCfg));
		//TODO: Cursor
		 // Screen.lockCursor = true;
		 // Cursor.visible = true;
		float[] array = new float[32];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = 150f;
		}
		array[17] = 1000f;
		this.mainCamera = base.transform.Find("Root/Main Camera").gameObject.GetComponent<Camera>();
		this.mainCamera.layerCullDistances = array;
		this.reticlePosition = new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f);
		this.ScreenWidth = (float)Screen.width;
		this.ScreenHeight = (float)Screen.height;
		this.screenXScale = this.ScreenWidth / 1280f;
		this.screenYScale = this.ScreenHeight / 720f;
		this.SetSnipeCameraViewConfig(snipeCameraCfg);
		this.snipeScopeFOV = new float[Constant.SNIPESCOPE_FOV.Length];
		Constant.SNIPESCOPE_FOV.CopyTo(this.snipeScopeFOV, 0);
		this.cameraSwingSpeed = (float)Singleton<GlobalData>.Instance.Sensitivity;
		this.started = true;
	}

	private IEnumerator InitCameraPositionAndRotation(PlayerSnipePoint snipeCameraCfg)
	{
		yield return null;
		this.angelH = this.player.GetTransform().rotation.eulerAngles.y;
		base.transform.rotation = Quaternion.Euler(-this.angelV, this.angelH, 0f);
		if (snipeCameraCfg == null)
		{
			base.transform.position = this.player.GetTransform().TransformPoint(this.cameraDistanceFromPlayer);
		}
		else
		{
			Vector3 position = this.player.GetTransform().TransformPoint(snipeCameraCfg.snipeCameraAnchor) - this.cameraTransform.forward * snipeCameraCfg.snipeCameraDistances;
			base.transform.position = position;
		}
		yield break;
	}

	public void SetRadialBlurEnable(bool _enable)
	{
		if (this.radialBlur != null)
		{
			this.radialBlur.enabled = _enable;
		}
	}

	public void SetRadialBlurFactor(float _factor)
	{
		if (this.radialBlur != null)
		{
			this.radialBlur.lerpFactor = _factor;
		}
	}

	public virtual void SetFollowCannon(Cannon _cannon)
	{
		this.cannon = _cannon;
		this.fMode = FollowMode.CannonMode;
	}

	public virtual void SetFollowPlayer()
	{
		this.cannon = null;
		this.fMode = FollowMode.PlayerMode;
		this.OnAniModeOverAction();
	}

	public virtual void SetFollowAnimation()
	{
		this.fMode = FollowMode.Animation;
		Vector3 pos = this.cameraTransform.position;
		Quaternion rotation = this.cameraTransform.rotation;
		this.OnAniModeOverAction = delegate()
		{
			this.cameraTransform.position = pos;
			this.cameraTransform.rotation = rotation;
		};
	}

	public virtual void SetFollowSnipe()
	{
		this.fMode = FollowMode.SnipeMode;
	}

	public void SetSnipeCameraViewConfig(PlayerSnipePoint cameraCfg)
	{
		if (cameraCfg == null)
		{
			return;
		}
		this.snipeLimitAngelH = cameraCfg.maxAngelH;
		this.snipeMinAngelV = cameraCfg.minAngelV;
		this.snipeMaxAngelV = cameraCfg.maxAngelV;
		this.snipeCameraAnchor = cameraCfg.snipeCameraAnchor;
		this.snipeStartAngelH = cameraCfg.transform.rotation.eulerAngles.y;
		this.snipeCameraDistance = cameraCfg.snipeCameraDistances;
		this.cameraComponent.fieldOfView = this.CAMERA_SNIPE_FOV;
	}

	public void SetSnipeCameraFinalBulletAnchor(Transform trans)
	{
		this.snipeFinalBulletAnchor = trans;
	}

	public void SetMusic(bool isOn)
	{
		base.transform.Find("Main Camera").gameObject.GetComponent<AudioSource>().volume = (float)((!isOn) ? 0 : 1);
	}

	public float CheckCameraSpereCast(float targetZ)
	{
		Vector3 origin = this.player.GetTransform().position + this.player.GetTransform().right * this.cameraDistanceFromPlayer.x + this.player.GetTransform().up * this.cameraDistanceFromPlayer.y;
		Ray ray = new Ray(origin, base.transform.forward * -1f);
		if (Physics.SphereCast(ray, this.cameraSphereRadius, out this.cameraSphereCastHit, Mathf.Abs(targetZ), 2048))
		{
			return -this.cameraSphereCastHit.distance;
		}
		return targetZ;
	}

	public float CheckCameraXAxis(float targetX)
	{
		Vector3 origin = this.player.GetTransform().position + this.player.GetTransform().right * this.cameraDistanceFromPlayer.x + this.player.GetTransform().up * this.cameraDistanceFromPlayer.y;
		Ray ray = new Ray(origin, base.transform.right);
		if (Physics.SphereCast(ray, this.cameraSphereRadius, out this.cameraSphereCastHit, Mathf.Abs(targetX), 2048))
		{
			return this.cameraSphereCastHit.distance;
		}
		return targetX;
	}

	public virtual void SetMainCameraFOV(float _fov)
	{
		this.cameraComponent.fieldOfView = _fov;
	}

	public virtual void ZoomIn(float deltaTime)
	{
		this.cameraDistanceFromPlayer.x = Mathf.Lerp(this.cameraDistanceFromPlayer.x, this.CheckCameraXAxis(this.cameraDistanceFromPlayerWhenAimed.x), deltaTime * this.cameraZoomSpeed);
		this.cameraDistanceFromPlayer.y = Mathf.Lerp(this.cameraDistanceFromPlayer.y, this.cameraDistanceFromPlayerWhenAimed.y, deltaTime * this.cameraZoomSpeed);
		this.cameraDistanceFromPlayer.z = Mathf.Lerp(this.cameraDistanceFromPlayer.z, this.CheckCameraSpereCast(this.cameraDistanceFromPlayerWhenAimed.z), deltaTime * this.cameraZoomSpeed);
		this.cameraComponent.fieldOfView = Mathf.Lerp(this.cameraComponent.fieldOfView, this.CAMERA_AIM_FOV, deltaTime * this.cameraZoomSpeed);
	}

	public virtual void ZoomOut(float deltaTime)
	{
		this.cameraDistanceFromPlayer.x = Mathf.Lerp(this.cameraDistanceFromPlayer.x, this.CheckCameraXAxis(this.cameraDistanceFromPlayerWhenIdle.x), deltaTime * this.cameraZoomSpeed);
		this.cameraDistanceFromPlayer.y = Mathf.Lerp(this.cameraDistanceFromPlayer.y, this.cameraDistanceFromPlayerWhenIdle.y, deltaTime * this.cameraZoomSpeed);
		this.cameraDistanceFromPlayer.z = Mathf.Lerp(this.cameraDistanceFromPlayer.z, this.CheckCameraSpereCast(this.cameraDistanceFromPlayerWhenIdle.z), deltaTime * this.cameraZoomSpeed);
		this.cameraComponent.fieldOfView = Mathf.Lerp(this.cameraComponent.fieldOfView, this.CAMERA_NORMAL_FOV, deltaTime * this.cameraZoomSpeed);
	}

	public virtual void CheckCameraCollider(float deltaTime)
	{
		this.cameraDistanceFromPlayer.x = Mathf.Lerp(this.cameraDistanceFromPlayer.x, this.CheckCameraXAxis(this.cameraDistanceFromPlayerWhenIdle.x), deltaTime * this.cameraZoomSpeed);
		this.cameraDistanceFromPlayer.z = Mathf.Lerp(this.cameraDistanceFromPlayer.z, this.CheckCameraSpereCast(this.cameraDistanceFromPlayerWhenIdle.z), deltaTime * this.cameraZoomSpeed);
	}

	public void RegisterCamerControl(Action<GameObject> ctrl)
	{
		this.cameraControl = ctrl;
	}

	public void SetGunStability(GunStability stability)
	{
		this.gunShootStabilityHMax = stability.stabilityHMax;
		this.gunShootStabilityHMin = stability.stabilityHMin;
		this.gunShootStabilityVMax = stability.stabilityVMax;
		this.gunShootStabilityVMin = stability.stabilityVMin;
	}

	public virtual void ChangeCameraControl(FollowMode _fmode)
	{
	}

	private void Update()
	{
		reticlePosition = new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f);
	}

	[Space]
	[CNName("水平稳定Min")]
	public float gunShootStabilityHMin;

	[CNName("水平稳定Max")]
	public float gunShootStabilityHMax;

	[CNName("垂直稳定Min")]
	public float gunShootStabilityVMin;

	[CNName("垂直稳定Max")]
	public float gunShootStabilityVMax;

	[Space]
	[CNName("时间回复比例")]
	public float timeComeBack = 1f;

	[CNName("射击状态持续时间")]
	public float shoot_state_duration = 0.1f;

	[CNName("角色瞄准点")]
	public Transform playerAimTarget;

	[CNName("相机组件")]
	public Camera cameraComponent;

	[CNName("摄像机碰撞半径")]
	public float cameraSphereRadius = 0.05f;

	public BloomOptimized bloom;

	[CNName("滤镜")]
	public ColorCorrectionCurves colorCurves;

	[CNName("模糊效果")]
	public RadialBlurEffect radialBlur;

	protected float angelH;

	protected float angelV;

	protected float lastUpdateTime;

	protected float deltaTime;

	public Player player;

	public GameScene gameScene;

	protected Cannon cannon;

	[Space]
	[Header("相机参数")]
	public Vector3 cameraDistanceFromPlayerWhenIdle;

	public Vector3 cameraDistanceFromPlayerWhenAimed;

	public Vector3 cameraDistanceFromPlayer;

	[CNName("狙击模式相机锚点")]
	public Vector3 snipeCameraAnchor;

	[CNName("狙击模式相机转轴长度")]
	public float snipeCameraDistance;

	public float CAMERA_AIM_FOV = 45f;

	public float CAMERA_NORMAL_FOV = 60f;

	[CNName("狙击初始FOV")]
	public float CAMERA_SNIPE_FOV = 60f;

	public float cameraZoomSpeed = 3f;

	public float cameraSwingSpeed = 15f;

	[CNName("垂直滑屏速度")]
	public float swingSpeedVertical = 1.5f;

	[CNName("水平滑屏速度")]
	public float swingSpeedHorizontal = 1.7f;

	[Space]
	[Header("狙击枪倍镜FOV,测试用")]
	public float[] snipeScopeFOV;

	[Space]
	public float minAngelV;

	public float maxAngelV;

	public float fixedAngelV;

	public bool isAngelVFixed;

	public bool limitReticle;

	public bool allowReticleMove;

	public float reticleLogoRange = 0.15f;

	public float reticleMoveSpeed = 20f;

	public float mutipleSizeReticle;

	[HideInInspector]
	public float snipeMinAngelV;

	[HideInInspector]
	public float snipeMaxAngelV;

	[HideInInspector]
	public float snipeLimitAngelH;

	[HideInInspector]
	public float snipeStartAngelH;

	protected Vector3 moveTo;

	protected bool started;

	protected Vector2 reticlePosition;

	protected Transform cameraTransform;

	protected global::CameraType cameraType;

	public AudioSource loseAudio;

	public FollowMode fMode;

	protected Rect camRect = new Rect(-0.2f, -0.1f, 1.2f, 1.1f);

	protected RaycastHit cameraSphereCastHit;

	protected float cameraSphereCastTargetZ;

	protected Action OnAniModeOverAction;

	protected Camera mainCamera;

	protected Action<GameObject> cameraControl;

	protected float ScreenWidth;

	protected float ScreenHeight;

	protected const float DESIGN_SCREEN_WIDTH = 1280f;

	protected const float DESIGN_SCREEN_HEIGHT = 720f;

	protected float screenXScale;

	protected float screenYScale;

	protected Transform snipeFinalBulletAnchor;
}
