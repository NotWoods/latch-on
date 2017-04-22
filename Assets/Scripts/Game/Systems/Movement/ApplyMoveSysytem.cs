using UnityEngine;
using Prime31;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Input;
using LatchOn.ECS.Components.Mover;

namespace LatchOn.ECS.Systems {
	/// Finalized move step
	public class ApplyMoveSystem : EgoSystem<
		Velocity, CharacterController2D, VJoystick, HasGravity
	>, IStep {
		public void Step(EgoComponent entity) {
			var bundle = _bundles[entity];
			Velocity vel = bundle.component1;
			CharacterController2D controller = bundle.component2;
			VJoystick input = bundle.component3;
			HasGravity hasGravity = bundle.component4;

			Vector2 velocity = vel.Value;
			if (velocity.y < -hasGravity.MaxFallSpeed)
				velocity.y = -hasGravity.MaxFallSpeed;

			controller.Move(velocity * Time.deltaTime);
			vel.Value = controller.velocity;

			input.ClearPressed();
		}

		public override void FixedUpdate() {
			ForEachGameObject((ego, vel, controller, input, hasGravity) => {
				Vector2 velocity = vel.Value;

				if (velocity.y < -hasGravity.MaxFallSpeed)
					velocity.y = -hasGravity.MaxFallSpeed;

				controller.Move(velocity * Time.deltaTime);
				vel.Value = controller.velocity;

				input.ClearPressed();
			});
		}
	}
}
