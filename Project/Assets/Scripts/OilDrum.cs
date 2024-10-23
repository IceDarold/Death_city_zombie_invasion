using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombie3D;

public class OilDrum : MonoBehaviour, BreakAble
{
	private IEnumerator Start()
	{
		yield return null;
		this.gameScene = GameApp.GetInstance().GetGameScene();
		this.gameScene.allBreakAbleDrums.Add(this);
		yield break;
	}

	public void OnDestroy()
	{
		this.gameScene.allBreakAbleDrums.Remove(this);
	}

	public Transform GetTransform()
	{
		return base.transform;
	}

	public void OnHit(DamageProperty dp, WeaponType weaponType, Vector3 pos, Bone _bone)
	{
		if (this.isExplod)
		{
			return;
		}
		this.isExplod = true;
		if (this.gameScene.PlayingMode != GamePlayingMode.SnipeMode)
		{
			this.DoExplod();
		}
		else
		{
			this.DoExplod2LockedEnemies();
		}
	}

	private void DoExplod()
	{
		base.gameObject.SetActive(false);
		UnityEngine.Object.Destroy(base.gameObject);
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Singleton<ResourceConfigScript>.Instance.woodExplode);
		gameObject.transform.position = base.transform.position;
		Player player = this.gameScene.GetPlayer();
		if (this.dmg2Player > 0f)
		{
			float sqrMagnitude = (player.GetTransform().position - base.transform.position).sqrMagnitude;
			if (sqrMagnitude < this.radius * this.radius)
			{
				player.OnHit(player.GetMaxHp() * this.dmg2Player, false, AttackType.BOMB);
			}
		}
		this.gameScene.Bomb2Enemies(this.dmg2Normal, this.dmg2Elite, this.radius, base.transform.position, this.force, WeaponType.PLAYER_BOMBER);
		this.gameScene.CheckAllBreakableDrums(base.transform.position, this.radius, this);
	}

	private void DoExplod2LockedEnemies()
	{
		base.gameObject.SetActive(false);
		UnityEngine.Object.Destroy(base.gameObject);
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Singleton<ResourceConfigScript>.Instance.woodExplode);
		gameObject.transform.position = base.transform.position;
		bool flag = false;
		bool flag2 = false;
		for (int i = 0; i < this.snipeModeLockedEnemy.Count; i++)
		{
			WeaponHitInfo weaponHitInfo = this.snipeModeLockedEnemy[i];
			if (weaponHitInfo.enemy == null)
			{
				break;
			}
			if (!flag && weaponHitInfo.hitBone == Bone.Head && weaponHitInfo.enemy.SimulateHitEnemy(weaponHitInfo.damage))
			{
				flag = true;
			}
			if (!flag2 && weaponHitInfo.enemy.SimulateHitEnemy(weaponHitInfo.damage) && this.gameScene.sceneMissions.CheckEnemyInMission(weaponHitInfo.hitBone == Bone.Head, weaponHitInfo.enemy.KeyEnemy))
			{
				flag2 = true;
			}
			Enemy enemy = weaponHitInfo.enemy;
			enemy.OnHit(new DamageProperty(weaponHitInfo.damage, weaponHitInfo.hitForce, weaponHitInfo.hitDirection), WeaponType.Sniper, weaponHitInfo.hitPos, weaponHitInfo.hitBone);
		}
		if (flag)
		{
			Singleton<GlobalMessage>.Instance.SendGlobalMessage(MessageType.SniperHeadShot);
		}
		else if (flag2)
		{
			Singleton<GlobalMessage>.Instance.SendGlobalMessage(MessageType.SniperKillMissionTarget);
		}
	}

	public List<WeaponHitInfo> SimulateKillEnemy()
	{
		if (this.gameScene.PlayingMode != GamePlayingMode.SnipeMode)
		{
			return null;
		}
		this.snipeModeLockedEnemy.Clear();
		Hashtable enemies = this.gameScene.GetEnemies();
		object[] array = new object[enemies.Count];
		enemies.Keys.CopyTo(array, 0);
		int i = 0;
		int num = array.Length;
		while (i < num)
		{
			Enemy enemy = enemies[array[i]] as Enemy;
			float sqrMagnitude = (enemy.GetTransform().position - base.transform.position).sqrMagnitude;
			if (sqrMagnitude <= this.radius * this.radius)
			{
				this.snipeModeLockedEnemy.Add(new WeaponHitInfo
				{
					hitBone = Bone.MiddleSpine,
					enemy = enemy,
					hitPos = enemy.GetTransform().position,
					damage = ((enemy.GetEnemyProbability() != EnemyProbability.NORMAL) ? (enemy.MaxHp * this.dmg2Elite) : (enemy.MaxHp * this.dmg2Normal)),
					hitDirection = (enemy.GetTransform().position - base.transform.position).normalized,
					hitForce = this.force
				});
			}
			i++;
		}
		return this.snipeModeLockedEnemy;
	}

	[CNName("生命值")]
	public float hp = 1f;

	[CNName("爆炸范围")]
	public float radius = 3f;

	[CNName(0f, 1f, "对小怪伤害")]
	public float dmg2Normal;

	[CNName(0f, 1f, "对精英怪伤害")]
	public float dmg2Elite;

	[CNName(0f, 1f, "对角色伤害")]
	public float dmg2Player;

	[CNName("爆炸击飞力")]
	public float force = 50f;

	protected GameScene gameScene;

	protected bool isExplod;

	protected List<WeaponHitInfo> snipeModeLockedEnemy = new List<WeaponHitInfo>();
}
