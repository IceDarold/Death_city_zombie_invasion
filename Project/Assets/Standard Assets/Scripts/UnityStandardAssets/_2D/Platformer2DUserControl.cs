using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
	[RequireComponent(typeof(PlatformerCharacter2D))]
	public class Platformer2DUserControl : MonoBehaviour
	{
		private void Awake()
		{
			this.m_Character = base.GetComponent<PlatformerCharacter2D>();
		}

		private void Update()
		{
			if (!this.m_Jump)
			{
				this.m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
			}
		}

		private void FixedUpdate()
		{
			bool key = UnityEngine.Input.GetKey(KeyCode.LeftControl);
			float axis = CrossPlatformInputManager.GetAxis("Horizontal");
			this.m_Character.Move(axis, key, this.m_Jump);
			this.m_Jump = false;
		}

		private PlatformerCharacter2D m_Character;

		private bool m_Jump;
	}
}
