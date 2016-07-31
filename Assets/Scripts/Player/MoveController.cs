using UnityEngine;

namespace Player {
	public class MoveController : BaseController {
		[SerializeField] float maxSpeedX = 7;
		[SerializeField] float speed = 2;
		[SerializeField] float jumpForce = 10;
		[SerializeField] float bounciness = 0;
		[SerializeField] float snapTime = 0.2f;

		float stillTime = 0;	
		Vector2 velocity {
			get {return rigidbody.velocity;}
			set {rigidbody.velocity = value;}
		}	

		PhysicsMaterial2D physicsMat;
		void Start() {
			physicsMat = new PhysicsMaterial2D();
		}

		public virtual float friction {
			get {
				return collider.sharedMaterial.friction;
			}
			set {
				physicsMat.friction = value;

				collider.sharedMaterial = physicsMat;
				sensorCollider.sharedMaterial = physicsMat;
			}
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
			if (!isGrounded) friction = 0;	
			else {
				if (xInput == 0 && stillTime > snapTime) friction = 100;
				else friction = 0.2f;
			}

			// apply left impulse, but only if max velocity is not reached yet
			if (xInput < 0 && speedX < maxSpeedX) 
				rigidbody.AddForce(Vector2.right * -speed, ForceMode2D.Impulse);

			// apply right impluse, but only if max velocity is not reached yet
			if (xInput > 0 && speedX < maxSpeedX) 
				rigidbody.AddForce(Vector2.right * speed, ForceMode2D.Impulse);

			if (Input.GetButtonDown("Jump") && isGrounded) {
				velocity = new Vector2(velocity.x, 0);

				rigidbody.position += Vector2.up * 0.01f;
				rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
			} 
		}

		void OnDisable() {
			PhysicsMaterial2D physicsMat = new PhysicsMaterial2D();
			
			physicsMat.bounciness = bounciness;
			physicsMat.friction = 0.2f;

			collider.sharedMaterial = physicsMat;
			sensorCollider.sharedMaterial = physicsMat;
		}

		protected override void OnValidate() {
			base.OnValidate();
			if (physicsMat != null) physicsMat.bounciness = bounciness;
		}
	}
}