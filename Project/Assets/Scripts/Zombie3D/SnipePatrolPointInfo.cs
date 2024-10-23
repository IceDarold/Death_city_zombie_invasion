using System;
using UnityEngine;

namespace Zombie3D
{
	[Serializable]
	public class SnipePatrolPointInfo : MonoBehaviour
	{
		public void Reset()
		{
			this.tempWait = this.waitTime;
		}

		public bool InWait()
		{
			return this.tempWait > 0f;
		}

		public void DoLogic(float deltaTime)
		{
			this.tempWait -= deltaTime;
		}

		[CNName("等待动作")]
		public string waitAction;

		[CNName("等待时间")]
		public float waitTime;

		[CNName("切换动画名 / 逃脱动作")]
		public string patrolAnimationName;

		[CNName("移动速度 / 逃脱速度")]
		public float speed;

		[CNName("下一点")]
		public Transform nextPoint;

		[CNName("逃脱点")]
		public bool isEscape;

		[Space]
		[CNName("消失并检测任务")]
		public bool removeAndDetectMission;

		[Header("测试用")]
		public float tempWait;
	}
}
