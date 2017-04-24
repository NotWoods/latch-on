using UnityEngine;

namespace LatchOn.ECS.Components.Rope {
	/// Component for needle prop
	[DisallowMultipleComponent]
	public class Hook : MonoBehaviour {
		public float Speed = 50;
		public float HookLength = 1;
		public Vector3 StorageLocation = Vector3.back * 20;

		public Vector2 Target = new Vector2();
		public bool Deployed = false;


		/// Calculates the position of the pinhead,
		/// or where the rope would attach to the hook
		public static Vector2 CalculatePinHead(Hook hook) {
			Transform transform = hook.transform;
			Vector2 direction = transform.rotation * Vector2.up;
			Vector2 shift = direction.normalized * hook.HookLength;

			return (Vector2) transform.position + shift;
		}

		public Vector2 CalculatePinHead() { return CalculatePinHead(this); }
	}
}
