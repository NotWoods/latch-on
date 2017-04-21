using UnityEngine;

namespace LatchOn.ECS.Components.Rope {
	[DisallowMultipleComponent]
	public class HookReference : MonoBehaviour {
		public GameObject HookPrefab;

		/// Hook object entity reference
		public EgoComponent Hook;

		public bool DidThrow = false;
	}
}
