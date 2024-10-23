using System;
using UnityEngine;

namespace Zombie3D
{
	public class InputInfo
	{
		public bool IsMoving()
		{
			return this.moveDirection.x != 0f || this.moveDirection.z != 0f;
		}

		public bool fire;

		public bool stopFire;

		public Vector3 moveDirection = Vector3.zero;
	}
}
