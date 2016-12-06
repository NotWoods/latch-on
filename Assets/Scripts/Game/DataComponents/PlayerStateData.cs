using UnityEngine;

public enum PlayerState {
	StandardMovement,
	HookedMovement
}

[DisallowMultipleComponent]
public class PlayerStateData : MonoBehaviour, IDataComponent {
	public PlayerState CurrentState { get; private set; }

	public bool SetTo(PlayerState newState) {
		// TODO verification
		CurrentState = newState;
		return true;
	}
}
