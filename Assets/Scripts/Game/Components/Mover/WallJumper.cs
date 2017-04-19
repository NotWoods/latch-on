using UnityEngine;

namespace LatchOn.ECS.Components.Mover {
	/// Indicates that an entity can wall jump
	[DisallowMultipleComponent]
	public class WallJumper : MonoBehaviour {
		[SerializeField]
		float _maxSlideSpeed = 2.5f;
		[SerializeField]
		float _baseStickTime = 0.25f;
		[SerializeField]
		Vector2 _climbingJump = new Vector2(7.5f, 4);
		[SerializeField]
		Vector2 _swingingJump = new Vector2(1, 12);
		[SerializeField]
		Vector2 _fallOffJump = new Vector2(8.5f, 7);
		[SerializeField]
		Vector2 _leapingJump = new Vector2(18, 17);

		/// When sliding on a wall, the entity doesn't slide down faster than this
		/// speed.
		public float MaxSlideSpeed { get { return _maxSlideSpeed; } }
		/// TimeToUnstick should reset to this value
		public float BaseStickTime { get { return _baseStickTime; } }

		/// If the player is agaisnt a wall, this value should indicate which
		/// side of the player the wall is on.
		public Side AgainstSide = Side.None;

		/// When the player moves in the opposite direction of the wall they are
		/// sliding on, there is a should buffer before they fall off. This way,
		/// they can preform a wall jump without falling off first.
		public float TimeToUnstick = 0;
		public void ResetUnstickTime() { TimeToUnstick = _baseStickTime; }

		public Vector2 ClimbingJump { get { return _climbingJump; } }
		public Vector2 SwingingJump { get { return _swingingJump; } }
		public Vector2 FallOffJump { get { return _fallOffJump; } }
		public Vector2 LeapingJump { get { return _leapingJump; } }
	}
}
