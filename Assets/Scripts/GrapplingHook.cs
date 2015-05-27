using UnityEngine;
using System.Collections;

public class GrapplingHook : MonoBehaviour {

	public float retractSpeed = 2.0f;
	public float crosshairDistance = 2.5f;
	public float offsetMagnitude = 0.1f;
	public float minSeperation = 0.1f;
	public LayerMask grappleable;
	public LayerMask solid;
	public float maxDistance = 20f;
	public float armPosZ = 0.48f;
	
	public Texture2D crossTexture;
	public AudioClip hookNoise;
	public AudioClip hookNoise2;
	
	public Transform hook;
	private SpringJoint2D rope;
	private DistanceJoint2D dragRope;
	private Renderer hookRenderer;
	private UnityStandardAssets._2D.PlatformerCharacter2D controller;
	private Rigidbody2D body;
	private HingeJoint2D hookJoint;
	[HideInInspector]
	public Rigidbody2D hookBody;
	[HideInInspector]
	public RectTransform crosshair;
	private Canvas crosshairRenderer;
	[HideInInspector]
	public UnityStandardAssets._2D.Camera2DFollow cameraScript;
	private AudioSource hookAudio;
	private Animator anim;
	private Transform rightArm;
	private Transform rightHand;
	private Transform rightCoil;
	[HideInInspector]
	public Transform handUpper;
	[HideInInspector]
	public Transform handLower;

	[HideInInspector]
	public bool active = false;
	private float desiredDistance;
	private float controllerAim = -1.0f;
	private bool controllerActive = false;
	private bool justBroken = false;
	private bool autoRegrapple = true;
	[HideInInspector]
	public Transform lastGrappled;

	void Start() {
		controller = gameObject.GetComponent<UnityStandardAssets._2D.PlatformerCharacter2D>();
		rope = GetComponent<SpringJoint2D>();
		dragRope = GetComponent<DistanceJoint2D>();
		body = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		rightArm = transform.Find("robot/Arm_Right");
		rightHand = rightArm.Find("Arm_Front");
		rightCoil = GameObject.Find("ArmPart Seperate").transform;
	}

	public void StartTwo() {
		hookRenderer = hook.GetComponent<Renderer>();
		hookJoint = hook.GetComponent<HingeJoint2D>();
		hookAudio = hook.GetComponent<AudioSource>();
		crosshairRenderer = crosshair.gameObject.GetComponent<Canvas>();
		handUpper = hook.Find("Hand_Top");
		handLower = hook.Find("Hand_Bottom");
		GetComponent<SpringJoint2D>().connectedBody = hookBody;
		GetComponent<DistanceJoint2D>().connectedBody = hookBody;
	}

	Vector2 Vector3to2(Vector3 v3) {return new Vector2(v3.x, v3.y);}

	Vector2 GetMousePoint() {
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = Mathf.Abs(Camera.main.transform.position.z);
		Vector3 v3 = Camera.main.ScreenToWorldPoint(mousePos);
		return new Vector2(v3.x, v3.y);
	}
	
	RaycastHit2D GrappleRaycast(Vector2 point) {
		float angle;
		if (point.y >= transform.position.y) {
			angle = Vector2.Angle(point - Vector3to2(transform.position), Vector2.right);
		} else {
			angle = 360f - Vector2.Angle(point - Vector3to2(transform.position), Vector2.right);
		}
		if (controllerActive) {angle = controllerAim;}
		Vector3 rotation = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
		return Physics2D.Raycast(transform.position, rotation, maxDistance, solid);
	}
	
	float AngleToPoint(Vector2 point, bool flip) {
		Vector2 side = Vector2.right; if (!flip) {side *= -1f;}
		float result = Vector2.Angle(point - Vector3to2(transform.position), side);
		if (point.y < transform.position.y) {result = 360f - result;}
		return result; 
	}
	
	float AngleToPlayer(Vector2 point, Vector2 dir) {
		float result = Vector2.Angle(point - Vector3to2(transform.position), dir);
		if (point.x > transform.position.x) {result *= -1f;}
		return result;
	}

