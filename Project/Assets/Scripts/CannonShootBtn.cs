using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CannonShootBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IEventSystemHandler
{
	public void OnPointerDown(PointerEventData eventData)
	{
		this.image.color = Color.gray;
		this.inGamePage.DoCannonShoot(true);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		this.image.color = Color.white;
		this.inGamePage.DoCannonShoot(false);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		this.image.color = Color.white;
		this.inGamePage.DoCannonShoot(false);
	}

	public InGamePage inGamePage;

	public Image image;
}
