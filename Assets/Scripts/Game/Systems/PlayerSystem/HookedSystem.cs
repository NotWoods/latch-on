using UnityEngine;
using Prime31;

namespace PlayerSystem {
	public class HookedSystem : SystemBase<HookedSystem>, IPlayerSystem {
		public bool OnEntry(int id) {
			InputComponent input = Manager.GetComponent<InputComponent>(id);
			LineComponent line = Manager.GetComponent<LineComponent>(id);
			Transform transform = Manager.GetUnityComponent<Transform>(id);

			RaycastHit2D hit = Physics2D.Raycast(
				transform.position,
				input.PointerDir,
				line.StartingLength,
				line.NormalGround
			);

			if (hit) {
				Vector2 hitPoint = hit.point;
				line.SetAnchor(hitPoint);
				return true;
			} else {
				return false;
			}
		}

		public void Update(int id, float deltaTime) {
			PlayerStateComponent state = Manager.GetComponent<PlayerStateComponent>(id);
			InputComponent input = Manager.GetComponent<InputComponent>(id);
			if (!input.HookDown) {
				state.SetTo(PlayerState.StandardMovement);
				return;
			}

			CharacterStatsComponent stats = Manager.GetComponent<CharacterStatsComponent>(id);
			CharacterController2D controller = Manager.GetUnityComponent<CharacterController2D>(id);
			Transform transform = Manager.GetUnityComponent<Transform>(id);
		}

		public void OnExit(int id) {
			InputComponent input = Manager.GetComponent<InputComponent>(id);
			LineComponent line = Manager.GetComponent<LineComponent>(id);

			line.ClearPoints();
		}
	}
}
