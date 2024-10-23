using System;
using System.Collections.Generic;
using UnityEngine;

namespace RacingMode
{
	public class RacingSceneInfo : MonoBehaviour
	{
		public List<RacingTrackTag> Tracks = new List<RacingTrackTag>();

		public List<RacingZombieTag> Zombies = new List<RacingZombieTag>();

		public List<RacingObstacleTag> Obstacles = new List<RacingObstacleTag>();
	}
}
