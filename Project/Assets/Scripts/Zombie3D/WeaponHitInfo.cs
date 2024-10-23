using System;
using UnityEngine;

namespace Zombie3D
{
	public class WeaponHitInfo
	{
		public Bone hitBone;

		public float damage;

		public float hitForce;

		public Vector3 hitPos;

		public Vector3 hitDirection;

		public Enemy enemy;

		public BreakAble breakableDrum;

		public WeaponType weaponType;
	}
}
