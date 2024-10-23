using System;
using RootMotion.FinalIK;
using UnityEngine;

namespace RootMotion.Demos
{
	public class OffsetEffector : OffsetModifier
	{
		protected override void Start()
		{
			base.Start();
			foreach (OffsetEffector.EffectorLink effectorLink in this.effectorLinks)
			{
				effectorLink.localPosition = base.transform.InverseTransformPoint(this.ik.solver.GetEffector(effectorLink.effectorType).bone.position);
				if (effectorLink.effectorType == FullBodyBipedEffector.Body)
				{
					this.ik.solver.bodyEffector.effectChildNodes = false;
				}
			}
		}

		protected override void OnModifyOffset()
		{
			foreach (OffsetEffector.EffectorLink effectorLink in this.effectorLinks)
			{
				Vector3 a = base.transform.TransformPoint(effectorLink.localPosition);
				this.ik.solver.GetEffector(effectorLink.effectorType).positionOffset += (a - (this.ik.solver.GetEffector(effectorLink.effectorType).bone.position + this.ik.solver.GetEffector(effectorLink.effectorType).positionOffset)) * this.weight * effectorLink.weightMultiplier;
			}
		}

		public OffsetEffector.EffectorLink[] effectorLinks;

		[Serializable]
		public class EffectorLink
		{
			public FullBodyBipedEffector effectorType;

			public float weightMultiplier = 1f;

			[HideInInspector]
			public Vector3 localPosition;
		}
	}
}
