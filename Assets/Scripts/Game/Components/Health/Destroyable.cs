using UnityEngine;

namespace LatchOn.ECS.Components.Health {
	[DisallowMultipleComponent]
	public class Destroyable : MonoBehaviour {
		[SerializeField]
		int _maxHealth = 3;

		public int MaxHealth { get { return _maxHealth; } }
		public int CurrentHealth = 3;

		public string DamageMessage;

		[ContextMenu("Reset Health")]
		public void ResetHealth() {
			CurrentHealth = MaxHealth;
			DamageMessage = "";
		}
	}
}
