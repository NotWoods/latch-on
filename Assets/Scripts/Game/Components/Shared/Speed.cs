using UnityEngine;

namespace LatchOn.ECS.Components.Base {
	[DisallowMultipleComponent]
	public class Speed : MonoBehaviour {
		[SerializeField]
		float _value;

		public float Value { get { return _value; } }
	}
}
