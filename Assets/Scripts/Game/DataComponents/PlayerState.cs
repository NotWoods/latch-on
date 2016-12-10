using UnityEngine;

[DisallowMultipleComponent]
public class PlayerState : MonoBehaviour, IDataComponent {
	public Mode CurrentMode;

	public void Set(Mode m) { CurrentMode = m; }

	/// Checks if the current state is the same as any of the given options.
	public bool Any(params Mode[] states) {
		for (int i = 0; i < states.Length; i++) {
			if (CurrentMode == states[i]) return true;
		}

		return false;
	}

	public enum Mode { Walk, Swing, Flung, Fall, WallSlide }
	public static readonly Mode Walk = Mode.Walk;
	public static readonly Mode Swing = Mode.Swing;
	public static readonly Mode Flung = Mode.Flung;
	public static readonly Mode Fall = Mode.Fall;
	public static readonly Mode WallSlide = Mode.WallSlide;
}
