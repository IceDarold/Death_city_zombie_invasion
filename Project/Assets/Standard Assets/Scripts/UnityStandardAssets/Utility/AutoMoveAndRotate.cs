using System;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
	public class AutoMoveAndRotate : MonoBehaviour
	{
		private void Start()
		{
			this.m_LastRealTime = Time.realtimeSinceStartup;
		}

		private void Update()
		{
			float d = Time.deltaTime;
			if (this.ignoreTimescale)
			{
				d = Time.realtimeSinceStartup - this.m_LastRealTime;
				this.m_LastRealTime = Time.realtimeSinceStartup;
			}
			base.transform.Translate(this.moveUnitsPerSecond.value * d, this.moveUnitsPerSecond.space);
			base.transform.Rotate(this.rotateDegreesPerSecond.value * d, this.moveUnitsPerSecond.space);
		}

		public AutoMoveAndRotate.Vector3andSpace moveUnitsPerSecond;

		public AutoMoveAndRotate.Vector3andSpace rotateDegreesPerSecond;

		public bool ignoreTimescale;

		private float m_LastRealTime;

		[Serializable]
		public class Vector3andSpace
		{
			public Vector3 value;

			public Space space = Space.Self;
		}
	}
}
