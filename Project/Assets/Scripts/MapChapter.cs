using System;
using System.Collections.Generic;
using DataCenter;
using UnityEngine;

public class MapChapter : MonoBehaviour
{
	public void Refresh()
	{
		CheckpointData currentCheckpoint = CheckpointDataManager.GetCurrentCheckpoint();
		for (int i = 0; i < this.MainLine.Count; i++)
		{
			if (currentCheckpoint.Chapters == this.Chapter)
			{
				if (this.MainLine[i].currentIndex <= currentCheckpoint.Index + 1)
				{
					this.MainLine[i].gameObject.SetActive(true);
					this.MainLine[i].RefreshPage();
				}
				else
				{
					this.MainLine[i].gameObject.SetActive(false);
				}
			}
			else if (currentCheckpoint.Chapters < this.Chapter)
			{
				this.MainLine[i].gameObject.SetActive(false);
			}
			else if (currentCheckpoint.Chapters > this.Chapter)
			{
				this.MainLine[i].gameObject.SetActive(true);
				this.MainLine[i].RefreshPage();
			}
		}
	}

	public ChapterEnum Chapter;

	public Transform BossPoint;

	public Transform GoldPoint;

	public Transform RandomPoint;

	public Transform WeaponPoint;

	public Transform SniperPoint;

	public List<LevelItem> MainLine = new List<LevelItem>();
}
