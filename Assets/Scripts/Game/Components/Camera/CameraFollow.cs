using UnityEngine;
using UnityEngine.SceneManagement;

namespace LatchOn.ECS.Components.Camera {
	[DisallowMultipleComponent]
	/// Component for a camera to follow a changing Target point
	public class CameraFollow : MonoBehaviour {
		public Vector2 Target = Vector2.zero;
		public Vector3 Offset = Vector3.back * 13;
	}
}
