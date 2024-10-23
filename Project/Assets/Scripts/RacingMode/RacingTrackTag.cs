using System;
using UnityEngine;

namespace RacingMode
{
	[Serializable]
	public class RacingTrackTag
	{
		public string Name;

		public Vector3 Position;

		public Quaternion Rotation;

		public Vector3 Scale;

		public Vector3 StartPoint;

		public Vector3 EndPoint;
	}
}
