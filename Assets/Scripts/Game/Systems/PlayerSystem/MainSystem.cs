using System;
using UnityEngine;
using Prime31;

namespace PlayerSystem {
	public interface IPlayerSystem {
		void Update(
			PlayerStateData stateContainer,
			Transform transform,
			CharacterData charStats,
			InputData input,
			LineData line,
			CharacterController2D controller
		);

		bool OnEntry(
			Transform transform, CharacterData stats, InputData input, LineData line
		);

		void OnExit(
			Transform transform, CharacterData stats, InputData input, LineData line
		);
	}

	public class Main : EgoSystem<
		PlayerStateData,
		Transform,
		CharacterData,
		InputData,
		LineData,
		CharacterController2D
	> {
		private IPlayerSystem GetInnerSystem(PlayerState state) {
			switch (state) {
				case PlayerState.StandardMovement: return MoveSystem.Instance;
				case PlayerState.HookedMovement: return HookedSystem.Instance;
				default:
					// throw new InvalidOperationException("TODO: Not yet implemented");
					throw new InvalidOperationException("Invalid PlayerState");
			}
		}

		public override void Update() {
			ForEachGameObject((
				egoComponent,
				stateContainer,
				transform,
				charStats,
				input,
				line,
				controller
			) => {
				GetInnerSystem(stateContainer.CurrentState)
					.Update(
						stateContainer,
						transform,
						charStats,
						input,
						line,
						controller
					);
			});
		}
	}
}
