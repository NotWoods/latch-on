namespace LatchOn {
	/// Represents a side as an int, which can also be used with
	/// multiplication and Mathf.Side
	public enum Side {
		Left = -1,
		None = 0,
		Right = 1
	}

	/// State machine for current movement mode.
	/// An entity doesn't nessecarily use all of these.
	public enum MoveType {
		Walk,
		Swing,
		Flung,
		Fall,
		Dive,
	}
}
