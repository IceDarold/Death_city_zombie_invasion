using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TeachTrigger : MonoBehaviour
{
	public void Awake()
	{
		base.GetComponent<Collider>().isTrigger = true;
		base.gameObject.layer = 31;
	}

	public void OnTriggerEnter(Collider other)
	{
		base.GetComponent<Collider>().enabled = false;
		GamePage page = Singleton<UiManager>.Instance.GetPage(PageName.InGamePage);
		InGamePage inGamePage = page as InGamePage;
		inGamePage.CheckTeach(this.teachType);
	}

	[EnumLabel("教学类型")]
	public TeachType teachType = TeachType.NONE;
}
