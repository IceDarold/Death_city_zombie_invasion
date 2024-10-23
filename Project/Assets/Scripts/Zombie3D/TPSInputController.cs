using ui;
using UnityEngine;

namespace Zombie3D
{
	public class TPSInputController : InputController
	{
		public override void ProcessInput(float deltaTime, InputInfo inputInfo)
		{
			PlayingState playingState = GameApp.GetInstance().GetGameScene().PlayingState;
			
			if (Singleton<UiControllers>.Instance.IsMobile)
			{
				MobileMoving();
				MobileRotation();
				MobileManualShot(inputInfo);
			}
			else
			{
				DesktopMoving();
				DesktopRotation();
							
				QTE();
				Prop1();
				Prop2();
				ChangeWeapon();
				RefreshWeapon();
				Shot(inputInfo);
			}
			if (playingState != PlayingState.GamePlaying && playingState != PlayingState.WaitForEnd)
			{
				return;
			}
			Transform transform = this.player.GetTransform();
			if (this.gameScene.PlayingMode == GamePlayingMode.SnipeMode)
			{
				inputInfo.fire = false;
			}
			else if (this.gameScene.ControlMode == GameControlMode.AUTOFIRE && this.player.GetWeaponAimed() == AimState.Aimed)
			{
				inputInfo.fire = true;
			}

			// if (Application.isEditor)
			// {
			// 	if (base.EnableMoveInput)
			// 	{
			// 		this.moveDirection = new Vector3(UnityEngine.Input.GetAxis("Horizontal"), 0f, UnityEngine.Input.GetAxis("Vertical"));
			// 	}
			// 	MouseRotation();
			// }
			// else
			// {
			// 	this.touchX = 0f;
			// 	this.touchY = 0f;
			// 	this.cameraRotation.x = 0f;
			// 	this.cameraRotation.y = 0f;
			// 	if (UnityEngine.Input.touchCount == 0)
			// 	{
			// 		this.thumbTouchFingerId = -1;
			// 		this.shootingTouchFingerId = -1;
			// 		this.lastShootTouch = this.shootThumbCenterToScreen;
			// 	}
			// 	for (int i = 0; i < UnityEngine.Input.touchCount; i++)
			// 	{
			// 		if (i == 2)
			// 		{
			// 			break;
			// 		}
			// 		Touch touch = UnityEngine.Input.GetTouch(i);
			// 		Vector2 vector = touch.position - this.thumbCenterToScreen;
			// 		bool flag = vector.sqrMagnitude < this.thumbRadius * this.thumbRadius;
			// 		bool flag2 = touch.fingerId == this.thumbTouchFingerId;
			// 		if (touch.phase == TouchPhase.Began)
			// 		{
			// 			if (this.gameScene.PlayingMode == GamePlayingMode.SnipeMode)
			// 			{
			// 				if (this.moveTouchFingerId != -1)
			// 				{
			// 					return;
			// 				}
			// 				PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
			// 				pointerEventData.position = touch.position;
			// 				GraphicRaycaster component = Singleton<UiManager>.Instance.PageRoot.GetComponent<Canvas>().gameObject.GetComponent<GraphicRaycaster>();
			// 				List<RaycastResult> list = new List<RaycastResult>();
			// 				component.Raycast(pointerEventData, list);
			// 				if (list.Count <= 0)
			// 				{
			// 					this.moveTouchFingerId = touch.fingerId;
			// 				}
			// 			}
			// 			else if (touch.position.x < (float)(Screen.width / 2))
			// 			{
			// 				this.moveTouchStartPosition = touch.position;
			// 				this.thumbCenter = touch.position;
			// 				this.thumbCenterToScreen = base.ThumbCenter;
			// 				this.thumbTouchFingerId = touch.fingerId;
			// 			}
			// 			if (this.gameScene.ControlMode == GameControlMode.MANUALFIRE && this.gameScene.IsInShootThumb(touch.position))
			// 			{
			// 				inputInfo.fire = true;
			// 				this.shootingTouchFingerId = touch.fingerId;
			// 				this.gameScene.SetTouchShootThumb(true);
			// 			}
			// 		}
			// 		else if (touch.phase == TouchPhase.Stationary)
			// 		{
			// 			if (this.gameScene.ControlMode == GameControlMode.MANUALFIRE && touch.fingerId == this.shootingTouchFingerId)
			// 			{
			// 				inputInfo.fire = true;
			// 			}
			// 			if (flag || flag2)
			// 			{
			// 				if (flag)
			// 				{
			// 					this.touchX = vector.x / this.thumbRadius;
			// 					this.touchY = vector.y / this.thumbRadius;
			// 					this.moveThumbPos.x = vector.x / this.thumbRadius * 50f;
			// 					this.moveThumbPos.y = vector.y / this.thumbRadius * 50f;
			// 				}
			// 				else
			// 				{
			// 					this.touchX = vector.x / this.thumbRadius;
			// 					this.touchY = vector.y / this.thumbRadius;
			// 					this.moveThumbPos.x = vector.normalized.x * 50f;
			// 					this.moveThumbPos.y = vector.normalized.y * 50f;
			// 					if (Mathf.Abs(this.touchX) > Mathf.Abs(this.touchY))
			// 					{
			// 						this.touchY /= Mathf.Abs(this.touchX);
			// 						this.touchX = (float)((this.touchX <= 0f) ? -1 : 1);
			// 					}
			// 					else if (this.touchY != 0f)
			// 					{
			// 						this.touchX /= Mathf.Abs(this.touchY);
			// 						this.touchY = (float)((this.touchY <= 0f) ? -1 : 1);
			// 					}
			// 					else
			// 					{
			// 						this.touchX = 0f;
			// 						this.touchY = 0f;
			// 					}
			// 				}
			// 				this.thumbTouchFingerId = touch.fingerId;
			// 			}
			// 		}
			// 		else if (touch.phase == TouchPhase.Moved)
			// 		{
			// 			if (this.gameScene.ControlMode == GameControlMode.MANUALFIRE && touch.fingerId == this.shootingTouchFingerId)
			// 			{
			// 				inputInfo.fire = true;
			// 			}
			// 			if (flag || flag2)
			// 			{
			// 				if (flag)
			// 				{
			// 					this.touchX = vector.x / this.thumbRadius;
			// 					this.touchY = vector.y / this.thumbRadius;
			// 					this.moveThumbPos.x = vector.x / this.thumbRadius * 50f;
			// 					this.moveThumbPos.y = vector.y / this.thumbRadius * 50f;
			// 				}
			// 				else
			// 				{
			// 					this.touchX = vector.x / this.thumbRadius;
			// 					this.touchY = vector.y / this.thumbRadius;
			// 					this.moveThumbPos.x = vector.normalized.x * 50f;
			// 					this.moveThumbPos.y = vector.normalized.y * 50f;
			// 					if (Mathf.Abs(this.touchX) > Mathf.Abs(this.touchY))
			// 					{
			// 						this.touchY /= Mathf.Abs(this.touchX);
			// 						this.touchX = (float)((this.touchX <= 0f) ? -1 : 1);
			// 					}
			// 					else if (this.touchY != 0f)
			// 					{
			// 						this.touchX /= Mathf.Abs(this.touchY);
			// 						this.touchY = (float)((this.touchY <= 0f) ? -1 : 1);
			// 					}
			// 					else
			// 					{
			// 						this.touchX = 0f;
			// 						this.touchY = 0f;
			// 					}
			// 				}
			// 				this.thumbTouchFingerId = touch.fingerId;
			// 			}
			// 			else if (base.EnableTurningAround)
			// 			{
			// 				if (this.lastMoveTouch.phase == TouchPhase.Moved)
			// 				{
			// 					if (touch.fingerId == this.moveTouchFingerId)
			// 					{
			// 						float num = touch.position.x - this.lastMoveTouch.position.x;
			// 						float num2 = touch.position.y - this.lastMoveTouch.position.y;
			// 						if (num != 0f)
			// 						{
			// 							float num3 = Mathf.Atan(Mathf.Abs(num2) / Mathf.Abs(num)) * 57.29578f;
			// 							if (num3 < this.limitDegree)
			// 							{
			// 								num2 = 0f;
			// 							}
			// 						}
			// 						this.cameraRotation.x = num * this.cameraRotationXRatio;
			// 						this.cameraRotation.y = num2 * this.cameraRotationYRatio;
			// 					}
			// 					else if (touch.fingerId == this.moveTouchFingerId2)
			// 					{
			// 						float num4 = touch.position.x - this.lastMoveTouch2.position.x;
			// 						float num5 = touch.position.y - this.lastMoveTouch2.position.y;
			// 						this.cameraRotation.x = num4 * this.cameraRotationXRatio;
			// 						this.cameraRotation.y = num5 * this.cameraRotationYRatio;
			// 					}
			// 				}
			// 				if (this.gameScene.PlayingMode != GamePlayingMode.SnipeMode)
			// 				{
			// 					if (this.moveTouchFingerId == -1)
			// 					{
			// 						this.moveTouchFingerId = touch.fingerId;
			// 					}
			// 					if (this.moveTouchFingerId != -1 && touch.fingerId != this.moveTouchFingerId)
			// 					{
			// 						this.moveTouchFingerId2 = touch.fingerId;
			// 					}
			// 				}
			// 				if (touch.fingerId == this.moveTouchFingerId)
			// 				{
			// 					this.lastMoveTouch.phase = TouchPhase.Moved;
			// 					this.lastMoveTouch.position = touch.position;
			// 				}
			// 				if (touch.fingerId == this.moveTouchFingerId2)
			// 				{
			// 					this.lastMoveTouch2.phase = TouchPhase.Moved;
			// 					this.lastMoveTouch2.position = touch.position;
			// 				}
			// 			}
			// 		}
			// 		else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
			// 		{
			// 			if (touch.fingerId == this.thumbTouchFingerId)
			// 			{
			// 				this.thumbTouchFingerId = -1;
			// 				this.moveTouchStartPosition = Vector2.one * -9999f;
			// 				this.moveThumbPos = Vector2.zero;
			// 			}
			// 			if (touch.fingerId == this.shootingTouchFingerId)
			// 			{
			// 				this.shootingTouchFingerId = -1;
			// 				this.lastShootTouch = this.shootThumbCenterToScreen;
			// 				this.shootThunmPos = Vector2.zero;
			// 				inputInfo.fire = false;
			// 				this.gameScene.SetTouchShootThumb(false);
			// 			}
			// 			if (touch.fingerId == this.moveTouchFingerId)
			// 			{
			// 				this.moveTouchFingerId = -1;
			// 				this.lastMoveTouch.phase = TouchPhase.Ended;
			// 			}
			// 			if (touch.fingerId == this.moveTouchFingerId2)
			// 			{
			// 				this.moveTouchFingerId2 = -1;
			// 				this.lastMoveTouch2.phase = TouchPhase.Ended;
			// 			}
			// 		}
			// 		this.lastTouch[i] = touch;
			// 	}
			// 	this.touchX = Mathf.Clamp(this.touchX, -1f, 1f);
			// 	this.touchY = Mathf.Clamp(this.touchY, -1f, 1f);
			// 	this.moveDirection = new Vector3(this.touchX, 0f, this.touchY);
			// }
			if (this.player.GetState() == Player.CANNON_STATE)
			{
				this.player.SetPlayerIdle(false);
				return;
			}
			this.player.SetPlayerMoveDirection(this.moveDirection.x, this.moveDirection.z);
			this.moveDirection = transform.TransformDirection(this.moveDirection);
			if (!base.EnableMoveInput)
			{
				this.moveDirection = Vector3.zero;
			}
			if (!base.EnableShootingInput || this.player.GetWeapon().BulletCount <= 0)
			{
				inputInfo.fire = false;
			}
			this.moveDirection += Physics.gravity * deltaTime * 10f;
			inputInfo.moveDirection = this.moveDirection;
		}

