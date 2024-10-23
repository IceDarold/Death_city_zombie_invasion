using System;
using UnityEngine;

namespace RootMotion.Demos
{
	[RequireComponent(typeof(FPSAiming))]
	[RequireComponent(typeof(Animator))]
	public class FPSCharacter : MonoBehaviour
	{
		private void Start()
		{
			this.animator = base.GetComponent<Animator>();
			this.FPSAiming = base.GetComponent<FPSAiming>();
		}

		private void Update()
		{
			this.FPSAiming.sightWeight = Mathf.SmoothDamp(this.FPSAiming.sightWeight, (!Input.GetMouseButton(1)) ? 0f : 1f, ref this.sVel, 0.1f);
			if (this.FPSAiming.sightWeight < 0.001f)
			{
				this.FPSAiming.sightWeight = 0f;
			}
			if (this.FPSAiming.sightWeight > 0.999f)
			{
				this.FPSAiming.sightWeight = 1f;
			}
			this.animator.SetFloat("Speed", this.walkSpeed);
		}

		private void OnGUI()
		{
			GUI.Label(new Rect((float)(Screen.width - 210), 10f, 200f, 25f), "Hold RMB to aim down the sight");
		}

		[Range(0f, 1f)]
		public float walkSpeed = 0.5f;

		private float sVel;

		private Animator animator;

		private FPSAiming FPSAiming;
	}
}
