using UnityEngine;

[DisallowMultipleComponent]
public class PlayerState : MonoBehaviour, IDataComponent {
	public enum Mode { Walk, Swing, Flung, Fall }
	public Mode CurrentMode;

	/// Short alias for current mode.
	public Mode E {
		get { return CurrentMode; }
		set { CurrentMode = value; }
	}

	public void Set(Mode m) { E = m; }
}
