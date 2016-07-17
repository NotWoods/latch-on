using UnityEngine;
using UnityEngine.SceneManagement;

namespace Level {
	public class Goal : MonoBehaviour {
		public Scene level;
		int levelIndex {
			get { 
				if (level != null) {
					return level.buildIndex; 
				} else {
					Scene current = SceneManager.GetActiveScene();
					int nextLevel = current.buildIndex + 1;
					if (nextLevel >= SceneManager.sceneCount) nextLevel = 0;
					return nextLevel;
				}
			}
		}

		/// Time before the goal activates the next level
		public float delaySeconds = 1;
		/// Remaining time before the goal activates
		public float secondsRemaining {get; protected set;}

		/// Ensures the collider is a player that can trigger the timer, 
		/// and not some box that fell into the goal area.
		protected virtual bool IsValidTrigger(Collider2D player) {
			return true;
		}

		void Activate() {
			SceneManager.LoadScene(levelIndex);
		}

		void OnTriggerStay2D(Collider2D player) {
			if (!IsValidTrigger(player)) return;
			if (secondsRemaining <= 0) {
				Activate();
			} else {
				secondsRemaining -= Time.deltaTime;
			}
		}

		void OnTriggerEnter2D(Collider2D player) {
			if (!IsValidTrigger(player)) return;
			secondsRemaining = delaySeconds;
		}
	}
}