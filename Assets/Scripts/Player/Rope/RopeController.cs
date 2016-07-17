using UnityEngine;

namespace Rope {
	public class RopeController : CharacterController2Dd {
		[HideInInspector]
		public Vector2 tetherPoint;
		[HideInInspector]
		public float desiredTetherLength;
		[HideInInspector]
		public float currentTetherLength;

		public float maxTetherLength = 10;

		public LayerMask grapplePlatformMask = 0;

		public float tetherChangeRate = 1;

		[HideInInspector]
		public bool iAmTethered = false;

		public Quaternion rotation;

		void Update() {
			if (iAmTethered) {
				Debug.DrawLine(transform.position, tetherPoint, Color.green);
			}
		}

		void OnDrawGizmosSelected() {
			if (iAmTethered) {
				Gizmos.color = Color.magenta;
				Gizmos.DrawRay(tetherPoint, 
					(transform.position - (Vector3) tetherPoint).normalized 
					* desiredTetherLength);
				
				Gizmos.color = Color.gray;
				Gizmos.DrawSphere(tetherPoint, 0.5f);

				Gizmos.color = Color.white;
				Gizmos.DrawWireSphere(tetherPoint, currentTetherLength);
			}
		}

		void FixedUpdate() {
			/*if (iAmTethered) {
				desiredTetherLength = Vector2.Distance(transform.position, tetherPoint);
				//currentTetherLength = Mathf.MoveTowards(currentTetherLength, 
				//	desiredTetherLength, tetherChangeRate);
				currentTetherLength = desiredTetherLength;
				rotation = transform.rotation; ///TODO
			} else {
				rotation = transform.rotation;
			}*/
		}

		void DebugDrawCross(Vector2 center, float size = 1) {
			Debug.DrawLine(
				center - (Vector2.right * size),
				center + (Vector2.right * size)
			);
			Debug.DrawLine(
				center - (Vector2.up * size),
				center + (Vector2.up * size)
			);
		}

		public override CollisionFlags Move(Vector2 deltaMovement) {
			MoveCalculation(ref deltaMovement);

			if (iAmTethered) {
				Vector2 testPosition = (Vector2) transform.position + deltaMovement;
				if (Vector2.Distance(testPosition, tetherPoint) > currentTetherLength) {
					testPosition = (testPosition - tetherPoint).normalized 
						* currentTetherLength + tetherPoint;
					deltaMovement = testPosition - (Vector2) transform.position;
				}
			}

			transform.Translate(deltaMovement);
			return collisionFlags;
		}

		public bool GrappleToward(Vector2 towardPoint) {
			RaycastHit2D hit = Raycast2D.towardsPoint(
				transform.position, towardPoint,
				maxTetherLength, grapplePlatformMask
			);
			if (hit) {
				tetherPoint = hit.point;
				desiredTetherLength = Vector2.Distance(transform.position, tetherPoint);
				currentTetherLength = desiredTetherLength;
				return iAmTethered = true;
			} else return false;
		}
	}
}