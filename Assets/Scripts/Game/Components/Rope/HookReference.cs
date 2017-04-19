using UnityEngine;

namespace LatchOn.ECS.Components.Rope {
	[DisallowMultipleComponent]
	public class HookReference : MonoBehaviour {
		GameObject HookPrefab;
		GameObject Hook;

		/// Hook object entity ID
		public int HookID;
		public bool DidThrow = false;
	}
}
