using UnityEngine;

namespace LatchOn.ECS.Components.Mover {
	[DisallowMultipleComponent]
	public class Diver : MonoBehaviour {
		[SerializeField]
		float _minYVelocity = -5;
		[SerializeField]
		float _maxYVelocity = 5;
		[SerializeField]
		Vector2 _divingVelocity = new Vector2(11, -11);

		public float MinYVelocity { get { return _minYVelocity; } }
		public float MaxYVelocity { get { return _maxYVelocity; } }
		public Vector2 DivingVelocity { get { return _divingVelocity; } }
	}
}
