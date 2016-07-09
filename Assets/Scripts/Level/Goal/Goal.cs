using UnityEngine;
using UnityEngine.SceneManagement;

namespace Level {
	[RequireComponent (typeof (Collider2D))]
	public class Goal : MonoBehaviour, IGoal {
		[SerializeField]
		private Scene level;
		public int levelIndex {
			get { return level.buildIndex; }
		}

		/// Time before the goal activates the next level
		public float delaySeconds {get; set;}
		/// Remaining time before the goal activates
		public float secondsRemaining {get; protected set;}

		/// Ensures the collider is a player that can trigger the timer, 
		/// and not some box that fell into the goal area.
		bool IsValidTrigger(Collider2D player) {
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