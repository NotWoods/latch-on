﻿using UnityEngine;

public class EgoInterface : MonoBehaviour {
	static EgoInterface() {
		EgoSystems.Add(
			new MoveSystem(),
			new HookSystem(),
			new InputSystem(),
			new LineRendererSystem()
		);
	}

	void Start() { EgoSystems.Start(); }
	void Update() { EgoSystems.Update(); }
	void FixedUpdate() { EgoSystems.Update(); }
}
