using System;
using DataCenter;
using inApps;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class DailyLimitedChild : MonoBehaviour
{
	public void Refresh(StoreData data)
	{
		this.Name.text = Singleton<GlobalData>.Instance.GetText(data.Name);
		this.Describe.text = Singleton<GlobalData>.Instance.GetText(data.Describe).Replace("\\n", "\n");
		this.Icon.sprite = Singleton<UiManager>.Instance.GetSprite(data.Icon);
		this.Icon.preserveAspect = true;
		if (data.Count < data.LimitCount)
		{
			this.BuyButton.interactable = true;
			//this.Price.text = "$" + StoreDataManager.GetChargePoint(data.ChargePoint).Price.ToString();
			this.Price.text = Singleton<InApps>.Instance.GetPrice(data.ChargePoint);
			this.Timer.gameObject.SetActive(false);
		}
		else
		{
			this.BuyButton.interactable = false;
			this.Price.text = Singleton<GlobalData>.Instance.GetText("ALREADY_BUY");
			this.count_down = (float)GameTimeManager.GetLeftSecondsToday();
			this.Timer.gameObject.SetActive(true);
		}
		Singleton<FontChanger>.Instance.SetFont(Name);
		Singleton<FontChanger>.Instance.SetFont(Describe);
		Singleton<FontChanger>.Instance.SetFont(Price);
	}

	private void Update()
	{
		if (this.Timer.gameObject.activeSelf)
		{
			if (this.count_down > 0f)
			{
				this.count_down -= Time.deltaTime;
				this.Timer.text = GameTimeManager.ConvertToString((int)this.count_down);
			}
			else
			{
				this.count_down = 0f;
				this.Timer.gameObject.SetActive(true);
			}
		}
	}

	public Text Name;

	public Text Describe;

	public Text Price;

	public Image Icon;

	public Button BuyButton;

	public Text Timer;

	private float count_down;
}
