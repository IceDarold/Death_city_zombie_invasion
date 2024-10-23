using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollLogic : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IEventSystemHandler
{
	public int CurIndex
	{
		get
		{
			return this.curIndex;
		}
	}

	public float[] Pages
	{
		get
		{
			return this.PageIndex;
		}
	}

	public void Update()
	{
		if (!this.canLerp && !this.currentUp)
		{
			return;
		}
		this.NormalizedPosition = this.scroll.horizontalNormalizedPosition;
		if (this.canLerp)
		{
			this.scroll.horizontalNormalizedPosition = Mathf.Lerp(this.PreHorizontalNormalizedPosition, this.lerpTarget, (Time.time - this.startTime) * this.smooth);
			if (Mathf.Abs(this.scroll.horizontalNormalizedPosition - this.lerpTarget) < 1E-06f)
			{
				this.scroll.horizontalNormalizedPosition = this.lerpTarget;
				this.canLerp = false;
				MapsPage.instance.RefreshPage();
			}
		}
		if (this.currentUp)
		{
			this.scroll.horizontalNormalizedPosition = Mathf.Lerp(this.UpHorizontalNormalizedPosition, this.lerpTarget, (Time.time - this.startTime) * this.smooth);
			if (Mathf.Abs(this.scroll.horizontalNormalizedPosition - this.UpHorizontalNormalizedPosition) < 1E-07f)
			{
				this.scroll.horizontalNormalizedPosition = this.lerpTarget;
				this.currentUp = false;
				MapsPage.instance.RefreshPage();
			}
		}
	}

	public void DoDrag(PointerEventData eventData)
	{
		this.scroll.OnDrag(eventData);
	}

	public void BeginDrag(PointerEventData eventData)
	{
		this.scroll.OnBeginDrag(eventData);
	}

	public void EndDrag(PointerEventData eventData)
	{
		this.scroll.OnEndDrag(eventData);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		this.canLerp = false;
		this.startTime = Time.time;
		if (this.scroll.horizontalNormalizedPosition <= 1f && this.scroll.horizontalNormalizedPosition >= 0f)
		{
			this.PreHorizontalNormalizedPosition = this.scroll.horizontalNormalizedPosition;
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (this.canLerp)
		{
			return;
		}
		this.UpHorizontalNormalizedPosition = this.scroll.horizontalNormalizedPosition;
		this.DoLerp();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
	}

	public void OnSwitchChapter(int dir)
	{
		int num = (dir <= 0) ? ((this.curIndex + dir < 0) ? this.curIndex : (this.curIndex + dir)) : ((this.curIndex + dir >= this.PageIndex.Length) ? this.curIndex : (this.curIndex + dir));
		if (this.curIndex == num)
		{
			return;
		}
		this.curIndex = num;
		this.LerpToPage(num);
	}

	private void DoLerp()
	{
		float num = this.scroll.horizontalNormalizedPosition - this.PreHorizontalNormalizedPosition;
		if (Mathf.Abs(num) >= 0.02f)
		{
			if (num > 0f)
			{
				if (this.curIndex + 1 < this.PageIndex.Length)
				{
					this.curIndex++;
				}
			}
			else if (this.curIndex - 1 >= 0)
			{
				this.curIndex--;
			}
		}
		this.canLerp = true;
		MapsPage.instance.PageIndex = this.curIndex;
		this.lerpTarget = this.PageIndex[this.curIndex];
	}

	public void SetPageIndex(int index = 0)
	{
		this.scroll.horizontalNormalizedPosition = this.PageIndex[index];
		this.curIndex = index;
	}

	public void LerpToPage(int index = 0)
	{
		this.startTime = Time.time;
		this.PreHorizontalNormalizedPosition = this.scroll.horizontalNormalizedPosition;
		this.lerpTarget = this.PageIndex[index];
		this.canLerp = true;
	}

	public ScrollRect scroll;

	public float[] PageIndex = new float[]
	{
		0f,
		0.5f,
		1f
	};

	public float smooth = 1f;

	public bool canLerp;

	public bool currentUp;

	protected float lerpTarget;

	public float PreHorizontalNormalizedPosition;

	public float UpHorizontalNormalizedPosition;

	public int curIndex;

	public float NormalizedPosition;

	public float startTime;
}
