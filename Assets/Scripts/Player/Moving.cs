using UnityEngine;
using System;

namespace Player {
	[RequireComponent (typeof(CircleCollider2D))]
	public class Moving : MonoBehaviour, IMoveable {
		public GameObject gameObject {get {return gameObject;}}

		public void Drive(float velocity) {
			transform.Translate(Vector2.right * velocity, Space.World);
		}

		public float skinWidth = 0.15f;
		Vector2[] raycastOrigins = new Vector2[3];
		float[] angles;
		CircleCollider2D circleCollider;

		public int rayCount {
			get {
				return raycastOrigins.Length;
			}
			set {
				Array.Resize(ref raycastOrigins, value);
				Array.Resize(ref angles, value);

				int valMinus1 = value - 1;
				for (int i = 0; i < value; i++) {
					angles[i] = Mathf.PI + ((i / valMinus1) * Mathf.PI);
				}
			}
		}

		void UpdateRaycastOrigins() {
			float radius = circleCollider.radius - skinWidth;
			Vector2 pos = 
				circleCollider.transform.TransformPoint(circleCollider.offset);

			for (int i = 0; i < rayCount; i++) {
				raycastOrigins[i] = Circle.PointOnCircumference(radius, angles[i], pos);
			}
		}

		public bool Jump() {
			return false;
		}

		void Awake() {
			circleCollider = gameObject.GetComponent<CircleCollider2D>();
			rayCount = rayCount;
		}
	}
}