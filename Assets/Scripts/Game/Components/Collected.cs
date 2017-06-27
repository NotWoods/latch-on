using UnityEngine;
using System.Collections.Generic;

namespace LatchOn.ECS.Components {
	[DisallowMultipleComponent]
	public class Collected : MonoBehaviour {
		public List<Collectable> CollectedItems = new List<Collectable>();
	}
}
