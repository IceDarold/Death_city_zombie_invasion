using System;
using UnityEngine;

namespace RootMotion.Demos
{
	public class UserControlThirdPerson : MonoBehaviour
	{
		private void Start()
		{
			this.cam = Camera.main.transform;
		}

		protected virtual void Update()
		{
			this.state.crouch = (this.canCrouch && UnityEngine.Input.GetKey(KeyCode.C));
			this.state.jump = (this.canJump && Input.GetButton("Jump"));
			float axisRaw = UnityEngine.Input.GetAxisRaw("Horizontal");
			float axisRaw2 = UnityEngine.Input.GetAxisRaw("Vertical");
			Quaternion rotation = this.cam.rotation;
			Vector3 vector = new Vector3(axisRaw, 0f, axisRaw2);
			Vector3 vector2 = rotation * vector.normalized;
			if (vector2 != Vector3.zero)
			{
				Vector3 up = base.transform.up;
				Vector3.OrthoNormalize(ref up, ref vector2);
				this.state.move = vector2;
			}
			else
			{
				this.state.move = Vector3.zero;
			}
			bool key = UnityEngine.Input.GetKey(KeyCode.LeftShift);
			float d = (!this.walkByDefault) ? ((!key) ? 1f : 0.5f) : ((!key) ? 0.5f : 1f);
			this.state.move = this.state.move * d;
			this.state.lookPos = base.transform.position + this.cam.forward * 100f;
		}

		public bool walkByDefault;

		public bool canCrouch = true;

		public bool canJump = true;

		public UserControlThirdPerson.State state = default(UserControlThirdPerson.State);

		protected Transform cam;

		public struct State
		{
			public Vector3 move;

			public Vector3 lookPos;

			public bool crouch;

			public bool jump;

			public int actionIndex;
		}
	}
}
