using System;
using UnityEngine;
using UnityEngine.UI;

public class UITip : MonoBehaviour
{
	public void SetContent(string _content, Action cb)
	{
		this.content.text = _content;
		this.callback = cb;
	}

	public void OnGoToShopPressed()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("UIShop")) as GameObject;
		gameObject.transform.parent = GameObject.Find("Canvas").transform;
		gameObject.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, -700f);
		gameObject.transform.localScale = Vector3.one;
		gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		gameObject.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
		base.gameObject.SetActive(false);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void OnVideoPressed()
	{
		base.gameObject.SetActive(false);
		UnityEngine.Object.Destroy(base.gameObject);
		if (this.callback != null)
		{
			this.callback();
		}
	}

	public Text content;

	private Action callback;
}
