namespace PlayerSystem {
	public class Main : SystemBase {
		public void Update(int id, float deltaTime) {
			PlayerStateComponent state = manager.GetComponent<PlayerStateComponent>(id);
			switch (state.CurrentState) {
				case PlayerState.Walk: WalkSystem.Instance.Update(id, deltaTime); break;
			}
		}
	}
}

public enum PlayerStateCopy {
	Idle,   // not moving
	Walk,   // running along the ground
	Jump,   // jumping up
	Hooked, // swinging around on a grappling hook
	Soar,   // flying after releasing from grappling hook
	Fall    // falling downward
}
