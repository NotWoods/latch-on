using UnityEngine;
using Player;
using Util;

namespace Rope {
	[RequireComponent(typeof(DistanceJoint2D))]
	public class GrappleController : PlayerController, ITether {
		public LayerMask grapplePlatformMask = 0;
		///Pads the space between the raycast hit and platform
		public float padWidth = 0.1f;
		public Vector2 centerOfMass = Vector2.up * -0.5f;

		///How close the player must be to the tether connection point
		public float maxTetherRange = 20;
		///When the rope wraps around something, if the player is too close
		///to the object according to this distance, the rope will break.
		public float tooCloseDistance = 0.1f;
		///Shortens rope distance by this amount every second.
		public float autoRetractSpeed = 0.1f;
		///Cannot retract past this length.
		public float minRopeLength = 0.1f;

		[HideInInspector]
		public DistanceJoint2D rope;
		float padRadius = 0;

		void Start() {
			rope = GetComponent<DistanceJoint2D>();
			
			if (centerOfMass != Vector2.zero)	rigidbody.centerOfMass = centerOfMass;
			padRadius = Vector2.Distance(
				collider.bounds.center, collider.bounds.min
			);
		}

		public bool isTethered {get {return rope.enabled;}}
		public Vector2 tetherPoint {get {return rope.connectedAnchor;}}

		public override void Respawn() {
			base.Respawn();
			Break(false);
		}

		public void Break(bool playNoise) {
			rope.enabled = false;
		}
		public void Break() { Break(true); }

		///Tries to connect toward the given point. Returns true if connected.
		public bool GrappleToward(Vector2 point) {
			RaycastHit2D hit = Physics2D.Raycast(
				transform.position, point - (Vector2) transform.position,
				maxTetherRange,	grapplePlatformMask
			);

			if (hit) LinkTo(hit.point);

			return false;
		}

		void DebugCross(Vector2 point, Color color, float length = 0.5f) {
			Debug.DrawLine(point - Vector2.right * length, 
				point + Vector2.right * length, color, 2);
			Debug.DrawLine(point - Vector2.up * length, 
				point + Vector2.up * length, color, 2);
		}

		void DebugCross(Vector2 point, float length = 0.5f) {
			DebugCross(point, Color.red, length);
		}

		///TODO
		///Reduce the initial grappling length by the returned length.
		///Used so that the rope pulls you up from the ground rather than
		///sliding along the floor.
		float CalculateDistanceShrink(Vector2 tetherPoint) {
			/*float dist = Vector2.Distance(transform.position, tetherPoint);
			float testRadius = dist	+ padRadius;
			Vector2 bottomPoint = tetherPoint + (Vector2.up * testRadius * -1);
			float bottomAngle = 270;

			float angleDiff = Vector2.Angle(transform.position, bottomPoint);
			bool leftOfTetherPoint = transform.position.x < tetherPoint.x;

			if (leftOfTetherPoint) {
				float startAngle = bottomAngle - angleDiff;
				for (float i = startAngle; i < bottomAngle; i+=5) {
					Vector2 testPoint = 
						Circle.PointOnCircumference(i * Mathf.Deg2Rad, testRadius, tetherPoint);
					Debug.Log(testPoint);
					DebugCross(testPoint);
					Collider2D collided = Physics2D.OverlapPoint(testPoint, platformMask);
					if (collided) 
						testRadius -= 1;
				}
			} else {
				float startAngle = bottomAngle + angleDiff;
				for (float i = startAngle; i > bottomAngle; i-=5) {
					Vector2 testPoint = 
						Circle.PointOnCircumference(i * Mathf.Deg2Rad, testRadius, tetherPoint);
					if (i == startAngle) DebugCross(testPoint, Color.blue);
					DebugCross(testPoint);
					Collider2D collided = Physics2D.OverlapPoint(testPoint, platformMask);
					if (collided) 
						testRadius -= 1;
				}	
			}
			
			return testRadius - padRadius;*/
			return Vector2.Distance(transform.position, tetherPoint);
		}

		public void LinkTo(Vector2 worldPoint, Vector2 objectCenter) {
			float paddingX = padWidth * Mathf.Sign(worldPoint.x - objectCenter.x);
			float paddingY = padWidth * Mathf.Sign(worldPoint.y - objectCenter.y);
			LinkTo(worldPoint + new Vector2(paddingX, paddingY));
		}

		public void LinkTo(Vector2 point) {
			rope.connectedAnchor = point;
			rope.distance = CalculateDistanceShrink(point);

			rope.enabled = true;
		}

		///Raycasts along the rope to check if something is in the way.
		void CheckRopeIntegrity() {
			RaycastHit2D hit = Physics2D.Raycast(
				transform.position,
				tetherPoint - (Vector2) transform.position,
				Vector2.Distance(tetherPoint, transform.position) - padWidth,
				platformMask
			);

			if (hit) {
				bool isTooClose = 
					Vector2.Distance(hit.point, transform.position) < tooCloseDistance;
				bool wrappedAroundRigidbody =
					hit.rigidbody != null && !hit.rigidbody.isKinematic;
				
				if (isTooClose || wrappedAroundRigidbody) Break();
				else WrapAround(hit);
			}
		}

		void WrapAround(RaycastHit2D hit) {
			int hitLayer = hit.transform.gameObject.layer;
			bool isInGrappleLayer = (grapplePlatformMask.value & (1 << hitLayer)) > 0;

			if (isInGrappleLayer) {
				bool dontWrap = hitLayer == LayerMask.NameToLayer("GrappleLock");

				if (!dontWrap && Input.GetMouseButton(0)) {
					LinkTo(hit.point, hit.transform.position);
				}
			}
		}

		void FixedUpdate() {
			if (isTethered) {
				float newRopeLength = rope.distance - autoRetractSpeed * Time.deltaTime;

				if (newRopeLength < minRopeLength) newRopeLength = minRopeLength;

				rope.distance = newRopeLength;
				CheckRopeIntegrity();
			}

			bool notMovingFast = rigidbody.velocity.magnitude < 1;
			bool groundedOrTitled = false;//controller.grounded;// || transform.rotation.z != 0;
			if (!isTethered && notMovingFast && groundedOrTitled) {
				if (transform.rotation.z != 0) transform.position += Vector3.up;

				Vector2 facing = Vector2.right; ///TODO
				transform.rotation = Quaternion.LookRotation(facing, Vector2.up);
			}
		}

		protected override void Update() {
			base.Update();
			if (Input.GetKeyDown(KeyCode.R)) Respawn();
			if (Input.GetMouseButtonDown(0) && !isTethered) {
				Vector2 clickPoint = 
					Camera.main.ScreenToWorldPoint(Input.mousePosition);
				GrappleToward(clickPoint);
			} else if (Input.GetMouseButtonUp(0)) Break(isTethered);
		}

		void OnDrawGizmosSelected() {
			if (!rope) return;

			if (isTethered) {
				Gizmos.color = Color.white;
				Gizmos.DrawWireSphere(rope.connectedAnchor, rope.distance);
			}

			RaycastHit2D previewHit = Physics2D.Raycast(
				transform.position, 
				Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position,
				maxTetherRange,	grapplePlatformMask
			);
			if (previewHit) {
				Gizmos.color = Color.black;
				Gizmos.DrawWireSphere(previewHit.point, 1);
			}
		}
	}
}