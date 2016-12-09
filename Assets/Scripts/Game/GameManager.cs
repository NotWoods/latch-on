using System.Collections.Generic;

public class GameManager : SingletonMonoBehaviour<GameManager> {
	public List<PlayerMarker> Players = new List<PlayerMarker>();
}
