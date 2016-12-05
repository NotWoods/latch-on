using UnityEngine;
using UnityEngine.UI;

namespace UI {
	public class CustomColor : MonoBehaviour {
		Image background;
		Toggle toggle;
		Slider redSlider, greenSlider, blueSlider;
		GameObject sliderPane; 

		ColorMenu robotColorScript;

		void Awake() {
			background = transform.Find("Background").GetComponent<Image>();
			toggle = GetComponent<Toggle>();
			robotColorScript = GameObject.Find("Robot Preview")
				.GetComponent<ColorMenu>();
			redSlider = GameObject.Find("Custom Color/RED").GetComponent<Slider>();
			greenSlider = GameObject.Find("Custom Color/GREEN")
				.GetComponent<Slider>();
			blueSlider = GameObject.Find("Custom Color/BLUE").GetComponent<Slider>();

			sliderPane = GameObject.Find("Custom Color");
			sliderPane.SetActive(false);
		}

		public Color UpdateColor() {
			Color c = new Color(
				redSlider.value, greenSlider.value, blueSlider.value
			);
			background.color = c;
			UpdateRobot(c);
			return c;
		}

		public void UpdateRobot(Color color) {
			toggle.isOn = true;
			robotColorScript.SetRobotColor(color);
		}

		public void UpdateRobot() {
			UpdateRobot(background.color);
		}

		public void CustomToggled() {
			sliderPane.SetActive(toggle.isOn);
			robotColorScript.SetRobotColor(background.color);
		}
	}
}