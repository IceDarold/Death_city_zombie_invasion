using System;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using DG.Tweening;
using UnityEngine;
using Zombie3D;

public class TurretProp : GameProp
{
	protected override void Init()
	{
		base.Init();
		this.Target = null;
		if (!this.isDebug)
		{
			PropData propData = PropDataManager.GetPropData(this.ID);
			this.Type = propData.Type;
			this.Range = propData.Range * (TalentDataManager.GetTalentValue(Talent.TURRET_DETECT_RANGE) + 1f);
			this.Delay = propData.Delay;
			this.Rate = propData.Rate;
			this.Duration = propData.Duration;
		}
		this.BulletParticle.gameObject.SetActive(false);
		this.FireParticle.gameObject.SetActive(false);
		this.SuicideParticle.gameObject.SetActive(false);
		this.TurretBody.gameObject.SetActive(true);
		this.TurretBody.localEulerAngles = Vector3.zero;
		this.TurretStand.gameObject.SetActive(true);
	}

	public override void Activate(Vector3 direction = default(Vector3))
	{
		this.Init();
		UnityEngine.Debug.Log("启用炮台");
		base.transform.rotation = Quaternion.Euler(direction);
		base.gameObject.SetActive(true);
		this.isEffect = true;
		this.isWorking = false;
	}

	private void OnWork()
	{
		if (this.rate < this.Rate)
		{
			this.rate += Time.deltaTime;
		}
		else if (!this.isWorking)
		{
			this.isWorking = true;
			RaycastHit raycastHit;
			if (this.Target == null)
			{
				this.SetTarget();
			}
			else if (Vector3.Distance(base.transform.position, this.Target.GetAimBone().position) > this.Range)
			{
				this.SetTarget();
			}
			else if (Physics.Raycast(base.transform.position, this.Target.GetAimBone().position - base.transform.position, out raycastHit))
			{
				this.TurretFire();
			}
			else
			{
				this.SetTarget();
			}
		}
	}

