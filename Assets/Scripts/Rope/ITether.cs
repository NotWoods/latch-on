using UnityEngine;

namespace Rope {
	public interface ITether {
		bool isTethered {get;}

		bool GrappleToward(Vector2 point);

		void Break();
	}
}