using UnityEngine;
using Prime31;
using Rope;

[RequireComponent(typeof(CharacterController2D))]
[RequireComponent(typeof(GrappleController))]
public class Movement : MonoBehaviour {
	public float gravity = -25f;
	public float runSpeed = 8f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpHeight = 3f;

	CharacterController2D charController;
	GrappleController grappleController;
	Vector2 velocity;

	bool moveControling = true;

	void Awake() {
		charController = GetComponent<CharacterController2D>();
		grappleController = GetComponent<GrappleController>();
	}

	public float HorizontalSpeed() {
		return Input.GetAxis("Horizontal");
	}

	public float Jump() {
		if (charController.isGrounded && Input.GetKeyDown(KeyCode.Space)) {
			return velocity.y = Mathf.Sqrt(2 * jumpHeight * -gravity);
		}
		return 0;
	}

	void MoveUpdate() {
		if (charController.isGrounded) velocity.y = 0;
		Jump();

		float smootedMovementFactor = 
			charController.isGrounded? groundDamping : inAirDamping;
		velocity.x = Mathf.Lerp(velocity.x, HorizontalSpeed() * runSpeed,
			Time.deltaTime * smootedMovementFactor);
		
		velocity.y += gravity * Time.deltaTime;

		charController.move(velocity * Time.deltaTime);
		velocity = charController.velocity;
	}

	void OnCollisonEnter2D(Collision2D collision) {
		if (!moveControling && !grappleController.isTethered) {
			moveControling = true;
		}
	}

	void GrappleUpdate() {
		if (Input.GetMouseButtonUp(0)) grappleController.Detatch();
	}

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			Vector2 click = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			moveControling = !grappleController.GrappleToward(click);
		}
		if (moveControling) MoveUpdate();
		else GrappleUpdate();
	}
}