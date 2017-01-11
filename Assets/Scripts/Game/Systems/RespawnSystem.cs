using UnityEngine;
using Prime31;

public class RespawnSystem : EgoSystem<VJoystick, CharacterController2D, Velocity> {
	public override void FixedUpdate() {
		ForEachGameObject((ego, input, controller, velocity) => {
			if (input.ShouldRespawn) {
				controller.transform.position = Vector2.up * 2;
				controller.warpToGrounded();
				velocity.Value = Vector2.zero;
				input.ShouldRespawn = false;
			}
		});
	}
}
