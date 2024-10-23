using System;
using RootMotion.FinalIK;
using UnityEngine;

namespace RootMotion.Demos
{
	public class UserControlInteractions : UserControlThirdPerson
	{
		protected override void Update()
		{
			if (this.disableInputInInteraction && this.interactionSystem != null && (this.interactionSystem.inInteraction || this.interactionSystem.IsPaused()))
			{
				float minActiveProgress = this.interactionSystem.GetMinActiveProgress();
				if (minActiveProgress > 0f && minActiveProgress < this.enableInputAtProgress)
				{
					this.state.move = Vector3.zero;
					this.state.jump = false;
					return;
				}
			}
			base.Update();
		}

		private void OnGUI()
		{
			if (!this.character.onGround)
			{
				return;
			}
			if (this.interactionSystem.IsPaused() && this.interactionSystem.IsInSync())
			{
				GUILayout.Label("Press E to resume interaction", new GUILayoutOption[0]);
				if (UnityEngine.Input.GetKey(KeyCode.E))
				{
					this.interactionSystem.ResumeAll();
				}
				return;
			}
			int closestTriggerIndex = this.interactionSystem.GetClosestTriggerIndex();
			if (closestTriggerIndex == -1)
			{
				return;
			}
			if (!this.interactionSystem.TriggerEffectorsReady(closestTriggerIndex))
			{
				return;
			}
			GUILayout.Label("Press E to start interaction", new GUILayoutOption[0]);
			if (UnityEngine.Input.GetKey(KeyCode.E))
			{
				this.interactionSystem.TriggerInteraction(closestTriggerIndex, false);
			}
		}

		[SerializeField]
		private CharacterThirdPerson character;

		[SerializeField]
		private InteractionSystem interactionSystem;

		[SerializeField]
		private bool disableInputInInteraction = true;

		public float enableInputAtProgress = 0.8f;
	}
}
