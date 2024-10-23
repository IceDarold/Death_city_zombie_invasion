using System;
using UnityEngine;
using Zombie3D;

public class SceneSubway : MonoBehaviour
{
	public void OnTriggerEnter(Collider other)
	{
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		if (gameScene.PlayingState != PlayingState.GamePlaying)
		{
			return;
		}
		int layer = other.gameObject.layer;
		if (layer == 8)
		{
			gameScene.GetPlayer().OnHit(float.PositiveInfinity, false, AttackType.REMOTE);
		}
		else if (layer == 9 || layer == 27)
		{
			string enemyID = other.gameObject.name.Split(new char[]
			{
				'|'
			})[0];
			Enemy enemyByID = GameApp.GetInstance().GetGameScene().GetEnemyByID(enemyID);
			enemyByID.OnHit(new DamageProperty(float.PositiveInfinity), WeaponType.NoGun, base.transform.position, Bone.MiddleSpine);
		}
	}
}
