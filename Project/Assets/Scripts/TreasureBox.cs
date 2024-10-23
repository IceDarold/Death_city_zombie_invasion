using System;
using UnityEngine;

public class TreasureBox : MonoBehaviour
{
	public void DoOpen()
	{
		this.collider.enabled = false;
		Singleton<DropItemManager>.Instance.ShowBossDieDrop(base.transform.position);
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer != 8)
		{
			return;
		}
		InGamePage inGamePage = Singleton<UiManager>.Instance.GetPage(PageName.InGamePage) as InGamePage;
		inGamePage.DoShowTreasureBoxBtn(this);
	}

	public void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer != 8)
		{
			return;
		}
		InGamePage inGamePage = Singleton<UiManager>.Instance.GetPage(PageName.InGamePage) as InGamePage;
		inGamePage.DoShowTreasureBoxBtn(null);
	}

	public Collider collider;
}
