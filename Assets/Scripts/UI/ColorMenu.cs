using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace UI {
	public class ColorMenu : MonoBehaviour {
		public string[] paths = new string[] {
			"Robot Preview/Head", 
			"Robot Preview/Arm Back", 
			"Robot Preview/Arm Front"
		};
		List<Image> previewImages;
		Image background;
		InputField nameField;

		public void SetRobotColor(Color color) {
			foreach (Image img in previewImages) img.color = color;
		}

		void Start() {
			foreach (string path in paths) {
				previewImages.Add(GameObject.Find(path).GetComponent<Image>());
			}
			background = GameObject.Find("Custom/Background").GetComponent<Image>();
			nameField = GameObject.Find("Username").GetComponent<InputField>();
			LoadCustomize();
		}

		public void SaveCustomize() {
			Color color = previewImages[0].color;
			string name = nameField.text;

			PlayerPrefs.SetString("Player Name", name);
			PlayerPrefs.SetFloat("Robot Color R", color.r);
			PlayerPrefs.SetFloat("Robot Color G", color.g);
			PlayerPrefs.SetFloat("Robot Color B", color.b);
			PlayerPrefs.Save();
		}

		public void LoadCustomize() {
			background.color = new Color(
				PlayerPrefs.GetFloat("Robot Color R"),
				PlayerPrefs.GetFloat("Robot Color G"),
				PlayerPrefs.GetFloat("Robot Color B")
			);
			SetRobotColor(background.color);

			nameField.text = PlayerPrefs.GetString("Player Name");
		}
	}
}