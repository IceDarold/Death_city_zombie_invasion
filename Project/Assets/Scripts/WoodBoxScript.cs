using System;
using System.Collections;
using UnityEngine;
using Zombie3D;

[ExecuteInEditMode]
public class WoodBoxScript : MonoBehaviour
{
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(0.1f);
		this.boxTransform = base.gameObject.transform;
		this.player = GameApp.GetInstance().GetGameScene().GetPlayer();
		yield break;
	}

	public void InitBezier(Vector3 _endPos, float _dmg2Player)
	{
		this.dmg2Player = _dmg2Player;
		this.startPos = base.transform.position;
		this.endPos = _endPos;
		Vector3 p = this.startPos + (this.endPos - this.startPos) / 2f + Vector3.up * 1f;
		this.moveBezier = new ThreePointBezier(this.startPos, p, this.endPos);
		this.rotateAxis = base.transform.forward * UnityEngine.Random.Range(0f, 1f) + base.transform.up * UnityEngine.Random.Range(0f, 1f) + base.transform.right * UnityEngine.Random.Range(0f, 1f);
		this.startBezier = true;
	}

	private void Update()
	{
		if (!this.startBezier)
		{
			return;
		}
		base.transform.position = this.moveBezier.GetPointAtTime(this.movePercent);
		this.movePercent += Time.deltaTime;
		base.transform.Rotate(this.rotateAxis, 180f * Time.deltaTime, Space.Self);
	}

	public void OnTriggerEnter(Collider other)
	{
		if (!this.startBezier)
		{
			return;
		}
		int layer = other.gameObject.layer;
		if (layer != 8)
		{
			if (layer != 9 && layer != 27)
			{
			}
		}
		this.OnHit(float.PositiveInfinity);
	}

	public void SetBindEnemy(Zombie_Throw enemy)
	{
		this.bindEnemy = enemy;
	}

	public void SetExplosiveUnable()
	{
		this.explosive = false;
		base.GetComponent<Rigidbody>().useGravity = true;
	}

	public void OnHit(float damage)
	{
		this.hp -= damage;
		if (this.hp <= 0f)
		{
			if (this.bindEnemy != null)
			{
				this.bindEnemy.SetStateSpeical(false);
			}
			GameScene gameScene = GameApp.GetInstance().GetGameScene();
			IEnumerator enumerator = gameScene.GetEnemies().Values.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Enemy enemy = (Enemy)obj;
					if ((base.transform.position - enemy.GetTransform().position).sqrMagnitude < this.explodeRadius * this.explodeRadius)
					{
						EnemyType enemyType = enemy.EnemyType;
						DamageProperty dp = new DamageProperty(enemy.HP);
						enemy.OnHit(dp, WeaponType.NoGun, Bone.None);
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			Singleton<SoundManager>.Instance.PlayEffect(this.bombClip);
			if (this.dmg2Player > 0f)
			{
				float sqrMagnitude = (base.transform.position - this.player.GetTransform().position).sqrMagnitude;
				if (sqrMagnitude < 25f)
				{
				}
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public float hp = 10f;

	[CNName("爆炸半径")]
	public float explodeRadius = 1f;

	[CNName("爆炸伤害")]
	public float explodeDamage = 1000000f;

	[CNName("爆炸音效")]
	public AudioClip bombClip;

	public Transform testEndPos;

	protected Transform boxTransform;

	private bool startBezier;

	protected Vector3 startPos;

	protected Vector3 endPos;

	protected ThreePointBezier moveBezier;

	protected float movePercent;

	protected bool explosive = true;

	protected Vector3 rotateAxis;

	protected Zombie_Throw bindEnemy;

	protected Player player;

	protected float dmg2Player;
}
