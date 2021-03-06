using UnityEngine;

namespace LatchOn.ECS.Components {
	[DisallowMultipleComponent]
	public class CursorData : MonoBehaviour {
		[SerializeField]
		float _highlightScale = 1;
		[SerializeField]
		float _darkScale = 0.5f;
		[SerializeField]
		Color _highlightColor = Color.white;
		[SerializeField]
		Color _darkColor = Color.gray;

		public float HighlightScale { get { return _highlightScale; } }
		public float DarkScale { get { return _darkScale; } }
		public Color HighlightColor { get { return _highlightColor; } }
		public Color DarkColor { get { return _darkColor; } }

		/// If true, use the highlight color and scale.
		/// Otherwise, use the dark equivalents.
		public bool Highlighted = false;

		/// If true, disable the cursor's renderer
		public bool Hidden = false;
	}
}
