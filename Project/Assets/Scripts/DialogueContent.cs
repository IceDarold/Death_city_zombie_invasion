using System;
using DataCenter;
using DG.Tweening;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class DialogueContent : MonoBehaviour
{
	public void Show(DialogueData data)
	{
		this.Name.text = Singleton<GlobalData>.Instance.GetText(data.RoleName);
		Singleton<FontChanger>.Instance.SetFont(Name);
		Singleton<FontChanger>.Instance.SetFont(Content);
		this.Content.text = string.Empty;
		this.Content.DOText(Singleton<GlobalData>.Instance.GetText(data.Content), 0.5f, true, ScrambleMode.None, null);
	}

	public Text Name;

	public Text Content;
}
