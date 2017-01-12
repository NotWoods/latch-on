using UnityEngine;

/// Current movement state of the entity
[DisallowMultipleComponent]
public class MoveState : MonoBehaviour {
	public enum Mode { Walk, Swing, Flung, Fall }
	public Mode Value;

	public static readonly Mode Walk = Mode.Walk;
	public static readonly Mode Swing = Mode.Swing;
	public static readonly Mode Flung = Mode.Flung;
	public static readonly Mode Fall = Mode.Fall;

	/// Checks if the current state is the same as any of the given options.
	public bool Any(params Mode[] states) {
		for (int i = 0; i < states.Length; i++) {
			if (Value == states[i]) return true;
		}

		return false;
	}
}