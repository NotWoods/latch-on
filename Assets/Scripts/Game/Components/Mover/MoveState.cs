using UnityEngine;

namespace LatchOn.ECS.Components.Mover {
	/// Current movement state of the entity
	[DisallowMultipleComponent]
	public class MoveState : MonoBehaviour {
		public MoveType Value = default(MoveType);
	}
}
