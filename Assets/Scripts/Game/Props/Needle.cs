using UnityEngine;

public class Needle : MonoBehaviour {
	// const string SpriteChildName = "Needle Sprite";
	const string LoopChildName = "Loop";

	// private SpriteRenderer spriteRenderer;
	private Transform loop;

	void Awake() {
		// spriteRenderer = transform.Find(SpriteChildName).GetComponent<SpriteRenderer>();
		loop = transform.Find(LoopChildName);
	}

	/// Attaches needle to some point and returns the position of the loop.
	public Vector2 ThrowTo(Vector2 point, Vector2 direction) {
		transform.SetParent(GameManager.Instance.PropsContainer);
		transform.position = point;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		return loop.position;
	}

	public void GiveTo(Transform character) {
		transform.SetParent(character);
		transform.localPosition = Vector2.right * 0.5f;
		transform.rotation = Quaternion.identity;
	}
}