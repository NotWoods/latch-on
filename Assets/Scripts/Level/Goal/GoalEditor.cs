using UnityEditor;
using Level;

[CustomEditor(typeof(Goal))]
[CanEditMultipleObjects]
public class GoalEditor : Editor {
	SerializedProperty level;
	SerializedProperty delaySeconds;
	SerializedProperty secondsRemaining;

	void OnEnable() {
		level = serializedObject.FindProperty("level");
		delaySeconds = serializedObject.FindProperty("delaySeconds");
		secondsRemaining = serializedObject.FindProperty("secondsRemaining");
	}

	public override void OnInspectorGUI() {
		serializedObject.Update();
		
		EditorGUILayout.PropertyField(level);
		EditorGUILayout.PropertyField(delaySeconds);
		EditorGUILayout.LabelField(
			"Seconds Remaining: ",
			secondsRemaining.stringValue
		);
		serializedObject.ApplyModifiedProperties();
	}
}