using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
[RequireComponent (typeof (PolygonCollider2D))]
[RequireComponent (typeof (MeshRenderer))]
[RequireComponent (typeof (MeshFilter))]
public class PolygonMesh2D : MonoBehaviour {
	protected PolygonCollider2D polygon;
	protected MeshFilter meshFilter;

	int pathIndex = 0;
	float z = 0f;

	void Start() {
		polygon = gameObject.GetComponent<PolygonCollider2D>();
		meshFilter = gameObject.GetComponent<MeshFilter>();
	} 

	#if UNITY_EDITOR
	void OnColliderUpdate() {
		Vector2[] path = polygon.GetPath(pathIndex);
		Mesh msh = new Mesh();

		msh.vertices = path.Select(v => new Vector3(v.x, v.y, z)).ToArray();
		msh.triangles = new Util.Triangulator(path).Triangulate();

		msh.RecalculateNormals();
		msh.RecalculateBounds();
		meshFilter.mesh = msh;
	}
	#endif
}