	private Enemy SearchTarget()
	{
		Hashtable enemies = GameApp.GetInstance().GetGameScene().GetEnemies();
		List<Enemy> list = new List<Enemy>();
		IDictionaryEnumerator enumerator = enemies.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Enemy enemy = ((DictionaryEntry)obj).Value as Enemy;
				float num = Vector3.Distance(base.transform.position, enemy.GetAimBone().position);
				if (num > 1f && num <= this.Range)
				{
					list.Add(enemy);
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
		list.Sort(delegate(Enemy a, Enemy b)
		{
			float num2 = Vector3.Distance(base.transform.position, a.GetAimBone().position);
			float value = Vector3.Distance(base.transform.position, b.GetAimBone().position);
			return num2.CompareTo(value);
		});
		for (int i = 0; i < list.Count; i++)
		{
			RaycastHit raycastHit;
			Physics.Raycast(base.transform.position, list[i].GetAimBone().position - base.transform.position, out raycastHit);
			if (raycastHit.collider.gameObject.layer == 9 || raycastHit.collider.gameObject.layer == 27)
			{
				return list[i];
			}
		}
		return null;
	}

	private void SetTarget()
	{
		Enemy enemy = this.SearchTarget();
		if (enemy != null)
		{
			this.TurretBody.transform.DOLookAt(enemy.GetAimBone().position, this.Speed, AxisConstraint.None, null).OnComplete(delegate
			{
				this.Target = enemy;
				this.TurretFire();
			});
		}
		else
		{
			this.rate = 0f;
			this.isWorking = false;
		}
	}

	private void SetTurretState()
	{
		if (this.Target == null)
		{
			this.FireParticle.gameObject.SetActive(false);
			this.BulletParticle.gameObject.SetActive(false);
			this.TurretStandby();
		}
		else
		{
			this.FireParticle.gameObject.SetActive(true);
			this.BulletParticle.gameObject.SetActive(true);
			this.TurretBody.LookAt(this.Target.GetAimBone().position);
			this.Barrel.localEulerAngles += new Vector3(0f, 1440f * Time.deltaTime, 0f);
			if (this.Target.HP <= 0f)
			{
				this.Target = null;
			}
		}
	}

	private void TurretFire()
	{
		this.rate = 0f;
		this.isWorking = false;
		Singleton<GameAudioManager>.Instance.PlaySoundInGame(this.FireClip, false);
		float damage = 0f;
		if (this.Target.GetEnemyProbability() == global::EnemyProbability.NORMAL)
		{
			damage = this.Target.MaxHp * 0.1f + this.Value;
		}
		else if (this.Target.GetEnemyProbability() == global::EnemyProbability.ELITE)
		{
			damage = this.Target.MaxHp * 0.01f + this.Value;
		}
		else if (this.Target.GetEnemyProbability() == global::EnemyProbability.BOSS)
		{
			damage = this.Target.MaxHp * 0.00025f + this.Value;
		}
		DamageProperty dp = new DamageProperty(damage, this.ForceToEnemy, this.Target.GetAimBone().position - base.transform.position);
		this.Target.OnHit(dp, Zombie3D.WeaponType.PLAYER_CANNON, Bone.None);
	}

	private void CreateBullet()
	{
		if (this.Target != null)
		{
			Vector3 position = this.Target.GetAimBone().position;
			for (int i = 0; i < this.Bullets.Count; i++)
			{
				if (!this.Bullets[i].gameObject.activeSelf)
				{
					this.Bullets[i].transform.position = this.Muzzle.position;
					this.Bullets[i].transform.rotation = Quaternion.Euler(this.Muzzle.rotation.eulerAngles);
					this.Bullets[i].gameObject.SetActive(true);
					this.Bullets[i].Activate(position);
					return;
				}
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Prefab);
			gameObject.transform.SetParent(this.BulletPool);
			gameObject.transform.position = this.Muzzle.position;
			gameObject.transform.rotation = Quaternion.Euler(this.Muzzle.rotation.eulerAngles);
			TurretBullet component = gameObject.GetComponent<TurretBullet>();
			this.Bullets.Add(component);
			component.Activate(position);
		}
	}

	private void TurretStandby()
	{
		if (this.TurretBody != null && (int)this.TurretBody.transform.localEulerAngles.y != 0)
		{
			if (this.TurretBody.transform.localEulerAngles.y < 180f)
			{
				this.TurretBody.transform.Rotate(Vector3.up, -45f * Time.deltaTime);
			}
			else if (this.TurretBody.transform.localEulerAngles.y >= 180f)
			{
				this.TurretBody.transform.Rotate(Vector3.up, 45f * Time.deltaTime);
			}
		}
	}

	private IEnumerator Suicide()
	{
		this.TurretBody.gameObject.SetActive(false);
		this.TurretStand.gameObject.SetActive(false);
		this.SuicideParticle.Play(true);
		Singleton<GameAudioManager>.Instance.PlaySoundInGame(this.SuicideClip, false);
		yield return new WaitForSeconds(this.SuicideParticle.main.duration);
		base.gameObject.SetActive(false);
		this.TurretBody.gameObject.SetActive(true);
		this.TurretStand.gameObject.SetActive(true);
		yield break;
	}

	private void Update()
	{
		if (this.isEffect && Time.timeScale != 0f)
		{
			if (this.duration < this.Duration)
			{
				this.duration += Time.deltaTime;
				this.OnWork();
				this.SetTurretState();
			}
			else
			{
				this.isEffect = false;
				this.duration = 0f;
				base.StartCoroutine(this.Suicide());
			}
		}
	}

	[Header("支架")]
	public Transform TurretStand;

	[Header("炮台主体")]
	public Transform TurretBody;

	[Header("炮管")]
	public Transform Barrel;

	[Header("子弹特效")]
	public ParticleSystem BulletParticle;

	[Header("开火特效")]
	public ParticleSystem FireParticle;

	[Header("自毁特效")]
	public ParticleSystem SuicideParticle;

	[Header("开火音效")]
	public AudioClip FireClip;

	[Header("自毁音效")]
	public AudioClip SuicideClip;

	[Header("转向音效")]
	public AudioClip RotateClip;

	[Header("速度")]
	public float Speed;

	[Header("子弹预制体")]
	public GameObject Prefab;

	[Header("枪口")]
	public Transform Muzzle;

	[Header("子弹对象池")]
	public Transform BulletPool;

	private bool isWorking;

	private List<TurretBullet> Bullets = new List<TurretBullet>();
}
