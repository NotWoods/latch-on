using UnityEngine;

namespace Player {
	public class Shooting : MonoBehaviour, IShoot {
		IRope rope;

		public float maxRange = 10f;
		public int isGrapplableLayers;

		public float ropeLength {	get {return rope.length;}	}

		void Start() {
			rope.OnBreak = () => isConnected = false;
		}

		public bool ShootToward(Vector2 towardsPoint, Vector2 fromPosition) {
			RaycastHit2D hit = Raycast2D.towardsPoint(
				fromPosition, towardsPoint,
				maxRange, isGrapplableLayers
			);
			if (hit.collider != null) {
				if (hit.rigidbody != null) rope.LinkTo(hit.point, hit.rigidbody);
				else rope.LinkTo(hit.point);
				return isConnected = true;
			} else return false;
		}

		public bool ShootToward(Vector2 towardsPoint) {
			return ShootToward(towardsPoint, transform.position);
		}

		public bool Retract(float velocity) {
			return false;
		}

		public bool isConnected {get; protected set;}
	}
}