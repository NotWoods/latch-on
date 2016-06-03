using UnityEngine;
using System.Collections.Generic;
using System;

namespace Hook.Rope {
	// Represents a list of joints in a rope.
	// The top of the stack is the most recently created
	// joint.
	public class JointList : List<Vector2> {
		public Vector2 connectedAnchor;
		
		JointList() : base() {}
		JointList(Vector2 _connectedAnchor) : base() {
			connectedAnchor = _connectedAnchor;
		}
		
		public Vector2 Peek() {
			if (base.Count == 0) {
				return connectedAnchor;
			} else {
				return base[base.Count - 1];
			}
		}
		
		public Vector2 PeekNext() {
			if (base.Count <= 1) {
				throw new InvalidOperationException("No joints to unwrap");
			} else {
				return base[base.Count - 2];
			}
		}
		
		public Vector2 PopNext() {
			if (base.Count <= 1) {
				throw new InvalidOperationException("No joints to unwrap");
			} else {
				Vector2 point = base[base.Count - 2];
				base.RemoveAt(base.Count - 2);
				return point;
			}
		}
		
		public List<Vector2> FullList() {
			List<Vector2> list = (List<Vector2>) base.MemberwiseClone();
			list.Insert(0, connectedAnchor);
			return list;
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
		void Unwrap(Vector2 oldPoint);
	}
}