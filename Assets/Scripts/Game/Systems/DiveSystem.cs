using UnityEngine;

/// Rudimentary diving system.
public class DiveSystem : EgoSystem<Velocity, MoveState, VJoystick> {
	Vector2 DivingVelocity = new Vector2(30, -10);

	public override void FixedUpdate() {
		ForEachGameObject((ego, velocity, state, input) => {
			if (state.Any(MoveState.Fall, MoveState.Flung)
			&& input.SinkPressed && CanDive(velocity.Value, ego)) {
				Debug.Log("Dive! Dive! Dive!");
				velocity.Value = new Vector2(
					Mathf.Sign(velocity.x) * DivingVelocity.x,
					DivingVelocity.y
				);
			}
		});
	}

	private bool CanDive(Vector2 velocity, EgoComponent ego) {
		float ySpeed = Mathf.Abs(velocity.y);
		if (ySpeed < 2) {
			WallJumper wallJumper;
			if (ego.TryGetComponents<WallJumper>(out wallJumper)) {
				return !wallJumper.IsSliding;
			} else {
				return true;
			}
		} else {
			return false;
		}
	}
}
