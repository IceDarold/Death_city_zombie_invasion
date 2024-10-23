using System;
using UnityEngine;

namespace SplineUtilities
{
	[Serializable]
	public class AutomaticUpdater
	{
		public bool Update()
		{
			AutomaticUpdater.UpdateMode updateMode = this.mode;
			if (updateMode == AutomaticUpdater.UpdateMode.EveryFrame)
			{
				return true;
			}
			if (updateMode == AutomaticUpdater.UpdateMode.EveryXFrames)
			{
				return Time.frameCount % this.deltaFrames == 0;
			}
			if (updateMode != AutomaticUpdater.UpdateMode.EveryXSeconds)
			{
				return false;
			}
			this.passedTime += Time.deltaTime;
			if (this.passedTime >= this.deltaSeconds)
			{
				this.passedTime = 0f;
				return true;
			}
			return false;
		}

		public AutomaticUpdater.UpdateMode mode;

		public float deltaSeconds = 0.1f;

		public int deltaFrames = 2;

		private float passedTime;

		public enum UpdateMode
		{
			DontUpdate,
			EveryFrame,
			EveryXFrames,
			EveryXSeconds
		}
	}
}
