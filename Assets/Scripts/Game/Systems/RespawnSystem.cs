using UnityEngine;
using Prime31;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Health;
using LatchOn.ECS.Components.Input;
using LatchOn.ECS.Components.Camera;
using LatchOn.ECS.Systems.Cameras;

namespace LatchOn.ECS.Systems {
	public class RespawnSystem : EgoSystem<
		EgoConstraint<Destroyable, Transform, VJoystick, CharacterController2D, Velocity>
	> {
		private EgoConstraint<CameraTarget, CameraFollow> cameraConstraint;
		public RespawnSystem(CameraTargetingSystem camSystem) : base() {
			cameraConstraint = camSystem.Constraint;
		}

		EgoComponent SpawnPlayer(EgoComponent player) {
			var gm = GameManager.Instance;
			if (player == null) {
				player = gm.NewEntity(gm.PlayerPrefab);

				var marker = Ego.AddComponent<LocalPlayer>(player);
				marker.Controller = ControlType.Keyboard;

				if (gm.ActorContainer) {
					Ego.SetParent(gm.ActorContainer.GetComponent<EgoComponent>(), player);
				}
			}

			Transform transform;
			CharacterController2D controller;
			Velocity velocity;
			VJoystick input;
			Destroyable destroyable;
			if (player.TryGetComponents(
				out transform, out controller,
				out velocity, out input, out destroyable
			)) {
				RespawnPlayer(player, transform, controller, velocity, input, destroyable);
				return player;
			} else {
				return null;
			}
		}

		void RespawnPlayer(
			EgoComponent player, Transform transform, CharacterController2D controller,
			Velocity velocity, VJoystick input, Destroyable hp
		) {
			player.gameObject.SetActive(true);

			transform.position = GameManager.Instance.SpawnPoint;
			controller.warpToGrounded();
			velocity.Value = Vector2.zero;
			hp.ResetHealth();

			input.ShouldRespawn = false;
			input.HookDown = false;
			input.ClearPressed();

			Collider2D collider = player.GetComponent<Collider2D>();
			if (collider) {
				cameraConstraint.ForEachGameObject((cam, target, follower) => {
					if (target.TargetedEntity == null) target.TargetedEntity = collider;
				});
			}
		}

		void KillPlayer(EgoComponent player, VJoystick input, Destroyable hp) {
			player.gameObject.SetActive(false);

			hp.CurrentHealth = -1;

			input.HookDown = false;
			input.ClearPressed();

			Collider2D collider = player.GetComponent<Collider2D>();
			if (collider) {
				cameraConstraint.ForEachGameObject((cam, target, follower) => {
					if (target.TargetedEntity == collider) target.TargetedEntity = null;
				});
			}

			UIManager.Instance.Log(hp.DamageMessage);
		}

		public override void FixedUpdate() {
			constraint.ForEachGameObject((ego, hp, transform, input, controller, velocity) => {
				bool isAlive = GameManager.IsActive(ego);

				if (input.ShouldRespawn) {
					if (!isAlive) RespawnPlayer(ego, transform, controller, velocity, input, hp);
					else {
						input.ShouldRespawn = false;
						hp.DamageMessage = "Self-destructed";
						KillPlayer(ego, input, hp);
					}
				} else if (hp.CurrentHealth < 0 && isAlive) {
					KillPlayer(ego, input, hp);
				}
			});
		}
	}
}
