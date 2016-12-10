using UnityEngine;
using System.Collections.Generic;

public class GameManager : SingletonMonoBehaviour<GameManager> {
	const string SpawnPointName = "Spawn Point";

	public GameObject PlayerPrefab;

	[SerializeField]
	private Dictionary<ControlType, GameObject> Players;
	private Transform spawnPoint;

	void Awake() {
		Players = new Dictionary<ControlType, GameObject>();
		spawnPoint = transform.Find(SpawnPointName);
	}

	public GameObject SpawnPlayer(ControlType controller = ControlType.Keyboard) {
		if (Players.ContainsKey(controller)) return null;

		GameObject player = Instantiate(PlayerPrefab, spawnPoint.position, Quaternion.identity);

		PlayerMarker marker = player.AddComponent<PlayerMarker>();
		marker.Controller = controller;

		Players.Add(controller, player);
		player.GetComponent<Prime31.CharacterController2D>().warpToGrounded();
		return player;
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Time.timeScale = Time.timeScale == 0 ? 1 : 0;
		}
	}
}
