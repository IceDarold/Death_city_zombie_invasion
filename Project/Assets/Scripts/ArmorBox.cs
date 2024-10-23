using System;
using UnityEngine;
using Zombie3D;

public class ArmorBox : MonoBehaviour
{
	private void Awake()
	{
		this.timeCount = this.capacityTime;
	}

	public void SetFocus(bool _focus)
	{
		if (this.hasFocus == _focus)
		{
			return;
		}
		this.hasFocus = _focus;
		GameApp.GetInstance().GetGameScene().SetFillBulletEvnt(UIDisplayEvnt.FOCUS_ON_ARMORBOX, (!this.hasFocus) ? 0f : 1f);
	}

	public void DoFill(float dt)
	{
		if (this.player.GetState() == Player.CHARGER_STATE)
		{
			return;
		}
		float num = dt / this.capacityTime;
		float num2 = 0f;
		for (int i = 0; i < this.playerWeapon.Length; i++)
		{
			if (this.capacity <= 0f || this.playerWeapon[i].IsBulletFull())
			{
				num2 += 1f / (float)this.playerWeapon.Length;
			}
			else
			{
				this.capacity -= num;
				float num3 = (float)this.playerWeapon[i].MaxCapacity * num;
				this.bulletRemain[i] += num3;
				if (this.bulletRemain[i] < 1f)
				{
					num2 += (float)(this.playerWeapon[i].BulletCount + this.playerWeapon[i].MaxGunload) / (float)(this.playerWeapon[i].MaxCapacity + this.playerWeapon[i].MaxBulletNum) / (float)this.playerWeapon.Length;
				}
				else
				{
					bool flag = this.playerWeapon[i].AddBullet2Total((int)this.bulletRemain[i]);
					this.bulletRemain[i] %= 1f;
					if (flag)
					{
						this.bulletRemain[i] = 0f;
					}
					num2 += (float)(this.playerWeapon[i].BulletCount + this.playerWeapon[i].MaxGunload) / (float)(this.playerWeapon[i].MaxCapacity + this.playerWeapon[i].MaxBulletNum) / (float)this.playerWeapon.Length;
				}
			}
		}
		if (this.capacity <= 0f)
		{
			this.Close();
		}
		GameApp.GetInstance().GetGameScene().SetFillBulletEvnt(UIDisplayEvnt.FILL_BULLET_PERCENT, num2);
	}

	public void Close()
	{
		this.raycastTrigger.SetActive(false);
		this.render.material.shader = Shader.Find("Mobile/Diffuse");
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer != 8)
		{
			return;
		}
		if (this.player == null)
		{
			this.player = GameApp.GetInstance().GetGameScene().GetPlayer();
		}
		this.player.TriggeredArmorBox = this;
		this.playerWeapon = this.player.WeaponList.ToArray();
		this.bulletRemain = new float[this.playerWeapon.Length];
	}

	public void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer != 8)
		{
			return;
		}
		this.player.TriggeredArmorBox = null;
		GameApp.GetInstance().GetGameScene().SetFillBulletEvnt(UIDisplayEvnt.FOCUS_ON_ARMORBOX, 0f);
	}

	public float capacityTime = 1f;

	public float percent = 1f;

	public float capacity = 10f;

	public GameObject raycastTrigger;

	public Renderer render;

	protected bool startFill;

	protected Player player;

	protected float timeCount;

	protected Weapon[] playerWeapon;

	protected float[] bulletRemain;

	protected bool hasFocus;
}
