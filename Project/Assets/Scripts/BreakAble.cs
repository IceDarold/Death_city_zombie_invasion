using System;
using System.Collections.Generic;
using UnityEngine;
using Zombie3D;

public interface BreakAble
{
	Transform GetTransform();

	void OnHit(DamageProperty dp, WeaponType weaponType, Vector3 pos, Bone _bone);

	List<WeaponHitInfo> SimulateKillEnemy();
}
