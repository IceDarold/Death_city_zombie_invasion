using System;
using UnityEngine;
using Zombie3D;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyAttackBox : MonoBehaviour
{
	public void SetEnemy(Enemy enemy, int id)
	{
		this.thisEnemy = enemy;
		this.boxID = id;
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 8)
		{
			this.CheckHitPlayer(this.boxID);
		}
		else if (other.gameObject.layer == 30)
		{
			this.CheckHitNPC(other.gameObject, this.boxID);
		}
		else if (other.gameObject.layer == 9 || other.gameObject.layer == 27)
		{
			this.CheckHitEnemy(other);
		}
		this.thisEnemy.OnAttackBoxEnterTrigger(other, this.boxID);
	}

	private void CheckHitEnemy(Collider other)
	{
		string enemyID = other.gameObject.name.Split(new char[]
		{
			'|'
		})[0];
		Enemy enemyByID = GameApp.GetInstance().GetGameScene().GetEnemyByID(enemyID);
		if (this.thisEnemy.AttAttribute.oneShotKill)
		{
			enemyByID.OnHit(new DamageProperty(enemyByID.MaxHp), WeaponType.NoGun, base.transform.position, Bone.MiddleSpine);
		}
		else
		{
			enemyByID.OnHit(new DamageProperty(enemyByID.AttackRange), WeaponType.NoGun, base.transform.position, Bone.MiddleSpine);
		}
	}

	private void CheckHitPlayer(int boxID)
	{
		Player player = GameApp.GetInstance().GetGameScene().GetPlayer();
		player.OnHit(this.thisEnemy.AttackDamage, this.thisEnemy.Attr.isCritical[boxID], AttackType.NORMAL);
	}

	private void CheckHitNPC(GameObject obj, int boxID)
	{
		EnemyTarget component = obj.GetComponent<EnemyTarget>();
		if (component == null)
		{
			UnityEngine.Debug.LogError("npc onhit is null  name = " + obj.name);
			return;
		}
		component.OnHit(this.thisEnemy.AttackDamage, this.thisEnemy.Attr.isCritical[boxID], AttackType.NORMAL);
	}

	protected Enemy thisEnemy;

	protected int boxID;
}
