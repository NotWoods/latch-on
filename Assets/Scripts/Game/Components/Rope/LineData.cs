using System;
using System.Collections.Generic;
using UnityEngine;

namespace LatchOn.ECS.Components.Rope {
	/// Data for the line that connects the entity and some point
	[DisallowMultipleComponent]
	public class LineData : MonoBehaviour {
		[SerializeField]
		float _minLength = 0.5f;
		[SerializeField]
		float _retractSpeed = 2;
		[SerializeField]
		float _quickRetractSpeed = 4;

		/// Smallest valid length of the rope
		public float MinLength { get { return _minLength; } }
		/// The current length of the line.
		public float CurrentLength = 10f;
		/// How quickly the line will retract.
		public float RetractSpeed { get { return _retractSpeed; } }
		/// How quiclly the line will retract, in quick state.
		public float QuickRetractSpeed { get { return _quickRetractSpeed; } }
		/// True if the rope is anchored to a point
		public bool IsAnchored = false;
		/// Point where the rope is anchored. Should be ignored if !IsAnchored
		public Vector2 AnchorPoint = default(Vector2);

		void OnDrawGizmosSelected() {
			if (!IsAnchored) return;

			Gizmos.color = Color.white;
			Gizmos.DrawLine(AnchorPoint, (Vector2) transform.position);
		}
	}
}
