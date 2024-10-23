using System;
using UnityEngine;
using UnityEngine.UI;
using Zombie3D;

public class EnemyRadar : MonoBehaviour
{
	private void OnEnable()
	{
		this.player = GameApp.GetInstance().GetGameScene().GetPlayer();
		if (this.Target.GetEnemyProbability() == EnemyProbability.BOSS)
		{
			this.Icon.sprite = this.TargetSprites[1];
			this.Icon.SetNativeSize();
		}
		else if (this.Target.GetEnemyProbability() == EnemyProbability.ELITE)
		{
			this.Icon.sprite = this.TargetSprites[1];
			this.Icon.SetNativeSize();
		}
		else if (this.Target.GetEnemyProbability() == EnemyProbability.NORMAL)
		{
			this.Icon.sprite = this.TargetSprites[0];
			this.Icon.SetNativeSize();
		}
	}

	private void Update()
	{
		if (base.gameObject.activeSelf)
		{
			if (this.Target.HP > 0f)
			{
				float num = Vector3.Distance(this.Target.GetTransform().position, this.player.GetTransform().position);
				float num2 = (this.WarnRange - num) / this.WarnRange;
				if (num2 < 0.2f)
				{
					num2 = 0.2f;
				}
				this.Icon.color = new Color(1f, 1f, 1f, num2);
				Vector3 to = this.Target.GetTransform().position - this.player.GetTransform().position;
				float num3 = Vector3.Angle(this.player.GetTransform().forward, to);
				if (Vector3.Angle(this.player.GetTransform().right, to) <= 90f)
				{
					base.transform.localEulerAngles = new Vector3(0f, 0f, 180f - num3);
				}
				else
				{
					base.transform.localEulerAngles = new Vector3(0f, 0f, num3 - 180f);
				}
			}
			else
			{
				base.gameObject.SetActive(false);
			}
		}
	}

	public Enemy Target;

	public Image Icon;

	public Sprite[] TargetSprites;

	[HideInInspector]
	public float WarnRange;

	private Player player;
}
