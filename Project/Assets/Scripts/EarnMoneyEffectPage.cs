using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarnMoneyEffectPage : GamePage
{
	private new void OnEnable()
	{
		Singleton<UiManager>.Instance.SetTopEnable(false, false);
		this.GoldTarget.position = this.GoldRoot.position;
		this.DiamondTarget.position = this.DiamondRoot.position;
		this.DNATarget.position = this.DNARoot.position;
		for (int i = 0; i < this.GoldEffect.Count; i++)
		{
			if (i < this.GoldCount)
			{
				this.GoldEffect[i].Target = this.GoldTarget;
				this.GoldEffect[i].gameObject.SetActive(true);
			}
			else
			{
				this.GoldEffect[i].gameObject.SetActive(false);
			}
		}
		for (int j = 0; j < this.DiamondEffect.Count; j++)
		{
			if (j < this.DiamondCount)
			{
				this.DiamondEffect[j].Target = this.DiamondTarget;
				this.DiamondEffect[j].gameObject.SetActive(true);
			}
			else
			{
				this.DiamondEffect[j].gameObject.SetActive(false);
			}
		}
		for (int k = 0; k < this.DNAEffect.Count; k++)
		{
			if (k < this.DNACount)
			{
				this.DNAEffect[k].Target = this.DNATarget;
				this.DNAEffect[k].gameObject.SetActive(true);
			}
			else
			{
				this.DNAEffect[k].gameObject.SetActive(false);
			}
		}
		base.StartCoroutine(this.AutoClose());
	}

	private IEnumerator AutoClose()
	{
		yield return new WaitForSeconds(2f);
		Singleton<UiManager>.Instance.SetTopEnable(true, true);
		base.Close();
		yield break;
	}

	public int GoldCount;

	public int DiamondCount;

	public int DNACount;

	public Transform GoldRoot;

	public Transform DiamondRoot;

	public Transform DNARoot;

	public Transform GoldTarget;

	public Transform DiamondTarget;

	public Transform DNATarget;

	public List<EarnMoneyEffectChild> GoldEffect = new List<EarnMoneyEffectChild>();

	public List<EarnMoneyEffectChild> DiamondEffect = new List<EarnMoneyEffectChild>();

	public List<EarnMoneyEffectChild> DNAEffect = new List<EarnMoneyEffectChild>();
}
