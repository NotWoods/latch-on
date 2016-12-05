public enum PlayerState {
	StandardMovement,
	HookedMovement
}

public class PlayerStateComponent : IComponent {
	public PlayerState CurrentState { get; private set; }

	public bool SetTo(PlayerState newState) {
		// TODO verification
		CurrentState = newState;
		return true;
	}
}
