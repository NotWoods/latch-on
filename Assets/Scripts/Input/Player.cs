using UnityEngine;

namespace Input {
	///<summary>Helper functions for player input</summary>
	public class Player {
		///<summary>Return movement axises as a Vector2</summary>
		public Vector2 Movement() {
			return new Vector2(
				Input.GetAxis("X"),
				Input.GetAxis("Y")
			);
		}
		
		///<summary>Return if grapple button is active</summary>
		public bool Grapple(int ropeNum) {
			if (ropeNum == 0) {
				return Input.GetButton("grapple-0");
			} else if (ropeNum == 1) {
				return Input.GetButton("grapple-1");
			} else {
				return false;
			}
		}
		
		
	}
}