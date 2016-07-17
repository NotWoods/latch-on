using UnityEngine;

namespace Player {
	public class PlatformerCharacter : MonoBehaviour {
				///The fastest the player can travel in the x axis.
				[SerializeField] float m_MaxSpeed = 10f; 
				///Amount of force added when the player jumps.
				[SerializeField] float m_JumpForce = 400f;                  
				/// Amount of maxSpeed applied to crouching movement. 1 = 100%
				[Range(0, 1)] [SerializeField] float m_CrouchSpeed = .36f;  
				/// Whether or not a player can steer while jumping;
				[SerializeField] bool m_AirControl = false;                 
				/// A mask determining what is ground to the character
				[SerializeField] LayerMask m_WhatIsGround;                  

				/// A position marking where to check if the player is grounded.
				Vector2 groundCheck;
				/// Radius of the overlap circle to determine if grounded
				const float k_GroundedRadius = .2f; 
				/// Whether or not the player is grounded.
				bool m_Grounded;            
				public bool grounded {get {return m_Grounded;}}
				/// A position marking where to check for ceilings
				Vector2 ceilingCheck;   
				/// Radius of the overlap circle to determine if the player can stand up
				const float k_CeilingRadius = .01f; 
				/// Reference to the player's animator component.
				//Animator m_Anim;            
				Rigidbody2D m_Rigidbody2D;
				new Collider2D collider;
				/// For determining which way the player is currently facing.
				bool m_FacingRight = true;  
				public bool facingRight {get {return m_FacingRight;}}

				/// Returns true when the object is too tilted for the normal ground sensor to activate
				public bool tiltedGrounded = false;

				void Awake() {
						// Setting up references.
						//m_Anim = GetComponent<Animator>();
						m_Rigidbody2D = GetComponent<Rigidbody2D>();
						collider = GetComponent<Collider2D>();
				}

				void OnDrawGizmosSelected() {
					Gizmos.color = new Color(1, 0.64f, 0);
					if (!m_Grounded)
						Gizmos.DrawWireSphere(groundCheck, k_GroundedRadius);
					else 
						Gizmos.DrawSphere(groundCheck, k_GroundedRadius);

					Gizmos.DrawWireSphere(ceilingCheck, k_CeilingRadius);
				}

				protected bool CheckForGround() {
					groundCheck = 
						new Vector2(collider.bounds.center.x, collider.bounds.min.y);
					ceilingCheck = 
						new Vector2(groundCheck.x, collider.bounds.max.y);

					m_Grounded = false;
					Collider2D col = Physics2D.OverlapCircle(
						groundCheck, k_GroundedRadius, m_WhatIsGround
					);
					if (col) return m_Grounded = true;
					else return false;
				}


				protected virtual void FixedUpdate() {
					 	CheckForGround();

						//m_Anim.SetBool("Ground", m_Grounded);
						// Set the vertical animation
						//m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
				}


				public void Move(float move, bool crouch, bool jump) {
						// If crouching, check to see if the character can stand up
						if (!crouch && false) {// && m_Anim.GetBool("Crouch")) {
							// If the character has a ceiling preventing them from standing up, keep them crouching
							if (Physics2D.OverlapCircle(
									ceilingCheck, k_CeilingRadius, m_WhatIsGround)
							) crouch = true;
						}

						// Set whether or not the character is crouching in the animator
						//m_Anim.SetBool("Crouch", crouch);

						//only control the player if grounded or airControl is turned on
						if (m_Grounded || m_AirControl)
						{
								// Reduce the speed if crouching by the crouchSpeed multiplier
								move = (crouch ? move*m_CrouchSpeed : move);

								// The Speed animator parameter is set to the absolute value of the horizontal input.
								//m_Anim.SetFloat("Speed", Mathf.Abs(move));

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
						if (m_Grounded && jump) // && m_Anim.GetBool("Ground"))
						{
								// Add a vertical force to the player.
								m_Grounded = false;
								//m_Anim.SetBool("Ground", false);
								m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
						}
				}


				void Flip() {
						// Switch the way the player is labelled as facing.
						m_FacingRight = !m_FacingRight;

						// Multiply the player's x local scale by -1.
						Vector3 theScale = transform.localScale;
						theScale.x *= -1;
						transform.localScale = theScale;
				}
	}
}
