using UnityEngine;

namespace Hook.Rope {
	public interface IRope {
		Vector2 anchor {get;}
		Vector2 connectedAnchor {get;}
		
		void ConnectTo(RaycastHit2D hit);
		void Break();
	}
}