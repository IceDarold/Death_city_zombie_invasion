using System;
using System.Collections;
using UnityEngine;
using Zombie3D;

[AddComponentMenu("TPS/ProjectileScript")]
public class ProjectileScript : MonoBehaviour
{
	public void Start()
	{
	}

	public void Update()
	{
		this.deltaTime += Time.deltaTime;
		if (this.deltaTime < 0.03f)
		{
			return;
		}
		if (this.gunType == WeaponType.Sniper)
		{
			if (this.targetTransform != null)
			{
				if (this.targetTransform.gameObject.active)
				{
					this.dir = (this.targetTransform.position - this.proTransform.position).normalized;
					this.targetPos = this.targetTransform.position;
				}
				else
				{
					this.targetTransform = null;
				}
			}
			this.proTransform.LookAt(this.targetPos);
			this.initAngel -= this.deltaTime * 80f;
			if (this.initAngel <= 0f)
			{
				this.initAngel = 0f;
			}
			this.proTransform.rotation = Quaternion.AngleAxis(this.initAngel, -1f * this.proTransform.right) * this.proTransform.rotation;
			this.dir = this.proTransform.forward;
			if (Time.time - this.lastCheckPosTime > 0.3f)
			{
				this.lastCheckPosTime = Time.time;
				if ((this.proTransform.position - this.lastPos).sqrMagnitude < 2f)
				{
					UnityEngine.Object.DestroyObject(base.gameObject);
					return;
				}
				this.lastPos = this.proTransform.position;
			}
		}
		if (this.gunType == WeaponType.GrenadeRifle || this.gunType == WeaponType.NurseSaliva)
		{
			this.speed += Physics.gravity.y * Vector3.up * this.deltaTime;
			this.proTransform.Translate(this.speed * this.deltaTime, Space.World);
			this.proTransform.LookAt(this.proTransform.position + this.speed * 10f);
		}
		else
		{
			this.proTransform.Translate(this.flySpeed * this.dir * this.deltaTime, Space.World);
			if (this.gunType == WeaponType.RocketLauncher)
			{
				this.proTransform.Rotate(Vector3.forward, this.deltaTime * 900f, Space.Self);
			}
		}
		if (Time.time - this.createdTime > this.life)
		{
			UnityEngine.Object.DestroyObject(base.gameObject);
		}
		this.deltaTime = 0f;
	}

	private void OnTriggerStay(Collider other)
	{
		if (this.gunType == WeaponType.LaserGun)
		{
			GameScene gameScene = GameApp.GetInstance().GetGameScene();
			Player player = gameScene.GetPlayer();
			Weapon weapon = player.GetWeapon();
			if (weapon.GetWeaponType() == WeaponType.LaserGun)
			{
				LaserGun laserGun = weapon as LaserGun;
				if (laserGun.CouldMakeNextShoot() && other.gameObject.layer == 9)
				{
					Enemy enemyByID = gameScene.GetEnemyByID(other.gameObject.name);
					if (enemyByID != null && enemyByID.GetState() != Enemy.DEAD_STATE)
					{
						UnityEngine.Object.Instantiate<GameObject>(this.laserHitObject, enemyByID.GetPosition(), Quaternion.identity);
						DamageProperty dp = new DamageProperty(this.damage, (this.dir + new Vector3(0f, 0.18f, 0f)) * this.hitForce);
						enemyByID.OnHit(dp, this.gunType, Bone.None);
					}
				}
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		Player player = gameScene.GetPlayer();
		if (this.gunType == WeaponType.RocketLauncher || this.gunType == WeaponType.GrenadeRifle || this.gunType == WeaponType.Sniper)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.explodeObject, this.proTransform.position, Quaternion.identity);
			UnityEngine.Object.DestroyObject(base.gameObject);
			int num = 0;
			IEnumerator enumerator = gameScene.GetEnemies().Values.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Enemy enemy = (Enemy)obj;
					if (enemy.GetState() != Enemy.DEAD_STATE)
					{
						float sqrMagnitude = (enemy.GetPosition() - this.proTransform.position).sqrMagnitude;
						float num2 = this.explodeRadius * this.explodeRadius;
						if (sqrMagnitude < num2)
						{
							DamageProperty dp;
							if (sqrMagnitude * 4f < num2)
							{
								dp = new DamageProperty(this.damage * player.PowerBuff, (this.dir + new Vector3(0f, 0.18f, 0f)) * this.hitForce);
							}
							else
							{
								dp = new DamageProperty(this.damage / 2f * player.PowerBuff, (this.dir + new Vector3(0f, 0.18f, 0f)) * this.hitForce);
							}
							enemy.OnHit(dp, this.gunType, Bone.None);
						}
						if (enemy.HP <= 0f)
						{
							num++;
						}
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
			if (num >= 4)
			{
			}
			return;
		}
		if (this.gunType == WeaponType.LaserGun)
		{
			if (other.gameObject.layer == 9)
			{
				Enemy enemyByID = gameScene.GetEnemyByID(other.gameObject.name);
				if (enemyByID != null && enemyByID.GetState() != Enemy.DEAD_STATE)
				{
					DamageProperty dp2 = new DamageProperty(this.damage, (this.dir + new Vector3(0f, 0.18f, 0f)) * this.hitForce);
					enemyByID.OnHit(dp2, this.gunType, Bone.None);
				}
			}
			else if (other.gameObject.layer == 19)
			{
				WoodBoxScript component = other.gameObject.GetComponent<WoodBoxScript>();
				component.OnHit(this.damage);
			}
		}
		else if (this.gunType == WeaponType.NurseSaliva && other.gameObject.layer != 9)
		{
			Ray ray = new Ray(this.proTransform.position + Vector3.up * 1f, Vector3.down);
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit, 100f, 32768))
			{
				float y = raycastHit.point.y;
			}
		}
	}

	public WeaponType GunType
	{
		set
		{
			this.gunType = value;
		}
	}

	protected GameObject resObject;

	protected GameObject explodeObject;

	protected GameObject smallExplodeObject;

	protected GameObject laserHitObject;

	public Transform targetTransform;

	protected Transform proTransform;

	protected WeaponType gunType;

	protected ResourceConfigScript rConf;

	public Vector3 dir;

	public float hitForce;

	public float explodeRadius;

	public float flySpeed;

	public Vector3 speed;

	public float life = 2f;

	public float damage;

	protected float createdTime;

	protected float lastTriggerTime;

	protected float gravity = 16f;

	protected float downSpeed;

	protected float deltaTime;

	protected Vector3 targetPos;

	protected Vector3 lastPos;

	protected float initAngel = 40f;

	protected float lastCheckPosTime;
}
