using UnityEngine;

namespace Rope {
	public class Needle : MonoBehaviour {
		[HideInInspector]
		new SpriteRenderer renderer;

		public bool rendering {
			set {renderer.enabled = value;}
		}

		Transform loop;

		void Start() {
			renderer = transform.GetChild(0).gameObject
				.GetComponent<SpriteRenderer>();
			loop = transform.GetChild(1);
		}

		public Vector2 AttachTo(Vector2 tetherPoint, Vector2 direction) {
			transform.position = tetherPoint;
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
			return loop.position;
		}
	}
}