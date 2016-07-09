using UnityEngine;
using UnityEngine.SceneManagement;

namespace Level {
	public class Goal : MonoBehaviour, IGoal {
		[SerializeField]
		private Scene level;
		public int levelIndex {
			get { return level.buildIndex; }
		}

		public float delaySeconds {get; set;}
		public float secondsRemaining {get; protected set;}

		void OnTriggerStay2D(Collider2D player) {
			if (secondsRemaining <= 0) {
				SceneManager.LoadScene(levelIndex);
			} else {
				secondsRemaining -= Time.deltaTime;
			}
		}

		void OnTriggerEnter2D(Collider2D player) {
			secondsRemaining = delaySeconds;
		}
	}
}