using UnityEngine;
using System.Collections.Generic;

namespace Hook {
	public class Debugger : MonoBehaviour {
		public Dictionary<string, Color> colors = new Dictionary<string, Color>();
		
		public bool drawPlatforms = false;
		public float jointSize = 1.0f;
		
		public Vector2 PlayerLocation;
		public float ScanRadius;
		public List<Vector2> Rope = new List<Vector2>();
		private List<Block> platforms = new List<Block>();
		public List<Block> Platforms {
			get {return platforms;}
			set {
				platforms = value;
				platforms.Sort(delegate(Block one, Block two) {
					if (one.isSolid == two.isSolid) return 0;
					else if (one.isSolid) return -1;
					else return 1;
				});
			}
		}
		public Vector2 PlayerSize;
		
		void Awake() {
			colors.Add("radius", Color.red);
			colors.Add("rope", Color.green);
			colors.Add("ropejoint", Color.cyan);
			
			colors.Add("player", Color.blue);
			
			colors.Add("block", Color.white);
			colors.Add("solid", Color.gray);
		}
		
		void OnDrawGizmosSelected() {
			//Draw ScanRadius
			Gizmos.color = colors["radius"];
			Gizmos.DrawWireSphere(PlayerLocation, ScanRadius);
			
			//Draw Rope
			for (int i = 1; i < Rope.Count; i++) {
				Gizmos.color = colors["ropejoint"];
				Gizmos.DrawWireSphere(Rope[i], jointSize);
				
				if (i > 0) {
					Gizmos.color = colors["rope"];
					Gizmos.DrawLine(Rope[i - 1], Rope[i]);
				}
			}
			
			//Draw Player Collider
			Gizmos.color = colors["player"];
			Gizmos.DrawWireCube(PlayerLocation, PlayerSize);
			
			//Draw Platforms
			if (drawPlatforms) {
				Gizmos.color = colors["block"];
				int index = platforms.Count;
				for (int i = 0; i < platforms.Count; i++) {
					if (platforms[i].isSolid) {
						index = i;
						break;
					}
					Gizmos.DrawWireCube(platforms[i].position, platforms[i].size);
				}
				
				//Draw Solid Platforms
				if (index != platforms.Count) {
					Gizmos.color = colors["solid"];
					for (int i = index; i < platforms.Count; i++) {
						Gizmos.DrawWireCube(platforms[i].position, platforms[i].size);
					}
				}
			}
		}
	}
}