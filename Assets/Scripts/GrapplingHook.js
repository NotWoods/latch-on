#pragma strict

import System.Collections.Generic;

var retractSpeed = 1.0;
var distanceShift = 3.0;
var crosshairDistance = 2.5;
var offsetMagnitude : float = 0.1;
var minSeperation = 0.1;
var grappleable : LayerMask;
var solid : LayerMask;
var maxDistance : float = 1000;
var autoRetract = false;

var crossTexture : Texture2D;
var hookNoise : AudioClip;
var hookNoise2 : AudioClip;

private var hook : GameObject;
private var hook2 : GameObject;
private var rope : SpringJoint2D;
private var rope2 : SpringJoint2D;
private var dragRope : DistanceJoint2D;
private var dragRope2 : DistanceJoint2D;
private var hookRenderer : Renderer;
private var hookRenderer2 : Renderer;
private var line : LineRenderer;
private var controller : UnityStandardAssets._2D.PlatformerCharacter2D;
private var body : Rigidbody2D;
private var hookJoint : HingeJoint2D;
private var hookBody : Rigidbody2D;
private var hookJoint2 : HingeJoint2D;
private var hookBody2 : Rigidbody2D;
private var crosshair : RectTransform;
private var crosshairRenderer : Canvas;
private var cameraScript : UnityStandardAssets._2D.Camera2DFollow;
private var hookAudio : AudioSource;
private var hookAudio2 : AudioSource;
private var anim : Animator;
private var minimapLine : LineRenderer;
private var minimapLine2 : LineRenderer;

private var active = -1;
private var desiredDistance : float;
private var controllerAim : float = -1.0;
private var controllerActive : boolean = false;
private var justBroken = false;
private var autoRegrapple = true;
private var lastGrappled : Transform;

function Awake() {
	controller = gameObject.GetComponent(UnityStandardAssets._2D.PlatformerCharacter2D);
	cameraScript = Camera.main.GetComponent(UnityStandardAssets._2D.Camera2DFollow);
}

function Start() {
	hook = GameObject.Find("GrappleHook");
	hook2 = GameObject.Find("GrappleHook2");
	rope = GetComponents.<SpringJoint2D>()[0];
	rope2 = GetComponents.<SpringJoint2D>()[1];
	dragRope = GetComponents.<DistanceJoint2D>()[0];
	dragRope2 = GetComponents.<DistanceJoint2D>()[1];
	hookRenderer = hook.GetComponent(Renderer);
	hookRenderer2 = hook2.GetComponent(Renderer);
	line = GetComponent(LineRenderer);
	body = GetComponent(Rigidbody2D);
	hookJoint = hook.GetComponent(HingeJoint2D);
	hookBody = hook.GetComponent(Rigidbody2D);
	hookJoint2 = hook2.GetComponent(HingeJoint2D);
	hookBody2 = hook2.GetComponent(Rigidbody2D);
	crosshair = GameObject.Find("Crosshair").GetComponent(RectTransform);
	crosshairRenderer = crosshair.gameObject.GetComponent(Canvas);
	hookAudio = hook.GetComponent(AudioSource);
	hookAudio2 = hook2.GetComponent(AudioSource);
	anim = GetComponent(Animator);
	minimapLine = GameObject.Find("MM Hook").GetComponent(LineRenderer);
	minimapLine2 = GameObject.Find("MM Hook2").GetComponent(LineRenderer);
	
	rope.connectedBody = hookBody;
	dragRope.connectedBody = hookBody;
}

function Vector3to2(v3 : Vector3) : Vector2 {return new Vector2(v3.x, v3.y);}

function GetMousePoint() : Vector2 {
	var v3 : Vector3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	return new Vector2(v3.x, v3.y);
}

function GrappleRaycast() : RaycastHit2D {
	var angleToMouse : float;
	var mousePoint = GetMousePoint();
	if (mousePoint.y >= transform.position.y) {
		angleToMouse = Vector2.Angle(mousePoint - Vector3to2(transform.position), Vector2.right);
	} else {
		angleToMouse = 360 - Vector2.Angle(mousePoint - Vector3to2(transform.position), Vector2.right);
	}
	if (controllerActive) {angleToMouse = controllerAim;}
	var rotation = Quaternion.AngleAxis(angleToMouse, Vector3.forward) * Vector3.right;
	return Physics2D.Raycast(transform.position, rotation, maxDistance, grappleable);
}

