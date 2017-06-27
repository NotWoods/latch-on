using UnityEngine;
using LatchOn.ECS.Components;
using LatchOn.ECS.Components.Health;
using LatchOn.ECS.Components.Input;
using System.Collections.Generic;

namespace LatchOn.ECS.Systems.Cameras {
	public class HUDSystem : EgoSystem<
		EgoConstraint<UITarget, Destroyable, Collected, VJoystick>
	> {
		const float StandStillMinTime = 3;
		const float InteractionVisibleTime = 3;

		struct LastState {
			public int health;
			public int collectedCount;
			public LastState(Destroyable health, Collected pocket) {
				this.health = health.CurrentHealth;
				collectedCount = pocket.CollectedItems.Count;
			}
		}

		HUDWrapper hud { get { return UIManager.Instance.HUD; } }

		Dictionary<EgoComponent, LastState> last;
		public override void Start() {
			last = new Dictionary<EgoComponent, LastState>();
			constraint.ForEachGameObject((ego, target, health, pocket, input) => {
				last[ego] = new LastState(health, pocket);
			});
		}

		public override void Update() {
			constraint.ForEachGameObject((ego, target, health, pocket, input) => {
				hud.CollectableObtained = pocket.CollectedItems.Count > 0;
				hud.Health = health.CurrentHealth;

				if (input.NoInput()) {
					input.IdleTime += Time.deltaTime;
				} else {
					input.IdleTime = 0;
				}

				var lastState = last[ego];
				var newState = new LastState(health, pocket);
				if (lastState.health != newState.health
				|| lastState.collectedCount != newState.collectedCount) {
					target.VisibleTime = InteractionVisibleTime;
					last[ego] = newState;
				}

				if (input.IdleTime > StandStillMinTime) {
					hud.Visible = true;
				} else if (target.VisibleTime > 0) {
					hud.Visible = true;
					target.VisibleTime -= Time.deltaTime;
					if (target.VisibleTime < 0) target.VisibleTime = 0;
				} else {
					hud.Visible = false;
				}
			});
		}
	}
}
