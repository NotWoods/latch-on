#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using LatchOn.ECS.Components.Base;

[CanEditMultipleObjects]
[CustomEditor(typeof (WorldPosition))]
/// Makes the component blank
public class WorldPositionEditor : Editor {
	public override void OnInspectorGUI() {
		serializedObject.Update();
    serializedObject.ApplyModifiedProperties();
	}
}
#endif
