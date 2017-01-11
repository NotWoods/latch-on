using UnityEngine;

[DisallowMultipleComponent]
public class Damping : MonoBehaviour {
	// Replace with Dictionary<MoveState.Mode, float> in the future
	[SerializeField]
	private float WalkDamping = 20;
	[SerializeField]
	private float SwingDamping = 0.5f;
	[SerializeField]
	private float FlungDamping = 1;
	[SerializeField]
	private float FallDamping = 5;

	public float GetValue(MoveState.Mode state) {
		switch (state) {
			case MoveState.Mode.Walk: return WalkDamping;
			case MoveState.Mode.Swing: return SwingDamping;
			case MoveState.Mode.Flung: return FlungDamping;
			case MoveState.Mode.Fall: return FallDamping;
			default: return 1;
		}
	}
}
