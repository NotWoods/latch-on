using UnityEngine;

public enum PlayerState {
	StandardMovement,
	HookedMovement
}

[DisallowMultipleComponent]
public class PlayerStateData : MonoBehaviour, IDataComponent {
	public PlayerState CurrentState;

	public bool SetTo(PlayerState newState) {
		CurrentState = newState;
		return true;
	}
}
