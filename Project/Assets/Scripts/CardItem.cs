using System;
using DataCenter;
using DG.Tweening;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class CardItem : MonoBehaviour
{
	public void RefreshPage()
	{
		this.RestCard();
		if (this.data.ItemTag == DataCenter.ItemType.Debris)
		{
			this.QualityTxt.gameObject.SetActive(true);
		}
		else
		{
			this.QualityTxt.gameObject.SetActive(false);
		}
		this.SetIcon(this.data.ID);
		this.bgItem.SetActive(BoxDataManager.GetItemById(this.BoxDataId).ItemID > 8000);
		this.RewardNameTxt.text = Singleton<GlobalData>.Instance.GetText(this.data.Name);
		Singleton<FontChanger>.Instance.SetFont(RewardNameTxt);
		this.NumTxt.text = "X" + BoxDataManager.GetItemById(this.BoxDataId).ItemCount.ToString();
		int num = UnityEngine.Random.Range(-3, 6);
		int num2 = UnityEngine.Random.Range(-10, 10);
		float duration = (float)UnityEngine.Random.Range(20, 40) / 10f;
		this.CardInfo.transform.DOLocalMove(new Vector3((float)num, (float)num2, 0f), duration, false).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
	}

	public void SetIcon(int id)
	{
		if (id == 1)
		{
			this.cardImg.sprite = this.CommonSprite[0];
			this.RewardIcon.gameObject.SetActive(true);
			this.RewardIcon.sprite = Singleton<UiManager>.Instance.GetSprite(ItemDataManager.GetCommonItem(CommonDataType.GOLD).Icon);
		}
		else if (id == 2)
		{
			this.cardImg.sprite = this.CommonSprite[1];
			this.RewardIcon.gameObject.SetActive(true);
			this.RewardIcon.sprite = Singleton<UiManager>.Instance.GetSprite(ItemDataManager.GetCommonItem(CommonDataType.DIAMOND).Icon);
		}
		else if (id == 3)
		{
			this.cardImg.sprite = this.CommonSprite[2];
			this.RewardIcon.gameObject.SetActive(true);
			this.RewardIcon.sprite = Singleton<UiManager>.Instance.GetSprite(ItemDataManager.GetCommonItem(CommonDataType.DNA).Icon);
		}
		else if (id > 8000)
		{
			this.RewardIcon.gameObject.SetActive(true);
			ItemData itemData = ItemDataManager.GetItemData(DebrisDataManager.GetDebrisData(id).ItemID);
			if (itemData.ItemTag == DataCenter.ItemType.Weapon)
			{
				this.cardImg.transform.DORotate(new Vector3(0f, 0f, -30f), 0.1f, RotateMode.Fast);
				this.cardImg.rectTransform.sizeDelta = new Vector2(200f, this.cardImg.rectTransform.sizeDelta.y);
				this.RewardIcon.sprite = this.CommonSprite[7];
			}
			else if (itemData.ItemTag == DataCenter.ItemType.Prop)
			{
				this.RewardIcon.sprite = this.CommonSprite[8];
			}
			this.cardImg.sprite = Singleton<UiManager>.Instance.GetSprite(ItemDataManager.GetItemData(DebrisDataManager.GetDebrisData(id).ItemID).Icon);
		}
		else
		{
			this.RewardIcon.gameObject.SetActive(false);
			this.cardImg.sprite = Singleton<UiManager>.Instance.GetSprite(this.data.Icon);
		}
	}

	public void RestCard()
	{
		this.Model.SetActive(true);
		this.Card.SetActive(false);
		this.PraticlAll.SetActive(false);
		this.NewTip.SetActive(false);
	}

	public void OnclickModel()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		this.ani.SetBool("OpenCard", true);
	}

	public void OpenCard()
	{
		Singleton<GameAudioManager>.Instance.PlaySoundInGame(this.QualityAudio[BoxDataManager.GetItemById(this.BoxDataId).Quality - 1], false);
		this.NomalImg.sprite = this.cardSprite[BoxDataManager.GetItemById(this.BoxDataId).Quality];
		this.PraticlAll.SetActive(true);
		this.PraticlQuality[BoxDataManager.GetItemById(this.BoxDataId).Quality - 1].SetActive(true);
		for (int i = 0; i < this.Stars.Length; i++)
		{
			if (i <= BoxDataManager.GetItemById(this.BoxDataId).Quality - 1)
			{
				this.Stars[i].SetActive(true);
			}
			else
			{
				this.Stars[i].SetActive(false);
			}
		}
		this.Model.transform.GetComponent<Button>().enabled = false;
		this.Model.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.1f).OnComplete(delegate
		{
			this.Model.transform.DOScale(Vector3.one, 0.5f);
		});
		this.Model.transform.DORotate(new Vector3(0f, -90f, 0f), 0.5f, RotateMode.Fast).OnComplete(delegate
		{
			this.Model.transform.GetComponent<Image>().sprite = this.cardSprite[BoxDataManager.GetItemById(this.BoxDataId).Quality];
			this.Model.transform.DORotate(new Vector3(0f, -180f, 0f), 0.5f, RotateMode.Fast).OnComplete(delegate
			{
				this.Model.SetActive(false);
				this.Card.SetActive(true);
			});
		});
		this.CurBox.TitleGo02.SetActive(true);
		if (this.CurBox.num > 1)
		{
			this.CurBox.num--;
			this.CurBox.NumRemindTxt.text = "(" + this.CurBox.num.ToString();
		}
		else
		{
			this.CurBox.num--;
			this.CurBox.TitleGo02.SetActive(false);
			this.CurBox.ContinueGo.SetActive(true);
			Singleton<UiManager>.Instance.ShowTopBar(false);
			Singleton<UiManager>.Instance.SetTopEnable(false, false);
		}
	}

	public Sprite[] CommonSprite;

	public GameObject Model;

	public GameObject CardInfo;

	public GameObject Card;

	public GameObject NewTip;

	public ItemData data;

	public int BoxDataId;

	public GameObject[] Stars;

	public Sprite[] cardSprite;

	public Image cardImg;

	public Image NomalImg;

	public GameObject bgItem;

	public GameObject PraticlAll;

	public GameObject[] PraticlQuality;

	[CNName("品质音效，由低到高")]
	public AudioClip[] QualityAudio;

	public Image Bg;

	public Image TitleBg;

	public Image ItemBg;

	public Text NumTxt;

	public Text TypeTxt;

	public Text TipTxt;

	public Text QualityTxt;

	public Text RewardNameTxt;

	public Image RewardIcon;

	public BoxPage CurBox;

	public ParticleSystem Ps;

	public Animator ani;
}
