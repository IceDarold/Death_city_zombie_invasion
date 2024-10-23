using System;
using System.Collections;
using DataCenter;
using UnityEngine;
using Zombie3D;

public class SneerBomb : GameProp, EnemyTarget
{
	public void OnHit(float damage, bool isCritical, AttackType type)
	{
	}

	public Collider GetCollider()
	{
		return null;
	}

	public Transform GetTransform()
	{
		return base.transform;
	}

	public bool IsVisible()
	{
		return base.gameObject.activeInHierarchy;
	}

	public EnemyTargetType GetTargetType()
	{
		return EnemyTargetType.SNEER_BOMB;
	}

	public void DoRevive()
	{
	}

	public void DoPause()
	{
	}

	public void DoResume()
	{
	}

	protected override void Init()
	{
		base.Init();
		if (!this.isDebug)
		{
			PropData propData = PropDataManager.GetPropData(this.ID);
			this.Type = propData.Type;
			this.Value = (float)propData.Value * Mathf.Pow(1.132f, (float)CheckpointDataManager.GetCurrentCheckpointIndex());
			this.Range = propData.Range;
			this.Delay = propData.Delay;
			this.Rate = propData.Rate;
			this.Duration = propData.Duration;
		}
		this.BombBody.gameObject.SetActive(true);
		this.ExplodeParticle.gameObject.SetActive(false);
	}

	public override void Activate(Vector3 direction = default(Vector3))
	{
		this.Init();
		base.gameObject.SetActive(true);
		base.transform.rotation = Quaternion.Euler(direction);
		base.GetComponent<Rigidbody>().AddForce(direction.normalized * this.Force, ForceMode.Impulse);
		this.isEffect = true;
		Singleton<GameAudioManager>.Instance.PlaySoundInGame(this.WarningSound, true);
	}

	private void OnWork()
	{
		if (this.isEffect)
		{
			if (this.duration < this.Duration)
			{
				this.duration += Time.deltaTime;
				if (this.rate < this.Rate)
				{
					this.rate += Time.deltaTime;
				}
				else
				{
					this.rate = 0f;
					this.FindEnemyInRange();
				}
			}
			else
			{
				this.isEffect = false;
				this.duration = 0f;
				base.Explode();
				ShakeCamera.Instance.Shake(CameraShakeType.DRASTIC);
				base.StartCoroutine(this.Suicide());
			}
		}
	}

	private void FindEnemyInRange()
	{
		Hashtable enemies = GameApp.GetInstance().GetGameScene().GetEnemies();
		IDictionaryEnumerator enumerator = enemies.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Enemy enemy = ((DictionaryEntry)obj).Value as Enemy;
				if (Vector3.Distance(base.transform.position, enemy.GetTransform().position) <= this.Range)
				{
					if (enemy.GetTarget().GetTargetType() != EnemyTargetType.SNEER_BOMB)
					{
						enemy.SetForceFocusTarget(this);
					}
				}
				else if (enemy.GetTarget() == this)
				{
					enemy.SetForceFocusTarget(null);
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
	}

	private void SetEnemyTarget(EnemyTarget target)
	{
		Hashtable enemies = GameApp.GetInstance().GetGameScene().GetEnemies();
		IDictionaryEnumerator enumerator = enemies.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Enemy enemy = ((DictionaryEntry)obj).Value as Enemy;
				if (enemy.GetTarget() == this)
				{
					enemy.SetForceFocusTarget(null);
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
	}

	private IEnumerator Suicide()
	{
		this.BombBody.gameObject.SetActive(false);
		this.ExplodeParticle.gameObject.SetActive(true);
		this.SetEnemyTarget(null);
		yield return new WaitForSeconds(this.ExplodeParticle.main.duration);
		base.gameObject.SetActive(false);
		this.BombBody.gameObject.SetActive(true);
		this.ExplodeParticle.gameObject.SetActive(false);
		yield break;
	}

	private void Update()
	{
		this.OnWork();
	}

	public Transform BombBody;

	public ParticleSystem ExplodeParticle;

	[Header("警示音")]
	public AudioClip WarningSound;

	public float Force;
}
