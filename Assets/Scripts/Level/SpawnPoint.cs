using UnityEngine;
using Player;

namespace Level {
	public class SpawnPoint : MonoBehaviour {
		public GameObject player;

		public Vector2 spawnOffset;

		void Create() {
			Vector2 worldPos = transform.TransformPoint(spawnOffset);
			Instantiate(player, worldPos, Quaternion.identity);
		}
	}
}