using UnityEngine;
using LatchOn.ECS.Systems;
using LatchOn.ECS.Systems.Movement;
using LatchOn.ECS.Systems.Rendering;

public class EgoInterface : MonoBehaviour {
	static EgoInterface() {
		var cameraTargetingSystem = new CameraTargetingSystem();

		EgoSystems.Add(
			new InputSystem(),
			new PauseSystem(),
			new BoundarySystem(),

			new HookSystem(),
			new WrapSystem(),

			new MoveStateSystem(),
			new MoveSystem(),
			new SwingingSystem(),
			new WallJumpingSystem(),
			new DiveSystem(),
			new ApplyMoveSystem(),

			new DebugLineSystem(),
			new LineRendererSystem(),
			new ArmExtenderSystem(),
			new CursorUpdateSystem(),
			new CursorRendererSystem(),
			new SwingingParticleSystem(),
			new HookTrailSystem(),
			new SpriteRotationSystem(),
			new RobotShoulderSystem(),

			new StretchySystem(),
			new HideWhenGrapplingSystem(),
			new LoadingZoneSystem(),
			new AnimatorStateSystem(),

			new TouchDamageSystem(),
			new RespawnSystem(cameraTargetingSystem),

			new CameraInputSystem(),
			cameraTargetingSystem,
			new CameraZoomSystem(),
			new CameraMoveSystem()
		);
	}

	void Start() { EgoSystems.Start(); }
	void Update() { EgoSystems.Update(); }
	void FixedUpdate() { EgoSystems.FixedUpdate(); }
}
