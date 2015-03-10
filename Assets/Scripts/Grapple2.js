#pragma strict

//var hook : GameObject;
var retractSpeed = 1.0;
var distanceShift = 3.0;
var crosshairDistance = 2.5;
var crossTexture : Texture2D;
var hookNoise : AudioClip;
var hookNoise2 : AudioClip;

private var hook : GameObject;
private var rope : SpringJoint2D;
private var dragRope : DistanceJoint2D;
private var hookRenderer : Renderer;
private var line : LineRenderer;
private var controller : UnityStandardAssets._2D.PlatformerCharacter2D;
private var body : Rigidbody2D;
private var hookJoint : HingeJoint2D;
private var hookBody : Rigidbody2D;
private var crosshair : RectTransform;
private var crosshairRenderer : Canvas;
private var cameraScript : UnityStandardAssets._2D.Camera2DFollow;
private var hookAudio : AudioSource;

private var active = false;
private var desiredDistance : float;
private var controllerAim : float = -1.0;
private var controllerActive : boolean = false;
private var hookPoints : Vector2[];

function Awake() {
	controller = gameObject.GetComponent(UnityStandardAssets._2D.PlatformerCharacter2D);
	cameraScript = Camera.main.GetComponent(UnityStandardAssets._2D.Camera2DFollow);
}

function Start() {
	hook = GameObject.Find("GrappleHook");
	rope = GetComponent(SpringJoint2D);
	dragRope = GetComponent(DistanceJoint2D);
	hookRenderer = hook.GetComponent(Renderer);
	line = GetComponent(LineRenderer);
	body = GetComponent(Rigidbody2D);
	hookJoint = hook.GetComponent(HingeJoint2D);
	hookBody = hook.GetComponent(Rigidbody2D);
	crosshair = GameObject.Find("Crosshair").GetComponent(RectTransform);
	crosshairRenderer = crosshair.gameObject.GetComponent(Canvas);
	hookAudio = hook.GetComponent(AudioSource);
	
	rope.connectedBody = hookBody;
	dragRope.connectedBody = hookBody;
}

function Vector3to2(v3 : Vector3) : Vector2 {return new Vector2(v3.x, v3.y);}

function GetMousePoint() : Vector2 {
	var v3 : Vector3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	return new Vector2(v3.x, v3.y);
}

function GrappleLinecast() : RaycastHit2D {
	return Physics2D.Linecast(Vector3to2(transform.position), GetMousePoint());
}
function GrappleRaycast() : RaycastHit2D {
	var angleToMouse : float;
	var mousePoint = GetMousePoint();
	if (mousePoint.y >= transform.position.y) {
		angleToMouse = Vector2.Angle(mousePoint - Vector3to2(transform.position), Vector3.right);
	} else {
		angleToMouse = 360 - Vector2.Angle(mousePoint - Vector3to2(transform.position), Vector3.right);
	}
	if (controllerActive) {angleToMouse = controllerAim;}
	var rotation = Quaternion.AngleAxis(angleToMouse, Vector3.forward) * Vector3.right;
	Debug.DrawRay(transform.position, rotation);
	return Physics2D.Raycast(transform.position, rotation);
}

function FixedUpdate() {
	if (Input.GetButton("Grapple")) {
		if (active) {
			if (Input.GetAxis("Rope") != 0) {
				if (controller.m_Grounded || controller.m_Tilted) {}
				desiredDistance += (Input.GetAxis("Rope") * retractSpeed);
				if (desiredDistance < 0) {desiredDistance = 0;}
			}
			if (!hookJoint.enabled) {
				rope.distance = Mathf.Lerp(rope.distance, desiredDistance, Time.deltaTime);
				if (transform.position.y < hook.transform.position.y && body.velocity != Vector3.zero) {
					rope.enabled = true;
				}
				var hit = GrappleRaycast();
				if (hit.collider != null) {
					//Create grapple at this point to connect to old one
					
				}
			}
			if (dragRope.enabled) {dragRope.distance = Mathf.Lerp(dragRope.distance, desiredDistance, Time.deltaTime);}
		} else {
			var grapple = GrappleRaycast();
			if (grapple.collider != null) {
				hook.transform.position = grapple.point;
				if (grapple.rigidbody != null && !grapple.rigidbody.isKinematic) {
					hookJoint.connectedBody = grapple.rigidbody;
					hookJoint.connectedAnchor = grapple.transform.InverseTransformPoint(grapple.point);
					hookJoint.enabled = true;
					hookBody.isKinematic = false;
					
					dragRope.distance = Vector2.Distance(transform.position, hook.transform.position);
					dragRope.enabled = true;
					desiredDistance = dragRope.distance;
					hookAudio.PlayOneShot(hookNoise2, 0.5);
				} else {
					rope.distance = Vector2.Distance(transform.position, hook.transform.position) - distanceShift;
					if (transform.position.y < hook.transform.position.y) {
						rope.enabled = true;
					}
					desiredDistance = rope.distance;
					body.fixedAngle = false;
					hookAudio.PlayOneShot(hookNoise);
				}
				active = true;
			}
		}
		if (Input.GetMouseButton(0) && controllerActive) {controllerActive = false;}
	} else {
		active = false;
		rope.enabled = false;
		dragRope.enabled = false;
		hookJoint.enabled = false;
		hookBody.isKinematic = true;
	}
	if ((controller.m_Grounded || controller.m_Tilted) && !body.fixedAngle && !active) {
		if (controller.m_Tilted) {transform.position.y += 1;}
		transform.eulerAngles.z = 0;
		body.fixedAngle = true;
	}
	if (Input.GetKey(KeyCode.R)) {
		transform.position = Vector3.zero;
	}
	if (Input.GetKey(KeyCode.B)) {
		GameObject.Find("CratePink").transform.position = Vector3.left * 5;
	}
}

function Update() {
	if (active) {
		line.enabled = true;
		hookRenderer.enabled = true;
		line.SetPosition(0, transform.position);
		line.SetPosition(1, hook.transform.position);
	} else {
		line.enabled = false;
		hookRenderer.enabled = false;
	}
	
	if ((Input.GetAxis("AimHorizontal") != 0) || (Input.GetAxis("AimVertical") != 0)) {
		if (Input.GetAxis("AimVertical") < 0) {
			controllerAim = Vector2.Angle(new Vector2(Input.GetAxis("AimHorizontal"), 
				Input.GetAxis("AimVertical")), Vector2.right);
		} else {
			controllerAim = 360 - Vector2.Angle(new Vector2(Input.GetAxis("AimHorizontal"), 
				Input.GetAxis("AimVertical")), Vector2.right);
		}
		controllerActive = true;
	}
	if (controllerActive) {
		crosshairRenderer.enabled = true;
		crosshair.position = transform.position + Vector3.back + (crosshairDistance * 
			(Quaternion.AngleAxis(controllerAim, Vector3.forward) * Vector3.right));
		Cursor.visible = false;
		Debug.Log(controllerActive);
	} else {crosshairRenderer.enabled = false; Cursor.visible = true;}
	
	if (Input.GetAxis("Vertical") >= 1) {
		cameraScript.lookOffset = 6 * Vector3.up;
	} else if (Input.GetAxis("Vertical") <= -1) {
		cameraScript.lookOffset = 10 * Vector3.down;
	} else {cameraScript.lookOffset = Vector3.zero;}
	
	if (Input.GetButton("Reel")) {desiredDistance = 0;}
}