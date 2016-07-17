using UnityEngine;

namespace Rope {
	public class Rope : MonoBehaviour {
		[HideInInspector]
		public SpringJoint2D springJoint {get; private set;}
		[HideInInspector]
		public GameObject connectedObject;
	}
}