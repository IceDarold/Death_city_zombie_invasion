using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShootBtn : MonoBehaviour
{
	public void Start()
	{
		Vector3 vector = Singleton<UiManager>.Instance.UiCamera.WorldToScreenPoint(this.rect.position);
		this.screenPosition = new Vector2(vector.x, vector.y);
		this.thumbRadius = this.rect.sizeDelta.x;
	}

	public bool IsInShootRadius(Vector2 touchPos)
	{
		return (touchPos - this.screenPosition).sqrMagnitude < this.thumbRadius * this.thumbRadius;
	}

	public void OnTouchEnter()
	{
		this.isPressed = true;
		this.image.color = Color.gray;
	}

	public void OnTouchRelease()
	{
		this.isPressed = false;
		this.image.color = Color.white;
	}

	public Image image;

	public bool isPressed;

	public Action OnPlayerShootBtnPressDown;

	public RectTransform rect;

	protected Vector2 screenPosition;

	protected float thumbRadius;
}