	void BreakRope(bool noise) {
		active = false;
		rope.enabled = false;
		dragRope.enabled = false;
		hookJoint.enabled = false;
		hookBody.isKinematic = true;
		justBroken = true;
	}

	void GrappleInitial(Vector2 toPoint) {
		if (!justBroken || autoRegrapple) {
			RaycastHit2D grapple = GrappleRaycast(toPoint);

			if (grapple.collider != null && (grappleable.value & (1 << grapple.transform.gameObject.layer)) > 0) { 
				Vector2 variance = Vector2.zero;
				if (grapple.point.x > grapple.transform.position.x) {variance.x = offsetMagnitude;
				} else {variance.x = offsetMagnitude * -1f;}
				if (grapple.point.y > grapple.transform.position.y) {variance.y = offsetMagnitude;
				} else {variance.y = offsetMagnitude * -1f;}
				hook.position = grapple.point + variance;
				hook.position += Vector3.back*armPosZ;
				lastGrappled = grapple.transform; 
				
				if (grapple.rigidbody != null && !grapple.rigidbody.isKinematic) {
					hookJoint.connectedBody = grapple.rigidbody;
					hookJoint.connectedAnchor = grapple.transform.InverseTransformPoint(grapple.point);
					hookJoint.enabled = true;
					hookBody.isKinematic = false;
					
					dragRope.distance = Vector2.Distance(transform.position, hook.position);
					dragRope.enabled = true;
					desiredDistance = dragRope.distance;
					hookAudio.PlayOneShot(hookNoise2, 0.5f);
				} else {
					rope.enabled = true;
					rope.distance = Vector2.Distance(transform.position, hook.position) - 1f;
					
					desiredDistance = rope.distance;
					body.fixedAngle = false;
					hookAudio.PlayOneShot(hookNoise);
				}
				active = true;
			}
		}
	}

	public void GrappleInitialSync() {
		Transform blocks = GameObject.Find("World").transform;
		float closestDist = Mathf.Infinity;
		foreach (Transform block in blocks) {
			if ((grappleable.value & (1 << block.gameObject.layer)) > 0) {
				float dist = Vector2.Distance(hook.position, block.position);
				if (dist < closestDist) {
					closestDist = dist;
					lastGrappled = block;
				}
			}
		}
		Rigidbody2D lastRigidbody = lastGrappled.GetComponent<Rigidbody2D>();
		if (lastRigidbody != null && !lastRigidbody.isKinematic) {
			hookJoint.connectedBody = lastRigidbody;
			hookJoint.connectedAnchor = lastGrappled.InverseTransformPoint(hook.position);
			hookJoint.enabled = true;
			hookBody.isKinematic = false;
			
			dragRope.distance = Vector2.Distance(transform.position, hook.position);
			dragRope.enabled = true;
			desiredDistance = dragRope.distance;
			hookAudio.PlayOneShot(hookNoise2, 0.5f);
		} else {
			rope.enabled = true;
			rope.distance = Vector2.Distance(transform.position, hook.position) - 1f;
			
			desiredDistance = rope.distance;
			body.fixedAngle = false;
			hookAudio.PlayOneShot(hookNoise);
		}
		active = true;
	}

	void CheckRope() {
		float angle = AngleToPlayer(hook.position, Vector2.up);
		RaycastHit2D hit = Physics2D.Raycast(transform.position, 
		                            Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up, 
		                            Vector2.Distance(transform.position, hook.position), solid);
		if (hit.collider != null && (hit.rigidbody == null || hit.rigidbody.isKinematic) && 
		    Vector2.Distance(hit.point, hook.position) > minSeperation) {
			if ((grappleable.value & (1 << hit.transform.gameObject.layer)) > 0) {
				if (!((hit.transform.gameObject.layer == LayerMask.NameToLayer("GrappleLock")) && (lastGrappled == hit.transform))) {
					GrappleInitial(hit.point);}
			} else {
				BreakRope(true);
			}
		}
	}

