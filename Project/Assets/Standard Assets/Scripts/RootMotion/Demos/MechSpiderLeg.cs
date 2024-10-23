using System;
using System.Collections;
using RootMotion.FinalIK;
using UnityEngine;

namespace RootMotion.Demos
{
	public class MechSpiderLeg : MonoBehaviour
	{
		public bool isStepping
		{
			get
			{
				return this.stepProgress < 1f;
			}
		}

		public Vector3 position
		{
			get
			{
				return this.ik.GetIKSolver().GetIKPosition();
			}
			set
			{
				this.ik.GetIKSolver().SetIKPosition(value);
			}
		}

		private void Start()
		{
			this.ik = base.GetComponent<IK>();
			this.stepProgress = 1f;
			this.hit = default(RaycastHit);
			IKSolver.Point[] points = this.ik.GetIKSolver().GetPoints();
			this.position = points[points.Length - 1].transform.position;
			this.hit.point = this.position;
			this.defaultPosition = this.mechSpider.transform.InverseTransformPoint(this.position + this.offset * this.mechSpider.scale);
		}

		private Vector3 GetStepTarget(out bool stepFound, float focus, float distance)
		{
			stepFound = false;
			Vector3 a = this.mechSpider.transform.TransformPoint(this.defaultPosition);
			a += (this.hit.point - this.position) * this.velocityPrediction;
			Vector3 vector = this.mechSpider.transform.up;
			Vector3 rhs = this.mechSpider.body.position - this.position;
			Vector3 axis = Vector3.Cross(vector, rhs);
			vector = Quaternion.AngleAxis(focus, axis) * vector;
			if (Physics.Raycast(a + vector * this.mechSpider.raycastHeight * this.mechSpider.scale, -vector, out this.hit, this.mechSpider.raycastHeight * this.mechSpider.scale + distance, this.mechSpider.raycastLayers))
			{
				stepFound = true;
			}
			return this.hit.point + this.mechSpider.transform.up * this.footHeight * this.mechSpider.scale;
		}

		private void Update()
		{
			if (this.isStepping)
			{
				return;
			}
			if (Time.time < this.lastStepTime + this.minDelay)
			{
				return;
			}
			if (this.unSync != null && this.unSync.isStepping)
			{
				return;
			}
			bool flag = false;
			Vector3 stepTarget = this.GetStepTarget(out flag, this.raycastFocus, this.mechSpider.raycastDistance * this.mechSpider.scale);
			if (!flag)
			{
				stepTarget = this.GetStepTarget(out flag, -this.raycastFocus, this.mechSpider.raycastDistance * 3f * this.mechSpider.scale);
			}
			if (!flag)
			{
				return;
			}
			if (Vector3.Distance(this.position, stepTarget) < this.maxOffset * this.mechSpider.scale * UnityEngine.Random.Range(0.9f, 1.2f))
			{
				return;
			}
			base.StopAllCoroutines();
			base.StartCoroutine(this.Step(this.position, stepTarget));
		}

		private IEnumerator Step(Vector3 stepStartPosition, Vector3 targetPosition)
		{
			this.stepProgress = 0f;
			while (this.stepProgress < 1f)
			{
				this.stepProgress += Time.deltaTime * this.stepSpeed;
				this.position = Vector3.Lerp(stepStartPosition, targetPosition, this.stepProgress);
				this.position += this.mechSpider.transform.up * this.yOffset.Evaluate(this.stepProgress) * this.mechSpider.scale;
				yield return null;
			}
			this.position = targetPosition;
			if (this.sand != null)
			{
				this.sand.transform.position = this.position - this.mechSpider.transform.up * this.footHeight * this.mechSpider.scale;
				this.sand.Emit(20);
			}
			this.lastStepTime = Time.time;
			yield break;
		}

		public MechSpider mechSpider;

		public MechSpiderLeg unSync;

		public Vector3 offset;

		public float minDelay = 0.2f;

		public float maxOffset = 1f;

		public float stepSpeed = 5f;

		public float footHeight = 0.15f;

		public float velocityPrediction = 0.2f;

		public float raycastFocus = 0.1f;

		public AnimationCurve yOffset;

		public ParticleSystem sand;

		private IK ik;

		private float stepProgress = 1f;

		private float lastStepTime;

		private Vector3 defaultPosition;

		private RaycastHit hit = default(RaycastHit);
	}
}
