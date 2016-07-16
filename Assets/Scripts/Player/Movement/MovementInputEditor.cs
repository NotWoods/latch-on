using UnityEditor;
using UnityEngine;
using Player;

[ExecuteInEditMode]
[CustomEditor(typeof(MovementInput))]
public class MovementInputEditor : Editor {
	SerializedProperty gravity;
	SerializedProperty jumpVelocity;

	float lastJumpHeight = 0;
	float lastTimeToJumpApex = 0;

	float jumpHeight = 4;
	float timeToJumpApex = 0.4f;

	SerializedProperty speed;
	SerializedProperty accelGround;
	SerializedProperty accelAir;

	void OnEnable() {
		gravity = serializedObject.FindProperty("gravity");
		jumpVelocity = serializedObject.FindProperty("jumpVelocity");
		speed = serializedObject.FindProperty("speed");
		accelGround = serializedObject.FindProperty("accelerationTimeGrounded");
		accelAir = serializedObject.FindProperty("accelerationTimeAirborne");
		RecalculateVariables(ref gravity, ref jumpVelocity);
	}

	void RecalculateVariables(
		ref SerializedProperty gravity, ref SerializedProperty jumpVelocity
	) {
		if (jumpHeight != lastJumpHeight || timeToJumpApex != lastTimeToJumpApex) {
			lastJumpHeight = jumpHeight; lastTimeToJumpApex = timeToJumpApex;

			gravity.floatValue = (2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
			jumpVelocity.floatValue = gravity.floatValue * timeToJumpApex;
		}
	}

	public override void OnInspectorGUI() {
		serializedObject.Update();

		jumpHeight = EditorGUILayout.FloatField("Jump Height", jumpHeight);
		timeToJumpApex = EditorGUILayout.FloatField(
			"Time To Jump Apex", timeToJumpApex);

		RecalculateVariables(ref gravity, ref jumpVelocity);

		EditorGUILayout.SelectableLabel("Gravity: " + gravity.floatValue 
			+ ", Jump Velocity: " + jumpVelocity.floatValue);

		EditorGUILayout.PropertyField(speed);
		EditorGUILayout.PropertyField(accelGround);
		EditorGUILayout.PropertyField(accelAir);

		serializedObject.ApplyModifiedProperties();
	}
}