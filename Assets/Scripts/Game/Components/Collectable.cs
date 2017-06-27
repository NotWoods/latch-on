using UnityEngine;

namespace LatchOn.ECS.Components {
	[DisallowMultipleComponent]
	public class Collectable : MonoBehaviour {
		public bool BeenCollected = false;
	}
}
