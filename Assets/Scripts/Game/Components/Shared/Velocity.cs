using UnityEngine;

namespace LatchOn.ECS.Components.Base {
	[DisallowMultipleComponent]
	public class Velocity : MonoBehaviour {
		public Vector2 Value = new Vector2();
		public float x { get { return Value.x; } }
		public float y { get { return Value.y; } }
	}
}
