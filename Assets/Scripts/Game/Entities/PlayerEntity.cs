using UnityEngine;
using Prime31;

[RequireComponent (typeof (CharacterController2D))]
public class PlayerEntity : Entity {
	public enum State {
		Idle,   // not moving
		Walk,   // running along the ground
		Jump,   // jumping up
		Hooked, // swinging around on a grappling hook
		Soar,   // flying after releasing from grappling hook
		Fall    // falling downward
	}

	public State CurrentState = State.Idle;

	void Awake() {
		AssignID();
		CreateComponent<InputComponent>();
		CreateComponent<CharacterStatsComponent>();
	}
}
