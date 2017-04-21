using UnityEngine;

namespace LatchOn.ECS.Events {
	public class LineConnected: EgoEvent {
		public readonly EgoComponent egoComponent;
		public readonly Vector2 anchor;

		public LineConnected(EgoComponent ego, Vector2 anchor) {
			egoComponent = ego;
			this.anchor = anchor;
		}
	}
}
