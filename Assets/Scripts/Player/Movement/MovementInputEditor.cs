using UnityEditor;
using UnityEngine;
using Player;

[ExecuteInEditMode]
[CustomEditor(typeof(MovementInput))]
public class MovementInputEditor : Editor {
	SerializedProperty gravity;
	SerializedProperty jumpVelocity;
	SerializedProperty speed;

	float lastJumpHeight = 0;
	float lastTimeToJumpApex = 0;

	float jumpHeight = 4;
	float timeToJumpApex = 0.4f;

	void OnEnable() {
		gravity = serializedObject.FindProperty("gravity");
		jumpVelocity = serializedObject.FindProperty("jumpVelocity");
		speed = serializedObject.FindProperty("speed");
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

		serializedObject.ApplyModifiedProperties();
	}
}