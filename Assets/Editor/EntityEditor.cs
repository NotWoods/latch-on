using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof (Entity), true)]
public class EntityEditor : Editor {
	private static EntityManager Manager = EntityManager.Instance;
	private Dictionary<Type, bool> showFoldout = new Dictionary<Type, bool>();

	SerializedProperty id;
	void OnEnable() {
		id = serializedObject.FindProperty("ID");
	}

	public override void OnInspectorGUI() {
		serializedObject.Update();

		int entityId = id.intValue;
		EditorGUILayout.LabelField("ID: ", entityId.ToString());

		foreach (IComponent c in Manager.GetComponents(entityId)) {
			if (!(c is UnityEngine.Object)) {
				Debug.LogWarning("IComponent is not an object, cannot inspect");
				continue;
			}

			UnityEngine.Object targetObj = (UnityEngine.Object) c;
			SerializedObject serializedObject = new SerializedObject(targetObj);

			Type cType = c.GetType();
			bool showCFoldout = showFoldout.ContainsKey(cType)
				? showFoldout[cType]
				: false;

			showFoldout[cType] = EditorGUILayout.Foldout(showCFoldout, cType.ToString());
			if (showFoldout[cType]) {
				SerializedProperty prop = serializedObject.GetIterator();
				while (prop.NextVisible(true)) EditorGUILayout.PropertyField(prop);
			}
		}

		serializedObject.ApplyModifiedProperties();
	}
}
