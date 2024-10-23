using System;
using UnityEngine;

namespace RacingMode
{
	public class RacingPlotAction : MonoBehaviour
	{
		private void OnEnable()
		{
			this.ActionState = 0;
		}

		public void DoAction()
		{
			this.ActionState = 1;
			if (this.Type == RacingPlotType.Action)
			{
				if (this.PlotAnimator != null)
				{
					this.PlotAnimator.Play(this.ActionName);
				}
			}
			else if (this.Type == RacingPlotType.Dialogue)
			{
				Singleton<UiManager>.Instance.ShowDialogue(2, this.DialogueTag, 0);
			}
			else if (this.Type == RacingPlotType.Visible)
			{
				this.TargetObject.SetActive(this.isVisible);
			}
		}

		private void EndAction()
		{
			this.ActionState = 2;
			if (this.Type == RacingPlotType.Action && !this.isCamera && this.PlotAnimator != null)
			{
				this.PlotAnimator.Play("Idle");
			}
		}

		private void Update()
		{
			if (this.ActionState == 0 && RacingPlot.Instace.PlotTimer > this.StartTime)
			{
				this.DoAction();
			}
			if (this.ActionState == 1 && RacingPlot.Instace.PlotTimer > this.EndTime)
			{
				this.EndAction();
			}
		}

		[EnumLabel("类型")]
		public RacingPlotType Type;

		[Header("开始时间")]
		public float StartTime;

		[Header("结束时间")]
		public float EndTime;

		[Header("对话编号")]
		public int DialogueTag;

		[Header("动画控制器")]
		public Animator PlotAnimator;

		[Header("动画名称")]
		public string ActionName;

		[Header("是否是相机")]
		public bool isCamera;

		[Header("目标物体")]
		public GameObject TargetObject;

		[Header("显示状态")]
		public bool isVisible;

		[HideInInspector]
		public int ActionState;
	}
}
