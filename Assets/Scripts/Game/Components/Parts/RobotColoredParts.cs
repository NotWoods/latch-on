using UnityEngine;
using System.Collections.Generic;

namespace LatchOn.ECS.Components.Parts {
	/// Renderers that should be disabled when grappling or throwing
	[DisallowMultipleComponent]
	public class RobotColoredParts : MonoBehaviour {
		public Material BaseMaterial;
		public Color Color;

		public MeshRenderer Head;
		public MeshRenderer LeftShoulder;
		public MeshRenderer LeftArm;
		public MeshRenderer RightShoulder;
		public MeshRenderer RightArm;
	}
}