function AngleToPlayer(point : Vector2, dir : Vector2) : float {
	var result = Vector2.Angle(point - transform.position, dir);
	if (point.x > transform.position.x) {result *= -1;}
	return result;
}

function BreakRope(ropeNum : int, noise : boolean) {
	active = -1;
	if (ropeNum == 0) {
		rope.enabled = false;
		dragRope.enabled = false;
		hookJoint.enabled = false;
		hookBody.isKinematic = true;
	} else if (ropeNum == 1) {
		rope2.enabled = false;
		dragRope2.enabled = false;
		hookJoint2.enabled = false;
		hookBody2.isKinematic = true;
	}
	justBroken = true;
}

function Grapple(ropeNum : int, toPoint : Vector2) {
	if (!justBroken || autoRegrapple) {
		var angleToPoint : float;
		//Get correct angle
		if (toPoint.y >= transform.position.y) {
			angleToPoint = Vector2.Angle(toPoint - Vector3to2(transform.position), Vector2.right);
		} else {
			angleToPoint = 360 - Vector2.Angle(toPoint - Vector3to2(transform.position), Vector2.right);
		}
		//if (controllerActive) {angleToPoint = controllerAim;} //If using controller...
		var rotation = Quaternion.AngleAxis(angleToPoint, Vector3.forward) * Vector3.right;
		var grapple = Physics2D.Raycast(transform.position, rotation, maxDistance, grappleable);
		
		if (grapple.collider != null) { //If I hit something
			//Move hook away from center a bit
			var variance = Vector2.zero;
			if (grapple.point.x > grapple.transform.position.x) {variance.x = offsetMagnitude;
			} else {variance.x = offsetMagnitude * -1;}
			if (grapple.point.y > grapple.transform.position.y) {variance.y = offsetMagnitude;
			} else {variance.y = offsetMagnitude * -1;}
			hook.transform.position = grapple.point + variance;
			lastGrappled = grapple.transform; //Save the grappled object
			
			var swingRope : SpringJoint2D; var pullRope : DistanceJoint2D;
			if (ropeNum == 0) {swingRope = rope; pullRope = dragRope;
			} else if (ropeNum == 1) {swingRope = rope2; pullRope = dragRope2;}
			
			if (grapple.rigidbody != null && !grapple.rigidbody.isKinematic) {
				hookJoint.connectedBody = grapple.rigidbody;
				hookJoint.connectedAnchor = grapple.transform.InverseTransformPoint(grapple.point);
				hookJoint.enabled = true;
				hookBody.isKinematic = false;
				
				pullRope.distance = Vector2.Distance(transform.position, hook.transform.position);
				pullRope.enabled = true;
				desiredDistance = pullRope.distance;
				hookAudio.PlayOneShot(hookNoise2, 0.5);
				minimapLine.SetColors(Color.blue, Color.cyan);
			} else {
				if (transform.position.y < hook.transform.position.y) {
					swingRope.enabled = true;
					swingRope.distance = Vector2.Distance(transform.position, hook.transform.position) - distanceShift;
				} else if (body.velocity.x == 0) {
					swingRope.enabled = true;
					swingRope.distance = Vector2.Distance(transform.position, hook.transform.position);
				}
				desiredDistance = swingRope.distance;
				body.fixedAngle = false;
				hookAudio.PlayOneShot(hookNoise);
				minimapLine.SetColors(Color.blue, Color.red);
			}
			active = ropeNum;
		}
	}
}

function CheckRope(ropeNum : int) {
	var currentHook : GameObject; //var swingRope : SpringJoint2D;
	if (ropeNum == 0) {
		currentHook = hook;
		//swingRope = rope; pullRope = dragRope;
	} else if (ropeNum == 1) {
		currentHook = hook2;
		//swingRope = rope2; pullRope = dragRope2;
	}
	var angle = AngleToPlayer(currentHook.transform.position, Vector2.up);
	var hit = Physics2D.Raycast(transform.position, 
		Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up, 
		Vector2.Distance(transform.position, hook.transform.position), solid);
	if (hit.collider != null && (hit.rigidbody == null || hit.rigidbody.isKinematic)) {
		if ((grappleable.value & (1 << hit.transform.gameObject.layer)) > 0) {
			if ((hit.transform.gameObject.layer == LayerMask.NameToLayer("GrappleLock")) && 
					(lastGrappled == hit.transform)) {

			} else {
				Grapple(ropeNum, hit.point);
			}
		} else {
			BreakRope(ropeNum, true);
		}
	}
}

