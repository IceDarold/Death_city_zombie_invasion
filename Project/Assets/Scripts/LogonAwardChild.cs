using System;
using DataCenter;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class LogonAwardChild : MonoBehaviour
{
	public void init(LogonAward data)
	{
		this.Name.text = Singleton<GlobalData>.Instance.GetText(data.Name);
		this.Count.text = "X " + data.AwardCount.ToString();
		Singleton<FontChanger>.Instance.SetFont(Name);
		Singleton<FontChanger>.Instance.SetFont(Count);
		this.Icon.sprite = Singleton<UiManager>.Instance.GetSprite(data.Icon);
		this.Icon.preserveAspect = true;
		if (data.AwardItem >= 2000 && data.AwardItem < 3000)
		{
			this.Icon.transform.localEulerAngles = new Vector3(0f, 0f, -45f);
			this.Icon.transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
		}
		else
		{
			this.Icon.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			this.Icon.transform.localScale = Vector3.one;
		}
		int state = data.State;
		if (state != 0)
		{
			if (state != 1)
			{
				if (state == 2)
				{
					this.NameRoot.SetActive(true);
					if (data.ReceiveTimes == 1)
					{
						this.ReceiveButton.gameObject.SetActive(true);
						this.VedioImage.gameObject.SetActive(true);
						this.Count.gameObject.SetActive(true);
						this.ButtonName.text = Singleton<GlobalData>.Instance.GetText("ONCE_AGAIN");
						this.ButtonName.color = this.TextColor[1];
						this.Count.color = this.TextColor[2];
					}
					else
					{
						this.ReceiveButton.gameObject.SetActive(false);
						this.VedioImage.gameObject.SetActive(false);
						this.Count.gameObject.SetActive(false);
						this.ButtonName.text = Singleton<GlobalData>.Instance.GetText("RECEIVED");
						this.ButtonName.color = this.TextColor[0];
					}
				}
			}
			else
			{
				this.ReceiveButton.gameObject.SetActive(true);
				this.NameRoot.SetActive(true);
				this.VedioImage.gameObject.SetActive(false);
				this.ButtonName.text = Singleton<GlobalData>.Instance.GetText("RECEIVE");
				this.Count.gameObject.SetActive(true);
				this.ButtonName.color = this.TextColor[1];
				this.Count.color = this.TextColor[2];
			}
			Singleton<FontChanger>.Instance.SetFont(ButtonName);
		}
		else
		{
			this.ReceiveButton.gameObject.SetActive(false);
			this.NameRoot.SetActive(false);
			this.VedioImage.gameObject.SetActive(false);
			this.Count.gameObject.SetActive(true);
			this.Count.color = this.TextColor[0];
		}
		base.gameObject.SetActive(true);
	}

	public Text Name;

	public Text Count;

	public Image Icon;

	public Button ReceiveButton;

	public GameObject NameRoot;

	public Image VedioImage;

	public Text ButtonName;

	public Color[] TextColor;
}
