#pragma strict

var fadeSpeed : float = 1;
var nextLevel = -1;

private var trigger : BoxCollider2D;
private var playerRenderer : SpriteRenderer;
private var levelDone = false;
private var async : AsyncOperation = null;

function Start() {
	trigger = GetComponent(BoxCollider2D);
	//playerRenderer = GameObject.FindWithTag("Player").GetComponent(SpriteRenderer);
	if (nextLevel < 0) {
		nextLevel = Application.loadedLevel + 1;
		if (nextLevel >= Application.levelCount) {nextLevel = 0;}
	}
}

function OnTriggerEnter2D(other : Collider2D) {
	if (other.gameObject.tag == "Player") 
		levelDone = true;
}

function Update() {
	if (Input.touchCount >= 6) {
		if (async == null) {
			async = Application.LoadLevelAsync(nextLevel);}
	}
	if (levelDone) {
		if (async == null) {
			async = Application.LoadLevelAsync(nextLevel);}
		//playerRenderer.color.a = Mathf.Lerp(playerRenderer.color.a,0,Time.deltaTime*fadeSpeed);
	}
}