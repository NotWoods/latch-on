using UnityEngine;

namespace LatchOn.ECS.Components.Rope {
	/// Component for needle prop
	[DisallowMultipleComponent]
	public class Hook : MonoBehaviour {
		[SerializeField]
		float _length = 1;

		/// The position where the hook should go
		public Vector2 Target = new Vector2();
		/// If the hook is retracted or not
		public bool Deployed = false;
		/// The size of the hook
		public float Length { get { return _length; } }

		/// Calculates the position of the pinhead,
		/// or where the rope would attach to the hook
		public Vector2 CalculatePinHead() {
			Vector2 position = transform.position;
			Vector2 direction = transform.rotation * Vector2.up;
			Vector2 shift = direction.normalized * _length;

			return position + shift;
		}
	}
}
