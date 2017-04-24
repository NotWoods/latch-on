using UnityEngine;

namespace LatchOn.ECS.Components.Mover {
	[DisallowMultipleComponent]
	public class Damping : MonoBehaviour {
		// Replace with Dictionary<MoveType, float> in the future
		[SerializeField]
		private float WalkDamping = 20;
		[SerializeField]
		private float SwingDamping = 0.5f;
		[SerializeField]
		private float FlungDamping = 1;
		[SerializeField]
		private float FallDamping = 5;

		/// Multiplier for velocity.
		/// A value of 1 has no effect on the base velocity.
		public float GetValue(MoveType state) {
			switch (state) {
				case MoveType.Walk: return WalkDamping;
				case MoveType.Swing: return SwingDamping;
				case MoveType.Flung: return FlungDamping;
				case MoveType.Fall: return FallDamping;
				default: return 1;
			}
		}
	}
}
