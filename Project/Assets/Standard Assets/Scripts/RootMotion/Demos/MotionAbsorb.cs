using System;
using System.Collections;
using RootMotion.FinalIK;
using UnityEngine;

namespace RootMotion.Demos
{
	public class MotionAbsorb : MonoBehaviour
	{
		private void OnCollisionEnter()
		{
			if (this.timer > 0f)
			{
				return;
			}
			base.StartCoroutine(this.AbsorbMotion());
		}

		private IEnumerator AbsorbMotion()
		{
			this.timer = 1f;
			for (int i = 0; i < this.absorbers.Length; i++)
			{
				this.absorbers[i].SetToBone(this.ik.solver);
			}
			while (this.timer > 0f)
			{
				this.timer -= Time.deltaTime * this.falloffSpeed;
				float w = this.falloff.Evaluate(this.timer);
				for (int j = 0; j < this.absorbers.Length; j++)
				{
					this.absorbers[j].SetEffectorWeights(this.ik.solver, w * this.weight);
				}
				yield return null;
			}
			yield return null;
			yield break;
		}

		[Tooltip("Reference to the FBBIK component")]
		public FullBodyBipedIK ik;

		[Tooltip("Array containing the absorbers")]
		public MotionAbsorb.Absorber[] absorbers;

		[Tooltip("The master weight")]
		public float weight = 1f;

		[Tooltip("Weight falloff curve (how fast will the effect reduce after impact)")]
		public AnimationCurve falloff;

		[Tooltip("How fast will the impact fade away. (if 1, effect lasts for 1 second)")]
		public float falloffSpeed = 1f;

		private float timer;

		[Serializable]
		public class Absorber
		{
			public void SetToBone(IKSolverFullBodyBiped solver)
			{
				solver.GetEffector(this.effector).position = solver.GetEffector(this.effector).bone.position;
				solver.GetEffector(this.effector).rotation = solver.GetEffector(this.effector).bone.rotation;
			}

			public void SetEffectorWeights(IKSolverFullBodyBiped solver, float w)
			{
				solver.GetEffector(this.effector).positionWeight = w * this.weight;
				solver.GetEffector(this.effector).rotationWeight = w * this.weight;
			}

			[Tooltip("The type of effector (hand, foot, shoulder...) - this is just an enum")]
			public FullBodyBipedEffector effector;

			[Tooltip("How much should motion be absorbed on this effector")]
			public float weight = 1f;
		}
	}
}
