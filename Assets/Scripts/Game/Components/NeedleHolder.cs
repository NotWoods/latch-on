using UnityEngine;

[DisallowMultipleComponent]
public class NeedleHolder : MonoBehaviour {
	public GameObject NeedlePrefab;
	public GameObject Needle;

	internal bool DidThrow = false;
}