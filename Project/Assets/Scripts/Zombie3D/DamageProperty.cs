using System;
using UnityEngine;

namespace Zombie3D
{
	public class DamageProperty
	{
		protected DamageProperty()
		{
		}

		public DamageProperty(float _damage) : this(_damage, 0f, Vector3.zero)
		{
		}

		public DamageProperty(float _damage, Vector3 _hitDir) : this(_damage, 0f, _hitDir)
		{
		}

		public DamageProperty(float _damage, float _force, Vector3 _hitDir)
		{
			this.damage = _damage;
			this.force = _force;
			this.hitDir = _hitDir;
		}

		public Vector3 hitDir;

		public float damage;

		public float force;
	}
}
