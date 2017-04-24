using UnityEngine;

namespace LatchOn.ECS.Components.Mover {
	/// Current movement state of the entity
	[DisallowMultipleComponent]
	public class MoveState : MonoBehaviour {
		public MoveType Value = default(MoveType);


		/// Checks if the current state is the same as any of the given options.
		public bool Any(params MoveType[] states) {
			for (int i = 0; i < states.Length; i++) {
				if (Value == states[i]) return true;
			}

			return false;
		}

		/// Checks if the current MoveState is one of the given MoveTypes.
		/// Multiple values can be listed as `IsType(MoveType.A | MoveType.B)`
		public bool IsType(MoveType type) {
			return (Value & type) > 0;
		}
	}
}
