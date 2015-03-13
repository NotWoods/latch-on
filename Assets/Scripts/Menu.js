#pragma strict

var grappleCursor : Texture2D;
var menuCursor : Texture2D;

private var map : Camera;

function Start() {
	map = GameObject.Find("Minimap Camera").GetComponent(Camera);

	Cursor.SetCursor(grappleCursor, new Vector2(16, 16), CursorMode.Auto);
}

function Update () {
	if (Input.GetKeyDown(KeyCode.Escape)) {
		if (Time.timeScale == 1) {Time.timeScale = 0;} else {Time.timeScale = 1;}
	}
	if (Input.GetKeyDown(KeyCode.M)) {
		map.enabled = !map.enabled;
	}
}

function Respawn() {Application.LoadLevel(Application.loadedLevel);}
