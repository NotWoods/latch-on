﻿using UnityEngine;

namespace Hook.Rope {
	public interface IDeprecatedRope {
		Vector2 anchor {get;}
		Vector2 connectedAnchor {get;}
		bool isActive {get;}
		
		void ConnectTo(RaycastHit2D hit);
		void Break();
	}
}