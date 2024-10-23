using System;
using RacingMode;
using UnityEngine;
using UnityEngine.EventSystems;

public class RacingJoyStick : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IEventSystemHandler
{
	public void OnBeginDrag(PointerEventData eventData)
	{
		Write.Log("OnBeginDrag");
		this.TouchPosition = eventData.position;
		this.Stick.localPosition = Vector3.zero;
		this.isTouch = true;
	}

	public void OnDrag(PointerEventData eventData)
	{
		Write.Log("OnDrag");
		if (this.isTouch)
		{
			if (eventData.position.x - this.TouchPosition.x > 0f)
			{
				this.Stick.localPosition = new Vector3(Mathf.Clamp(eventData.position.x - this.TouchPosition.x, 0f, this.MaxDistance), 0f, 0f);
			}
			else
			{
				this.Stick.localPosition = new Vector3(Mathf.Clamp(eventData.position.x - this.TouchPosition.x, -this.MaxDistance, 0f), 0f, 0f);
			}
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		Write.Log("OnEndDrag");
		this.isTouch = false;
		RacingSceneManager.Instance.Car.StopTurning();
	}

	public void FixedUpdate()
	{
		if (this.isTouch)
		{
			if (this.Stick.localPosition.x > 0f)
			{
				RacingSceneManager.Instance.Car.TurningRight();
			}
			else
			{
				RacingSceneManager.Instance.Car.TurningLeft();
			}
		}
		else
		{
			this.Stick.localPosition = Vector3.Lerp(this.Stick.localPosition, Vector3.zero, Time.deltaTime * this.BackSpeed);
		}
	}

	public Transform Stick;

	public float BackSpeed = 8f;

	public float MaxDistance = 80f;

	private Vector2 TouchPosition;

	private bool isTouch;
}
