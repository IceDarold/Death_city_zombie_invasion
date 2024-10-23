using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zombie3D
{
	public class TopWatchingInputController : InputController
	{
		public override void ProcessInput(float deltaTime, InputInfo inputInfo)
		{
			Weapon weapon = this.player.GetWeapon();
			GameObject playerObject = this.player.PlayerObject;
			Vector3 zero = Vector3.zero;
			List<Weapon> list = new List<Weapon>();
			Transform respawnTransform = this.player.GetRespawnTransform();
			if (Application.platform != RuntimePlatform.Android)
			{
				if (Input.GetButton("Fire1"))
				{
					this.player.Fire(deltaTime);
				}
				else
				{
					this.player.StopFire();
				}
				this.moveDirection = new Vector3(UnityEngine.Input.GetAxis("Horizontal"), 0f, UnityEngine.Input.GetAxis("Vertical"));
			}
			else
			{
				this.touchX = 0f;
				this.touchY = 0f;
				this.lastShootTouch = this.shootThumbCenterToScreen;
				this.cameraRotation.x = 0f;
				this.cameraRotation.y = 0f;
				bool flag = false;
				for (int i = 0; i < UnityEngine.Input.touchCount; i++)
				{
					if (i == 2)
					{
						break;
					}
					Touch touch = UnityEngine.Input.GetTouch(i);
					this.phaseStr = string.Concat(new object[]
					{
						touch.phase.ToString(),
						touch.fingerId,
						" p:",
						touch.position.x,
						",",
						touch.position.y
					});
					Vector2 vector = touch.position - this.thumbCenterToScreen;
					bool flag2 = vector.sqrMagnitude < this.thumbRadius * this.thumbRadius;
					bool flag3 = touch.fingerId == this.thumbTouchFingerId;
					if (touch.phase == TouchPhase.Stationary)
					{
						if (flag2 || flag3)
						{
							if (flag2)
							{
								this.touchX = vector.x / this.thumbRadius;
								this.touchY = vector.y / this.thumbRadius;
							}
							else
							{
								this.touchX = vector.x / this.thumbRadius;
								this.touchY = vector.y / this.thumbRadius;
								if (Mathf.Abs(this.touchX) > Mathf.Abs(this.touchY))
								{
									this.touchY /= Mathf.Abs(this.touchX);
									this.touchX = (float)((this.touchX <= 0f) ? -1 : 1);
								}
								else if (this.touchY != 0f)
								{
									this.touchX /= Mathf.Abs(this.touchY);
									this.touchY = (float)((this.touchY <= 0f) ? -1 : 1);
								}
								else
								{
									this.touchX = 0f;
									this.touchY = 0f;
								}
							}
							this.thumbTouchFingerId = touch.fingerId;
						}
						else if ((touch.position - this.shootThumbCenterToScreen).sqrMagnitude < this.thumbRadius * this.thumbRadius)
						{
							this.player.Fire(deltaTime);
							this.shootingTouchFingerId = touch.fingerId;
							flag = true;
							this.lastShootTouch = touch.position;
						}
					}
					else if (touch.phase == TouchPhase.Moved)
					{
						if (flag2 || flag3)
						{
							if (flag2)
							{
								this.touchX = vector.x / this.thumbRadius;
								this.touchY = vector.y / this.thumbRadius;
							}
							else
							{
								this.touchX = vector.x / this.thumbRadius;
								this.touchY = vector.y / this.thumbRadius;
								if (Mathf.Abs(this.touchX) > Mathf.Abs(this.touchY))
								{
									this.touchY /= Mathf.Abs(this.touchX);
									this.touchX = (float)((this.touchX <= 0f) ? -1 : 1);
								}
								else if (this.touchY != 0f)
								{
									this.touchX /= Mathf.Abs(this.touchY);
									this.touchY = (float)((this.touchY <= 0f) ? -1 : 1);
								}
								else
								{
									this.touchX = 0f;
									this.touchY = 0f;
								}
							}
							this.thumbTouchFingerId = touch.fingerId;
						}
						else
						{
							this.cameraRotation.x = touch.deltaPosition.x * 0.2f;
							this.cameraRotation.y = touch.deltaPosition.y * 0.2f;
							bool flag4 = (touch.position - this.shootThumbCenterToScreen).sqrMagnitude < this.thumbRadius * this.thumbRadius;
							if (this.shootingTouchFingerId == touch.fingerId || flag4)
							{
								this.player.Fire(deltaTime);
								flag = true;
								if (flag4)
								{
									this.lastShootTouch = touch.position;
								}
							}
						}
					}
					else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
					{
						if (touch.fingerId == this.thumbTouchFingerId)
						{
							this.thumbTouchFingerId = -1;
						}
						if (touch.fingerId == this.shootingTouchFingerId)
						{
							this.shootingTouchFingerId = -1;
						}
					}
					this.lastTouch[i] = touch;
				}
				if (!flag)
				{
					this.player.StopFire();
				}
				this.touchX = Mathf.Clamp(this.touchX, -1f, 1f);
				this.touchY = Mathf.Clamp(this.touchY, -1f, 1f);
				this.moveDirection = new Vector3(this.touchX, 0f, this.touchY);
			}
			this.moveDirection = respawnTransform.TransformDirection(this.moveDirection);
			this.moveDirection += Physics.gravity * deltaTime;
			zero.x = Mathf.Lerp(zero.x, 0f, 5f * Time.deltaTime);
			zero.y = Mathf.Lerp(zero.y, 0f, -Physics.gravity.y * Time.deltaTime);
			zero.z = Mathf.Lerp(zero.z, 0f, 5f * Time.deltaTime);
			for (int j = 1; j <= 3; j++)
			{
				if (Input.GetButton("Weapon" + j) && list[j - 1] != null)
				{
					this.player.ChangeWeapon(list[j - 1]);
				}
			}
		}
	}
}
