using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollBtn : MonoBehaviour, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IEventSystemHandler
{
	public void OnPointerUp(PointerEventData eventData)
	{
		this.logic.OnPointerUp(null);
	}

	public void OnDrag(PointerEventData eventData)
	{
		this.logic.DoDrag(eventData);
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		this.logic.BeginDrag(eventData);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		this.logic.EndDrag(eventData);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		this.logic.OnPointerDown(eventData);
	}

	public ScrollLogic logic;
}
