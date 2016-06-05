using UnityEngine;

namespace Level {
	[System.Serializable]
	public class Rect : Platform {
		BoxCollider2D AddCollider(GameObject gameObject) {
			BoxCollider2D collider = 
				gameObject.AddComponent("BoxCollider2D") as BoxCollider2D;
			
			//Path should start in top right and go clockwise
			collider.size.y = Vector2.Distance(path[0], path[1]);
			collider.size.x = Vector2.Distance(path[1], path[2]);
			
			return collider;
		}
	}
}