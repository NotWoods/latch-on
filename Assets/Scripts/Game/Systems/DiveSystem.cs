using UnityEngine;

/// Rudimentary diving system. Needs to be remodeled based on the wall
/// jump impulses.
public class DiveSystem : EgoSystem<Velocity, MoveState, VJoystick> {
	public override void FixedUpdate() {
		ForEachGameObject((ego, velocity, state, input) => {
			if (state.Any(MoveState.Fall, MoveState.Flung)
			&& input.SinkPressed && AtDivingVelocity(velocity.Value)) {
				Debug.Log("Dive! Dive! Dive!");
				Vector2 angle = new Vector2(Mathf.Sign(velocity.x) * 3, -1);
				velocity.Value = angle * 10;
			}
		});
	}

	private bool AtDivingVelocity(Vector2 velocity) {
		float ySpeed = Mathf.Abs(velocity.y);
		return ySpeed < 2;
	}
}
