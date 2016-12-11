public class GravitySystem : EgoSystem<PlayerState, CharacterData> {
	public override void FixedUpdate() {
		ForEachGameObject((ego, state, charStats) => {
			charStats.GravityScale = GetScale(state.CurrentMode);
		});
	}

	private float GetScale(PlayerState.Mode state) {
		switch (state) {
			case PlayerState.Mode.Flung: return 0.75f;
			//case PlayerState.Mode.Swing: return 1.5f;
			default: return 1;
		}
	}
}
