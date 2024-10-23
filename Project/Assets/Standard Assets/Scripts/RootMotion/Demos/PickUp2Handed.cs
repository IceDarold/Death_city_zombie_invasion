using System;
using RootMotion.FinalIK;
using UnityEngine;

namespace RootMotion.Demos
{
	public abstract class PickUp2Handed : MonoBehaviour
	{
		private void OnGUI()
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Space((float)this.GUIspace);
			if (!this.holding)
			{
				if (GUILayout.Button("Pick Up " + this.obj.name, new GUILayoutOption[0]))
				{
					this.interactionSystem.StartInteraction(FullBodyBipedEffector.LeftHand, this.obj, false);
					this.interactionSystem.StartInteraction(FullBodyBipedEffector.RightHand, this.obj, false);
				}
			}
			else if (GUILayout.Button("Drop " + this.obj.name, new GUILayoutOption[0]))
			{
				this.interactionSystem.ResumeAll();
			}
			GUILayout.EndHorizontal();
		}

		protected abstract void RotatePivot();

		private void Start()
		{
			InteractionSystem interactionSystem = this.interactionSystem;
			interactionSystem.OnInteractionStart = (InteractionSystem.InteractionDelegate)Delegate.Combine(interactionSystem.OnInteractionStart, new InteractionSystem.InteractionDelegate(this.OnStart));
			InteractionSystem interactionSystem2 = this.interactionSystem;
			interactionSystem2.OnInteractionPause = (InteractionSystem.InteractionDelegate)Delegate.Combine(interactionSystem2.OnInteractionPause, new InteractionSystem.InteractionDelegate(this.OnPause));
			InteractionSystem interactionSystem3 = this.interactionSystem;
			interactionSystem3.OnInteractionResume = (InteractionSystem.InteractionDelegate)Delegate.Combine(interactionSystem3.OnInteractionResume, new InteractionSystem.InteractionDelegate(this.OnDrop));
		}

		private void OnPause(FullBodyBipedEffector effectorType, InteractionObject interactionObject)
		{
			if (effectorType != FullBodyBipedEffector.LeftHand)
			{
				return;
			}
			if (interactionObject != this.obj)
			{
				return;
			}
			this.obj.transform.parent = this.interactionSystem.transform;
			Rigidbody component = this.obj.GetComponent<Rigidbody>();
			if (component != null)
			{
				component.isKinematic = true;
			}
			this.pickUpPosition = this.obj.transform.position;
			this.pickUpRotation = this.obj.transform.rotation;
			this.holdWeight = 0f;
			this.holdWeightVel = 0f;
		}

		private void OnStart(FullBodyBipedEffector effectorType, InteractionObject interactionObject)
		{
			if (effectorType != FullBodyBipedEffector.LeftHand)
			{
				return;
			}
			if (interactionObject != this.obj)
			{
				return;
			}
			this.RotatePivot();
			this.holdPoint.rotation = this.obj.transform.rotation;
		}

		private void OnDrop(FullBodyBipedEffector effectorType, InteractionObject interactionObject)
		{
			if (effectorType != FullBodyBipedEffector.LeftHand)
			{
				return;
			}
			if (interactionObject != this.obj)
			{
				return;
			}
			this.obj.transform.parent = null;
			if (this.obj.GetComponent<Rigidbody>() != null)
			{
				this.obj.GetComponent<Rigidbody>().isKinematic = false;
			}
		}

		private void LateUpdate()
		{
			if (this.holding)
			{
				this.holdWeight = Mathf.SmoothDamp(this.holdWeight, 1f, ref this.holdWeightVel, this.pickUpTime);
				this.obj.transform.position = Vector3.Lerp(this.pickUpPosition, this.holdPoint.position, this.holdWeight);
				this.obj.transform.rotation = Quaternion.Lerp(this.pickUpRotation, this.holdPoint.rotation, this.holdWeight);
			}
		}

		private bool holding
		{
			get
			{
				return this.interactionSystem.IsPaused(FullBodyBipedEffector.LeftHand);
			}
		}

		private void OnDestroy()
		{
			if (this.interactionSystem == null)
			{
				return;
			}
			InteractionSystem interactionSystem = this.interactionSystem;
			interactionSystem.OnInteractionStart = (InteractionSystem.InteractionDelegate)Delegate.Remove(interactionSystem.OnInteractionStart, new InteractionSystem.InteractionDelegate(this.OnStart));
			InteractionSystem interactionSystem2 = this.interactionSystem;
			interactionSystem2.OnInteractionPause = (InteractionSystem.InteractionDelegate)Delegate.Remove(interactionSystem2.OnInteractionPause, new InteractionSystem.InteractionDelegate(this.OnPause));
			InteractionSystem interactionSystem3 = this.interactionSystem;
			interactionSystem3.OnInteractionResume = (InteractionSystem.InteractionDelegate)Delegate.Remove(interactionSystem3.OnInteractionResume, new InteractionSystem.InteractionDelegate(this.OnDrop));
		}

		[SerializeField]
		private int GUIspace;

		public InteractionSystem interactionSystem;

		public InteractionObject obj;

		public Transform pivot;

		public Transform holdPoint;

		public float pickUpTime = 0.3f;

		private float holdWeight;

		private float holdWeightVel;

		private Vector3 pickUpPosition;

		private Quaternion pickUpRotation;
	}
}