		private void DesktopMoving()
		{
			this.moveDirection = new Vector3(UnityEngine.Input.GetAxis("Horizontal"), 0f, UnityEngine.Input.GetAxis("Vertical"));
		}

		private void MobileMoving()
		{
			var joystickDirection = Singleton<UiControllers>.Instance.LeftJoystick.Direction;
			moveDirection = new Vector3(joystickDirection.x, 0, joystickDirection.y);
		}

		private void MobileRotation()
		{
			var joystickDirection = GameApp.GetInstance().GetGameScene().PlayingMode == GamePlayingMode.Normal ? 
				Singleton<UiControllers>.Instance.RightJoystick.Direction : 
				Singleton<UiControllers>.Instance.SnipeJoystick.Direction;
			
			Rotation(joystickDirection.x * 7, joystickDirection.y * 7);
		}
		
		private void DesktopRotation()
		{
			Rotation(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
		}

		private void Rotation(float x, float y)
		{
			this.touchX = 0f;
			this.touchY = 0f;
			this.cameraRotation.x = 0f;
			this.cameraRotation.y = 0f;
			float num = x * 10;
			float num2 = y * 10;
			if (num != 0f)
			{
				float num3 = Mathf.Atan(Mathf.Abs(num2) / Mathf.Abs(num)) * 57.29578f;
				if (num3 < this.limitDegree)
				{
					num2 = 0f;
				}
			}
			this.cameraRotation.x = num * this.cameraRotationXRatio;
			this.cameraRotation.y = num2 * this.cameraRotationYRatio;

			this.touchX = Mathf.Clamp(this.touchX, -1f, 1f);
			this.touchY = Mathf.Clamp(this.touchY, -1f, 1f);
		}

		private void QTE()
		{
			if (Input.GetKeyDown(KeyCode.E))
			{
				var inGamePage = Singleton<UiManager>.Instance.GetPage(PageName.InGamePage) as InGamePage;
				inGamePage.OnclickQTE();
			}
		}

		private void Prop1()
		{
			if (Input.GetKeyDown(KeyCode.G))
			{
				var inGamePage = Singleton<UiManager>.Instance.GetPage(PageName.InGamePage) as InGamePage;
				inGamePage.OnclickProp();
			}
		}
		
		private void Prop2()
		{
			if (Input.GetKeyDown(KeyCode.F))
			{
				var inGamePage = Singleton<UiManager>.Instance.GetPage(PageName.InGamePage) as InGamePage;
				inGamePage.OnclickProp2();
			}
		}
		
		private void ChangeWeapon()
		{
			if (Input.GetKeyDown(KeyCode.Q))
			{
				var inGamePage = Singleton<UiManager>.Instance.GetPage(PageName.InGamePage) as InGamePage;
				inGamePage.OnclickChangeWeapon();
			}
		}		
		
		private void RefreshWeapon()
		{
			if (Input.GetKeyDown(KeyCode.R))
			{
				var inGamePage = Singleton<UiManager>.Instance.GetPage(PageName.InGamePage) as InGamePage;
				inGamePage.ChangeBullet();
			}
		}

		private void Shot(InputInfo inputInfo)
		{
			if (gameScene.PlayingState != PlayingState.GamePlaying) return;
			if (Input.GetMouseButtonDown(0))
			{
				//Debug.Log("SHOT");
				Singleton<UiControllers>.Instance.Shot();
				if (this.gameScene.PlayingMode == GamePlayingMode.Cannon)
				{
					var inGamePage = Singleton<UiManager>.Instance.GetPage(PageName.InGamePage) as InGamePage;
					inGamePage.DoCannonShoot(true);
				}
			}

			if (Input.GetMouseButtonUp(0))
			{
				if (this.gameScene.PlayingMode == GamePlayingMode.Cannon)
				{
					var inGamePage = Singleton<UiManager>.Instance.GetPage(PageName.InGamePage) as InGamePage;
					inGamePage.DoCannonShoot(false);
				}
			}

			if (Input.GetMouseButton(0))
			{
				inputInfo.fire = true;
			}
		}

		private void MobileManualShot(InputInfo inputInfo)
		{
			var position = Singleton<UiControllers>.Instance.RightJoystick.StartInput;
			if (this.gameScene.ControlMode == GameControlMode.MANUALFIRE && this.gameScene.IsInShootThumb(position))
			{
				inputInfo.fire = true;
				this.gameScene.SetTouchShootThumb(true);
			}
		}

		protected TouchInfo lastMoveTouch = new TouchInfo();

		protected TouchInfo lastMoveTouch2 = new TouchInfo();

		private bool scanLeft = true;

		private bool scanRight;

		private Vector2 scanPoint = Vector2.zero;

		private bool find;

		private Vector3 targetEuler;

		private Collider targetCollider;
	}
}
