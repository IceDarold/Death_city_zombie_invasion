using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
	public class CameraFollow : MonoBehaviour
	{
		private void Awake()
		{
			this.m_Player = GameObject.FindGameObjectWithTag("Player").transform;
		}

		private bool CheckXMargin()
		{
			return Mathf.Abs(base.transform.position.x - this.m_Player.position.x) > this.xMargin;
		}

		private bool CheckYMargin()
		{
			return Mathf.Abs(base.transform.position.y - this.m_Player.position.y) > this.yMargin;
		}

		private void Update()
		{
			this.TrackPlayer();
		}

		private void TrackPlayer()
		{
			float num = base.transform.position.x;
			float num2 = base.transform.position.y;
			if (this.CheckXMargin())
			{
				num = Mathf.Lerp(base.transform.position.x, this.m_Player.position.x, this.xSmooth * Time.deltaTime);
			}
			if (this.CheckYMargin())
			{
				num2 = Mathf.Lerp(base.transform.position.y, this.m_Player.position.y, this.ySmooth * Time.deltaTime);
			}
			num = Mathf.Clamp(num, this.minXAndY.x, this.maxXAndY.x);
			num2 = Mathf.Clamp(num2, this.minXAndY.y, this.maxXAndY.y);
			base.transform.position = new Vector3(num, num2, base.transform.position.z);
		}

		public float xMargin = 1f;

		public float yMargin = 1f;

		public float xSmooth = 8f;

		public float ySmooth = 8f;

		public Vector2 maxXAndY;

		public Vector2 minXAndY;

		private Transform m_Player;
	}
}
