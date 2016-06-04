using UnityEngine;

namespace Level {
	[Serializable]
	public class Platform {
		public Vector2[] path;
		public bool isSolid = false;
		public float angle;
		
		PolygonCollider2D AddCollider(GameObject gameObject) {
			PolygonCollider2D collider = 
				gameObject.AddComponent("PolygonCollider2D") as PolygonCollider2D;
			collider.SetPath(0, path);
			return collider;
		}
	}
}