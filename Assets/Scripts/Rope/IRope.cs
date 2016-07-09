using UnityEngine;
using System.Collections.Generic;
using System;

public interface IRope {
	float length {get;}
	List<Vector2> joints {get;}

	Action OnBreak {set;}

	// Connect the rope from a position
	void LinkFrom(Vector2 position);
	// Connect the rope from a position and body
	void LinkFrom(Vector2 position, Rigidbody2D thing);

	void LinkTo(Vector2 position);
	void LinkTo(Vector2 position, Rigidbody2D thing);
}