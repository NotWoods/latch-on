#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
// using LatchOn.ECS.Components.Mover;

[CanEditMultipleObjects]
[CustomEditor(typeof (MoveState))]
public class MoveStateEditor : Editor {
	SerializedProperty val;

	void OnEnable() {
		val = serializedObject.FindProperty("Value");
	}

	public override void OnInspectorGUI() {
		serializedObject.Update();
  	EditorGUILayout.PropertyField(val, GUIContent.none);
  	serializedObject.ApplyModifiedProperties();
	}
}
#endif
