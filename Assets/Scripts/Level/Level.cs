using UnityEngine;

namespace Level {
	[System.Serializable]
	public class Main : MonoBehaviour {
		public string label;
		public Platform[] platforms;
		
		public void LoadFile(string url) {
			StartCoroutine(Text.Load(url, (text) => {
				Main level = JsonUtility.FromJson<Main>(text);
				BuildLevel(level);
			}));
		}
		
		public void BuildLevel(Main level) {
			
		}
	}
}