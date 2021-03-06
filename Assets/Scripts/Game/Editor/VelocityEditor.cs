#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using LatchOn.ECS.Components.Base;

[CanEditMultipleObjects]
[CustomEditor(typeof (Velocity))]
public class VelocityEditor : Editor {
	SerializedProperty val;

	void OnEnable() {
		val = serializedObject.FindProperty("Value");
	}

	public override void OnInspectorGUI() {
		serializedObject.Update();
  	EditorGUILayout.PropertyField(val, GUIContent.none);
  	serializedObject.ApplyModifiedProperties();

		EditorGUILayout.LabelField("Speed: " + val.vector2Value.magnitude);
		EditorGUILayout.LabelField("Direction: " + val.vector2Value.normalized);
	}
}
#endif
