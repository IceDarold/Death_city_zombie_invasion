using System;
using DataCenter;
using UnityEngine;
using Zombie3D;

[RequireComponent(typeof(Collider))]
public class CollectItem : MonoBehaviour
{
	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer != 21)
		{
			return;
		}
		switch (this.type)
		{
		case DropItemType.GOLD:
			ItemDataManager.CollectItem(1, (int)this.num);
			break;
		case DropItemType.BLOOD:
			GameApp.GetInstance().GetGameScene().GetPlayer().AddHp(this.num);
			break;
		case DropItemType.BULLET:
			GameApp.GetInstance().GetGameScene().GetPlayer().AddBullet(this.num);
			break;
		case DropItemType.GRENADE:
		{
			InGamePage inGamePage = Singleton<UiManager>.Instance.GetPage(PageName.InGamePage) as InGamePage;
			inGamePage.AddGrenade(4003, (int)this.num);
			break;
		}
		}
		base.gameObject.SetActive(false);
	}

	[CNName("类型")]
	public DropItemType type;

	[CNName("数量")]
	public float num;
}
