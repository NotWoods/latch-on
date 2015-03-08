#pragma strict

private var map : Camera;

function Start () {
	map = GetComponent(Camera);
}

function Update () {
	if (Input.GetKeyDown(KeyCode.M)) {
		map.enabled = !map.enabled;
	}
}