function FixedUpdate() {
	if (Input.GetButton("Grapple")) {
		if (active > -1) {
			if (Input.GetAxis("Rope") != 0) {
				var extension = retractSpeed;
				if (Input.GetAxis("Rope") < 0) {extension *= -1;}
				if (controller.m_Grounded || controller.m_Tilted) {extension = 0;}
				desiredDistance += extension;
				if (desiredDistance < 0) {desiredDistance = 0;}
			}
			if (!hookJoint.enabled) {
				rope.distance = Mathf.Lerp(rope.distance, desiredDistance, Time.deltaTime);
				if (transform.position.y < hook.transform.position.y) {
					rope.enabled = true;
				}
				//CheckRope();
			}
			if (dragRope.enabled) {dragRope.distance = Mathf.Lerp(dragRope.distance, desiredDistance, Time.deltaTime);}
			CheckRope(0);
		} else {
			if (!justBroken || autoRegrapple) {
				var grapple = GrappleRaycast();
				if (grapple.collider != null) {
					var variance = Vector2.zero;
					if (grapple.point.x > grapple.transform.position.x) {variance.x = offsetMagnitude;
					} else {variance.x = offsetMagnitude * -1;}
					if (grapple.point.y > grapple.transform.position.y) {variance.y = offsetMagnitude;
					} else {variance.y = offsetMagnitude * -1;}
					hook.transform.position = grapple.point + variance;
					lastGrappled = grapple.transform;
					if (grapple.rigidbody != null && !grapple.rigidbody.isKinematic) {
						hookJoint.connectedBody = grapple.rigidbody;
						hookJoint.connectedAnchor = grapple.transform.InverseTransformPoint(grapple.point);
						hookJoint.enabled = true;
						hookBody.isKinematic = false;
						
						dragRope.distance = Vector2.Distance(transform.position, hook.transform.position);
						dragRope.enabled = true;
						desiredDistance = dragRope.distance;
						hookAudio.PlayOneShot(hookNoise2, 0.5);
						minimapLine.SetColors(Color.blue, Color.cyan);
					} else {
						if (transform.position.y < hook.transform.position.y) {
							rope.enabled = true;
							rope.distance = Vector2.Distance(transform.position, hook.transform.position) - distanceShift;
						} else if (body.velocity.x == 0) {
							rope.enabled = true;
							rope.distance = Vector2.Distance(transform.position, hook.transform.position);
						}
						desiredDistance = rope.distance;
						body.fixedAngle = false;
						hookAudio.PlayOneShot(hookNoise);
						minimapLine.SetColors(Color.blue, Color.red);
					}
					active = 0;
				}
			}
		}
		if (Input.GetMouseButton(0) && controllerActive) {controllerActive = false;}
	} else {
		/*active = false;
		rope.enabled = false;
		dragRope.enabled = false;
		hookJoint.enabled = false;
		hookBody.isKinematic = true;*/
		BreakRope(0, false);
		justBroken = false;
	}
	if ((controller.m_Grounded || controller.m_Tilted) && !body.fixedAngle && 
		(active == -1) && (body.velocity.magnitude < 1)) {
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
	if (active > -1) {
		line.enabled = true;
		minimapLine.enabled = true;
		hookRenderer.enabled = true;
		line.SetPosition(0, transform.position);
		line.SetPosition(1, hook.transform.position);
		minimapLine.SetPosition(0, transform.position);
		minimapLine.SetPosition(1, hook.transform.position);
	} else {
		line.enabled = false;
		minimapLine.enabled = false;
		hookRenderer.enabled = false;
	}
	anim.SetBool("Grappling", active > -1);
	
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
		//Debug.Log(controllerActive);
	} else {crosshairRenderer.enabled = false; Cursor.visible = true;}
	
	if (Input.GetAxis("Vertical") >= 1) {
		cameraScript.lookOffset = 6 * Vector3.up;
	} else if (Input.GetAxis("Vertical") <= -1) {
		cameraScript.lookOffset = 10 * Vector3.down;
	} else {cameraScript.lookOffset = Vector3.zero;}
	
	if (Input.GetButton("Reel")) {desiredDistance = 0;}
}