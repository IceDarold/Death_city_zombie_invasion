using System;
using System.Collections.Generic;
using UnityEngine;

namespace RacingMode
{
	public class RacingTrackInfo : MonoBehaviour
	{
		public void Awake()
		{
			this.DeserializableBakedLightInfo();
			StaticBatchingUtility.Combine(base.gameObject);
		}

		private void FixedUpdate()
		{
			if ((RacingSceneManager.Instance.GameState == RacingState.RACING || RacingSceneManager.Instance.GameState == RacingState.GAME_OVER) && this.EndPoint.position.z < RacingSceneManager.Instance.Car.transform.position.z - 500f)
			{
				base.gameObject.SetActive(false);
			}
		}

		[ContextMenu("记录光照信息")]
		private void SerializableBakedLightInfo()
		{
			MeshRenderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<MeshRenderer>();
			foreach (MeshRenderer meshRenderer in componentsInChildren)
			{
				this.lightInfos.Add(new RacingModeLightInfo
				{
					renderer = meshRenderer,
					lightMapIndex = meshRenderer.lightmapIndex,
					lightMapScaleOffset = meshRenderer.lightmapScaleOffset
				});
			}
		}

		[ContextMenu("还原光照信息")]
		private void DeserializableBakedLightInfo()
		{
			base.gameObject.isStatic = true;
			foreach (RacingModeLightInfo racingModeLightInfo in this.lightInfos)
			{
				racingModeLightInfo.renderer.gameObject.isStatic = true;
				racingModeLightInfo.renderer.lightmapIndex = racingModeLightInfo.lightMapIndex;
				racingModeLightInfo.renderer.lightmapScaleOffset = racingModeLightInfo.lightMapScaleOffset;
			}
		}

		public Transform StartPoint;

		public Transform EndPoint;

		public List<RacingModeLightInfo> lightInfos = new List<RacingModeLightInfo>();
	}
}
