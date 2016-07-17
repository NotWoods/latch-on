using UnityEngine;

[RequireComponent (typeof(BoxCollider2D))]
public class CharacterController2Dd : MonoBehaviour {
	/// All layers that the chracter should collide with
	public LayerMask platformMask = 0;

	void Awake() {
		boxCollider = GetComponent<BoxCollider2D>();
		RecalculateRaySpacing();
	}

	[HideInInspector]
	public BoxCollider2D boxCollider;

	#region chracter controller based fields

	///The center of the character's collider relative to the transform's position
	public Vector2 center {
		get {return boxCollider.offset;}
		set {boxCollider.offset = value;}
	}

	///What part of the capsule collided with the enviornment during the 
	///last CharacterController2D.Move call
	public CollisionFlags collisionFlags;

	///Determines whether other rigidbodies or character controllers collide with
	///this character controller (by default this is always enabled)
	public bool detectCollision = true;

	///The height of the character's collider
	public float height {
		get {return boxCollider.size.y;}
		set {boxCollider.size = new Vector2(boxCollider.size.x, value);}
	}

	///Was the CharacterController touching the ground during the last move?
	public bool isGrounded = false;
	//get {return (collisionFlags & CollisionFlags.Below) != 0;}

	///The width of the character's collider
	public float width {
		get {return boxCollider.size.x;}
		set {boxCollider.size = new Vector2(value, boxCollider.size.y);}
	}

	///The character's collision skin width
	public float skinWidth = 0.08f;

	///The character controller's slope limit in degrees.
	public float slopeLimit = 45f;

	///The character controller's step offset in meters.
	public float stepOffset = 0;

	///The current relative velocity of the Character
	public Vector2 velocity;

	///A more complex move function taking absolute movement deltas
	///<param name="motion">Delta movement</param>
	protected CollisionFlags MoveCalculation(ref  Vector2 deltaMovement) {
		collisionFlags = CollisionFlags.None;

		if (deltaMovement.x != 0) MoveHorizontally(ref deltaMovement);
		if (deltaMovement.y != 0) MoveVertically(ref deltaMovement);

		// Only calculates velocity when frame has changed
		if (Time.deltaTime > 0) velocity = deltaMovement / Time.deltaTime;

		return collisionFlags;
	}

	public virtual CollisionFlags Move(Vector2 deltaMovement) {
		MoveCalculation(ref deltaMovement);
		transform.Translate(deltaMovement);
		return collisionFlags;
	}

	///Moves the character with speed
	public bool SimpleMove(Vector2 speed) {
		return false;
	}

	#endregion

	Bounds shrunkBounds;
	Bounds SkinWidthBounds() {
		Bounds scaledBounds = boxCollider.bounds;
		scaledBounds.Expand(-2 * skinWidth);

		return scaledBounds;
	}

	public int horizontalRayCount = 4;
	public int verticalRayCount = 4;
	protected Vector2 raySpacing;
	public void RecalculateRaySpacing() {
		Bounds scaledBounds = SkinWidthBounds();

		raySpacing = new Vector2(
			scaledBounds.size.x / (horizontalRayCount - 1),
			scaledBounds.size.y / (verticalRayCount - 1)
		);
	}

	float lastSlopeAngle;

	CollisionFlags MoveHorizontally(ref Vector2 deltaMovement) {
		Bounds b = SkinWidthBounds();

		float dirX = Mathf.Sign(deltaMovement.x);
		bool isGoingLeft = dirX == -1;

		Vector2 origin = isGoingLeft?
			(Vector2) b.min : new Vector2(b.max.x, b.min.y);
		float rayLength = Mathf.Abs(deltaMovement.x) + skinWidth;
		bool isClimbing = false;

		for (int i = 0; i < horizontalRayCount; i++) {
			if (i > 0) origin += Vector2.up * raySpacing.x;

			RaycastHit2D hit = Physics2D.Raycast(origin, 
				Vector2.right * dirX,
				rayLength, platformMask);
			if (hit) {
				float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

				if (i == 0 && slopeAngle <= slopeLimit) {
					float distanceToSlope = 0;
					if (slopeAngle != lastSlopeAngle) {
						distanceToSlope = hit.distance - skinWidth;
						deltaMovement.x -= distanceToSlope * dirX;
					}
					velocity.x += distanceToSlope * dirX;

					isClimbing = ClimbSlope(ref deltaMovement, slopeAngle);
				}
				if (!isClimbing || slopeAngle > slopeLimit) {
					deltaMovement.x = (hit.distance - skinWidth) * dirX;
					rayLength = hit.distance;
					collisionFlags |= CollisionFlags.Sides;
				}
			}

			//Debug.DrawRay(origin, Vector2.right * rayLength * dirX, Color.red);
		}

		return collisionFlags;
	}

	CollisionFlags MoveVertically(ref Vector2 deltaMovement) {
		Bounds b = SkinWidthBounds(); 

		float dirY = Mathf.Sign(deltaMovement.y);
		bool isGoingDown = dirY == -1;

		Vector2 origin = isGoingDown?
			(Vector2) b.min : new Vector2(b.min.x, b.max.y);
		float rayLength = Mathf.Abs(deltaMovement.y) + skinWidth;

		for (int i = 0; i < verticalRayCount; i++) {
			if (i > 0) origin += Vector2.right * raySpacing.y;

			RaycastHit2D hit = Physics2D.Raycast(origin, 
				Vector2.up * dirY, 
				rayLength, platformMask);
			if (hit) {
				deltaMovement.y = (hit.distance - skinWidth) * dirY;
				rayLength = hit.distance;
				collisionFlags |= isGoingDown? 
					CollisionFlags.Below : CollisionFlags.Above;
			}

			//Debug.DrawRay(origin, Vector2.up * rayLength * dirY, Color.red);
		}

		return collisionFlags;
	}

	bool ClimbSlope(ref Vector2 deltaMovement, float slopeAngle) {
		float moveDist = Mathf.Abs(deltaMovement.x);
		float climbDeltaY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDist;
		
		bool isJumping = deltaMovement.y > climbDeltaY;
		if (!isJumping) {
			lastSlopeAngle = slopeAngle;
			
			deltaMovement.y = climbDeltaY;
			deltaMovement.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDist 
				* Mathf.Sign(deltaMovement.x);
			collisionFlags |= CollisionFlags.Below;
			return true;
		} else return false;
	}
}