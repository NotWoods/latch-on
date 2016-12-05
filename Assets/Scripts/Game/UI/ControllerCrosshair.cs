using UnityEngine;

namespace UI {
	[RequireComponent(typeof(Canvas))]
	public class ControllerCrosshair : Crosshair {
		public float offsetDistance = 2.5f;

		///Draw a cursor that orbits the player
		public void FromInputAxis(Vector2 towardsPoint) {
			Vector2 offset = towardsPoint.normalized * offsetDistance;
			if (transform.parent == trackedObject)
				transform.position = offset;
			else
				transform.position = (Vector2) trackedObject.position + offset;
		}
	}
}