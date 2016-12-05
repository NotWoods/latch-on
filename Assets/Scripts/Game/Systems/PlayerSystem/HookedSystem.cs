using UnityEngine;
using Prime31;

namespace PlayerSystem {
	public class HookedSystem : SystemBase<HookedSystem>, IPlayerSystem {
		public void OnEntry(int id) {
			InputComponent input = Manager.GetComponent<InputComponent>(id);
		}

		public void Update(int id, float deltaTime) {
			PlayerStateComponent state = Manager.GetComponent<PlayerStateComponent>(id);
			InputComponent input = Manager.GetComponent<InputComponent>(id);
			if (!input.HookDown) {
				state.SetTo(PlayerState.StandardMovement);
				return;
			}

			CharacterStatsComponent stats = Manager.GetComponent<CharacterStatsComponent>(id);
			CharacterController2D controller = Manager.GetComponent<CharacterController2D>(id);
		}

		public void OnExit(int id) {
			InputComponent input = Manager.GetComponent<InputComponent>(id);
			LineComponent line = Manager.GetComponent<LineComponent>(id);
		}
	}
}