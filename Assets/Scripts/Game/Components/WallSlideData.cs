using UnityEngine;

[DisallowMultipleComponent]
public class WallSlideData : MonoBehaviour {
	public float MaxSlideSpeed = 2.5f;

	public Vector2 WallJumpClimb = new Vector2(7.5f, 16);
	public Vector2 WallJumpSwing = new Vector2(1, 12);
	public Vector2 WallJumpOff = new Vector2(8.5f, 7);
	public Vector2 WallLeap = new Vector2(18, 17);

	/// Uses x direction sign to represent the side of the wall the player
	/// is agaisnt
	internal int Side = 0;
	public bool IsSliding { get { return Side != 0; } }

	public float StickTime = 0.25f;
	internal float TimeToUnstick = 0;

	public void ResetTime() { TimeToUnstick = StickTime; }
}
