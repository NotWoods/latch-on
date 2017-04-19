using UnityEngine;

/// Marks a GameObject as a local player.
[DisallowMultipleComponent]
public class LocalPlayer : MonoBehaviour {
	int ControllerNumber = -1;
	bool UseTouch = false;
	bool UsePointer = true;
	bool UseKeyboard = true;
}
