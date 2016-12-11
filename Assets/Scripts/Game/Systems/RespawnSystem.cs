using UnityEngine;
using Prime31;

public class RespawnSystem : EgoSystem<InputData, CharacterController2D, CharacterData> {
	public override void FixedUpdate() {
		ForEachGameObject((ego, input, controller, stats) => {
			if (input.ShouldRespawn) {
				controller.transform.position = Vector2.up * 2;
				controller.warpToGrounded();
				stats.Velocity = Vector2.zero;
				input.ShouldRespawn = false;
			}
		});
	}
}
