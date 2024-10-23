using System;
using DataCenter;
using UnityEngine;
using UnityEngine.UI;

public class UILabelTick : MonoBehaviour
{
	public void OnEnable()
	{
		this.perSec = 1f;
	}

	private void Update()
	{
		this.perSec += Time.deltaTime;
		if (this.perSec > 1f)
		{
			this.perSec = 0f;
			string text = string.Empty;
			switch (this.labelTickKind)
			{
			case tickKind.zhizaoji:
				text = UITick.getFreeManufacNeedSec();
				if (text.Length > 0)
				{
					this.freeLabel.text = text + this.strLastPart;
				}
				else
				{
					this.freeLabel.text = this.strFree;
				}
				break;
			case tickKind.choujiang:
				text = UITick.getLotteryNeedSec();
				if (text.Length > 0)
				{
					this.freeLabel.text = text + this.strLastPart;
				}
				else
				{
					this.freeLabel.text = this.strFree;
				}
				break;
			case tickKind.libaotuisong:
				text = UITick.getPushGiftNeedSec();
				if (text.Length > 0)
				{
					this.freeLabel.text = text + this.strLastPart;
				}
				else
				{
					this.freeLabel.text = this.strFree;
				}
				break;
			case tickKind.video_mian:
				text = UITick.getVideoMainNeedSec();
				if (text.Length > 0)
				{
					this.freeLabel.text = text + this.strLastPart;
				}
				else
				{
					this.freeLabel.text = this.strFree;
				}
				break;
			case tickKind.video_box:
				text = UITick.getFreeBoxNeedSec();
				if (text.Length > 0)
				{
					this.freeLabel.text = text + this.strLastPart;
				}
				else
				{
					this.freeLabel.text = this.strFree;
				}
				break;
			case tickKind.roleup:
				text = UITick.getRoleUpNeedSec(RoleDataManager.GetRoleData(this.id));
				if (text.Length > 0)
				{
					this.freeLabel.text = text + this.strLastPart;
				}
				else
				{
					this.freeLabel.text = this.strFree;
					RoleDataManager.Upgrade(this.id);
				}
				break;
			case tickKind.shoptimegift:
				text = UITick.getLimitGiftNeedSec();
				if (text.Length > 0)
				{
					this.freeLabel.text = text + this.strLastPart;
				}
				else
				{
					this.freeLabel.text = this.strFree;
				}
				break;
			case tickKind.shopcountgift:
				text = UITick.getNumGiftNeedSec();
				if (text.Length > 0)
				{
					this.freeLabel.text = text + this.strLastPart;
				}
				else
				{
					this.freeLabel.text = this.strFree;
				}
				break;
			case tickKind.wuping:
				text = UITick.getItemNeedSec(this.id, this.time);
				if (text.Length > 0)
				{
					this.freeLabel.text = text + this.strLastPart;
				}
				else
				{
					this.freeLabel.text = this.strFree;
				}
				this.perSec = 1f;
				break;
			case tickKind.goldLevel:
				text = UITick.getGoldLevelSec();
				if (text.Length > 0)
				{
					this.freeLabel.text = text + this.strLastPart;
				}
				else
				{
					this.freeLabel.text = this.strFree;
				}
				break;
			case tickKind.LimtTimeLevel:
				text = UITick.getLimitLevelSec(this.LevelId);
				if (text.Length > 0)
				{
					this.freeLabel.text = text + this.strLastPart;
				}
				else
				{
					this.freeLabel.text = this.strFree;
				}
				break;
			case tickKind.BoosLevel:
				text = UITick.getBoosLevelSec();
				if (text.Length > 0)
				{
					this.freeLabel.text = text + this.strLastPart;
				}
				else
				{
					this.freeLabel.text = this.strFree;
				}
				break;
			}
		}
	}

	public string strFree;

	public string strLastPart;

	public Text freeLabel;

	public tickKind labelTickKind = tickKind.zhizaoji;

	private float perSec = 1f;

	public int id;

	public int time;

	public int LevelId;
}
