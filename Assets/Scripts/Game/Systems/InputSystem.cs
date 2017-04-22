using System;
using UnityEngine;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Input;

namespace LatchOn.ECS.Systems {
	public class InputSystem : EgoSystem<LocalPlayer, VJoystick, WorldPosition> {
		Vector2 GetPointerDirection(LocalPlayer player, Vector2 position) {
			if (player.UseKeyboard) {
				Vector2 cursorScreenPoint;
				if (player.UseTouch) {
					cursorScreenPoint = Input.GetTouch(0).position;
				} else if (player.UsePointer) {
					cursorScreenPoint = Input.mousePosition;
				} else {
					throw new Exception("Must use touch or pointer w/ keyboard mode");
				}

				Vector2 cursorPoint = Camera.main.ScreenToWorldPoint(cursorScreenPoint);
				return cursorPoint - position;
			} else {
				return new Vector2(
					Input.GetAxis("Pointer X"),
					Input.GetAxis("Pointer Y")
				);
			}
		}

		public override void Update() {
			if (PauseSystem.Paused) return;

			ForEachGameObject((ego, player, input, position) => {
				input.XMoveAxis = Input.GetAxis("Horizontal");
				input.XMoveAxisRaw = Input.GetAxisRaw("Horizontal");

				if (Input.GetButtonDown("Jump")) input.JumpPressed = true;
				if (Input.GetButtonDown("Sink")) input.SinkPressed = true;

				bool touchDown = Input.touchCount > 0;
				bool mouseDown = Input.GetButton("Grapple To Point");
				bool controllerDown = Input.GetButton("Grapple Using Pointer");

				input.HookDown = touchDown || mouseDown || controllerDown;
				if (touchDown) {
					player.UseTouch = true;
					player.UsePointer = false;
				} else if (mouseDown) {
					player.UsePointer = true;
					player.UseTouch = false;
				}

				if (controllerDown) {
					player.UseKeyboard = false;
				} else {
					player.UseKeyboard = true;
				}

				input.AimAxis = GetPointerDirection(player, position.Value);
			});
		}
	}
}
