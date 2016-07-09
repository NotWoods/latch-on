using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class PlayerRope : MonoBehaviour, IRope {
	/// Represents part of the rope that is currently stationary.
	/// First joint should be that connected to a wall
	EdgeCollider2D staticRope;

	/// Represents flexible part of the rope.
	SpringJoint2D activeRope;

	/// Used to link the SpringJoint2D to the end of the EdgeCollider2D
	Rigidbody2D anchor;

	public Action OnBreak {protected get; set;}

	void Awake() {
		staticRope = gameObject.GetComponent<EdgeCollider2D>();

		activeRope = gameObject.GetComponent<SpringJoint2D>();
		anchor = gameObject.GetComponent<Rigidbody2D>();
	}

	public List<Vector2> joints {
		get {
			List<Vector2> points = staticRope.points.ToList();
			points.Add(activeRope.connectedBody.position);
			return points;
		}
		set {
			Vector2 endPoint = value.ElementAt(value.Count - 1);
			Vector2 anchorPoint = value.ElementAt(value.Count - 2);
			value.Remove(endPoint);

			staticRope.points = value.ToArray();
			anchor.position = anchorPoint;
			activeRope.connectedBody.position = endPoint;
		}
	}

	public float length {
		get {
			float length = Vector2.Distance(
				activeRope.connectedBody.position,
				anchor.position
			);
			Vector2 lastPoint = anchor.position;
			for (int i = staticRope.points.Length - 1; i > 0; i--) {
				length += Vector2.Distance(lastPoint, staticRope.points[i]);
				lastPoint = staticRope.points[i];
			}
			return length;
		}
	}

	public void LinkFrom(Vector2 position, Rigidbody2D body) {
		activeRope.connectedBody = body;
		LinkFrom(position);
	}
	public void LinkFrom(Vector2 position) {}

	public void LinkTo(Vector2 position) {
		staticRope.points = new Vector2[] {position};
		anchor.position = position;
	}

	public void LinkTo(Vector2 position, Rigidbody2D body) {
		
	}
}