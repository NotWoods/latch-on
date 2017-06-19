using UnityEngine;
using LatchOn.ECS.Events;

namespace LatchOn.ECS.Components.Mover {
	/// Current movement state of the entity
	[DisallowMultipleComponent]
	public class MoveState : MonoBehaviour {
		[SerializeField]
		MoveType _value = default(MoveType);

		public MoveType Value {
			get { return _value; }
			set {
				_value = value;
				var e = new MoveStateChangeEvent(GetComponent<EgoComponent>(), value);
				EgoEvents<MoveStateChangeEvent>.AddEvent(e);
			}
		}

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
