using UnityEngine;
using System.Collections.Generic;
using Prime31;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Events;

namespace LatchOn.ECS.Systems {
	public class RespawnSystem : EgoSystem<Transform, CharacterController2D, Velocity> {
		Queue<EgoComponent> deadEntities = new Queue<EgoComponent>();
		Vector2 spawnPosition = Vector2.up * 2;

		void RespawnPlayer(EgoComponent entity) {
			var bundle = _bundles[entity];
			Transform transform = bundle.component1;
			CharacterController2D controller = bundle.component2;
			Velocity velocity = bundle.component3;

			transform.position = spawnPosition;
			controller.warpToGrounded();
			velocity.Value = Vector2.zero;
		}

		void Handle(EntityDestroyed e) {
			deadEntities.Enqueue(e.egoComponent);
		}

		public override void Start() {
			EgoEvents<EntityDestroyed>.AddHandler(Handle);
		}

		public override void Update() {
			// when input signal is received,
			// respawn the next player in queue
			if (Input.GetButtonDown("Respawn") && deadEntities.Count > 0) {
				EgoComponent entity = deadEntities.Dequeue();
				RespawnPlayer(entity);
			}
		}
	}
}
