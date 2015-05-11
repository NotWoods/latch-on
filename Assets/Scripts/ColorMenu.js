#pragma strict

function SetRobotColor(toggle : Transform) {
	var color : Color = toggle.Find("Background").GetComponent.<UnityEngine.UI.Image>().color;

	GameObject.Find("Robot Preview/Head").GetComponent.<UnityEngine.UI.Image>().color = color;
	GameObject.Find("Robot Preview/Arm Back").GetComponent.<UnityEngine.UI.Image>().color = color;
	GameObject.Find("Robot Preview/Arm Front").GetComponent.<UnityEngine.UI.Image>().color = color;
}

function SaveCustomize() {
	var color : Color = GameObject.Find("Robot Preview/Head").GetComponent.<UnityEngine.UI.Image>().color;
	var name : String = GameObject.Find("Username").GetComponent.<UnityEngine.UI.InputField>().text;
	
	PlayerPrefs.SetString("Player Name", name);
	PlayerPrefs.SetFloat("Robot Color R", color.r);
	PlayerPrefs.SetFloat("Robot Color G", color.g);
	PlayerPrefs.SetFloat("Robot Color B", color.b);
	PlayerPrefs.Save();
}

function LoadCustomize() {
	var toggle = GameObject.Find("Custom").transform;
	var background = toggle.Find("Background").GetComponent.<UnityEngine.UI.Image>();
	var name = GameObject.Find("Username").GetComponent.<UnityEngine.UI.InputField>();
	
	background.color.r = PlayerPrefs.GetFloat("Robot Color R");
	background.color.g = PlayerPrefs.GetFloat("Robot Color G");
	background.color.b = PlayerPrefs.GetFloat("Robot Color B");
	SetRobotColor(toggle);
	
	name.text = PlayerPrefs.GetString("Player Name");
}

function Start() {
	LoadCustomize();
}