using System;
using System.Collections.Generic;
using ads;
using DataCenter;
using DG.Tweening;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class RolePage : GamePage
{
	public new void OnEnable()
	{
		Singleton<UiManager>.Instance.SetTopEnable(true, false);
        //Singleton<GlobalData>.Instance.ShowAdvertisement(11, null, null);
        //Advertisements.Instance.ShowInterstitial();
        Ads.ShowInter();
		this.RoleDataList = RoleDataManager.Roles;
		this.RefreshInfo();
	}

	private void OnDisable()
	{
		Singleton<UiManager>.Instance.SetTopEnable(true, true);
	}

	private void RefreshInfo()
	{
		this.CurrentRole = RoleDataManager.GetRoleData(PlayerDataManager.Player.Role);
		RoleData roleData = new RoleData();
		int num = this.selectIndex;
		if (num != 0)
		{
			if (num != 1)
			{
				if (num == 2)
				{
					roleData = RoleDataManager.Roles[2];
				}
			}
			else
			{
				roleData = RoleDataManager.Roles[0];
			}
		}
		else
		{
			roleData = RoleDataManager.Roles[1];
		}
		this.RoleName.text = Singleton<GlobalData>.Instance.GetText(roleData.Name);
		this.RoleDescribe.text = Singleton<GlobalData>.Instance.GetText(roleData.Describe);
		Singleton<FontChanger>.Instance.SetFont(RoleName);
		Singleton<FontChanger>.Instance.SetFont(RoleDescribe);
		List<RoleAttribute> attribute = RoleDataManager.GetAttribute(roleData, true);
		if (attribute.Count == 0)
		{
			this.RoleQte.gameObject.SetActive(false);
		}
		else
		{
			this.RoleQte.name = Singleton<GlobalData>.Instance.GetText(attribute[0].Name);
			this.RoleQte.Describe.text = Singleton<GlobalData>.Instance.GetText(attribute[0].Describe);
			this.RoleQte.Icon.sprite = Singleton<UiManager>.Instance.GetSprite(attribute[0].Icon);
			this.RoleQte.gameObject.SetActive(true);
			Singleton<FontChanger>.Instance.SetFont(RoleQte.Describe);
		}
		List<RoleAttribute> attribute2 = RoleDataManager.GetAttribute(roleData, false);
		for (int i = 0; i < this.RoleSkills.Length; i++)
		{
			if (i < attribute2.Count)
			{
				this.RoleSkills[i].Icon.sprite = Singleton<UiManager>.Instance.GetSprite(attribute2[i].Icon);
				this.RoleSkills[i].Describe.text = Singleton<GlobalData>.Instance.GetText(attribute2[i].Describe);
				this.RoleSkills[i].gameObject.SetActive(true);
				Singleton<FontChanger>.Instance.SetFont(RoleSkills[i].Describe);
			}
			else
			{
				this.RoleSkills[i].gameObject.SetActive(false);
			}
		}
		if (roleData.Enable)
		{
			this.FuncitonButton.gameObject.SetActive(true);
			this.FuncitonTxt.gameObject.SetActive(false);
			this.CurrencyIcon.gameObject.SetActive(false);
			if (this.CurrentRole.ID == roleData.ID)
			{
				this.FunctionButtonName.text = Singleton<GlobalData>.Instance.GetText("USING");
				this.FuncitonButton.enabled = false;
				this.FuncitonButton.GetComponent<Image>().sprite = Singleton<UiManager>.Instance.CommonSprites[2];
			}
			else
			{
				this.FunctionButtonName.text = Singleton<GlobalData>.Instance.GetText("CHOOSE");
				this.FuncitonButton.enabled = true;
				this.FuncitonButton.GetComponent<Image>().sprite = Singleton<UiManager>.Instance.CommonSprites[3];
			}
			Singleton<FontChanger>.Instance.SetFont(FunctionButtonName);
		}
		else if (roleData.ID == 1002)
		{
			this.FuncitonTxt.text = Singleton<GlobalData>.Instance.GetText("ROLE02_UNLOCK");
			Singleton<FontChanger>.Instance.SetFont(FuncitonTxt);
			this.FuncitonTxt.gameObject.SetActive(true);
			this.FuncitonButton.gameObject.SetActive(false);
			this.CurrencyIcon.gameObject.SetActive(false);
		}
		else
		{
			this.CurrencyIcon.gameObject.SetActive(true);
			this.FunctionButtonName.text = "4000";
			this.FuncitonButton.gameObject.SetActive(true);
			this.FuncitonButton.enabled = true;
			this.FuncitonButton.GetComponent<Image>().sprite = Singleton<UiManager>.Instance.CommonSprites[3];
			this.FuncitonTxt.gameObject.SetActive(false);
		}
		if (this.selectIndex == 0)
		{
			this.BtnLeft.SetActive(false);
			this.BtnRight.SetActive(true);
		}
		else if (this.selectIndex == 2)
		{
			this.BtnLeft.SetActive(true);
			this.BtnRight.SetActive(false);
		}
		else
		{
			this.BtnLeft.SetActive(true);
			this.BtnRight.SetActive(true);
		}
	}

	public void OnclickSelectRole(int num)
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		NpcScenceControl.instance.SetLayer(NpcScenceControl.instance.RolesGo[this.selectIndex], 0);
		this.selectIndex += num;
		NpcScenceControl.instance.CanShow = false;
		NpcScenceControl.instance.openBlur = true;
		Singleton<UiManager>.Instance.CanBack = false;
		NpcScenceControl.instance.SetLayer(NpcScenceControl.instance.RolesGo[this.selectIndex], 30);
		NpcScenceControl.instance.eventSystem.enabled = false;
		NpcScenceControl.instance.LookNpc(NpcScenceControl.instance.RolesGo[this.selectIndex], true, true);
		NpcScenceControl.instance.ChangeView(true);
		NpcScenceControl.instance.curCamer.transform.DORotateQuaternion(NpcScenceControl.instance.RolesCamPos[this.selectIndex].localRotation, this.speed);
		NpcScenceControl.instance.curCamer.transform.DOLocalMove(NpcScenceControl.instance.RolesCamPos[this.selectIndex].localPosition, this.speed, false).OnComplete(delegate
		{
			NpcScenceControl.instance.eventSystem.enabled = true;
			Singleton<UiManager>.Instance.CanBack = true;
			Singleton<UiManager>.Instance.ShowTopBar(true);
			NpcScenceControl.instance.RolesGo[this.selectIndex].GetComponent<NpcRoleControll>().SetAni(1);
		});
		this.RefreshInfo();
	}

	public void ClickOnFunctionButton()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		RoleData roleData = new RoleData();
		int num = this.selectIndex;
		if (num != 0)
		{
			if (num != 1)
			{
				if (num == 2)
				{
					roleData = RoleDataManager.Roles[2];
				}
			}
			else
			{
				roleData = RoleDataManager.Roles[0];
			}
		}
		else
		{
			roleData = RoleDataManager.Roles[1];
		}
		if (roleData.Enable)
		{
			if (roleData.ID != PlayerDataManager.Player.Role)
			{
				PlayerDataManager.SelectRole(roleData.ID);
			}
		}
		else if (roleData.ID != 1002)
		{
			if (ItemDataManager.GetCurrency(CommonDataType.DIAMOND) >= 4000)
			{
				ItemDataManager.SetCurrency(CommonDataType.DIAMOND, -4000);
				RoleDataManager.Unlcok(1003);
			}
			else
			{
				Singleton<UiManager>.Instance.ShowLackOfMoney(CommonDataType.DIAMOND, 4000);
			}
		}
		Singleton<UiManager>.Instance.TopBar.Refresh();
		this.RefreshInfo();
	}

	public Text RoleName;

	public Text RoleDescribe;

	public Button FuncitonButton;

	public Text FuncitonTxt;

	public Text FunctionButtonName;

	public RoleQteChild RoleQte;

	public RoleSkillChild[] RoleSkills;

	private List<RoleData> RoleDataList = new List<RoleData>();

	private RoleData CurrentRole = new RoleData();

	private float speed = 0.5f;

	public int selectIndex;

	public Image CurrencyIcon;

	public GameObject BtnLeft;

	public GameObject BtnRight;
}
