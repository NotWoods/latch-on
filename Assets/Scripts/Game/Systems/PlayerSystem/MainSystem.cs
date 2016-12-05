using System;

namespace PlayerSystem {
	public interface IPlayerSystem {
		void Update(int id, float deltaTime);
	}

	public class Main : SystemBase<Main> {
		private IPlayerSystem GetInnerSystem(int playerId) {
			PlayerStateComponent state = Manager.GetComponent<PlayerStateComponent>(playerId);
			switch (state.CurrentState) {
				case PlayerState.StandardMovement: return MoveSystem.Instance;
				case PlayerState.HookedMovement: return HookedSystem.Instance;
				default:
					// throw new InvalidOperationException("TODO: Not yet implemented");
					throw new InvalidOperationException("Invalid PlayerState");
			}
		}

		public void Update(int id, float deltaTime) {
			GetInnerSystem(id).Update(id, deltaTime);
		}
	}
}
