using UnityEngine;
using LatchOn.ECS.Systems;
using LatchOn.ECS.Systems.Movement;
using LatchOn.ECS.Systems.Rendering;

public class EgoInterface : MonoBehaviour {
	static EgoInterface() {
		var cursorRenderSys = new CursorRendererSystem();
		EgoSystems.Add(
			new InputSystem(),
			new PauseSystem(),

			new HookSystem(),
			new WrapSystem(),

			new MoveStateSystem(),
			new MoveSystem(),
			new SwingingSystem(),
			new WallJumpingSystem(),
			new DiveSystem(),
			new ApplyMoveSystem(),

			new LineRendererSystem(),
			new CursorUpdateSystem(cursorRenderSys),
			cursorRenderSys,
			new RespawnSystem(),
			new SwingingParticleSystem(),
			new HookTrailSystem(),
			new SpriteRotationSystem()
		);
	}

	void Start() { EgoSystems.Start(); }
	void Update() { EgoSystems.Update(); }
	void FixedUpdate() { EgoSystems.FixedUpdate(); }
	void LateUpdate() { EgoSystems.LateUpdate(); }
}
