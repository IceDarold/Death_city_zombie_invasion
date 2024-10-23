using System;
using UnityEngine;

namespace Zombie3D
{
	public abstract class InputController
	{
		public bool EnableMoveInput { get; set; }

		public bool EnableTurningAround { get; set; }

		public bool EnableShootingInput { get; set; }

		public int GetMoveTouchFingerID()
		{
			return this.thumbTouchFingerId;
		}

		public int GetShootingTouchFingerID()
		{
			return this.shootingTouchFingerId;
		}

		public string PhaseStr
		{
			get
			{
				return this.phaseStr;
			}
		}

		public Vector2 LastTouchPos
		{
			get
			{
				return new Vector2(this.thumbCenterToScreen.x + this.touchX * this.thumbRadius, this.thumbCenterToScreen.y + this.touchY * this.thumbRadius);
			}
		}

		public Vector2 LastShootTouch
		{
			get
			{
				return new Vector2(this.lastShootTouch.x, this.lastShootTouch.y);
			}
		}

		public Vector2 ThumbCenter
		{
			get
			{
				return this.thumbCenter;
			}
		}

		public Vector2 ShootThumbCenter
		{
			get
			{
				return this.shootThumbCenter;
			}
		}

		public float ThumbRadius
		{
			get
			{
				return this.thumbRadius;
			}
		}

		public Vector2 CameraRotation
		{
			get
			{
				return this.cameraRotation;
			}
			set
			{
				this.cameraRotation = value;
			}
		}

		public Vector2 Deflection
		{
			get
			{
				return this.deflection;
			}
		}

		public void Init()
		{
			this.thumbCenter = Vector2.one * -9999f;
			this.thumbRadius = (float)(Screen.width * 100 / 640 / 2);
			this.shootThumbCenter.x = (float)Screen.width - this.thumbRadius;
			this.shootThumbCenter.y = this.thumbRadius;
			this.thumbCenterToScreen = new Vector2(this.thumbCenter.x, this.thumbCenter.y);
			this.shootThumbCenterToScreen = new Vector2(this.shootThumbCenter.x, this.shootThumbCenter.y);
			this.lastShootTouch = this.shootThumbCenterToScreen;
			for (int i = 0; i < 2; i++)
			{
				this.lastTouch[i] = default(Touch);
			}
			this.gameScene = GameApp.GetInstance().GetGameScene();
			this.player = this.gameScene.GetPlayer();
			this.EnableMoveInput = true;
			this.EnableShootingInput = true;
			this.EnableTurningAround = true;
		}

		public abstract void ProcessInput(float deltaTime, InputInfo inputInfo);

		public Vector2 moveTouchStartPosition = Vector2.one * -9999f;

		public Vector2 moveThumbPos = Vector2.zero;

		public Vector2 shootThunmPos = Vector3.zero;

		public float cameraRotationXRatio = 0.3f;

		public float cameraRotationYRatio = 0.16f;

		public float cameraLimitMin = 0.8f;

		public float cameraLimitMax = 8f;

		public float cameraControlRatioMin = 0.5f;

		public float cameraControlRatioMax = 1f;

		public float limitRatio = 0.001f;

		public float controlRatio = 0.001f;

		public float limitDegree = 40f;

		protected Touch[] lastTouch = new Touch[2];

		protected Vector2 cameraRotation = new Vector2(0f, 0f);

		protected Vector2 deflection;

		protected Vector2 thumbCenter;

		protected Vector2 thumbCenterToScreen;

		protected Vector2 shootThumbCenter;

		protected Vector2 shootThumbCenterToScreen;

		protected Vector2 lastShootTouch = default(Vector2);

		protected float touchX;

		protected float touchY;

		protected float thumbRadius;

		protected int thumbTouchFingerId = -1;

		protected int shootingTouchFingerId = -1;

		protected int moveTouchFingerId = -1;

		protected int moveTouchFingerId2 = -1;

		protected string phaseStr = ".";

		protected Vector3 moveDirection = Vector3.zero;

		protected GameScene gameScene;

		protected Player player;
	}
}
