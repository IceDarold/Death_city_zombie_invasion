using System;
using System.Collections.Generic;
using DataCenter;
using DG.Tweening;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class AchievementPage : GamePage
{
	private new void Awake()
	{
		this.InitScrollView(this.AchievementRoot.GetComponent<GridLayoutGroup>(), true);
	}

	public void InitScrollView(GridLayoutGroup grid, bool isHorizontal)
	{
		int num = 2;
		for (int i = 0; i < AchievementDataManager.Achievements.Count; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("UI/AchievementChild")) as GameObject;
			gameObject.name = "#" + AchievementDataManager.Achievements[i].ID.ToString("D3");
			gameObject.transform.SetParent(grid.transform);
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localPosition = Vector3.zero;
			AchievementChild component = gameObject.GetComponent<AchievementChild>();
			component.CurrentPage = this;
			component.GetComponent<Toggle>().group = this.AchievementRoot.GetComponent<ToggleGroup>();
			this.AchievementChildren.Add(component);
		}
		int num2 = AchievementDataManager.Achievements.Count / num;
		num2 = ((AchievementDataManager.Achievements.Count % num != 0) ? num2 : (num2 - 1));
		if (isHorizontal)
		{
			float num3 = grid.cellSize.x * (float)(num2 + 1) + grid.spacing.x * (float)num2 + (float)grid.padding.left;
			UnityEngine.Debug.Log(num3);
			grid.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, num3);
		}
		else
		{
			float size = grid.cellSize.y * (float)(num2 + 1) + grid.spacing.y * (float)num2;
			grid.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
		}
	}

	public int GetUnitNum(GridLayoutGroup grid, bool isHorizontal)
	{
		int num;
		if (isHorizontal)
		{
			num = (int)(grid.GetComponent<RectTransform>().rect.height / grid.cellSize.y);
			while (grid.spacing.y * (float)(num - 1) + grid.cellSize.y * (float)num > grid.GetComponent<RectTransform>().rect.height && num > 1)
			{
				num--;
			}
		}
		else
		{
			num = (int)(grid.GetComponent<RectTransform>().rect.width / grid.cellSize.x);
			while (grid.spacing.x * (float)(num - 1) + grid.cellSize.x * (float)num > grid.GetComponent<RectTransform>().rect.width && num > 1)
			{
				num--;
			}
		}
		if (num < 1)
		{
			num = 1;
		}
		return num;
	}

	private new void OnEnable()
	{
		this.canvasGroup.alpha = 0f;
		this.canvasGroup.DOFade(1f, 0.4f);
		this.scro.horizontalNormalizedPosition = 0f;
		this.ImageLeft.gameObject.SetActive(false);
		this.ImageRight.gameObject.SetActive(true);
		this.Refresh();
	}

	public void ClosePage()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		this.Close();
	}

	public override void Refresh()
	{
		base.Refresh();
		this.TitleText.text = Singleton<GlobalData>.Instance.GetText("ACHIEVEMENT");
		Singleton<FontChanger>.Instance.SetFont(TitleText);
		for (int i = 0; i < this.AchievementChildren.Count; i++)
		{
			int index = i;
			this.AchievementChildren[i].init(AchievementDataManager.Achievements[index]);
		}
	}

	public override void Close()
	{
		base.Close();
		Singleton<UiManager>.Instance.CurrentPage.Refresh();
	}

	public void OnValueChange()
	{
		if (this.scro.horizontalNormalizedPosition > 0f)
		{
			this.ImageLeft.gameObject.SetActive(true);
			if (this.scro.horizontalNormalizedPosition < 1f)
			{
				this.ImageRight.gameObject.SetActive(true);
			}
			else
			{
				this.ImageRight.gameObject.SetActive(false);
			}
		}
		else
		{
			this.ImageLeft.gameObject.SetActive(false);
			this.ImageRight.gameObject.SetActive(true);
		}
	}

	public Text TitleText;

	public ScrollRect scro;

	public Transform AchievementRoot;

	public GameObject ImageLeft;

	public GameObject ImageRight;

	public List<AchievementChild> AchievementChildren = new List<AchievementChild>();

	public CanvasGroup canvasGroup;
}
