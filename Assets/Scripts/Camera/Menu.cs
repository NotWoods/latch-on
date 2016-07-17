using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {
	public Texture2D grappleCursor;
	public Texture2D menuCursor;

	float normalTimeScale = 1;

	void Start() {
		Cursor.SetCursor(grappleCursor, Vector2.one * 16, CursorMode.Auto);
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (Time.timeScale == 0) Time.timeScale = normalTimeScale;
			else Time.timeScale = 0;
		}
	}

	///Reload current scene
	public void Restart() {
		int thisIndex = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(thisIndex);
	}

	///deprecated
	public void Respawn() {Restart();}
}