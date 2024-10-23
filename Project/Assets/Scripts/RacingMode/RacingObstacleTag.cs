using System;
using UnityEngine;

namespace RacingMode
{
	[Serializable]
	public class RacingObstacleTag
	{
		public string Name;

		public Vector3 Position;

		public Quaternion Rotation;

		public Vector3 Scale;

		public ObstacleEnum Type;

		public float HurtValue;

		public float SlowDownValue;

		public float HitPower;

		public float MassForceProportion;

		public Vector3 ExplosionDirection;

		public AnimationCurve JumpCuver;

		public AnimationCurve RiseCuver;

		public float JumpAngle;

		public int LandingIndex;
	}
}
