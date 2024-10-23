using System;
using UnityEngine;
using Zombie3D;

public class StraightLineBullet : MonoBehaviour
{
	public void Awake()
	{
		this.canMove = false;
	}

	public void Init(Vector3 direction, float speed, float _damage)
	{
		this.moveDir = direction;
		this.moveSpeed = speed;
		this.damage = _damage;
		this.canMove = true;
	}

	public void Update()
	{
		if (!this.canMove)
		{
			return;
		}
		base.transform.Translate(this.moveDir * this.moveSpeed * Time.deltaTime, Space.World);
	}

	public void OnTriggerEnter(Collider other)
	{
		int layer = other.gameObject.layer;
		if (this.canHurtPlayer && layer == 8)
		{
			GameApp.GetInstance().GetGameScene().GetPlayer().OnHit(this.damage, false, AttackType.REMOTE);
			this.canMove = false;
			base.gameObject.SetActive(false);
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else if (this.canHurtEnemy && layer == 9)
		{
			this.canMove = false;
			base.gameObject.SetActive(false);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	[CNName("攻击角色")]
	public bool canHurtPlayer;

	[CNName("攻击小怪")]
	public bool canHurtEnemy;

	protected Vector3 moveDir;

	protected float moveSpeed;

	protected float damage;

	protected bool canMove;
}
