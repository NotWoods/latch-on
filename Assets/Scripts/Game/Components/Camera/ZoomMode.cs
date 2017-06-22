using UnityEngine;
using UnityEngine.SceneManagement;

namespace LatchOn.ECS.Components.Camera {
	[DisallowMultipleComponent]
	public class ZoomMode : MonoBehaviour {
		public Zoom Value = Zoom.Standard;
	}
}
