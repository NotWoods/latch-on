using UnityEngine;
using System.Collections.Generic;

namespace LatchOn.ECS.Components.Parts {
	[DisallowMultipleComponent]
	public class RobotExtenderPart : MonoBehaviour {
		public Transform Part;
		public float DefaultScale = 0.18f;
		public Material PartMaterial;

		public List<Transform> ToHide = new List<Transform>();
	}
}
