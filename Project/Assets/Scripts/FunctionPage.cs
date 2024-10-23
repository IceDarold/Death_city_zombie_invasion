using System;
using System.Collections.Generic;
using ads;
using DataCenter;
using UnityEngine;

public class FunctionPage : GamePage
{
	private new void Awake()
	{
		FunctionPage.Instance = this;
		for (int i = 0; i < this.FunctionButtons.Count; i++)
		{
			int index = i;
			this.FunctionButtons[i].Current.onClick.AddListener(delegate()
			{
				this.SelectPageTag(index);
			});
		}
	}

	public override void Show()
	{
		Ads.ShowInter();
        //Advertisements.Instance.ShowInterstitial();
		//Singleton<GlobalData>.Instance.ShowAdvertisement(12, null, null);
		this.effect_time = 0f;
		base.Show();
		this.SelectImage.localPosition = this.FunctionButtons[(int)this.CurrentTag].transform.localPosition;
		this.SelectPageTag((int)this.CurrentTag);
	}

	public override void Refresh()
	{
		this.CheckTip(0);
		this.CheckLock();
	}

	public void CheckLock()
	{
		this.FunctionButtons[0].SetUnlock(CheckpointDataManager.GetCurrentCheckpoint().ID >= 4);
		this.FunctionButtons[1].SetUnlock(CheckpointDataManager.GetCurrentCheckpoint().ID >= 10);
		this.FunctionButtons[2].SetUnlock(CheckpointDataManager.GetCurrentCheckpoint().ID >= 8);
		this.FunctionButtons[3].SetUnlock(CheckpointDataManager.GetCurrentCheckpoint().ID >= 3);
	}

	public override void OnBack()
	{
		if (this.CurrentTag == FunctionPageTag.Weapon)
		{
			WeaponPanel component = this.PageList[(int)this.CurrentTag].GetComponent<WeaponPanel>();
			if (component.isPart)
			{
				component.OnBack();
				return;
			}
		}
		base.OnBack();
	}

	public void CheckTip(int index = 0)
	{
		switch (index)
		{
		case 0:
			this.CheckTip(1);
			this.CheckTip(2);
			this.CheckTip(3);
			this.CheckTip(4);
			break;
		case 1:
		{
			int num = 0;
			for (int i = 0; i < WeaponDataManager.Weapons.Count; i++)
			{
				if (WeaponDataManager.Weapons[i].isNew)
				{
					num++;
				}
			}
			this.FunctionButtons[0].Tip.gameObject.SetActive(num > 0);
			break;
		}
		case 2:
		{
			int num2 = 0;
			for (int j = 0; j < EquipmentDataManager.Equipments.Count; j++)
			{
				if (EquipmentDataManager.Equipments[j].isNew)
				{
					num2++;
				}
			}
			this.FunctionButtons[1].Tip.gameObject.SetActive(num2 > 0);
			break;
		}
		case 3:
			if (TalentDataManager.GetTotalPoints(this.TreeeID, 0) >= 200)
			{
				this.FunctionButtons[2].Tip.gameObject.SetActive(false);
			}
			else if (ItemDataManager.GetCurrency(CommonDataType.DNA) > TalentDataManager.GetUpgradeCost(this.TreeeID))
			{
				this.FunctionButtons[2].Tip.gameObject.SetActive(true);
			}
			else
			{
				this.FunctionButtons[2].Tip.gameObject.SetActive(false);
			}
			break;
		case 4:
		{
			int num3 = 0;
			for (int k = 0; k < PropDataManager.Props.Count; k++)
			{
				if (PropDataManager.Props[k].isNew)
				{
					num3++;
				}
			}
			this.FunctionButtons[3].Tip.gameObject.SetActive(num3 > 0);
			break;
		}
		}
	}

	private void OnDisable()
	{
		for (int i = 0; i < this.PageList.Count; i++)
		{
			this.PageList[i].gameObject.SetActive(false);
		}
	}

	private void SelectPageTag(int index)
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		this.PageList[(int)this.CurrentTag].gameObject.SetActive(false);
		this.PageList[index].gameObject.SetActive(true);
		this.CurrentTag = (FunctionPageTag)index;
		GameLogManager.SendPageLog(this.CurrentTag.ToString(), "null");
		this.Refresh();
	}

	public void ShowUpgradeEffect()
	{
		this.effect_time = 1f;
		this.UpgradeEffect.gameObject.SetActive(true);
		this.UpgradeEffect.Play(true);
	}

	private void Update()
	{
		if (base.gameObject.activeSelf)
		{
			this.SelectImage.localPosition = Vector3.Lerp(this.SelectImage.localPosition, this.FunctionButtons[(int)this.CurrentTag].transform.localPosition, 0.2f);
		}
		if (this.UpgradeEffect.gameObject.activeSelf)
		{
			if (this.effect_time > 0f)
			{
				this.effect_time -= Time.deltaTime;
			}
			else
			{
				this.effect_time = 0f;
				this.UpgradeEffect.gameObject.SetActive(false);
			}
		}
	}

	public static FunctionPage Instance;

	public FunctionPageTag CurrentTag;

	public Transform SelectImage;

	public ParticleSystem UpgradeEffect;

	public List<Transform> PageList = new List<Transform>();

	public List<FunctionButton> FunctionButtons = new List<FunctionButton>();

	private int TreeeID = 1;

	private float effect_time;
}
