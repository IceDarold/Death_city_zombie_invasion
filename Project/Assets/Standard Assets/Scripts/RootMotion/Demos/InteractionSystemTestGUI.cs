using System;
using RootMotion.FinalIK;
using UnityEngine;

namespace RootMotion.Demos
{
	[RequireComponent(typeof(InteractionSystem))]
	public class InteractionSystemTestGUI : MonoBehaviour
	{
		private void Awake()
		{
			this.interactionSystem = base.GetComponent<InteractionSystem>();
		}

		private void OnGUI()
		{
			if (this.interactionSystem == null)
			{
				return;
			}
			if (GUILayout.Button("Start Interaction With " + this.interactionObject.name, new GUILayoutOption[0]))
			{
				if (this.effectors.Length == 0)
				{
					UnityEngine.Debug.Log("Please select the effectors to interact with.");
				}
				foreach (FullBodyBipedEffector effectorType in this.effectors)
				{
					this.interactionSystem.StartInteraction(effectorType, this.interactionObject, true);
				}
			}
			if (this.effectors.Length == 0)
			{
				return;
			}
			if (this.interactionSystem.IsPaused(this.effectors[0]) && GUILayout.Button("Resume Interaction With " + this.interactionObject.name, new GUILayoutOption[0]))
			{
				this.interactionSystem.ResumeAll();
			}
		}

		[Tooltip("The object to interact to")]
		[SerializeField]
		private InteractionObject interactionObject;

		[Tooltip("The effectors to interact with")]
		[SerializeField]
		private FullBodyBipedEffector[] effectors;

		private InteractionSystem interactionSystem;
	}
}
