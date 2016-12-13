﻿using UnityEngine;

public class EgoInterface : MonoBehaviour {
	static EgoInterface() {
		EgoSystems.Add(
			new MoveSystem(),
			new HookSystem(),
			new InputSystem(),
			new LineRendererSystem(),
			new CursorRendererSystem(),
			new RespawnSystem()
		);
	}

	void Start() { EgoSystems.Start(); }
	void Update() { EgoSystems.Update(); }
	void FixedUpdate() { EgoSystems.FixedUpdate(); }
	void LateUpdate() { EgoSystems.LateUpdate(); }
}