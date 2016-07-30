using UnityEngine;

///No, not that Mario. Port of LibGDX code for a Box2D chracter controller.
///http://www.badlogicgames.com/wordpress/?p=2017
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class MarioController : MonoBehaviour {
	[SerializeField] float maxVelocity = 7;
	[SerializeField] float speed = 2;
	[SerializeField] float jumpForce = 30;
	public LayerMask platformMask = 0;

	bool jump = false;
	Rigidbody2D player;
	BoxCollider2D playerPhysicsFixture;
	Collider2D playerSensorFixture;
	float stillTime = 0;
	float lastGroundTime = 0;

	void Start() {
		player = GetComponent<Rigidbody2D>();
		playerPhysicsFixture = GetComponent<BoxCollider2D>();
	}

	void Update() {
		Vector2 vel = player.velocity;

		if (Input.GetKeyDown(KeyCode.W)) jump = true;
		else if (Input.GetKeyUp(KeyCode.W)) jump = false;

		bool grounded = CheckGrounded();
		if (grounded) lastGroundTime = Time.time;
		else if (Time.time - lastGroundTime < 0.1) grounded = true;

		// cap max velocity on x
		if (Mathf.Abs(vel.x) > maxVelocity) {
			vel.x = Mathf.Sign(vel.x) * maxVelocity;
			player.velocity = vel;
		}

		bool pressingKeys = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);
		// calculate stillTime and damping
		if (!pressingKeys) {
			stillTime += Time.deltaTime;

			Vector2 dampenedVel = new Vector2(vel.x * 0.9f, vel.y);
			player.velocity = dampenedVel;
		} else {
			stillTime = 0;
		}

		// disable friction while jumping
		if (!grounded) {
			playerPhysicsFixture.sharedMaterial.friction = 0;
		} else {
			if (!pressingKeys && stillTime > 0.2) 
				playerPhysicsFixture.sharedMaterial.friction = 100;
			else 
				playerPhysicsFixture.sharedMaterial.friction = 0.2f;
		}

		// apply left impulse, but only if max velocity is not reached yet
		if (Input.GetKey(KeyCode.A) && vel.x > -maxVelocity) 
			player.AddForce(Vector2.right * -speed, ForceMode2D.Impulse);
		
		// apply right impluse, but only if max velocity is not reached yet
		if (Input.GetKey(KeyCode.D) && vel.x < maxVelocity) 
			player.AddForce(Vector2.right * speed, ForceMode2D.Impulse);

		// jump, but only when grounded
		if (jump) {
			jump = false;
			if (grounded) {
				player.velocity = new Vector2(vel.x, 0);
				Debug.Log("jump before: " + player.velocity);

				player.position += Vector2.up * 0.01f;
				player.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
				Debug.Log("jump, " + player.velocity);
			}
		}
	}

	//Check if the player is on the ground
	bool CheckGrounded() {
		Vector2 startPos = new Vector2(
			transform.position.x, playerPhysicsFixture.bounds.min.y);
		RaycastHit2D hit = Physics2D.Raycast(startPos, Vector2.up * -1, 
			0.15f, platformMask);
		return hit? true : false;
	}
}