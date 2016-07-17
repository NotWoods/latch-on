using UnityEngine;

namespace Rope {
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(SpringJoint2D))]
	public class GrappleController : MonoBehaviour, IRope {
		public Vector2 tetherPoint;
		public float maxGrappleRange = 10;
		public bool isTethered = false;

		public LayerMask tetherablePlatformMask;

		[HideInInspector]
		public new Rigidbody2D rigidbody;
		public Vector2 centerOfMass = Vector2.up * -0.5f;
		public float rotation {
			get {return GetZRotation();}
		}

		public SpringJoint2D springJoint {get; private set;}

		void Awake() {
			rigidbody = GetComponent<Rigidbody2D>();
			springJoint = GetComponent<SpringJoint2D>();

			rigidbody.centerOfMass = centerOfMass;
			springJoint.enabled = false;
			springJoint.anchor = Vector2.zero;
		}

		float GetZRotation() {
			if (isTethered) {
				return Vector2.Angle(tetherPoint, transform.position);
			} else {
				return 0;
			}
		}	

		void OnDrawGizmos() {
			if (isTethered) {
				Gizmos.color = Color.cyan;
				Gizmos.DrawLine(tetherPoint, transform.position);
			}
		}

		void OnDrawGizmosSelected() {
			if (isTethered) {
				Gizmos.color = Color.gray;
				Gizmos.DrawSphere(tetherPoint, 0.3f);

				Gizmos.color = Color.magenta;
				Gizmos.DrawWireSphere(tetherPoint, springJoint.distance);
			}
		}

		public bool GrappleToward(Vector2 toPoint) {
			Vector2 direction = (toPoint - (Vector2) transform.position).normalized;
			RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 
				maxGrappleRange, tetherablePlatformMask);
			if (hit) {

				LinkTo(hit.point);
				rigidbody.isKinematic = false;
				return isTethered;

			} else return false;
		}

		public void Detatch() {
			springJoint.enabled = isTethered = false;
		}

		public void LinkTo(Vector2 point) {
			springJoint.connectedBody = null;
			tetherPoint = springJoint.connectedAnchor = point;
			springJoint.distance = Vector2.Distance(transform.position, point);
			springJoint.enabled = isTethered = true;
		}
	}
}