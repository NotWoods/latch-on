#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using LatchOn.ECS.Components.Base;

[CanEditMultipleObjects]
[CustomEditor(typeof (Speed))]
public class SpeedEditor : Editor {
	SerializedProperty val;

	void OnEnable() {
		val = serializedObject.FindProperty("_value");
	}

	public override void OnInspectorGUI() {
		serializedObject.Update();
  	EditorGUILayout.PropertyField(val, GUIContent.none);
  	serializedObject.ApplyModifiedProperties();
	}
}
#endif
