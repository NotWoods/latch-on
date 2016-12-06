using UnityEngine;
using Prime31;

namespace PlayerSystem {
	public class HookedSystem : Singleton<HookedSystem>, IPlayerSystem {
		public bool OnEntry(
			Transform transform, CharacterData s, InputData input, LineData line
		) {
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

		public void Update(
			PlayerStateData state,
			Transform transform,
			CharacterData stats,
			InputData input,
			LineData line,
			CharacterController2D controller
		) {
			if (!input.HookDown) {
				state.SetTo(PlayerState.StandardMovement);
				return;
			}
		}

		public void OnExit(
			Transform transform,
			CharacterData stats,
			InputData input,
			LineData line
		) {
			line.ClearPoints();
		}
	}
}
