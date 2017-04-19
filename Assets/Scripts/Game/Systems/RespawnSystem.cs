using UnityEngine;
using Prime31;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Input;

namespace LatchOn.ECS.Systems {
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
}
