using System;
using DataCenter;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class TalentChild : MonoBehaviour
{
	public void Refresh(TalentData data, bool select)
	{
		this.Name.text = Singleton<GlobalData>.Instance.GetText(data.Name);
		Singleton<FontChanger>.Instance.SetFont(Name);
		this.Icon.sprite = Singleton<UiManager>.Instance.GetSprite(data.Icon);
		this.Level.text = data.Level + "/" + data.MaxLevel;
		if (data.Level == 0)
		{
			this.Icon.color = Color.gray;
			this.Name.color = Color.gray;
			this.Level.color = Color.gray;
		}
		else if (data.Level == data.MaxLevel)
		{
			this.Icon.color = new Color32(253, 176, 24, byte.MaxValue);
			this.Name.color = new Color32(253, 176, 24, byte.MaxValue);
			this.Level.color = new Color32(253, 176, 24, byte.MaxValue);
		}
		else
		{
			this.Icon.color = Color.white;
			this.Name.color = Color.white;
			this.Level.color = Color.white;
		}
		this.SelectImage.gameObject.SetActive(select);
		this.LockImage.gameObject.SetActive(!data.Unlock);
	}

	public Image SelectImage;

	public Image Icon;

	public Text Name;

	public Text Level;

	public Image LockImage;
}
