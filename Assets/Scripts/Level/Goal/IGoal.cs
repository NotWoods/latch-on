namespace Level {
	public interface IGoal {
		/// The level this goal proceeds to
		int levelIndex {get;}

		// How long the player must touch the goal
		float delaySeconds {get;}

		// Time remaining. If equal to delaySeconds, the player
		// hasn't triggered the goal.
		float secondsRemaining {get;}
	}
}