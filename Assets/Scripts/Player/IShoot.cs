using UnityEngine;

namespace Player {
	public interface IShoot {
		bool isConnected {get;}
		bool ropeLength {get; set;}

		// Shoots a rope towards the given point
		bool ShootToward(Vector2 towardsPoint, Vector2 fromPosition);

		// Adjusts the rope at the given speed
		bool Retract(float velocity);
	}
}