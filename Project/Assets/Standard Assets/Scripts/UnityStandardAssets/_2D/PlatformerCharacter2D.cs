using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
	public class PlatformerCharacter2D : MonoBehaviour
	{
		private void Awake()
		{
			this.m_GroundCheck = base.transform.Find("GroundCheck");
			this.m_CeilingCheck = base.transform.Find("CeilingCheck");
			this.m_Anim = base.GetComponent<Animator>();
			this.m_Rigidbody2D = base.GetComponent<Rigidbody2D>();
		}

		private void FixedUpdate()
		{
			this.m_Grounded = false;
			Collider2D[] array = Physics2D.OverlapCircleAll(this.m_GroundCheck.position, 0.2f, this.m_WhatIsGround);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].gameObject != base.gameObject)
				{
					this.m_Grounded = true;
				}
			}
			this.m_Anim.SetBool("Ground", this.m_Grounded);
			this.m_Anim.SetFloat("vSpeed", this.m_Rigidbody2D.velocity.y);
		}

		public void Move(float move, bool crouch, bool jump)
		{
			if (!crouch && this.m_Anim.GetBool("Crouch") && Physics2D.OverlapCircle(this.m_CeilingCheck.position, 0.01f, this.m_WhatIsGround))
			{
				crouch = true;
			}
			this.m_Anim.SetBool("Crouch", crouch);
			if (this.m_Grounded || this.m_AirControl)
			{
				move = ((!crouch) ? move : (move * this.m_CrouchSpeed));
				this.m_Anim.SetFloat("Speed", Mathf.Abs(move));
				this.m_Rigidbody2D.velocity = new Vector2(move * this.m_MaxSpeed, this.m_Rigidbody2D.velocity.y);
				if (move > 0f && !this.m_FacingRight)
				{
					this.Flip();
				}
				else if (move < 0f && this.m_FacingRight)
				{
					this.Flip();
				}
			}
			if (this.m_Grounded && jump && this.m_Anim.GetBool("Ground"))
			{
				this.m_Grounded = false;
				this.m_Anim.SetBool("Ground", false);
				this.m_Rigidbody2D.AddForce(new Vector2(0f, this.m_JumpForce));
			}
		}

		private void Flip()
		{
			this.m_FacingRight = !this.m_FacingRight;
			Vector3 localScale = base.transform.localScale;
			localScale.x *= -1f;
			base.transform.localScale = localScale;
		}

		[SerializeField]
		private float m_MaxSpeed = 10f;

		[SerializeField]
		private float m_JumpForce = 400f;

		[Range(0f, 1f)]
		[SerializeField]
		private float m_CrouchSpeed = 0.36f;

		[SerializeField]
		private bool m_AirControl;

		[SerializeField]
		private LayerMask m_WhatIsGround;

		private Transform m_GroundCheck;

		private const float k_GroundedRadius = 0.2f;

		private bool m_Grounded;

		private Transform m_CeilingCheck;

		private const float k_CeilingRadius = 0.01f;

		private Animator m_Anim;

		private Rigidbody2D m_Rigidbody2D;

		private bool m_FacingRight = true;
	}
}
