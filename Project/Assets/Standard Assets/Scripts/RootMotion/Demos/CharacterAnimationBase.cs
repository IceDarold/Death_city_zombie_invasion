using System;
using UnityEngine;

namespace RootMotion.Demos
{
	public abstract class CharacterAnimationBase : MonoBehaviour
	{
		public virtual Vector3 GetPivotPoint()
		{
			return base.transform.position;
		}

		public virtual bool animationGrounded
		{
			get
			{
				return true;
			}
		}

		public float GetAngleFromForward(Vector3 worldDirection)
		{
			Vector3 vector = base.transform.InverseTransformDirection(worldDirection);
			return Mathf.Atan2(vector.x, vector.z) * 57.29578f;
		}

		protected virtual void Start()
		{
			if (base.transform.parent.GetComponent<CharacterBase>() == null)
			{
				UnityEngine.Debug.LogWarning("Animation controllers should be parented to character controllers!", base.transform);
			}
			this.lastPosition = base.transform.position;
			this.localPosition = base.transform.parent.InverseTransformPoint(base.transform.position);
			this.lastRotation = base.transform.rotation;
			this.localRotation = Quaternion.Inverse(base.transform.parent.rotation) * base.transform.rotation;
		}

		protected virtual void LateUpdate()
		{
			if (this.smoothFollow)
			{
				base.transform.position = Vector3.Lerp(this.lastPosition, base.transform.parent.TransformPoint(this.localPosition), Time.deltaTime * this.smoothFollowSpeed);
				base.transform.rotation = Quaternion.Lerp(this.lastRotation, base.transform.parent.rotation * this.localRotation, Time.deltaTime * this.smoothFollowSpeed);
			}
			this.lastPosition = base.transform.position;
			this.lastRotation = base.transform.rotation;
		}

		public bool smoothFollow = true;

		public float smoothFollowSpeed = 20f;

		private Vector3 lastPosition;

		private Vector3 localPosition;

		private Quaternion localRotation;

		private Quaternion lastRotation;
	}
}
