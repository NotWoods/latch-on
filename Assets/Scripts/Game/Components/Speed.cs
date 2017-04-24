using UnityEngine;

namespace LatchOn.ECS.Components.Base {
	[DisallowMultipleComponent]
	public class Speed : MonoBehaviour {
		[SerializeField]
		float _value = 8f;

		public float Value { get { return _value; } }
	}
}
