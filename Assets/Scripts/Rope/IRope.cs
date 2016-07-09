using UnityEngine;
using System.Collections.Generic;

public interface IRope {
	float length {get; set;}
	List<Vector2> joints {get;}

	// Connect the rope from a position
	void LinkFrom(Vector2 position);
	// Connect the rope from a position and body
	void LinkFrom(Vector2 position, Rigidbody2D thing);

	void LinkTo(Vector2 position);
	void LinkTo(Vector2 position, Rigidbody2D thing);

	void Wrap(Vector2 point);
	void Wrap(Vector2 point, Collider2D thing);
	void Wrap(Vector2 point, Rigidbody2D movingThing);
}