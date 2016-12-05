public enum PlayerState {
	Idle,   // not moving
	Walk,   // running along the ground
	Jump,   // jumping up
	Hooked, // swinging around on a grappling hook
	Soar,   // flying after releasing from grappling hook
	Fall    // falling downward
}

public class PlayerStateComponent : IComponent {
	public PlayerState CurrentState { get; private set; }

	public bool SetTo(PlayerState newState) {
		// TODO verification
		CurrentState = newState;
		return true;
	}
}
