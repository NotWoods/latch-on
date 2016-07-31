using UnityEngine;

namespace Player {
	public class MoveController : BaseController {
		[SerializeField] float maxSpeedX = 7;
		[SerializeField] float speed = 2;
		[SerializeField] float jumpForce = 10;
		[SerializeField] float bounciness = 0;

		bool jump = false;
		float stillTime = 0;	
		Vector2 velocity {
			get {return rigidbody.velocity;}
			set {rigidbody.velocity = value;}
		}	

		void Update() {
			float speedX = Mathf.Abs(velocity.x);

			if (speedX > maxSpeedX) {
				velocity = new Vector2(Mathf.Sign(velocity.x) * maxSpeedX, velocity.y);
			}

			float xInput = Input.GetAxis("Horizontal");
			if (xInput == 0) {
				stillTime += Time.deltaTime;

				velocity = new Vector2(velocity.x * 0.9f, velocity.y);
			} else {
				stillTime = 0;
			}

			// disable friction while jumping
			PhysicsMaterial2D physicsMat = new PhysicsMaterial2D();
			physicsMat.bounciness = bounciness;
			if (!isGrounded) physicsMat.friction = 0;	
			else {
				if (xInput == 0 && stillTime > 0.2) 
					physicsMat.friction = 100;
				else 
					physicsMat.friction = 0.2f;
			}
			collider.sharedMaterial = physicsMat;
			sensorCollider.sharedMaterial = physicsMat;

			// apply left impulse, but only if max velocity is not reached yet
			if (xInput < 0 && speedX < maxSpeedX) 
				rigidbody.AddForce(Vector2.right * -speed, ForceMode2D.Impulse);

			// apply right impluse, but only if max velocity is not reached yet
			if (xInput > 0 && speedX < maxSpeedX) 
				rigidbody.AddForce(Vector2.right * speed, ForceMode2D.Impulse);
			
			
			if (Input.GetButtonDown("Jump")) jump = true;
			else if (Input.GetButtonUp("Jump")) jump = false;

			if (jump) {
				jump = false;
				if (isGrounded) {
					velocity = new Vector2(velocity.x, 0);
					Debug.Log("jump before: " + velocity);

					rigidbody.position += Vector2.up * 0.01f;
					rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
					Debug.Log("jump, " + velocity);
				}
			} 
		}

		void OnDisable() {
			PhysicsMaterial2D physicsMat = new PhysicsMaterial2D();
			
			physicsMat.bounciness = bounciness;
			physicsMat.friction = 0.2f;

			collider.sharedMaterial = physicsMat;
			sensorCollider.sharedMaterial = physicsMat;
		}
	}
}