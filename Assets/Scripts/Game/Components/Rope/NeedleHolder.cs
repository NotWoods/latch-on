using UnityEngine;

namespace LatchOn.ECS.Components.Rope {
	[DisallowMultipleComponent]
	public class NeedleHolder : MonoBehaviour {
		public GameObject Needle;

		internal bool DidThrow = false;
	}
}
