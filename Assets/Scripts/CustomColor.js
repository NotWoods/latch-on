#pragma strict

private var background : UnityEngine.UI.Image;
private var toggle : UnityEngine.UI.Toggle;
private var robotColorScript : ColorMenu;

private var redSlider : UnityEngine.UI.Slider;
private var greenSlider : UnityEngine.UI.Slider;
private var blueSlider : UnityEngine.UI.Slider;

private var sliderPane : GameObject;

function Awake() {
	background = transform.Find("Background").GetComponent.<UnityEngine.UI.Image>();
	toggle = GetComponent.<UnityEngine.UI.Toggle>();
	robotColorScript = GameObject.Find("Robot Preview").GetComponent.<ColorMenu>();
	
	redSlider = GameObject.Find("Custom Color/RED").GetComponent.<UnityEngine.UI.Slider>();
	greenSlider = GameObject.Find("Custom Color/GREEN").GetComponent.<UnityEngine.UI.Slider>();
	blueSlider = GameObject.Find("Custom Color/BLUE").GetComponent.<UnityEngine.UI.Slider>();
	
	sliderPane = GameObject.Find("Custom Color");
	sliderPane.SetActive(false);
}

function UpdateColor() {
	background.color.r = redSlider.value;
	background.color.g = greenSlider.value;
	background.color.b = blueSlider.value;
	UpdateRobot();
}

function UpdateRobot() {
	toggle.isOn = true;
	robotColorScript.SetRobotColor(transform);
}

function CustomToggled() {
	sliderPane.SetActive(toggle.isOn);
	robotColorScript.SetRobotColor(transform);
}