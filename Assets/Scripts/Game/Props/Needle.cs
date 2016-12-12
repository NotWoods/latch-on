using UnityEngine;

public class Needle : MonoBehaviour {
	const string SpriteChildName = "Needle Sprite";
	const string LoopChildName = "Loop";

	private SpriteRenderer spriteRenderer;
	private Transform loop;

	void Start() {
		spriteRenderer = transform.Find(SpriteChildName).GetComponent<SpriteRenderer>();
		loop = transform.Find(LoopChildName);
	}

	/// Attaches needle to some point and returns the position of the loop.
	public Vector2 AttachTo(Vector2 point, Vector2 direction) {
		transform.position = point;
		transform.rotation = Quaternion.LookRotation(direction);
		return loop.position;
	}
}