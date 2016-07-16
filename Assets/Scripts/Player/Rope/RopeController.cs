using UnityEngine;

namespace Rope {
	public class RopeController : CharacterController2D {
		public Vector2 tetherPoint;
		public float desiredTetherLength;
		public float currentTetherLength;

		public float tetherChangeRate = 1;

		public bool iAmTethered = false;

		public Quaternion rotation;

		void FixedUpdate() {
			if (iAmTethered) {
				desiredTetherLength = Vector2.Distance(transform.position, tetherPoint);
				currentTetherLength = Mathf.MoveTowards(currentTetherLength, 
					desiredTetherLength, tetherChangeRate);
			} else {
				rotation = transform.rotation;
			}
		}

		public override CollisionFlags Move(Vector2 deltaMovement) {
			MoveCalculation(ref deltaMovement);

			if (iAmTethered) {
				Vector2 testPosition = (Vector2) transform.position + deltaMovement;
				if (Vector2.Distance(testPosition, tetherPoint) > currentTetherLength) {
					testPosition = (testPosition - tetherPoint).normalized 
						* currentTetherLength;
					deltaMovement = testPosition - (Vector2) transform.position;
				}
			}

			transform.Translate(deltaMovement);
			return collisionFlags;
		}
	}
}