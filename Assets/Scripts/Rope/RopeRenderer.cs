using UnityEngine;

namespace Rope {
	[RequireComponent(typeof(MeshRenderer))]
	public class RopeRenderer : MonoBehaviour {
		public GrappleController controller;
		Transform player;
		new MeshRenderer renderer;

		public Vector3 ropeSize = Vector3.one;
		public float offsetZ = 1;

		void Start() {
			renderer = GetComponent<MeshRenderer>();
			player = controller.gameObject.transform;
		}

		public void StretchTo(
			Vector2 startPoint, Vector2 endPoint, 
			bool mirrorZ = false
		) {
			Vector2 center = (startPoint + endPoint) / 2;
			transform.position = (Vector3) center + (Vector3.forward * offsetZ);

			Vector2 direction = (endPoint - startPoint).normalized;
			if (mirrorZ) direction *= -1;
			transform.right = direction;

			float scale = Vector2.Distance(startPoint, endPoint);
			transform.localScale = new Vector3(scale, ropeSize.y, ropeSize.z);
		}

		void LateUpdate() {
			if (controller.isTethered) {
				renderer.enabled = true;
				StretchTo(player.position, controller.tetherPoint);
			} else {
				renderer.enabled = false;
			}
		}
	}
}