using System;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using UnityEngine;
using Zombie3D;

public class GameProp : MonoBehaviour
{
	public void OnDrawGizmos()
	{
	}

	public virtual void Activate(Vector3 direction = default(Vector3))
	{
	}

	protected virtual void Init()
	{
		if (this.Type == PropType.MEDKIT)
		{
			AchievementDataManager.SetAchievementValue(DataCenter.AchievementType.USE_MEDKIT, 1);
		}
		else if (this.Type == PropType.GRENADE)
		{
			AchievementDataManager.SetAchievementValue(DataCenter.AchievementType.USE_GRENADE, 1);
		}
		this.isDelay = false;
		this.isEffect = false;
		this.delay = 0f;
		this.duration = 0f;
		this.rate = 0f;
	}

	private void SetExplodeObjects(ref List<Enemy> list)
	{
		Hashtable enemies = GameApp.GetInstance().GetGameScene().GetEnemies();
		IDictionaryEnumerator enumerator = enemies.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Enemy enemy = ((DictionaryEntry)obj).Value as Enemy;
				float num = Vector3.Distance(base.transform.position, enemy.GetAimBone().position);
				if (num <= this.Range)
				{
					RaycastHit raycastHit;
					Physics.Raycast(base.transform.position, enemy.GetAimBone().position - base.transform.position, out raycastHit, this.Range, 134220288);
					if (raycastHit.collider.gameObject.layer == 9 || raycastHit.collider.gameObject.layer == 27)
					{
						list.Add(enemy);
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
	}

	protected void Explode()
	{
		Singleton<GameAudioManager>.Instance.PlayGlobalEffect(GlobalEffect.NormalBomb);
		List<Enemy> list = new List<Enemy>();
		this.SetExplodeObjects(ref list);
		for (int i = 0; i < list.Count; i++)
		{
			float damage = 0f;
			if (list[i].GetEnemyProbability() == global::EnemyProbability.NORMAL)
			{
				damage = list[i].MaxHp * 0.5f + this.Value;
			}
			else if (list[i].GetEnemyProbability() == global::EnemyProbability.ELITE)
			{
				damage = list[i].MaxHp * 0.2f + this.Value;
			}
			else if (list[i].GetEnemyProbability() == global::EnemyProbability.BOSS)
			{
				damage = list[i].MaxHp * 0.05f + this.Value;
			}
			Vector3 hitDir = list[i].GetAimBone().position - base.transform.position;
			if (hitDir.y < 0f)
			{
				hitDir = new Vector3(hitDir.x, 0f, hitDir.z);
			}
			DamageProperty dp = new DamageProperty(damage, this.ForceToEnemy, hitDir);
			list[i].OnHit(dp, Zombie3D.WeaponType.PLAYER_BOMBER, Bone.MiddleSpine);
		}
	}

	[Header("是否使用调试值")]
	public bool isDebug;

	[Header("编号 -- 每个不同的道具具有唯一编号 -- 读取数据的标识")]
	public int ID;

	[Header("道具类型")]
	public PropType Type;

	[Tooltip("伤害或者医疗值")]
	public float Value;

	[Tooltip("延迟时间")]
	public float Delay;

	[Tooltip("作用时间")]
	public float Duration;

	[Tooltip("作用范围")]
	public float Range;

	[Tooltip("作用频率")]
	public float Rate;

	[Header("对怪物的作用力")]
	public float ForceToEnemy = 20f;

	protected Enemy Target;

	protected bool isDelay;

	protected bool isEffect;

	protected float delay;

	protected float duration;

	protected float rate;
}
