using UnityEngine;

[DisallowMultipleComponent]
public class PlayerState : MonoBehaviour, IDataComponent {
	public Mode CurrentMode;

	/// Short alias for current mode.
	public Mode E {
		get { return CurrentMode; }
		set { CurrentMode = value; }
	}

	public void Set(Mode m) { E = m; }

	public enum Mode { Walk, Swing, Flung, Fall }
	public static Mode Walk = Mode.Walk;
	public static Mode Swing = Mode.Swing;
	public static Mode Flung = Mode.Flung;
	public static Mode Fall = Mode.Fall;
}
