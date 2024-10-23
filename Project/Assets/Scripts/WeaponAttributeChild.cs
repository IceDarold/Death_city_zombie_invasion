using System;
using DataCenter;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class WeaponAttributeChild : MonoBehaviour
{
	public void Refresh(WeaponData weapon)
	{
		this.Name.text = Singleton<GlobalData>.Instance.GetText(this.Type.ToString());
		Singleton<FontChanger>.Instance.SetFont(Name);
		if (weapon.Type == WeaponType.SNIPER_RIFLE)
		{
			this.Value.text = WeaponDataManager.GetCurrentAttribute(weapon, this.Type).ToString();
			if (this.Type == WeaponAttribute.RELOADINGTIME || this.Type == WeaponAttribute.SHOOTSPEED)
			{
				this.ValueSlider.maxValue = 1000f / (float)WeaponDataManager.GetMaxAttribute(weapon, this.Type);
				this.ValueSlider.value = 10000f / (float)WeaponDataManager.GetCurrentAttribute(weapon, this.Type);
			}
			else
			{
				this.ValueSlider.maxValue = (float)WeaponDataManager.GetMaxAttribute(weapon, this.Type);
				this.ValueSlider.value = (float)WeaponDataManager.GetCurrentAttribute(weapon, this.Type);
			}
		}
		else
		{
			WeaponPartData waponPartData = WeaponPartSystem.GetWaponPartData(weapon.ID, WeaponPartEnum.BULLET);
			int attribute = WeaponDataManager.GetAttribute(weapon, this.Type, waponPartData.Level);
			int attribute2 = WeaponDataManager.GetAttribute(weapon, this.Type, waponPartData.MaxLevel);
			if (waponPartData.Level < waponPartData.MaxLevel)
			{
				int attribute3 = WeaponDataManager.GetAttribute(weapon, this.Type, waponPartData.Level + 1);
				if (attribute3 != attribute)
				{
					if (this.Type == WeaponAttribute.SHOOTSPEED)
					{
						this.Value.text = (1000f / (float)attribute).ToString("F2") + " >> <color=#00ff00ff>" + (1000f / (float)attribute3).ToString("F2") + "</color>";
					}
					else if (this.Type == WeaponAttribute.RELOADINGTIME)
					{
						this.Value.text = ((float)attribute / 1000f).ToString("F2") + " >> <color=#00ff00ff>" + ((float)attribute3 / 1000f).ToString("F2") + "</color>";
					}
					else
					{
						this.Value.text = attribute.ToString() + " >> <color=#00ff00ff>" + attribute3.ToString() + "</color>";
					}
				}
				else if (this.Type == WeaponAttribute.SHOOTSPEED)
				{
					this.Value.text = (1000f / (float)attribute).ToString("F2");
				}
				else if (this.Type == WeaponAttribute.RELOADINGTIME)
				{
					this.Value.text = ((float)attribute / 1000f).ToString("F2");
				}
				else
				{
					this.Value.text = attribute.ToString();
				}
			}
			else if (this.Type == WeaponAttribute.SHOOTSPEED)
			{
				this.Value.text = (1000f / (float)attribute).ToString("F2");
			}
			else if (this.Type == WeaponAttribute.RELOADINGTIME)
			{
				this.Value.text = ((float)attribute / 1000f).ToString("F2");
			}
			else
			{
				this.Value.text = attribute.ToString();
			}
			if (this.Type == WeaponAttribute.RELOADINGTIME || this.Type == WeaponAttribute.SHOOTSPEED)
			{
				this.ValueSlider.maxValue = 1000f / (float)WeaponDataManager.GetAttribute(weapon, this.Type, waponPartData.MaxLevel);
				this.ValueSlider.value = 1000f / (float)WeaponDataManager.GetAttribute(weapon, this.Type, waponPartData.Level);
			}
			else
			{
				this.ValueSlider.maxValue = (float)WeaponDataManager.GetAttribute(weapon, this.Type, waponPartData.MaxLevel);
				this.ValueSlider.value = (float)WeaponDataManager.GetAttribute(weapon, this.Type, waponPartData.Level);
			}
		}
		base.gameObject.SetActive(this.Type != WeaponAttribute.SCOPETIMES);
	}

	public void Refresh(WeaponData weapon, WeaponPartData part)
	{
		this.Name.text = Singleton<GlobalData>.Instance.GetText(this.Type.ToString());
		Singleton<FontChanger>.Instance.SetFont(Name);
		int currentAttribute = WeaponDataManager.GetCurrentAttribute(weapon, this.Type);
		int maxAttribute = WeaponDataManager.GetMaxAttribute(weapon, this.Type);
		if (part.Level < part.MaxLevel)
		{
			int num = WeaponDataManager.GetCurrentAttribute(weapon, this.Type) + WeaponPartSystem.GetAttribute(part, this.Type, part.Level + 1) - WeaponPartSystem.GetAttribute(part, this.Type, part.Level);
			if (num != currentAttribute)
			{
				if (this.Type == WeaponAttribute.SHOOTSPEED)
				{
					this.Value.text = (1000f / (float)currentAttribute).ToString("F2") + "<color=#00ff00ff> >> " + (1000f / (float)num).ToString("F2") + "</color>";
				}
				else if (this.Type == WeaponAttribute.RELOADINGTIME)
				{
					this.Value.text = ((float)currentAttribute / 1000f).ToString("F2") + "<color=#00ff00ff> >> " + ((float)num / 1000f).ToString("F2") + "</color>";
				}
				else
				{
					this.Value.text = currentAttribute.ToString() + "<color=#00ff00ff> >> " + num.ToString() + "</color>";
				}
			}
			else if (this.Type == WeaponAttribute.SHOOTSPEED)
			{
				this.Value.text = (1000f / (float)currentAttribute).ToString("F2");
			}
			else if (this.Type == WeaponAttribute.RELOADINGTIME)
			{
				this.Value.text = ((float)currentAttribute / 1000f).ToString("F2");
			}
			else
			{
				this.Value.text = currentAttribute.ToString();
			}
		}
		else if (this.Type == WeaponAttribute.RELOADINGTIME || this.Type == WeaponAttribute.SHOOTSPEED)
		{
			this.Value.text = (1000f / (float)currentAttribute).ToString("F2");
		}
		else if (this.Type == WeaponAttribute.RELOADINGTIME)
		{
			this.Value.text = ((float)currentAttribute / 1000f).ToString("F2");
		}
		else
		{
			this.Value.text = currentAttribute.ToString();
		}
		if (this.Type == WeaponAttribute.RELOADINGTIME || this.Type == WeaponAttribute.SHOOTSPEED)
		{
			this.ValueSlider.maxValue = 1f / (float)maxAttribute;
			this.ValueSlider.value = 1f / (float)currentAttribute;
		}
		else
		{
			this.ValueSlider.maxValue = (float)maxAttribute;
			this.ValueSlider.value = (float)currentAttribute;
		}
		base.gameObject.SetActive(true);
	}

	public WeaponAttribute Type;

	[Space(10f)]
	public Text Name;

	public Text Value;

	public Slider ValueSlider;
}
