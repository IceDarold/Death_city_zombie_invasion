using System;
using RootMotion.FinalIK;
using UnityEngine;

namespace RootMotion.Demos
{
	public class ExplosionDemo : MonoBehaviour
	{
		private void Start()
		{
			this.defaultScale = base.transform.localScale;
			this.r = this.character.GetComponent<Rigidbody>();
			this.ik = this.character.GetComponent<FullBodyBipedIK>();
		}

		private void Update()
		{
			this.weight = Mathf.Clamp(this.weight - Time.deltaTime * this.weightFalloffSpeed, 0f, 1f);
			if (UnityEngine.Input.GetKeyDown(KeyCode.E) && this.character.isGrounded)
			{
				this.ik.solver.IKPositionWeight = 1f;
				this.ik.solver.leftHandEffector.position = this.ik.solver.leftHandEffector.bone.position;
				this.ik.solver.rightHandEffector.position = this.ik.solver.rightHandEffector.bone.position;
				this.ik.solver.leftFootEffector.position = this.ik.solver.leftFootEffector.bone.position;
				this.ik.solver.rightFootEffector.position = this.ik.solver.rightFootEffector.bone.position;
				this.weight = 1f;
				Vector3 vector = this.r.position - base.transform.position;
				vector.y = 0f;
				float d = this.explosionForceByDistance.Evaluate(vector.magnitude);
				this.r.velocity = (vector.normalized + Vector3.up * this.upForce) * d * this.forceMlp;
			}
			if (this.weight < 0.5f && this.character.isGrounded)
			{
				this.weight = Mathf.Clamp(this.weight - Time.deltaTime * 3f, 0f, 1f);
			}
			this.SetEffectorWeights(this.weightFalloff.Evaluate(this.weight));
			base.transform.localScale = this.scale.Evaluate(this.weight) * this.defaultScale;
		}

		private void SetEffectorWeights(float w)
		{
			this.ik.solver.leftHandEffector.positionWeight = w;
			this.ik.solver.rightHandEffector.positionWeight = w;
			this.ik.solver.leftFootEffector.positionWeight = w;
			this.ik.solver.rightFootEffector.positionWeight = w;
		}

		public SimpleLocomotion character;

		public float forceMlp = 1f;

		public float upForce = 1f;

		public float weightFalloffSpeed = 1f;

		public AnimationCurve weightFalloff;

		public AnimationCurve explosionForceByDistance;

		public AnimationCurve scale;

		private float weight;

		private Vector3 defaultScale = Vector3.one;

		private Rigidbody r;

		private FullBodyBipedIK ik;
	}
}
