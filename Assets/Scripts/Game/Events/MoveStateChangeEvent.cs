using UnityEngine;

namespace LatchOn.ECS.Events {
	public class MoveStateChangeEvent : EgoEvent {
		public readonly EgoComponent egoComponent;
		public readonly MoveType newState;

		public MoveStateChangeEvent(EgoComponent source, MoveType newState) {
			egoComponent = source;
			this.newState = newState;
		}
	}
}
