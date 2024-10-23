using System;
using DataCenter;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LevelItem : MonoBehaviour
{
	public void InitData()
	{
		this.checkData = CheckpointDataManager.GetCheckpointData(this.currentTag, this.currentIndex);
	}

	public void Update()
	{
	}

	public void RefreshPage()
	{
		this.InitData();
		this.LevelNumTxt.text = (int)this.currentTag + "-" + this.currentIndex;
		if (MapsPage.instance.SelectData == this.checkData)
		{
			this.ShowChoose();
		}
		else
		{
			this.HideChoose();
		}
		if (this.checkData == CheckpointDataManager.GetCurrentCheckpoint())
		{
			this.SetLevelImage(4);
		}
		else if (this.checkData.Passed)
		{
			this.SetLevelImage(1);
			this.ButtonState[1].enabled = true;
		}
		else
		{
			this.SetLevelImage(0);
			this.ButtonState[0].enabled = false;
		}
	}

	public void SetLevelImage(int current)
	{
		for (int i = 0; i < this.ImageState.Length; i++)
		{
			if (i == current)
			{
				this.ImageState[i].gameObject.SetActive(true);
			}
			else
			{
				this.ImageState[i].gameObject.SetActive(false);
			}
		}
	}

	public void OnclickSelect()
	{
		if (!this.checkData.Unlock)
		{
			return;
		}
		if (MapsPage.instance.SelectData != this.checkData)
		{
			MapsPage.instance.SelectData = CheckpointDataManager.GetCheckpointData(this.currentTag, this.currentIndex);
			MapsPage.instance.RefreshInfoPart();
		}
		for (int i = 0; i < MapsPage.instance.MapChapters.Length; i++)
		{
			MapsPage.instance.MapChapters[i].Refresh();
		}
	}

	public void HideChoose()
	{
		this.curChoose.transform.localScale = Vector3.one;
		this.curChoose.transform.DOScale(0f, 0.2f).OnComplete(delegate
		{
			this.curChoose.SetActive(false);
		});
	}

	public void ShowChoose()
	{
		MapsPage.instance.SelectEffect.transform.SetParent(base.transform);
		MapsPage.instance.SelectEffect.transform.localPosition = new Vector3(0f, 0f, -100f);
		this.curChoose.transform.localScale = Vector3.zero;
		this.curChoose.SetActive(true);
		this.curChoose.transform.DOScale(1.2f, 0.2f).OnComplete(delegate
		{
			this.curChoose.transform.DOScale(1f, 0.05f);
		});
	}

	public Image[] ImageState;

	public Button[] ButtonState;

	public GameObject curChoose;

	public CheckpointData checkData;

	private UILabelTick timeLevel;

	public Text LevelNumTxt;

	[CNName("章节")]
	public ChapterEnum currentTag;

	[CNName("关卡序号")]
	public int currentIndex;
}
