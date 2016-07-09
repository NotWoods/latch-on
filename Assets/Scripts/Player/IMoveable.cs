using UnityEngine;

namespace Player {
	public interface IMoveable {
		GameObject gameObject {get;}

		// Causes the player to jump. Returns false if there is no floor.
		bool Jump();

		// Move the player right (positive velocity) or left.
		void Drive(float velocity);
	}
}