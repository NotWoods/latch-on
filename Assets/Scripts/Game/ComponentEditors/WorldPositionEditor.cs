#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using LatchOn.ECS.Components.Base;

[CustomEditor(typeof (WorldPosition))]
/// Makes the component blank
public class WorldPositionEditor : Editor {
	Transform transform;

	void OnEnable() {
		WorldPosition pos = (WorldPosition) target;
		transform = pos.transform;
	}

	public override void OnInspectorGUI() {
		serializedObject.Update();
    serializedObject.ApplyModifiedProperties();

		EditorGUILayout.LabelField(transform.position.ToString());
	}
}
#endif
