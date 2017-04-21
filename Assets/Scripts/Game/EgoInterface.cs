﻿using UnityEngine;
using LatchOn.ECS.Systems;

public class EgoInterface : MonoBehaviour {
	static EgoInterface() {
		EgoSystems.Add(
			new InputSystem(),

			new HookSystem(),
			new WrapSystem(),

			new MoveStateSystem(),
			new MoveSystem(),
			new SwingingSystem(),
			new WallJumpingSystem(),
			new DiveSystem(),
			new ApplyMoveSystem(),

			new LineRendererSystem(),
			new CursorRendererSystem(),
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
