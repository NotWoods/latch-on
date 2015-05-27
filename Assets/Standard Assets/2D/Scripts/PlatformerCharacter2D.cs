using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
        [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
        [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
        [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
		private Transform m_GroundCheckLeft;
		private Transform m_GroundCheckRight;
        const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        [HideInInspector]
		public bool m_Grounded;            // Whether or not the player is grounded.
		[HideInInspector]
		public bool m_Tilted;
        //private Transform m_CeilingCheck;   // A position marking where to check for ceilings
        //const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Animator m_Anim;            // Reference to the player's animator component.
        private Rigidbody2D m_Rigidbody2D;
		[HideInInspector]
        public bool m_FacingRight = true;  // For determining which way the player is currently facing.
		private Transform visual;
		private Vector3 desiredVisual = Vector3.up * 180;
		public float turnSpeed = 100;

        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            //m_CeilingCheck = transform.Find("CeilingCheck");
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
			m_GroundCheckLeft = transform.Find("GroundCheck Left");
			m_GroundCheckRight = transform.Find("GroundCheck Right");
			visual = transform.Find("robot");
        }

        private void FixedUpdate()
        {
            m_Grounded = false;
			m_Tilted = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
			Collider2D[] colliders;
			bool sideways = false;
			/*if ((transform.eulerAngles.z > 80 && transform.eulerAngles.z < 180 && m_FacingRight) || 
			    (transform.eulerAngles.z < 280 && transform.eulerAngles.z > 180 && !m_FacingRight)) {
				colliders = Physics2D.OverlapCircleAll(m_GroundCheckLeft.position, 
				                                       	k_GroundedRadius * 0.75f, m_WhatIsGround);
				sideways = true;
			} else if ((transform.eulerAngles.z < 280 && transform.eulerAngles.z > 180 && m_FacingRight) || 
			           (transform.eulerAngles.z > 80 && transform.eulerAngles.z < 180 && !m_FacingRight)) {
				colliders = Physics2D.OverlapCircleAll(m_GroundCheckRight.position, 
				                                       	k_GroundedRadius * 0.75f, m_WhatIsGround);
				sideways = true;
			} else {
				colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
			}*/
			if (transform.eulerAngles.z > 80 && transform.eulerAngles.z < 180) {
				colliders = Physics2D.OverlapCircleAll(m_GroundCheckLeft.position, 
				                                       k_GroundedRadius * 0.75f, m_WhatIsGround);
				sideways = true;
			} else if (transform.eulerAngles.z < 280 && transform.eulerAngles.z > 180) {
				colliders = Physics2D.OverlapCircleAll(m_GroundCheckRight.position, 
				                                       k_GroundedRadius * 0.75f, m_WhatIsGround);
				sideways = true;
			} else {
				colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
			}
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
				if (!sideways) {m_Grounded = true;} else {m_Tilted = true;}
            }
            m_Anim.SetBool("Ground", m_Grounded);
			m_Anim.SetBool ("Tilted", m_Tilted);

            // Set the vertical animation
            m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
        }


        public void Move(float move, bool crouch, bool jump)
        {
            // If crouching, check to see if the character can stand up
            /*if (!crouch && m_Anim.GetBool("Crouch"))
            {
                // If the character has a ceiling preventing them from standing up, keep them crouching
                if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
                {
                    crouch = true;
                }
            }*/

            // Set whether or not the character is crouching in the animator
            //m_Anim.SetBool("Crouch", crouch);

            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl)
            {
                // Reduce the speed if crouching by the crouchSpeed multiplier
                move = (crouch ? move*m_CrouchSpeed : move);

                // The Speed animator parameter is set to the absolute value of the horizontal input.
                m_Anim.SetFloat("Speed", Mathf.Abs(move));

                // Move the character
                m_Rigidbody2D.velocity = new Vector2(move*m_MaxSpeed, m_Rigidbody2D.velocity.y);

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                    // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
            }
            // If the player should jump...
            if (m_Grounded && jump && m_Anim.GetBool("Ground"))
            {
                // Add a vertical force to the player.
                m_Grounded = false;
                m_Anim.SetBool("Ground", false);
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }
        }


        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = visual.localScale;
            theScale.x *= -1;
			visual.localScale = theScale;
			/*float angle = 180f;
			if (!m_FacingRight) {angle = 0f;}
			visual.rotation = Quaternion.Euler(0f, angle, 0f);*/
        }

		/*private void Update() {
			//visual.rotation = Quaternion.Euler(visual.rotation.eulerAngles.x, Mathf.MoveTowards(visual.rotation.eulerAngles.y, desiredVisual.y, Time.deltaTime * turnSpeed), visual.rotation.eulerAngles.z);
		}*/
    }
}
