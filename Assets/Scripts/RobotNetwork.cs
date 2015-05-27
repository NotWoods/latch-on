using UnityEngine;
using System.Collections;

public class RobotNetwork : Bolt.EntityBehaviour<IRobot> {
	GrapplingHook grappleScript;

	public override void Attached() {
		grappleScript = GetComponent<GrapplingHook>();

		state.RobotTransform.SetTransforms(transform);
		state.HookTransform.SetTransforms(grappleScript.hook);

		if (entity.isOwner) {
			state.MainColor = transform.Find("robot/Arm_Right").GetComponent<Renderer>().material.color;
		}

		state.AddCallback("MainColor", ColorChanged);
		state.AddCallback("Grappling", SyncGrapple);
	}

	void ColorChanged() {
		transform.Find("robot/Arm_Right").GetComponent<Renderer>().material.color = state.MainColor;
		transform.Find("robot/Arm_Left").GetComponent<Renderer>().material.color = state.MainColor;
		transform.Find("robot/Arm_Right/Arm_Front").GetComponent<Renderer>().material.color = state.MainColor;
		transform.Find("robot/Arm_Left/Arm_Front_001").GetComponent<Renderer>().material.color = state.MainColor;
		transform.Find("robot/Head").GetComponent<Renderer>().materials[1].color = state.MainColor;
		grappleScript.hook.GetComponent<Renderer>().material.color = state.MainColor;

		Color dark = new Color(state.MainColor.r - 0.125f, state.MainColor.g - 0.125f, state.MainColor.b - 0.125f);
		transform.Find("robot/Head").GetComponent<Renderer>().materials[0].color = dark;
	}

	void SyncGrapple() {
		grappleScript.GrappleInitialSync();
	}
}
