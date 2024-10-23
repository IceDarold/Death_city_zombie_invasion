using System;
using UnityEngine;
using UnityEngine.UI;

public class ArenaReward : MonoBehaviour
{
	public void Init(int _lv, int _cash, int _dia, Sprite _bg, bool isCover = false)
	{
		this.lv.text = _lv.ToString();
		this.cash.text = _cash.ToString();
		this.dia.text = _dia.ToString();
		this.bg.sprite = _bg;
		this.gunName.gameObject.SetActive(false);
		this.gunSprite.gameObject.SetActive(false);
		this.cover.SetActive(isCover);
	}

	public void Init(int _lv, string _gunName, Sprite gunSp, Sprite _bg, bool isCover = false)
	{
		this.lv.text = _lv.ToString();
		this.gunName.text = _gunName.ToString();
		this.gunSprite.sprite = gunSp;
		this.bg.sprite = _bg;
		this.cash.transform.parent.gameObject.SetActive(false);
		this.dia.transform.parent.gameObject.SetActive(false);
		this.cover.SetActive(isCover);
	}

	public Text lv;

	public Text cash;

	public Text dia;

	public Text gunName;

	public Image gunSprite;

	public Image bg;

	public GameObject cover;
}
