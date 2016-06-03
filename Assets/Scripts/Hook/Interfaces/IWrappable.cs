using UnityEngine;
using System.Collections.Generic;
using System;

namespace Hook.Rope {
	// Represents a list of joints in a rope.
	// The top of the stack is the most recently created
	// joint.
	public class JointList : Stack<Vector2> {
		public Vector2 connectedAnchor;
		
		JointList() : base() {}
		JointList(Vector2 _connectedAnchor) : base() {
			connectedAnchor = _connectedAnchor;
		}
		
		public new Vector2 Peek() {
			if (base.Count == 0) {
				return connectedAnchor;
			} else {
				return base.Peek();
			}
		}
		
		public Vector2 PeekNext() {
			if (base.Count <= 1) {
				throw new InvalidOperationException("No joints to unwrap");
			} else {
				return base[base.Count - 2];
			}
		}
		
		public new List<Vector2> ToList() {
			List<Vector2> list = base.ToList();
			List.Insert(0, connectedAnchor);
		}
	}
	
	///<summary>
	///Interface used for ropes that can wrap around objects.
	///Adds a list of joints and event handlers, since a wrappable rope
	///will need to have joints and not just be a straight line.
	///</summary>
	public interface IWrappable : IRope	{
		JointList joints {get; set;}
		
		void WrapOn(RaycastHit2D hit);
		void Unwrap();
	}
}