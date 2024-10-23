using System;
using UnityEngine;

namespace RacingMode
{
	[Serializable]
	public class RacingZombieTag
	{
		public string Name;

		public Vector3 Position;

		public Quaternion Rotation;

		public Vector3 Scale;

		public RacingZombieType Type;

		public int MaxHp;

		public float WalkSpeed = 0.7f;

		public float RunSpeed = 0.7f;

		public int ChaseProbability;

		public float ChaseSpeed = 50f;

		public float JumpDistance = 10f;

		public float JumpTime = 1f;

		public float HurtRate = 4f;

		public float HurtValue;
	}
}
