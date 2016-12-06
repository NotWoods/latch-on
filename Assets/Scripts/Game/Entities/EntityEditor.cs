using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof (Entity))]
public class EntityEditor : Editor {
	private static EntityManager Manager = EntityManager.Instance;
	private Dictionary<Type, bool> showFoldout = new Dictionary<Type, bool>();

	public override void OnInspectorGUI() {
		SerializedProperty id = serializedObject.FindProperty("ID");
		int entityId = id.intValue;

		EditorGUILayout.LabelField("ID: ", entityId.ToString());
		foreach (IComponent c in Manager.GetComponents(entityId)) {
			if (!(c is UnityEngine.Object)) {
				Debug.LogWarning("IComponent is not an object, cannot inspect");
				continue;
			}

			UnityEngine.Object targetObj = (UnityEngine.Object) c;
			SerializedObject serializedObject = new SerializedObject(targetObj);

			Type componentType = typeof (c);
			bool showComponentFoldout = false;
			if (showFoldout.ContainsKey(componentType)) {

			} else {
				showFoldout[componentType] = false;
			}

			SerializedProperty prop = serializedObject.GetIterator();
			while (prop.NextVisible(true)) {
				EditorGUILayout.PropertyField(prop);
			}
		}
	}
}
