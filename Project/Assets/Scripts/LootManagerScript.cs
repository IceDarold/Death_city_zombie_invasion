using System;
using UnityEngine;
using Zombie3D;

internal class LootManagerScript : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private WeaponConfig BonusEquipment(int wave)
	{
		WeaponConfig result = null;
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		return result;
	}

	private void SpawnItem(ItemType itemType)
	{
	}

	public void OnLoot()
	{
		float value = UnityEngine.Random.value;
		float num = this.dropRate;
		if (value < num)
		{
			value = UnityEngine.Random.value;
			float num2 = 0f;
			for (int i = 0; i < this.itemTables.Length; i++)
			{
				if (this.rateTables[i] > 0f && value <= num2 + this.rateTables[i])
				{
					this.SpawnItem(this.itemTables[i]);
					return;
				}
				num2 += this.rateTables[i];
			}
		}
	}

	public float dropRate = 1f;

	public ItemType[] itemTables = new ItemType[10];

	public float[] rateTables = new float[10];
}
