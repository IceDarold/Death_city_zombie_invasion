using System;
using RootMotion.FinalIK;
using UnityEngine;

namespace RootMotion.Demos
{
	public class AnimationWarping : OffsetModifier
	{
		protected override void Start()
		{
			base.Start();
			this.lastMode = this.effectorMode;
		}

		public float GetWarpWeight(int warpIndex)
		{
			if (warpIndex < 0)
			{
				UnityEngine.Debug.LogError("Warp index out of range.");
				return 0f;
			}
			if (warpIndex >= this.warps.Length)
			{
				UnityEngine.Debug.LogError("Warp index out of range.");
				return 0f;
			}
			if (this.animator == null)
			{
				UnityEngine.Debug.LogError("Animator unassigned in AnimationWarping");
				return 0f;
			}
			AnimatorStateInfo currentAnimatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(this.warps[warpIndex].animationLayer);
			if (!currentAnimatorStateInfo.IsName(this.warps[warpIndex].animationState))
			{
				return 0f;
			}
			return this.warps[warpIndex].weightCurve.Evaluate(currentAnimatorStateInfo.normalizedTime - (float)((int)currentAnimatorStateInfo.normalizedTime));
		}

		protected override void OnModifyOffset()
		{
			for (int i = 0; i < this.warps.Length; i++)
			{
				float warpWeight = this.GetWarpWeight(i);
				Vector3 vector = this.warps[i].warpTo.position - this.warps[i].warpFrom.position;
				AnimationWarping.EffectorMode effectorMode = this.effectorMode;
				if (effectorMode != AnimationWarping.EffectorMode.PositionOffset)
				{
					if (effectorMode == AnimationWarping.EffectorMode.Position)
					{
						this.ik.solver.GetEffector(this.warps[i].effector).position = this.ik.solver.GetEffector(this.warps[i].effector).bone.position + vector;
						this.ik.solver.GetEffector(this.warps[i].effector).positionWeight = this.weight * warpWeight;
					}
				}
				else
				{
					this.ik.solver.GetEffector(this.warps[i].effector).positionOffset += vector * warpWeight * this.weight;
				}
			}
			if (this.lastMode == AnimationWarping.EffectorMode.Position && this.effectorMode == AnimationWarping.EffectorMode.PositionOffset)
			{
				foreach (AnimationWarping.Warp warp in this.warps)
				{
					this.ik.solver.GetEffector(warp.effector).positionWeight = 0f;
				}
			}
			this.lastMode = this.effectorMode;
		}

		private void OnDisable()
		{
			if (this.effectorMode != AnimationWarping.EffectorMode.Position)
			{
				return;
			}
			foreach (AnimationWarping.Warp warp in this.warps)
			{
				this.ik.solver.GetEffector(warp.effector).positionWeight = 0f;
			}
		}

		[Tooltip("Reference to the Animator component to use")]
		public Animator animator;

		[Tooltip("Using effector.positionOffset or effector.position with effector.positionWeight? The former will enable you to use effector.position for other things, the latter will weigh in the effectors, hence using Reach and Pull in the process.")]
		public AnimationWarping.EffectorMode effectorMode;

		[Space(10f)]
		[Tooltip("The array of warps, can have multiple simultaneous warps.")]
		public AnimationWarping.Warp[] warps;

		private AnimationWarping.EffectorMode lastMode;

		[Serializable]
		public struct Warp
		{
			[Tooltip("Layer of the 'Animation State' in the Animator.")]
			public int animationLayer;

			[Tooltip("Name of the state in the Animator to warp.")]
			public string animationState;

			[Tooltip("Warping weight by normalized time of the animation state.")]
			public AnimationCurve weightCurve;

			[Tooltip("Animated point to warp from. This should be in character space so keep this Transform parented to the root of the character.")]
			public Transform warpFrom;

			[Tooltip("World space point to warp to.")]
			public Transform warpTo;

			[Tooltip("Which FBBIK effector to use?")]
			public FullBodyBipedEffector effector;
		}

		[Serializable]
		public enum EffectorMode
		{
			PositionOffset,
			Position
		}
	}
}
