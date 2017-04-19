using UnityEngine;

namespace LatchOn.ECS.Components.Rope {
	/// LineData, but easier to look at inspector.
	[DisallowMultipleComponent]
	public class LineData : MonoBehaviour {
		[SerializeField]
		float _minLength = 0.5f;
		[SerializeField]
		float _retractSpeed = 1f;

		/// Smallest valid length of the rope
		public float MinLength { get { return _minLength; } }
		/// The current length of the line.
		public float CurrentLength = 10f;
		/// How quickly the line will retract.
		public float RetractSpeed { get { return _retractSpeed; } }
		/// True if the rope is anchored to a point
		public bool IsAnchored = false;
		/// Point where the rope is anchored. Should be ignored if !IsAnchored
		public Vector2 AnchorPoint = Vector2.zero;
	}
}
