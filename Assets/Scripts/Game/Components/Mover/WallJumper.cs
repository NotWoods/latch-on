using UnityEngine;

namespace LatchOn.ECS.Components.Mover {
	/// Indicates that an entity can wall jump
	[DisallowMultipleComponent]
	public class WallJumper : MonoBehaviour {
		public float MaxSlideSpeed = 2.5f;
		public float BaseStickTime = 0.25f;

		public Vector2 ClimbingJump = new Vector2(7.5f, 4);
		public Vector2 SwiningJump = new Vector2(1, 12);
		public Vector2 FallOffJump = new Vector2(8.5f, 7);
		public Vector2 LeapingJump = new Vector2(18, 17);

		/// Uses x direction sign to represent the side of the wall the player
		/// is agaisnt
		internal int AgaisntSide = 0;
		public bool IsSliding { get { return AgaisntSide != 0; } }

		internal float TimeToUnstick = 0;
		public void ResetTime() { TimeToUnstick = BaseStickTime; }
	}
}
