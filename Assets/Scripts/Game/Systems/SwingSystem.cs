using UnityEngine;
using Prime31;

public class SwingSystem : EgoSystem<Transform, CharacterData, InspectableLineData, CharacterController2D> {
	public override void Update() {
		ForEachGameObject((ego, transform, stats, line, controller) => {
			if (!line.IsAnchored()) return;


		});
	}
}
