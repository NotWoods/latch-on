using UnityEngine;
using Prime31;

namespace PlayerSystem {
	public class HookedSystem : SystemBase<HookedSystem>, IPlayerSystem {
		public void Update(int id, float deltaTime) {
			CharacterStatsComponent stats = Manager.GetComponent<CharacterStatsComponent>(id);
			InputComponent input = Manager.GetComponent<InputComponent>(id);
			CharacterController2D controller = Manager.GetComponent<CharacterController2D>(id);
			PlayerStateComponent state = Manager.GetComponent<PlayerStateComponent>(id);
		}
	}
}