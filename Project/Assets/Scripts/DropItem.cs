using System;
using System.Collections;
using DataCenter;
using DG.Tweening;
using UnityEngine;
using Zombie3D;

public class DropItem : MonoBehaviour
{
	public bool isActive { get; private set; }

	private void OnEnable()
	{
		base.GetComponent<Rigidbody>().AddForce(new Vector3(UnityEngine.Random.Range(-1f, 1f), -0.1f, UnityEngine.Random.Range(-1f, 1f)) * this.Force, ForceMode.Impulse);
		base.transform.localScale = Vector3.zero;
		base.transform.DOScale(Vector3.one, 0.7f);
		if (this.IsRotate)
		{
			base.transform.rotation = Quaternion.Euler(new Vector3((float)UnityEngine.Random.Range(0, 45), (float)UnityEngine.Random.Range(0, 180), 0f));
		}
		else
		{
			base.transform.rotation = Quaternion.Euler(Vector3.zero);
		}
		base.StartCoroutine(this.WaitForLanding());
	}

	private IEnumerator WaitForLanding()
	{
		while (!base.GetComponent<Rigidbody>().IsSleeping())
		{
			yield return null;
		}
		if (CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.GOLD)
		{
			this.Duration = 0.5f;
			this.CanPick = false;
		}
		else if (GameApp.GetInstance().GetGameScene().PlayingMode == GamePlayingMode.Cannon)
		{
			this.Duration = 0.5f;
			this.CanPick = false;
		}
		else if (!GameApp.GetInstance().GetGameScene().CaculatePath2Player(base.transform.position))
		{
			this.Duration = 0.5f;
			this.CanPick = false;
		}
		else
		{
			this.Duration = 20f;
			this.CanPick = true;
		}
		this.duration = 0f;
		this.isActive = true;
		yield break;
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 21)
		{
			switch (this.Type)
			{
			case DropItemType.GOLD:
				Singleton<DropItemManager>.Instance.AddDropItem(DropItemType.GOLD);
				break;
			case DropItemType.DNA:
				Singleton<DropItemManager>.Instance.AddDropItem(DropItemType.DNA);
				break;
			case DropItemType.BLOOD:
				GameApp.GetInstance().GetGameScene().GetPlayer().AddHp(5f);
				break;
			case DropItemType.BULLET:
				GameApp.GetInstance().GetGameScene().GetPlayer().AddBullet();
				break;
			}
			Singleton<GameAudioManager>.Instance.PlaySoundInGame(Singleton<GameAudioManager>.Instance.PickUpClip, false);
			Singleton<UiManager>.Instance.GetPage(PageName.InGamePage).GetComponent<InGamePage>().ShowPickup(this.Type, 1);
			this.isActive = false;
			base.gameObject.SetActive(false);
		}
	}

	private void Update()
	{
		if (this.isActive && Time.timeScale != 0f)
		{
			if (this.duration < this.Duration)
			{
				this.duration += Time.deltaTime;
			}
			else
			{
				this.isActive = false;
				this.duration = 0f;
				if (!this.CanPick)
				{
					switch (this.Type)
					{
					case DropItemType.GOLD:
						Singleton<DropItemManager>.Instance.AddDropItem(DropItemType.GOLD);
						break;
					case DropItemType.DNA:
						Singleton<DropItemManager>.Instance.AddDropItem(DropItemType.DNA);
						break;
					case DropItemType.BLOOD:
						GameApp.GetInstance().GetGameScene().GetPlayer().AddHp(5f);
						break;
					case DropItemType.BULLET:
						GameApp.GetInstance().GetGameScene().GetPlayer().AddBullet();
						break;
					}
					Singleton<GameAudioManager>.Instance.PlaySoundInGame(Singleton<GameAudioManager>.Instance.PickUpClip, false);
					Singleton<UiManager>.Instance.GetPage(PageName.InGamePage).GetComponent<InGamePage>().ShowPickup(this.Type, 1);
				}
				base.gameObject.SetActive(false);
			}
		}
	}

	[Header("生成时是否随机角度")]
	public bool IsRotate = true;

	[Header("类型")]
	public DropItemType Type;

	[Header("持续时间")]
	public float Duration;

	[Header("力量")]
	public float Force;

	private float duration;

	private bool CanPick = true;
}
