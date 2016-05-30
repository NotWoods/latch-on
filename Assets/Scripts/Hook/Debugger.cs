using UnityEngine;
using System.Collections.Generic;

namespace Hook {
	/// <summary>
	/// Draws debug wireframes when its gameobject is selected</summary>
	public class Debugger : MonoBehaviour {
		/// <summary>Colors to draw with, keyed based on their purpose</summary>
		public Dictionary<string, Color> colors = new Dictionary<string, Color>();
		
		public bool drawPlatforms = false; // draw platform wireframes?
		public float jointSize = 1.0f; // the size of the joint sphere wireframes
		
		public Vector2 PlayerLocation; // Current location of the player
		public float ScanRadius; // Radius that the grappling hook reaches
		public List<Vector2> Rope = new List<Vector2>(); // List of joints
		private List<Block> platforms = new List<Block>(); // List of platforms
		public List<Block> Platforms { // Sorts the platforms when set based on type
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
		public Vector2 PlayerSize; // Dimensions of the player
		
		/// <summary>Sets default colors</summary>
		void Awake() {
			colors.Add("radius", Color.red);
			colors.Add("rope", Color.green);
			colors.Add("ropejoint", Color.cyan);
			
			colors.Add("player", Color.blue);
			
			colors.Add("block", Color.white);
			colors.Add("solid", Color.gray);
		}
		
		/// <summary>Called every frame to draw wireframes</summary>
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