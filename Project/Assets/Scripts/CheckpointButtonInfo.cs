using System;
using DataCenter;
using DG.Tweening;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointButtonInfo : MonoBehaviour
{
	private void OnEnable()
	{
		base.transform.localScale = Vector3.zero;
		base.transform.DOScale(Vector3.one, 0.2f);
		this.Init();
	}

	public void Init()
	{
		string type = this.Type;
		Singleton<FontChanger>.Instance.SetFont(Name);
		if (type != null)
		{
			if (!(type == "Gold"))
			{
				if (!(type == "Boss"))
				{
					if (!(type == "Random"))
					{
						if (!(type == "Weapon"))
						{
							if (type == "Sniper")
							{
								this.Name.text = Singleton<GlobalData>.Instance.GetText("SNIPER_LEVEL");
								this.Timer.gameObject.SetActive(false);
							}
						}
						else
						{
							this.Name.text = Singleton<GlobalData>.Instance.GetText("WEAPONLEVEL");
							if (PlayerPrefs.GetInt("WEAPON_TRY_CHECKPOINT", 1) == 1)
							{
								this.CurrentButton.interactable = true;
								this.Timer.gameObject.SetActive(false);
							}
							else
							{
								this.CurrentButton.interactable = false;
								this.dt = (float)GameTimeManager.GetLeftSecondsToday();
								this.Timer.gameObject.SetActive(true);
							}
						}
					}
					else
					{
						this.Name.text = Singleton<GlobalData>.Instance.GetText("RANDOMLEVEL");
						this.Timer.gameObject.SetActive(false);
					}
				}
				else
				{
					this.Name.text = Singleton<GlobalData>.Instance.GetText("BOSSLEVEL");
					this.Timer.gameObject.SetActive(false);
				}
			}
			else
			{
				this.Name.text = Singleton<GlobalData>.Instance.GetText("MORE_MONEY");
				if (PlayerPrefs.GetInt("GOLD_CHECKPOINT", 2) == 2)
				{
					this.CurrentButton.interactable = true;
					this.Timer.gameObject.SetActive(false);
				}
				else if (PlayerPrefs.GetInt("GOLD_CHECKPOINT", 2) == 1)
				{
					if (3600.0 - GameTimeManager.CalculateTimeToNow("GOLD_CHECKPOINT") > 0.0)
					{
						this.CurrentButton.interactable = false;
						this.dt = 3600f - (float)GameTimeManager.CalculateTimeToNow("GOLD_CHECKPOINT");
						this.Timer.gameObject.SetActive(true);
					}
					else
					{
						this.CurrentButton.interactable = true;
						this.Timer.gameObject.SetActive(false);
					}
				}
				else
				{
					this.CurrentButton.interactable = false;
					this.dt = (float)GameTimeManager.GetLeftSecondsToday();
					this.Timer.gameObject.SetActive(true);
				}
			}
		}
	}

	private void Update()
	{
		if (this.Timer.gameObject.activeSelf)
		{
			if (this.dt > 0f)
			{
				this.dt -= Time.deltaTime;
				this.Timer.text = GameTimeManager.ConvertToString((int)this.dt);
			}
			else
			{
				this.CurrentButton.interactable = true;
				this.Timer.gameObject.SetActive(false);
			}
		}
	}

	public string Type;

	public Button CurrentButton;

	public Transform Background;

	public ParticleSystem SelectParticle;

	public Text Name;

	public Text Timer;

	private float dt;
}