	void GrappleUpdate() {
		desiredDistance -= retractSpeed;
		if (desiredDistance < 0) {desiredDistance = 0;}
		if (!hookJoint.enabled) {rope.distance = Mathf.Lerp(rope.distance, desiredDistance, Time.deltaTime);}
		if (dragRope.enabled) {dragRope.distance = Mathf.Lerp(dragRope.distance, desiredDistance, Time.deltaTime);}
		CheckRope();
	}

	void FixedUpdate() {
		if (Input.GetButton("Grapple") && Input.touchCount < 2) {
			if (active) {
				GrappleUpdate();
			} else {
				GrappleInitial(GetMousePoint());
			}
			if (Input.GetMouseButton(0) && controllerActive) {controllerActive = false;}
		} else {BreakRope(false); justBroken = false;}
		if ((controller.m_Grounded || controller.m_Tilted) && !body.fixedAngle && (!active) && (body.velocity.magnitude < 1f)) {
			if (controller.m_Tilted) {transform.position += Vector3.up;}
			//transform.eulerAngles.z = 0f;
			transform.rotation = Quaternion.identity;
			body.fixedAngle = true;
		}
		if (Input.GetKey(KeyCode.R) || Input.touchCount >= 3) {Respawn();}
	}

	public void Respawn() {
		transform.position = Vector3.zero;
		body.velocity = Vector3.zero;
		BreakRope(false);
	}

	void RenderHookLine() {
		if (active) {
			hookRenderer.enabled = true;
			handUpper.gameObject.SetActive(true);
			handLower.gameObject.SetActive(true);
			
			Vector3 between = hook.position - rightArm.position;
			float distance = between.magnitude;
			rightCoil.localScale = new Vector3(rightCoil.localScale.x, rightCoil.localScale.y, distance);// * 0.56448285185f);
			rightCoil.position = rightArm.position + (between / 2f);
			rightCoil.LookAt(hook.position);
			rightCoil.GetComponent<MeshRenderer>().enabled = true;
		} else {
			hookRenderer.enabled = false;
			handUpper.gameObject.SetActive(false);
			handLower.gameObject.SetActive(false);
			
			rightCoil.localPosition = Vector3.right * 0.151f;
			rightCoil.rotation = Quaternion.identity;
			rightCoil.localScale = new Vector3(0.15f, 0.15f, 0.15f);
			rightCoil.GetComponent<MeshRenderer>().enabled = false;
		}
		anim.SetBool("Grappling", active);
	}

	void ControllerAimInput() {
		if ((Input.GetAxis("AimHorizontal") != 0f) || (Input.GetAxis("AimVertical") != 0f)) {
			if (Input.GetAxis("AimVertical") < 0f) {
				controllerAim = Vector2.Angle(new Vector2(Input.GetAxis("AimHorizontal"), Input.GetAxis("AimVertical")), Vector2.right);
			} else {
				controllerAim = 360f - Vector2.Angle(new Vector2(Input.GetAxis("AimHorizontal"), Input.GetAxis("AimVertical")), Vector2.right);
			}
			controllerActive = true;
		}
		if (controllerActive) {
			crosshairRenderer.enabled = true;
			crosshair.position = transform.position + Vector3.back + (crosshairDistance * (Quaternion.AngleAxis(controllerAim, Vector3.forward) * Vector3.right));
			Cursor.visible = false;
		} else {crosshairRenderer.enabled = false; Cursor.visible = true;}
	}

	void Update() {
		RenderHookLine();
		ControllerAimInput();
		
		float armAngle;
		if (active) {
			armAngle = AngleToPoint(Vector3to2(hook.position), true);
			rightHand.gameObject.SetActive(false);
		} else {
			armAngle = AngleToPoint(GetMousePoint(), controller.m_FacingRight);
			rightHand.gameObject.SetActive(true);
		}
		Vector3 initRotation = rightArm.rotation.eulerAngles;
		initRotation.z = armAngle;
		rightArm.rotation = Quaternion.Euler(initRotation);
		hook.rotation = Quaternion.Euler(initRotation);
	}
}