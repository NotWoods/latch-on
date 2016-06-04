namespace Level {
	[Serializable]
	public class Main {
		public string name;
		public Platform[] platforms;
		
		public Main LoadFile(string url) {
			StartCoroutine(Text.Load(url, (text) => {
				Main level = JsonUtility.FromJson<Main>(text);
				BuildLevel(level);
			}))
		}
		
		public void BuildLevel(Main level) {
			
		}
	}
}