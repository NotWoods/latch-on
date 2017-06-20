using UnityEngine;

namespace LatchOn.ECS.Components.Health {
	[DisallowMultipleComponent]
	public class TouchDamager : MonoBehaviour {
		public int Damage = 1;
		public string Message;
	}
}
