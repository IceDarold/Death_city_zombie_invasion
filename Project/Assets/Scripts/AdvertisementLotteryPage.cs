using System;
using System.Collections;
using System.Collections.Generic;
using ads;
using DataCenter;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class AdvertisementLotteryPage : GamePage
{
	private new void Awake()
	{
		this.createItems();
	}

	public override void Show()
	{
		this._repeat = 1;
		this._speed = 0f;
		base.Show();
	}

	public override void Refresh()
	{
		this.VedioImage.gameObject.SetActive(Singleton<GlobalData>.Instance.FreeLotteryTimes <= 0);
		this.StartButton.interactable = (Singleton<GlobalData>.Instance.AdvertisementLotteryTimes > 0);
		this.StartButtonName.text = ((Singleton<GlobalData>.Instance.AdvertisementLotteryTimes <= 0) ? Singleton<GlobalData>.Instance.GetText("COME_TOMORROW") : Singleton<GlobalData>.Instance.GetText("START"));
		Singleton<FontChanger>.Instance.SetFont(StartButtonName);
	}

	public override void OnBack()
	{
		if (this.IsAction)
		{
			return;
		}
		base.OnBack();
	}

	private void createItems()
	{
		for (int i = 0; i < this.LotteryAwards.Count; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ChildPrefab);
			gameObject.transform.SetParent(this.Root);
			gameObject.transform.localPosition = new Vector3(0f, (float)i * 100f + 50f, 0f);
			gameObject.transform.localScale = Vector3.one;
			Image component = gameObject.transform.Find("Icon").GetComponent<Image>();
			component.sprite = this.Icons[(int)this.LotteryAwards[i].Type];
			Text component2 = gameObject.transform.Find("Count").GetComponent<Text>();
			component2.text = "X" + this.LotteryAwards[i].Count.ToString();
			gameObject.SetActive(true);
			this.Items.Add(gameObject.transform);
		}
	}

	private bool isWeaponDebrisEffective()
	{
		for (int i = 0; i < this.WeaponDebrisWeight.Count; i++)
		{
			if (DebrisDataManager.IsDebrisEffective(this.WeaponDebrisWeight[i].ID))
			{
				return true;
			}
		}
		return false;
	}

	private bool isPropDebrisEffective()
	{
		for (int i = 0; i < this.PropDebrisWeight.Count; i++)
		{
			if (DebrisDataManager.IsDebrisEffective(this.PropDebrisWeight[i].ID))
			{
				return true;
			}
		}
		return false;
	}

	private int getMaxWeight()
	{
		int num = 0;
		int i = 0;
		while (i < this.LotteryAwards.Count)
		{
			if (this.LotteryAwards[i].Type == LotteryAwardType.WEAPON_DEBRIS)
			{
				if (this.isWeaponDebrisEffective())
				{
					goto IL_62;
				}
			}
			else if (this.LotteryAwards[i].Type != LotteryAwardType.PROP_DEBRIS || this.isPropDebrisEffective())
			{
				goto IL_62;
			}
			IL_7A:
			i++;
			continue;
			IL_62:
			num += this.LotteryAwards[i].Weight;
			goto IL_7A;
		}
		return num;
	}

	private int getRandomItem()
	{
		int result = 0;
		int maxWeight = this.getMaxWeight();
		int num = UnityEngine.Random.Range(0, maxWeight);
		int num2 = 0;
		int i = 0;
		while (i < this.LotteryAwards.Count)
		{
			if (this.LotteryAwards[i].Type == LotteryAwardType.WEAPON_DEBRIS)
			{
				if (this.isWeaponDebrisEffective())
				{
					goto IL_78;
				}
			}
			else if (this.LotteryAwards[i].Type != LotteryAwardType.PROP_DEBRIS || this.isPropDebrisEffective())
			{
				goto IL_78;
			}
			IL_A0:
			i++;
			continue;
			IL_78:
			num2 += this.LotteryAwards[i].Weight;
			if (num < num2)
			{
				result = i;
				break;
			}
			goto IL_A0;
		}
		return result;
	}

	private int getRandomDebris(List<LotteryAwardWeight> list)
	{
		int index = 0;
		int num = 0;
		for (int i = 0; i < list.Count; i++)
		{
			if (DebrisDataManager.IsDebrisEffective(list[i].ID))
			{
				num += list[i].Weight;
			}
		}
		int num2 = UnityEngine.Random.Range(0, num);
		int num3 = 0;
		for (int j = 0; j < list.Count; j++)
		{
			if (DebrisDataManager.IsDebrisEffective(list[j].ID))
			{
				num3 += list[j].Weight;
				if (num2 < num3)
				{
					index = j;
					break;
				}
			}
		}
		return list[index].ID;
	}

	private void resetItems()
	{
		float y = this.Root.localPosition.y + 150f;
		this.Root.localPosition = new Vector3(0f, -150f, 0f);
		for (int i = 0; i < this.Items.Count; i++)
		{
			this.Items[i].localPosition += new Vector3(0f, y, 0f);
		}
	}

	private IEnumerator OnFinish()
	{
		this.Refresh();
		yield return new WaitForSeconds(0.2f);
		int id = 0;
		int count = this.LotteryAwards[this.AwardIndex].Count;
		switch (this.LotteryAwards[this.AwardIndex].Type)
		{
		case LotteryAwardType.GOLD:
			id = 1;
			break;
		case LotteryAwardType.DIAMOND:
			id = 2;
			break;
		case LotteryAwardType.DNA:
			id = 3;
			break;
		case LotteryAwardType.WEAPON_DEBRIS:
			id = this.getRandomDebris(this.WeaponDebrisWeight);
			break;
		case LotteryAwardType.PROP_DEBRIS:
			id = this.getRandomDebris(this.PropDebrisWeight);
			break;
		}
		Singleton<UiManager>.Instance.ShowAward(new int[]
		{
			id
		}, new int[]
		{
			count
		}, null);
		ItemDataManager.CollectItem(id, count);
		if (Singleton<UiManager>.Instance.TopBar != null)
		{
			Singleton<UiManager>.Instance.TopBar.Refresh();
		}
		yield break;
	}

	public void ClickOnConfirm()
	{
		if (this.IsAction)
		{
			return;
		}
		if (Singleton<GlobalData>.Instance.FreeLotteryTimes > 0)
		{
			GameLogManager.SendLotteryLog(2);
			Singleton<GlobalData>.Instance.FreeLotteryTimes--;
			this.start();
		}
		else if (Singleton<GlobalData>.Instance.AdvertisementLotteryTimes > 0)
		{
			Ads.ShowReward(() =>
			{
				GameLogManager.SendLotteryLog(3);
				Singleton<GlobalData>.Instance.AdvertisementLotteryTimes--;
				this.start();
			});
            //Advertisements.Instance.ShowRewardedVideo(OnFinished);
   //         Singleton<GlobalData>.Instance.ShowAdvertisement(-8, delegate
			//{
			//	GameLogManager.SendLotteryLog(3);
			//	Singleton<GlobalData>.Instance.AdvertisementLotteryTimes--;
			//	this.start();
			//}, delegate
			//{
			//	GameLogManager.SendLotteryLog(4);
			//});
		}
	}
    private void OnFinished(bool completed, string advertiser)
    {
        Debug.Log("Closed rewarded from: " + advertiser + " -> Completed " + completed);
        if (completed == true)
        {
            GameLogManager.SendLotteryLog(4);
        }

    }
    private void start()
	{
		this.Refresh();
		this.AwardIndex = this.getRandomItem();
		this._target = (float)this.Items.Count * 100f * (float)this._range + (float)this.AwardIndex * 100f + 50f;
		this.IsAction = true;
		Singleton<GameAudioManager>.Instance.PlaySound(this.Sounds[0], true);
	}

	public void ClickOnClose()
	{
		if (this.IsAction)
		{
			return;
		}
		this.Close();
	}

	private void Update()
	{
		if (this.IsAction)
		{
			if (this.Root.localPosition.y > -this._target + 2000f)
			{
				this._speed = Mathf.Lerp(this._speed, 2000f, Time.deltaTime * 2f);
				this.Root.localPosition -= new Vector3(0f, this._speed * Time.deltaTime, 0f);
			}
			else if (this.Root.localPosition.y > -this._target)
			{
				this._speed = Mathf.Lerp(this._speed, 50f, Time.deltaTime);
				this.Root.localPosition -= new Vector3(0f, this._speed * Time.deltaTime, 0f);
			}
			else
			{
				this._speed = 0f;
				this._repeat = 0;
				this.Root.localPosition = new Vector3(0f, -this._target, 0f);
				this.resetItems();
				this.IsAction = false;
				Singleton<GameAudioManager>.Instance.PlaySound(this.Sounds[1], false);
				base.StartCoroutine(this.OnFinish());
			}
			for (int i = 0; i < this.Items.Count; i++)
			{
				if (this.Items[i].position.y < this.BottomTag.position.y)
				{
					this.Items[i].localPosition = new Vector3(0f, (float)(this._repeat * this.Items.Count) * 100f + (float)i * 100f + 50f, 0f);
					if (i == this.Items.Count - 1)
					{
						this._repeat++;
					}
				}
			}
		}
	}

	public GameObject ChildPrefab;

	public Transform Root;

	public Transform BottomTag;

	public Button StartButton;

	public Image VedioImage;

	public Text StartButtonName;

	public AudioClip[] Sounds;

	public Sprite[] Icons;

	public List<LotteryAward> LotteryAwards = new List<LotteryAward>();

	public List<LotteryAwardWeight> WeaponDebrisWeight = new List<LotteryAwardWeight>();

	public List<LotteryAwardWeight> PropDebrisWeight = new List<LotteryAwardWeight>();

	private int AwardIndex;

	private int _repeat = 1;

	private int _range = 8;

	private float _target;

	private float _speed;

	private const float height = 100f;

	private List<Transform> Items = new List<Transform>();
}
