using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RacingMode
{
	public class RacingPlot : MonoBehaviour
	{
		private void Awake()
		{
			RacingPlot.Instace = this;
		}

		public void ShowPlot(UnityAction _callback)
		{
			this.PlotTimer = 0f;
			this.isPlot = true;
			this.PlotCamera.gameObject.SetActive(true);
			this.ShowActions();
			this.CallBack = _callback;
		}

		private void ShowActions()
		{
			for (int i = 0; i < this.Actions.Count; i++)
			{
				this.Actions[i].gameObject.SetActive(true);
			}
		}

		private void Update()
		{
			if (this.isPlot)
			{
				if (this.PlotTimer < this.TotalTime)
				{
					this.PlotTimer += Time.deltaTime;
				}
				else
				{
					this.isPlot = false;
					this.PlotTimer = this.TotalTime;
					this.PlotCamera.gameObject.SetActive(false);
					this.CallBack();
					base.gameObject.SetActive(false);
				}
			}
		}

		public static RacingPlot Instace;

		public List<RacingPlotAction> Actions = new List<RacingPlotAction>();

		[Header("剧情相机")]
		public Camera PlotCamera;

		[Header("剧情时长")]
		public float TotalTime;

		[HideInInspector]
		public float PlotTimer;

		private UnityAction CallBack;

		private bool isPlot;
	}
}
