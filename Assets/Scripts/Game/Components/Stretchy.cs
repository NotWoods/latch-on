using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace LatchOn.ECS.Components {
	[DisallowMultipleComponent]
	public class Stretchy : MonoBehaviour {
		public Material TileMaterial;
		public float DefaultScale = 0.18f;
		public List<MeshRenderer> ChildRenderers = new List<MeshRenderer>();

		public Vector2 StartPoint;
		public Vector2 EndPoint;
		public bool IsStretching = false;
	}
}
