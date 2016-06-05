using UnityEngine;
using System.Collections;

namespace Level {
	public class Text {
		public static IEnumerator Load(string url, System.Action<string> callback) {
			url = WWW.EscapeURL(url);
			WWW www = new WWW(url);
			yield return www;
			callback(www.text);
		}
	}
}