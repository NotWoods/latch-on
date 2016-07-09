using UnityEngine;

namespace Player {
	public interface IShoot {
		bool isConnected {get;}
		float ropeLength {get;}

		// Shoots a rope towards the given point
		bool ShootToward(Vector2 towardsPoint, Vector2 fromPosition);
		bool ShootToward(Vector2 towardsPoint);

		// Adjusts the rope at the given speed
		bool Retract(float velocity);
	}
}