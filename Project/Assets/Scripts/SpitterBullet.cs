using System;
using UnityEngine;
using Zombie3D;

public class SpitterBullet : MonoBehaviour
{
	public void Init(float _damage, string enemyname)
	{
		this.damage = _damage;
		this.selfName = enemyname;
	}

	public void OnTriggerEnter(Collider other)
	{
		int layer = other.gameObject.layer;
		if (layer != 8)
		{
			if (layer != 9)
			{
				switch (layer)
				{
				case 27:
					break;
				case 28:
				case 29:
					return;
				case 30:
					this.DoHitNPC(other);
					return;
				default:
					return;
				}
			}
			this.DoHitEnemy(other.gameObject.name);
		}
		else
		{
			this.DoHitPlayer();
		}
	}

	private void DoHitPlayer()
	{
		GameApp.GetInstance().GetGameScene().GetPlayer().OnHit(this.damage, false, AttackType.REMOTE);
	}

	private void DoHitEnemy(string enemyName)
	{
		string text = enemyName.Split(new char[]
		{
			'|'
		})[0];
		if (this.selfName == text)
		{
			return;
		}
		Enemy enemyByID = GameApp.GetInstance().GetGameScene().GetEnemyByID(text);
		Vector3 hitDir = enemyByID.GetTransform().position - base.transform.position;
		hitDir.y = 0f;
		DamageProperty dp = new DamageProperty(this.damage, hitDir);
		enemyByID.OnHit(dp, WeaponType.NoGun, Bone.MiddleSpine);
	}

	private void DoHitNPC(Collider other)
	{
		EnemyTarget component = other.gameObject.GetComponent<EnemyTarget>();
		if (component == null)
		{
			UnityEngine.Debug.LogError("npc onhit is null  name = " + other.gameObject.name);
			return;
		}
		component.OnHit(this.damage, false, AttackType.NORMAL);
	}

	private float damage;

	private string selfName;
}
