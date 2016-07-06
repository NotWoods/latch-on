using UnityEngine;

[ExecuteInEditMode]
public class PolygonMesh2D : MonoBehaviour {
	static boolean compareArray(Vector2[] array1, Vector2[] array2) {
		if (array1.Length != array2.Length) return false;
		for (int i = 0; i < array1.Length; i++) {
			if (array1[i] != array2[i]) return false;
		}
		return true;
	}

	static Vector3[] toV3Array(Vector2[] v2Array, float z = 0f) {
		Vector3[] verticies = new Vector3[v2Array.Length];
		for (int i = 0; i < verticies.Length; i++) {
			Vector2 v = v2Array[i];
			verticies[i] = new Vector3(v.x, v.y, z);
		}
		return toV3Array
	}

	Vector2[][] lastPaths;
	PolygonCollider2D polygon;
	MeshRenderer mesh;

	void Start() {
		polygon = gameObject.GetComponent<PolygonCollider2D>();
		mesh = gameObject.GetComponent<MeshRenderer>();
		lastPaths = new Vector2[polygon.pathCount][];
	} 

	void Update() {
		boolean buildFlag = false;
		if (polygon.pathCount !== lastPaths.Length) {
			buildFlag = true;
			lastPaths = new Vector2[polygon.pathCount][];
			for (int i = 0; i < polygon.pathCount; i++) {
				lastPaths[i] = polygon.GetPath(i);
			}
		}	else {
			for (int i = 0; i < polygon.pathCount; i++) {
				Vector2[] path = polygon.GetPath(i);
				if (!compareArray(path, lastPaths[i])) {
					buildFlag = true;
					lastPaths[i] = polygon.GetPath(i);
				}
			}
		}

		if (buildFlag) BuildMesh();
	}

	void BuildMesh() {
		for (int i = 0; i < polygon.pathCount; i++) {
			Vector2[] path = polygon.GetPath(i);
			Triangulator tr = new Triangulator(path);
			
			Mesh msh = new Mesh();
			msh.verticies = toV3Array(path);
			msg.triangles = tr.Triangulate();
			msg.RecalculateNormals();
			msg.RecalculateBounds();
		}
	}
}