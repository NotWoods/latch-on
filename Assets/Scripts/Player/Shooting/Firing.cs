using UnityEngine;

namespace Player {
	public class Firing : MonoBehaviour {
		IShoot player;

		void Start() {
			player = GetComponent<IShoot>();
		}

		void Update() {
			bool trigger = Input.GetMouseButtonDown(0);
			if (trigger) {
				Vector2 mousePos = Input.mousePosition;
				Vector2 pos = Camera.main.ScreenToWorldPoint(mousePos);

				player.ShootToward(pos);
			}

			float retraction = Input.GetAxis("Retraction");
			if (retraction != 0) player.Retract(retraction);
		}
	}
}