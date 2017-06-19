using UnityEngine;
using System.Collections.Generic;

namespace LatchOn.ECS.Components.Parts {
	/// Renderers that should be disabled when grappling or throwing
	[DisallowMultipleComponent]
	public class HideWhenGrappling : MonoBehaviour {
		public List<MeshRenderer> ToHide = new List<MeshRenderer>();
	}
}
