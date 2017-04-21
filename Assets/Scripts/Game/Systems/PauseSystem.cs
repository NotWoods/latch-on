using UnityEngine;

namespace LatchOn.ECS.Systems {
	public class PauseSystem : EgoSystem {
		float lastTimeScale = 1;
		KeyCode pauseKey = KeyCode.Escape;

		public static bool Paused { get { return Time.timeScale == 0; } }

		public override void Update() {
			if (Input.GetKeyDown(pauseKey)) {
				if (Paused) Time.timeScale = lastTimeScale;
				else {
					lastTimeScale = Time.timeScale;
					Time.timeScale = 0;
				}
			}
		}
	}
}
