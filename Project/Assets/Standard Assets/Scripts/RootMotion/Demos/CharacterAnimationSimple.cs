using System;
using UnityEngine;

namespace RootMotion.Demos
{
	[RequireComponent(typeof(Animator))]
	public class CharacterAnimationSimple : CharacterAnimationBase
	{
		protected override void Start()
		{
			base.Start();
			this.animator = base.GetComponentInChildren<Animator>();
		}

		public override Vector3 GetPivotPoint()
		{
			if (this.pivotOffset == 0f)
			{
				return base.transform.position;
			}
			return base.transform.position + base.transform.forward * this.pivotOffset;
		}

		private void Update()
		{
			float num = this.moveSpeed.Evaluate(this.characterController.animState.moveDirection.z);
			this.animator.SetFloat("Speed", num);
			this.characterController.Move(this.characterController.transform.forward * Time.deltaTime * num, Quaternion.identity);
		}

		[SerializeField]
		private CharacterThirdPerson characterController;

		[SerializeField]
		private float pivotOffset;

		[SerializeField]
		private AnimationCurve moveSpeed;

		private Animator animator;
	}
}
