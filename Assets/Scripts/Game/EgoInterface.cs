using UnityEngine;

public class EgoInterface : MonoBehaviour {
	static EgoInterface() {
		EgoSystems.Add(
			new InputSystem(),

			new HookSystem(),

			new MoveStateSystem(),
			new MoveSystem(),
			new SwingingSystem(),
			new WallJumpingSystem(),
			// new DiveSystem(),
			new ApplyMoveSystem(),

			new LineRendererSystem(),
			new CursorRendererSystem(),
			new RespawnSystem(),
			new SwingingParticleSystem()
		);
	}

	void Start() { EgoSystems.Start(); }
	void Update() { EgoSystems.Update(); }
	void FixedUpdate() { EgoSystems.FixedUpdate(); }
	void LateUpdate() { EgoSystems.LateUpdate(); }
}
