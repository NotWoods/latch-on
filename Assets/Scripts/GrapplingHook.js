#pragma strict

import System.Collections.Generic;

var retractSpeed = 1.0;
var crosshairDistance = 2.5;
var offsetMagnitude : float = 0.1;
var minSeperation = 0.1;
var grappleable : LayerMask;
var solid : LayerMask;
var maxDistance : float = 20;

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
private var anim : Animator;
private var colliderBox : BoxCollider2D;

private var active = false;
private var desiredDistance : float;
private var controllerAim : float = -1.0;
private var controllerActive : boolean = false;
private var justBroken = false;
private var autoRegrapple = true;
private var lastGrappled : Transform;
private var cameraPlane : Plane[];

function Awake() {
	controller = gameObject.GetComponent(UnityStandardAssets._2D.PlatformerCharacter2D);
	cameraScript = Camera.main.GetComponent(UnityStandardAssets._2D.Camera2DFollow);
}

function Start() {
	hook = GameObject.Find("GrappleHook");
	rope = GetComponents.<SpringJoint2D>()[0];
	dragRope = GetComponents.<DistanceJoint2D>()[0];
	hookRenderer = hook.GetComponent(Renderer);
	line = GetComponent(LineRenderer);
	body = GetComponent(Rigidbody2D);
	hookJoint = hook.GetComponent(HingeJoint2D);
	hookBody = hook.GetComponent(Rigidbody2D);
	crosshair = GameObject.Find("Crosshair").GetComponent(RectTransform);
	crosshairRenderer = crosshair.gameObject.GetComponent(Canvas);
	hookAudio = hook.GetComponent(AudioSource);
	anim = GetComponent(Animator);
	colliderBox = transform.Find("Minimap").gameObject.GetComponent(BoxCollider2D);
	cameraPlane = GeometryUtility.CalculateFrustumPlanes(Camera.main);
	
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
	return Physics2D.Raycast(transform.position, rotation, maxDistance, solid);
}

function AngleToPlayer(point : Vector2, dir : Vector2) : float {
	var result = Vector2.Angle(point - transform.position, dir);
	if (point.x > transform.position.x) {result *= -1;}
	return result;
}

function BreakRope(noise : boolean) {
	active = false;
	rope.enabled = false;
	dragRope.enabled = false;
	hookJoint.enabled = false;
	hookBody.isKinematic = true;
	justBroken = true;
}

function GrappleInitial(toPoint : Vector2) {
	if (!justBroken || autoRegrapple) {
		var angleToPoint : float;
		//Get correct angle
		if (toPoint.y >= transform.position.y) {
			angleToPoint = Vector2.Angle(toPoint - Vector3to2(transform.position), Vector2.right);
		} else {
			angleToPoint = 360 - Vector2.Angle(toPoint - Vector3to2(transform.position), Vector2.right);
		}
		var rotation = Quaternion.AngleAxis(angleToPoint, Vector3.forward) * Vector3.right;
		var grapple = Physics2D.Raycast(transform.position, rotation, maxDistance, solid);
		
		//If I hit something not metal
		if (grapple.collider != null && (grappleable.value & (1 << grapple.transform.gameObject.layer)) > 0) { 
			//Move hook away from center a bit
			var variance = Vector2.zero;
			if (grapple.point.x > grapple.transform.position.x) {variance.x = offsetMagnitude;
			} else {variance.x = offsetMagnitude * -1;}
			if (grapple.point.y > grapple.transform.position.y) {variance.y = offsetMagnitude;
			} else {variance.y = offsetMagnitude * -1;}
			hook.transform.position = grapple.point + variance;
			lastGrappled = grapple.transform; //Save the grappled object
			
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
				if (transform.position.y < hook.transform.position.y) {
					rope.enabled = true;
					rope.distance = Vector2.Distance(transform.position, hook.transform.position) - 1;
				} else if (body.velocity.x == 0) {
					rope.enabled = true;
					rope.distance = Vector2.Distance(transform.position, hook.transform.position);
				}
				desiredDistance = rope.distance;
				body.fixedAngle = false;
				hookAudio.PlayOneShot(hookNoise);
			}
			active = true;
		}
	}
}

function CheckRope() {
	var angle = AngleToPlayer(hook.transform.position, Vector2.up);
	var hit = Physics2D.Raycast(transform.position, 
		Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up, 
		Vector2.Distance(transform.position, hook.transform.position), solid);
	if (hit.collider != null && (hit.rigidbody == null || hit.rigidbody.isKinematic) && 
	Vector2.Distance(hit.point, hook.transform.position) > minSeperation) {
		if ((grappleable.value & (1 << hit.transform.gameObject.layer)) > 0) {
			if ((hit.transform.gameObject.layer == LayerMask.NameToLayer("GrappleLock")) && 
					(lastGrappled == hit.transform)) {

			} else {
				GrappleInitial(hit.point);
			}
		} else {
			BreakRope(true);
		}
	}
}

function GrappleUpdate() {
	desiredDistance -= retractSpeed;
	if (desiredDistance < 0) {desiredDistance = 0;}
	if (!hookJoint.enabled) {
		rope.distance = Mathf.Lerp(rope.distance, desiredDistance, Time.deltaTime);
		if (transform.position.y < hook.transform.position.y || 
			(Mathf.Abs(body.velocity.x) > (body.velocity.y * -1)) && (Mathf.Abs(body.velocity.y) > 2)) {
			rope.enabled = true;
		}
	}
	if (dragRope.enabled) {dragRope.distance = Mathf.Lerp(dragRope.distance, desiredDistance, Time.deltaTime);}
	CheckRope();
}

function FixedUpdate() {
	if (Input.GetButton("Grapple")) {
		if (active) {
			GrappleUpdate();
		} else {
			GrappleInitial(GetMousePoint());
		}
		if (Input.GetMouseButton(0) && controllerActive) {controllerActive = false;}
	} else {
		BreakRope(false);
		justBroken = false;
	}
	if ((controller.m_Grounded || controller.m_Tilted) && !body.fixedAngle && 
		(!active) && (body.velocity.magnitude < 1)) {
		if (controller.m_Tilted) {transform.position.y += 1;}
		transform.eulerAngles.z = 0;
		body.fixedAngle = true;
	}
	if (Input.GetKey(KeyCode.R) || Input.touchCount >= 3) {
		Respawn();
	}
	if (Input.GetKey(KeyCode.B)) {
		GameObject.Find("CratePink").transform.position = Vector3.left * 5;
	}
	
	/*if (!GeometryUtility.TestPlanesAABB(cameraPlane, colliderBox.bounds)) {
		Respawn();
	} */
}

function RenderHookLine() {
	if (active) {
		line.enabled = true;
		hookRenderer.enabled = true;
		line.SetPosition(0, transform.position);
		line.SetPosition(1, hook.transform.position);
	} else {
		line.enabled = false;
		hookRenderer.enabled = false;
	}
	anim.SetBool("Grappling", active);
}

function ControllerAimInput() {
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
	} else {crosshairRenderer.enabled = false; Cursor.visible = true;}
}

function Update() {
	RenderHookLine();
	ControllerAimInput();
}

function Respawn() {
	transform.position = Vector3.zero;
	body.velocity = Vector3.zero;
	BreakRope(false);
}