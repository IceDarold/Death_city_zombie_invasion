using System;
using UnityEngine;

public class GamePage : MonoBehaviour
{
	public virtual void Awake()
	{
	}

	public virtual void OnEnable()
	{
	}

	public virtual void Show()
	{
		if (this.Type == PageType.Normal)
		{
			if (!Singleton<UiManager>.Instance.PageStack.Contains(this))
			{
				Singleton<UiManager>.Instance.PageStack.Push(this);
			}
		}
		else if (this.Type == PageType.Popup && !Singleton<UiManager>.Instance.PopupStack.Contains(this))
		{
			Singleton<UiManager>.Instance.PopupStack.Push(this);
		}
		base.transform.SetAsLastSibling();
		base.gameObject.SetActive(true);
		this.Refresh();
	}

	public virtual void Hide()
	{
		base.gameObject.SetActive(false);
	}

	public virtual void Close()
	{
		if (this.Type == PageType.Normal)
		{
			Singleton<UiManager>.Instance.PageStack.Pop();
		}
		else if (this.Type == PageType.Popup)
		{
			Singleton<UiManager>.Instance.PopupStack.Pop();
		}
		Singleton<UiManager>.Instance.SetCurrentPage();
		base.gameObject.SetActive(false);
	}

	public virtual void OnBack()
	{
		this.Close();
		if (Singleton<UiManager>.Instance.CurrentPage != null)
		{
			if (Singleton<UiManager>.Instance.CurrentPage.Type == PageType.Popup && Singleton<UiManager>.Instance.PageStack.Peek() != null && !Singleton<UiManager>.Instance.PageStack.Peek().gameObject.activeSelf)
			{
				Singleton<UiManager>.Instance.PageStack.Peek().Show();
			}
			if (!Singleton<UiManager>.Instance.CurrentPage.gameObject.activeSelf)
			{
				Singleton<UiManager>.Instance.CurrentPage.Show();
			}
		}
	}

	public virtual void Refresh()
	{
	}

	public PageName Name;

	public PageType Type;

	protected bool IsAction;
}
