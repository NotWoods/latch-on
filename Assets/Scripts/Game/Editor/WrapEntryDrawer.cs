#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using LatchOn.ECS.Components.Rope;

[CustomPropertyDrawer(typeof (WrappingLine.Entry))]
public class WrapEntryDrawer : PropertyDrawer {
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		// Using BeginProperty / EndProperty on the parent property means that
    // prefab override logic works on the entire property.
    EditorGUI.BeginProperty(position, label, property);

		SerializedProperty point = property.FindPropertyRelative("point");
		GUIContent pointLabel = new GUIContent(point.vector2Value.ToString());

		// Draw label
  	position = EditorGUI.PrefixLabel(position,
			GUIUtility.GetControlID(FocusType.Passive), pointLabel);

    // Don't make child fields be indented
    int indent = EditorGUI.indentLevel;
    EditorGUI.indentLevel = 0;

		float third = position.width / 3;
		float pointRectSize = Mathf.Floor(third * 2);

    // Calculate rects
    Rect pointRect = new Rect(position.x, position.y,
			pointRectSize, position.height);
    Rect sideRect = new Rect(position.x + pointRectSize + 5, position.y,
			position.width - (pointRectSize + 5), position.height);

    // Draw fields - passs GUIContent.none to each so they are drawn without labels
    EditorGUI.PropertyField(pointRect, point, GUIContent.none);
    EditorGUI.PropertyField(sideRect, property.FindPropertyRelative("side"),
			GUIContent.none);

    // Set indent back to what it was
    EditorGUI.indentLevel = indent;

    EditorGUI.EndProperty();
	}
}
#endif
