using UnityEngine;
using System.Collections.Generic;
using Prime31;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Input;

namespace LatchOn.ECS.Systems {
	public class RespawnSystem : EgoSystem<Transform, CharacterController2D, Velocity> {
		Queue<EgoComponent> deadEntities = new Queue<EgoComponent>();
		Vector2 spawnPosition = Vector2.up * 2;

		public void RespawnPlayer(EgoComponent entity) {
			var transform = entity.GetComponent<Transform>();
			var controller = entity.GetComponent<CharacterController2D>();
			var velocity = entity.GetComponent<Velocity>();

			transform.position = spawnPosition;
			controller.warpToGrounded();
			velocity.Value = Vector2.zero;
		}

		public override void Update() {
			// when input signal is received,
			// respawn the next player in queue
			if (Input.GetButtonDown("Respawn")) {
				EgoComponent entity = deadEntities.Dequeue();
				RespawnPlayer(entity);
			}
		}
	}
}
